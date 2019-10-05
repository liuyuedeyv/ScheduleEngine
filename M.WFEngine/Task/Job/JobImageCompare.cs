using FD.Simple.Utils.Agent;
using System;
using System.Collections.Generic;
using System.Text;

namespace M.WorkFlow.Engine.Job
{
    [Autowired]
    public class JobDemo : IJob
    {
        public ETaskType TaskType => ETaskType.Work;

        public void Exe(string dataId)
        {
            Console.WriteLine("JobDemo开始");
        }
    }
}
