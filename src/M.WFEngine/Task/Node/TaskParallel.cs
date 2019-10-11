using System;
using System.Collections.Generic;
using System.Text;
using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;

namespace M.WorkFlow.Engine.Task
{
    [Autowired]
    public class TaskParallel : BaseTask
    {
        [Autowired]
        public override DataAccess _DataAccess { get; set; }

        public override ETaskType TaskType => ETaskType.BingXing;

        public override bool RunTask(WFFinsEntity fins, WFTinsEntity tinsEntity, WFTEventEntity mqEntity)
        {
            Console.WriteLine($"并行节点{tinsEntity.Taskname}开始执行……");

            return true;
        }
    }
}
