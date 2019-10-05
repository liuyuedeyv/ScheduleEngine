using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace M.WorkFlow.Engine.Task
{
    [Autowired]
    public class TaskStart : BaseTask
    {
        [Autowired]
        public override DataAccess _DataAccess { get; set; }
        public override ETaskType TaskType => ETaskType.Start;
        public override bool RunTask(WFFinsEntity fins, WFTinsEntity tinsEntity, WFTEventEntity mqEntity)
        {
            //开始节点不需要执行具体任务           
            Console.WriteLine($"开始节点{tinsEntity.Taskname}开始执行……");

            return true;
        }
    }
}
