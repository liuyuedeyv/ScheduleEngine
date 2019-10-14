using FD.Simple.DB;
using FD.Simple.Utils;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.WFEngine.Service;
using M.WFEngine.Task;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApiClient;

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
        #endregion

        #region Start
        public int Start(string serviceId, string dataId)
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
            var bisJsonData = GetBisData(startTask, dataId, serviceId, string.Empty);
            using (var trans = TransHelper.BeginTrans())
            {
                //1、创建流程实例、开始节点实例，执行开始节点任务
                var fins = _WorkFlowIns.CreatFlowInstance(serviceEntity.ID, serviceEntity.Currentflowid, dataId);
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
                _WFTask.GetTaskInfo(nextTask).CreateJob(fins, tinsNext, nextTask.Type == ETaskType.WorkAsyncSendHttp);
                trans.Commit();
            }
            return 1;
        }

        private string GetBisData(WFTaskEntity taskEntity, string dataId, string serviceId, string tinsId)
        {
            string jsonData = string.Empty;
            if (_WFTask.IsMultipleNextTask(taskEntity))
            {
                jsonData = _WFTask.GetTaskInfo(taskEntity).GetBisData(taskEntity, dataId, serviceId);
            }
            return jsonData;
        }

        #endregion

        #region ProcessWftEvent
        public int ProcessWftEvent(uint batchCount)
        {
            var filter = TableFilter.New().Equals("status", 0);//.Equals("waitcallback", 0);
            var jobs = _DataAccess.Query(WFTEventEntity.TableCode).FixField("*").Paging(1, batchCount).Where(filter).QueryList<WFTEventEntity>();

            Parallel.ForEach<WFTEventEntity>(jobs,
                new ParallelOptions()
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                }, (job) =>
                {
                    Run(job);
                });
            return jobs.Count();
        }
        private int Run(WFTEventEntity eventEntity)
        {
            var taskEntity = _WFTask.GetTaskById(eventEntity.Taskid);
            var taskSetting = _WFTask.GetTaskInfo(taskEntity);
            var tinsEntity = _WFTask.GetTinsById(eventEntity.Tinsid);
            var fins = _WorkFlowIns.GetFlowInstance(eventEntity.Finsid);
            var bisJsonData = GetBisData(taskEntity, eventEntity.Dataid, "", eventEntity.Tinsid);
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
                    ExeNextTask(fins, nextTasks);
                }
                tran.Commit();
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
                    taskSetting.CreateJob(fins, tinsNext, nextTask.Type == ETaskType.WorkAsyncSendHttp);
                }
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

            var tinsEntity = _WFTask.GetTinsById(eventEntity.Tinsid);
            var taskEntity = _WFTask.GetTaskById(eventEntity.Taskid);
            var fins = _WorkFlowIns.GetFlowInstance(eventEntity.Finsid);
            var bisJsonData = GetBisData(taskEntity, eventEntity.Dataid, "", eventEntity.Tinsid);
            var nextTasks = _WFTask.GetNextTasks(taskEntity, tinsEntity, bisJsonData);

            if (nextTasks.Length != 1)
            {
                throw new Exception("流程配置错误，下个任务节点数量必须是1");
            }
            using (var trans = TransHelper.BeginTrans())
            {
                //1、更新任务实例状态
                eventEntity.State = EDBEntityState.Modified;
                eventEntity.Waitcallback = 0;
                eventEntity.Callbackdate = DateTime.Now;
                _DataAccess.Update(eventEntity);

                tinsEntity.State = EDBEntityState.Modified;
                tinsEntity.Edate = DateTime.Now;
                _DataAccess.Update(tinsEntity);
                //2、如果配置了多个下个任务节点，则需要获取业务数据


                //2、找下个任务节点，并流转，回调节点不应该有多个下个节点
                ExeNextTask(fins, nextTasks);
                trans.Commit();
            }
            return 1;
        }

        #endregion
    }
}
