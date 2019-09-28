using System;
using System.Collections.Generic;
using System.Text;

namespace M.WorkFlow.Engine.Job
{
    public interface IJob
    {
        ETaskType TaskType { get; }
        void Exe(string dataId);
    }
}
