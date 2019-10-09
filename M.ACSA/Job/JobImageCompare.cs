using FD.Simple.Utils.Agent;
using M.WorkFlow.Engine;
using M.WorkFlow.Engine.Job;
using System;

namespace M.ACSA.Job
{
    [Autowired]
    public class JobImageCompare : IJob
    {
        public ETaskType TaskType => ETaskType.Work;

        public void Exe(string dataId)
        {
            Console.WriteLine("图像比对开始");
        }
    }
}
