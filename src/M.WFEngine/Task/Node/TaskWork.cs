using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WFEngine.Task.Job;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;


namespace M.WFEngine.Task
{

    [Autowired]
    public class TaskWork : BaseTask
    {
        [Autowired]
        public override DataAccess _DataAccess { get; set; }

        [Autowired]
        public IEnumerable<IJob> Jobs { get; set; }


        public override ETaskType TaskType => ETaskType.Work;

        public override bool RunTask(WFFinsEntity fins, WFTinsEntity tinsEntity, WFTEventEntity mqEntity)
        {
            Console.WriteLine($"同步节点{tinsEntity.Taskname}开始执行……");
            var job = Jobs.Where(j => j.TaskType == this.TaskType).FirstOrDefault();

            job.Exe("");

            return true;
        }
    }
}
