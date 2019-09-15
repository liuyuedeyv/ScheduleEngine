using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace M.WorkFlow.Engine.Task
{
    [Autowired]
    public class TaskWorkAsync : BaseTask
    {
        [Autowired]
        public override DataAccess _DataAccess { get; set; }
        public override ETaskType TaskType => ETaskType.WorkAsync;

        public override bool RunTask(WFFinsEntity fins, WFTinsEntity tinsEntity)
        {
            Console.WriteLine($"异步节点{tinsEntity.Taskname}开始执行……");

            return false;
        }
    }
}
