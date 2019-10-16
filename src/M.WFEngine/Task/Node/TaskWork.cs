using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;


namespace M.WFEngine.Task
{

    [Autowired]
    public class TaskWork : BaseTask
    {
        public override ETaskType TaskType => ETaskType.Work;
    }
}
