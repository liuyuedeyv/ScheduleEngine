using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.WFEngine.AccessService;
using M.WFEngine.Flow;
using M.WorkFlow.Model;
using System;

namespace M.WFEngine.Task
{
    //[Autowired]
    public class TaskEnd : BaseTask
    {
        public override ETaskType TaskType => ETaskType.End;
        public override bool RunTask(WFTaskEntity taskEntity, WFFinsEntity fins, WFTinsEntity tinsEntity, WFTEventEntity mqEntity)
        {
            //结束节点不需要做具体任务
            return false;
        }

        public override void CreateJob(WFFinsEntity fins, WFTinsEntity tinsEntity)
        {
            //Console.WriteLine($"结束节点{tinsEntity.Taskname}开始执行……");

            //不用为结束节点创建待执行任务
            fins.State = EDBEntityState.Modified;
            fins.Status = (int)EFlowStatus.Completed;
            fins.Edate = DateTime.Now;
            _DataAccess.Update(fins);
        }
        public override string GetBisData(WFTaskEntity taskEntity, string dataId, string serviceId, EAccessMessageType messageType)
        {
            //结束节点不用获取业务数据
            return string.Empty;
        }
    }
}
