using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace M.WFEngine.Task
{
    [Autowired]
    public class TaskStart : BaseTask
    {
        public override ETaskType TaskType => ETaskType.Start;
    }
}
