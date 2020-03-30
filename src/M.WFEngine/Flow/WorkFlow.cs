﻿using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Logging;
using FD.Simple.Utils.Serialize;
using M.WFEngine.AccessService;
using M.WFEngine.Task;
using M.WFEngine.Util;
using M.WorkFlow.Model;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FD.Simple.Utils.Provider;
using M.WFEngine.DAL;
using M.WFEngine.Flow.DAL;

namespace M.WFEngine.Flow
{
    [Autowired]
    public class WorkFlow : IWorkFlow
    {
        #region 自动注入对象
        [Autowired]
        public DataAccess _DataAccess { get; set; }

        [Autowired]
        public IWFTask _WFTask { get; set; }

        [Autowired]
        public IWorkFlowIns _WorkFlowIns { get; set; }

        [Autowired]
        public IJsonConverter _JsonConverter { get; set; }

        [Autowired]
        public IFDLogger _Logger { get; set; }

        [Autowired] //              
        private WfTEventDal _WfTEventDal { get; set; }

        [Autowired] //              
        private WfFinsDal _WfFinsDal { get; set; }
        #endregion

        #region Start
        public int Start(string serviceId, string dataId,string name)
        {
            var filter = TableFilter.New().Equals("status", 0);

            //根据业务id获取当前版本的id
            var serviceEntity = _DataAccess.Query(WFServiceEntity.TableCode).FixField("id,currentflowid").Where(TableFilter.New().Equals("ID", serviceId)).QueryFirst<WFServiceEntity>();
            if (serviceEntity == null || string.IsNullOrWhiteSpace(serviceEntity.Currentflowid))
            {
                return 0;
            }
            var startTask = _WFTask.GetStartTask(serviceEntity.Currentflowid);
            if (startTask == null)
            {
                throw new Exception("没有找到开始节点");
            }
            //判断是否
            var bisJsonData = GetBisData(startTask, dataId, serviceId, string.Empty, out bool isMultipleNextTask);
            using (var trans = TransHelper.BeginTrans())
            {
                //1、创建流程实例、开始节点实例，执行开始节点任务
                var fins = _WorkFlowIns.CreatFlowInstance(serviceEntity.ID, serviceEntity.Currentflowid, dataId,name);
                var tinsStart = _WFTask.CreateTaskIns(fins, startTask);
                var startTaskSetting = _WFTask.GetTaskInfo(startTask);
                startTaskSetting.RunTask(startTask, fins, tinsStart, null);


                var nextTasks = _WFTask.GetNextTasks(startTask, tinsStart, bisJsonData);
                if (nextTasks == null || nextTasks.Length != 1)
                {
                    throw new Exception("没有找到唯一的下一个任务节点");
                }
                //如果找到多个下个任务节点，则需要获取业务数据

                var nextTask = nextTasks[0];
                var tinsNext = _WFTask.CreateTaskIns(fins, nextTask);
                _WFTask.GetTaskInfo(nextTask).CreateJob(fins, tinsNext, IsNeedCallback(nextTask.Type));
                trans.Commit();
            }
            return 1;
        }

        private string GetBisData(WFTaskEntity taskEntity, string dataId, string serviceId, string tinsId, out bool isMultipleNextTask)
        {
            //从远端获取数据的条件：
            //1、只有接下来有多个节点并且当前节点不等于并行节点，才会去远端获取数据
            //2、当前节点的配置信息中用到的变量，则需要去远端解析
            bool hasTemplateInfo = taskEntity.HasTemplateInfo();
            isMultipleNextTask = _WFTask.IsMultipleNextTask(taskEntity) && taskEntity.Type != ETaskType.BingXing;
            string jsonData = string.Empty;
            if (isMultipleNextTask || hasTemplateInfo)
            {
                jsonData = _WFTask.GetTaskInfo(taskEntity).GetBisData(taskEntity, dataId, serviceId, hasTemplateInfo ? EAccessMessageType.GetVariable : EAccessMessageType.GetWorkflowServiceBisdata);
            }
            return jsonData;
        }
        #endregion

        #region ProcessWftEvent
        public int ProcessWftEvent(uint batchCount)
        {
            var filter = TableFilter.New().Equals("status", 0);//.Equals("waitcallback", 0);
                                                               //#if DEBUG
                                                               //            filter = TableFilter.New().Equals("status", 0).Equals("flowid", "00001F493WJRC0000A01");
                                                               //#endif
            var jobs = _DataAccess.Query(WFTEventEntity.TableCode).FixField("*").Paging(1, batchCount).Where(filter).QueryList<WFTEventEntity>();

            Parallel.ForEach<WFTEventEntity>(jobs,
                new ParallelOptions()
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                }, (job) =>
                {
                    try
                    {
                        Run(job);
                    }
                    catch (HttpRequestException ex)
                    {
                        _Logger.LogError($"处理wftevent发升网络错误：{ex.Message}", ex);
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                        {
                            _Logger.LogError($"处理wftevent发升错误InnerException：{ex.InnerException.Message}", ex.InnerException);
                        }
                        _Logger.LogError($"处理wftevent发升错误：{ex.Message}", ex);
                    }
                });
            return jobs.Count();
        }


        private int Run(WFTEventEntity eventEntity)
        {
            var taskEntity = _WFTask.GetTaskById(eventEntity.Taskid);
            var taskSetting = _WFTask.GetTaskInfo(taskEntity);
            var tinsEntity = _WFTask.GetTinsById(eventEntity.Tinsid);
            var fins = _WorkFlowIns.GetFlowInstance(eventEntity.Finsid);
            var bisJsonData = GetBisData(taskEntity, eventEntity.Dataid, fins.ServiceId, eventEntity.Tinsid, out bool isMultipleNextTask);
            if (isMultipleNextTask && string.IsNullOrWhiteSpace(bisJsonData))
            {
                throw new Exception("获取远端数据异常，请检查远端应用是否正常 ");
            }
            taskEntity.ReplaceTemplateInfo(bisJsonData);
            using (var tran = TransHelper.BeginTrans())
            {
                //1、执行具体任务，并更新信息
                var continueRun = taskSetting.RunTask(taskEntity, fins, tinsEntity, eventEntity);

                eventEntity.State = EDBEntityState.Modified;
                eventEntity.Status = 1;
                eventEntity.ProcessDate = DateTime.Now;
                _DataAccess.Update(eventEntity);

                if (continueRun)
                {
                    //2、更新任务实例
                    tinsEntity.State = EDBEntityState.Modified;
                    tinsEntity.Edate = DateTime.Now;
                    _DataAccess.Update(tinsEntity);

                    //3、生成下一个节点任务实例、并执行流转
                    var nextTasks = _WFTask.GetNextTasks(taskEntity, tinsEntity, bisJsonData);
                    //如果下一个节点是聚合节点，则需要加锁，防止与回调冲突。默认等待10S
                    if (nextTasks.Length == 1 && nextTasks[0].Type == ETaskType.JuHe)
                    {
                        var lockKey = $"{MutexConfig.WORKFLOWLOCKPRE}{fins.Dataid}";
                        var success = ExeWithLock(lockKey, MutexConfig.WORKFLOWLOCKTIMEOUT, () =>
                        {
                            ExeNextTask(fins, nextTasks);
                            tran.Commit();
                        });
                        if (!success)
                        {
                            throw new Exception($"未获取独占锁，请重试，dataid：{fins.Dataid }");
                        }
                    }
                    else
                    {
                        ExeNextTask(fins, nextTasks);
                        tran.Commit();
                    }
                }
                else
                {
                    tran.Commit();
                }
            }
            return 1;
        }

        /// <summary>
        /// 执行下一个节点任务
        /// </summary>
        /// <param name="fins"></param>
        /// <param name="nextTasks"></param>
        private void ExeNextTask(WFFinsEntity fins, WFTaskEntity[] nextTasks)
        {
            foreach (var nextTask in nextTasks)
            {
                bool parallelCompleted = true;

                if (nextTask.Type == ETaskType.JuHe)
                {
                    //判断上游节点是否全部完成
                    var preTasks = _WFTask.GetPreTasks(nextTask);
                    var tins = _WFTask.GetTinssById(fins.ID, preTasks.Select(t => t.ID).ToArray());
                    parallelCompleted = preTasks.Count() == tins.Count() && tins.Where(t => t.Edate == DateTime.MinValue).Count() == 0;
                }
                if (parallelCompleted)
                {
                    var tinsNext = _WFTask.CreateTaskIns(fins, nextTask);
                    var taskSetting = _WFTask.GetTaskInfo(nextTask);
                    taskSetting.CreateJob(fins, tinsNext, IsNeedCallback(nextTask.Type));
                }
            }
        }

        private bool ExeWithLock(string key, int timeout, Action action)
        {
            Mutex mutex = new Mutex(false, key, out bool hasLock);
            if (mutex.WaitOne(timeout, false))
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    _Logger.LogError(ex.Message, ex);
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Callback
        public int Callback(string mqId)
        {
            var filter = TableFilter.New().Equals("id", mqId);
            var eventEntity = _DataAccess.Query(WFTEventEntity.TableCode).FixField("*").Where(filter).QueryFirst<WFTEventEntity>();

            if (eventEntity == null)
            {
                throw new Exception($"回调任务{mqId}不存在");
            }
            if (eventEntity.ProcessDate == DateTime.MinValue)
            {
                throw new Exception($"回到任务会处理完毕，请稍后回调");
            }

            var tinsEntity = _WFTask.GetTinsById(eventEntity.Tinsid);
            var taskEntity = _WFTask.GetTaskById(eventEntity.Taskid);
            var fins = _WorkFlowIns.GetFlowInstance(eventEntity.Finsid);
            var bisJsonData = GetBisData(taskEntity, eventEntity.Dataid, fins.ServiceId, eventEntity.Tinsid, out bool isMultipleNextTask);
            var nextTasks = _WFTask.GetNextTasks(taskEntity, tinsEntity, bisJsonData);

            if (nextTasks.Length != 1)
            {
                throw new Exception("流程配置错误，下个任务节点数量必须是1");
            }
            taskEntity.ReplaceTemplateInfo(bisJsonData);
            using (var tran = TransHelper.BeginTrans())
            {
                //1、更新任务实例状态
                eventEntity.State = EDBEntityState.Modified;
                eventEntity.Waitcallback = 0;
                eventEntity.Callbackdate = DateTime.Now;
                _DataAccess.Update(eventEntity);

                tinsEntity.State = EDBEntityState.Modified;
                tinsEntity.Edate = DateTime.Now;
                _DataAccess.Update(tinsEntity);

                //2、找下个任务节点，并流转，回调节点不应该有多个下个节点
                //如果下一个节点是聚合节点，则需要加锁，防止与回调冲突。默认等待10S
                if (nextTasks[0].Type == ETaskType.JuHe)
                {
                    var lockKey = $"{MutexConfig.WORKFLOWLOCKPRE}{fins.Dataid}";
                    var success = ExeWithLock(lockKey, MutexConfig.WORKFLOWLOCKTIMEOUT, () =>
                        {
                            ExeNextTask(fins, nextTasks);
                            tran.Commit();
                        });
                    if (!success)
                    {
                        throw new Exception($"未获取独占锁，请重试，dataid：{fins.Dataid }");
                    }
                }
                else
                {
                    ExeNextTask(fins, nextTasks);
                    tran.Commit();
                }
            }
            return 1;
        }

        private bool IsNeedCallback(ETaskType taskType)
        {
            return taskType == ETaskType.WorkAsyncSendHttp
                  || taskType == ETaskType.WorkAsyncSendMQ;
        }

        #endregion

        #region CallbackForTaskError
        /// <summary>
        /// 回调任务并注册错误信息
        /// </summary>
        /// <param name="mqId"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public int CallbackForTaskError(string mqId, string errorMsg)
        {
            var filter = TableFilter.New().Equals("id", mqId);
            var eventEntity = _DataAccess.Query(WFTEventEntity.TableCode).FixField("*").Where(filter).QueryFirst<WFTEventEntity>();

            if (eventEntity == null)
            {
                throw new Exception($"回调任务{mqId}不存在");
            }
            //判断任务节点状态是否正确
            if (eventEntity.Waitcallback == 0)
            {
                throw new Exception($"回调任务{mqId}已经回调,无法继续处理");
            }
            if (eventEntity.Status == -1)
            {
                throw new Exception($"回调任务{mqId}已标记为异常,请尝试修复");
            }

            //插入  异常消息
            var flowEntity = _DataAccess.Query(WFFinsEntity.TableCode).FixField("ID,STATUS").Where(TableFilter.New().Equals("ID",eventEntity.Finsid)).QueryFirst<WFFinsEntity>();


            using (var tran = TransHelper.BeginTrans())
            {
                //流程实例更新为失败
                flowEntity.Status = -1;
                _DataAccess.Update(flowEntity);
                //1、更新任务实例状态
                eventEntity.State = EDBEntityState.Modified;
                eventEntity.Status = -1; //标记失败
                _DataAccess.Update(eventEntity);
                //写入event异常日志
                WFTEventMsgEntity en = _DataAccess.GetNewEntity<WFTEventMsgEntity>(); 
                en.EventId = mqId;
                en.CDate=DateTime.Now;
                en.Remark = errorMsg;
                en.RepairDate = new DateTime(9999,12,31);
                en.FinsId = flowEntity.ID;
                _DataAccess.Update(en);
                
                tran.Commit();
            }
            return 1;
        }


        #endregion

        #region 废弃流程
        /// <summary>
        /// 流程废弃
        /// </summary>
        /// <param name="mqId"></param>
        /// <returns></returns>
        public CommonResult<int> GiveUp(string mqId,string reason)
        {
            //根据MQID
            var en = this._WfTEventDal.Get(mqId);

            if (en == null)
            {
                return new WarnResult($"回调任务{mqId}不存在");
            }
            //判断任务节点状态是否正确
            if (en.Waitcallback == 0)
            {
                return new WarnResult($"回调任务{mqId}已经回调,无法继续处理");
            }
            

            var enFins = this._WfFinsDal.Get(en.Finsid);
             
            if (enFins==null)
            {
                return new WarnResult("未找到此流程");
            }
            if (!(enFins.Status == (int)EFlowStatus.Starting|| enFins.Status == (int)EFlowStatus.Error))
            {
                return new WarnResult("非审批中的流程不能废弃!"); 
            }
              
            using (var tran=TransHelper.BeginTrans())
            {
                var num=this._DataAccess.Update(WFFinsEntity.TableCode).Set("Status", (int)EFlowStatus.GiveUp).Where(TableFilter.New().Equals("ID", enFins.ID)).ExecuteNonQuery();
                if (num > 0)
                {
                    this._DataAccess.Update(WFTinsEntity.TableCode)
                        .Set("EDATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                        .Set("Memo", $"因为[{reason}]废弃流程")
                        .Where(TableFilter.New().Equals("FinsId", enFins.ID).IsNull("EDATE"))
                        .ExecuteNonQuery();
                }
                tran.Commit();
            } 

            return 1;
        }
        #endregion
    }
}
