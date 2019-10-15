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
        [Autowired]
        public override DataAccess _DataAccess { get; set; }
        public override ETaskType TaskType => ETaskType.Start;
        [Autowired]
        public override IJsonConverter _JsonConverter { get; set; }
    }
}
