using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.WFEngine.Flow;
using M.WFEngine.Task.Job;
using M.WorkFlow.Model;
using System;

namespace M.WFEngine.Task
{
    [Autowired]
    public class TaskEnd : BaseTask
    {
        [Autowired]
        public override DataAccess _DataAccess { get; set; }
        public override ETaskType TaskType => ETaskType.End;
        [Autowired]
        public override IWorkflowJobs _WFJobs { get; set; }
        [Autowired]
        public override IJsonConverter _JsonConverter { get; set; }

        public override bool RunTask(WFTaskEntity taskEntity, WFFinsEntity fins, WFTinsEntity tinsEntity, WFTEventEntity mqEntity)
        {
            //结束节点不需要做具体任务
            return false;
        }

        public override void CreateJob(WFFinsEntity fins, WFTinsEntity tinsEntity, bool needWaitCallback)
        {
            Console.WriteLine($"结束节点{tinsEntity.Taskname}开始执行……");

            //不用为结束节点创建待执行任务
            fins.State = EDBEntityState.Modified;
            fins.Status = (int)EFlowStatus.Completed;
            fins.Edate = DateTime.Now;
            _DataAccess.Update(fins);
        }
        public override string GetBisData(WFTaskEntity taskEntity, string dataId, string serviceId)
        {
            //结束节点不用获取业务数据
            return string.Empty;
        }
    }
}
