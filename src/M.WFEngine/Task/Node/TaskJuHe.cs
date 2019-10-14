using System;
using System.Collections.Generic;
using System.Text;
using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;

namespace M.WFEngine.Task
{
    [Autowired]
    public class TaskJuHe : BaseTask
    {
        [Autowired]
        public override DataAccess _DataAccess { get; set; }

        public override ETaskType TaskType => ETaskType.JuHe;

        public override bool RunTask(WFFinsEntity fins, WFTinsEntity tinsEntity, WFTEventEntity mqEntity)
        {
            Console.WriteLine($"聚合节点{tinsEntity.Taskname}开始执行……");

            return true;
        }
    }
}
