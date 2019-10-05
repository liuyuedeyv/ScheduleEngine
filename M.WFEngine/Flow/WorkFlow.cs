using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;
using System;
using System.Linq;

namespace M.WorkFlow.Engine
{
    [Autowired]
    public class WorkFlow : IWorkFlow
    {
        #region 自动注入对象
        [Autowired]
        public DataAccess _DataAccess { get; set; }

        [Autowired]
        public IWFTask _WFTask { get; set; }

        #endregion

        public WFFinsEntity CreatFlowInstance(string flowId, string dataId)
        {
            WFFinsEntity fins = new WFFinsEntity();
            fins.Add();
            fins.Flowid = flowId;
            fins.Dataid = dataId;
            fins.Name = dataId;
            fins.Cdate = DateTime.Now;
            fins.Status = (int)EFlowStatus.Starting;
            _DataAccess.Update(fins);
            return fins;
        }

        public WFFinsEntity GetFlowInstance(string finsId)
        {
            var filter = TableFilter.New().Equals("ID", finsId);
            return _DataAccess.Query(WFFinsEntity.TableCode).FixField("*")
                .Where(filter).QueryFirst<WFFinsEntity>();
        }

        public int Callback(string mqId)
        {
            var filter = TableFilter.New().Equals("id", mqId);
            var mqEntity = _DataAccess.Query(WFTEventEntity.TableCode).FixField("*").Where(filter).QueryFirst<WFTEventEntity>();

            if (mqEntity == null)
            {
                throw new Exception($"回调任务{mqId}不存在");
            }

            var tinsEntity = _WFTask.GetTinsById(mqEntity.Tinsid);
            var taskEntity = _WFTask.GetTaskById(mqEntity.Taskid);
            var fins = this.GetFlowInstance(mqEntity.Finsid);
            var nextTasks = _WFTask.GetNextTasks(taskEntity);
            if (nextTasks.Length != 1)
            {
                throw new Exception("流程配置错误，下个任务节点数量必须是1");
            }
            using (var trans = TransHelper.BeginTrans())
            {
                //1、更新任务实例状态
                mqEntity.State = EDBEntityState.Modified;
                mqEntity.Waitcallback = 0;
                mqEntity.Callbackdate = DateTime.Now;
                _DataAccess.Update(mqEntity);

                tinsEntity.State = EDBEntityState.Modified;
                tinsEntity.Edate = DateTime.Now;
                _DataAccess.Update(tinsEntity);

                //2、找下个任务节点，并流转
                var nextTask = nextTasks[0];
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
                    _WFTask.GetTaskInfo(nextTask).CreateJob(fins, tinsNext, nextTask.Type == ETaskType.WorkAsync);
                }
                trans.Commit();
            }
            return 1;
        }

        public int Run(WFTEventEntity mqEntity)
        {
            var taskEntity = _WFTask.GetTaskById(mqEntity.Taskid);
            var taskSetting = _WFTask.GetTaskInfo(taskEntity);
            var tinsEntity = _WFTask.GetTinsById(mqEntity.Tinsid);
            var fins = this.GetFlowInstance(mqEntity.Finsid);
            using (var tran = TransHelper.BeginTrans())
            {
                //1、执行具体任务，并更新任务队列信息
                var continueRun = taskSetting.RunTask(fins, tinsEntity, mqEntity);

                mqEntity.State = EDBEntityState.Modified;
                mqEntity.Status = 1;
                mqEntity.ProcessDate = DateTime.Now;
                _DataAccess.Update(mqEntity);

                if (continueRun)
                {
                    //2、更新任务实例
                    tinsEntity.State = EDBEntityState.Modified;
                    tinsEntity.Edate = DateTime.Now;
                    _DataAccess.Update(tinsEntity);

                    //3、生成下一个节点任务实例、并执行流转
                    var nextTasks = _WFTask.GetNextTasks(taskEntity);
                    foreach (var task in nextTasks)
                    {
                        var tinsNext = _WFTask.CreateTaskIns(fins, task);
                        taskSetting = _WFTask.GetTaskInfo(task);
                        if (task.Type == ETaskType.End)
                        {
                            taskSetting.RunTask(fins, tinsNext, mqEntity);
                        }
                        else
                        {
                            taskSetting.CreateJob(fins, tinsNext, task.Type == ETaskType.WorkAsync);
                        }
                    }
                }
                tran.Commit();
            }
            return 1;
        }

        public int Start(string flowId, string dataId)
        {
            var filter = TableFilter.New().Equals("status", 0);
            var jobs = _DataAccess.Query(WFTEventEntity.TableCode).FixField("*").Paging(1, 10).Where(filter).QueryList<WFTEventEntity>();


            var startTask = _WFTask.GetStartTask(flowId);
            if (startTask == null)
            {
                throw new Exception("没有找到开始节点");
            }
            var nextTasks = _WFTask.GetNextTasks(startTask);
            if (nextTasks == null || nextTasks.Length != 1)
            {
                throw new Exception("没有找到唯一的下一个任务节点");
            }
            var nextTask = nextTasks[0];
            using (var trans = TransHelper.BeginTrans())
            {
                //1、创建流程实例、开始节点实例，执行开始节点任务
                var fins = this.CreatFlowInstance(flowId, dataId);
                var tinsStart = _WFTask.CreateTaskIns(fins, startTask);
                _WFTask.GetTaskInfo(startTask).RunTask(fins, tinsStart, null);

                //2、获取下一个任务节点，并且创建待执行任务
                var tinsNext = _WFTask.CreateTaskIns(fins, nextTask);
                _WFTask.GetTaskInfo(nextTask).CreateJob(fins, tinsNext, nextTask.Type == ETaskType.WorkAsync);
                trans.Commit();
            }
            return 1;
        }
    }
}
