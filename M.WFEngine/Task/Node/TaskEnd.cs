using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace M.WorkFlow.Engine.Task
{
    [Autowired]
    public class TaskEnd : BaseTask
    {
        [Autowired]
        public override DataAccess _DataAccess { get; set; }
        public override ETaskType TaskType => ETaskType.End;

        public override bool RunTask(WFFinsEntity fins, WFTinsEntity tinsEntity, WFMQEntity mqEntity)
        {
            //结束节点不需要做具体任务
            Console.WriteLine($"结束节点{tinsEntity.Taskname}开始执行……");

            fins.State = EDBEntityState.Modified;
            fins.Status = (int)EFlowStatus.Completed;
            fins.Edate = DateTime.Now;
            _DataAccess.Update(fins);

            return false;
        }

        public override void CreateJob(WFFinsEntity fins, WFTinsEntity tinsEntity, bool needWaitCallback)
        {
            //不用为结束节点创建待执行任务，直接执行runtask即可
            RunTask(fins, tinsEntity, null);
        }
    }
}
