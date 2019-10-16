using System;
using System.Collections.Generic;
using System.Text;
using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.WorkFlow.Model;

namespace M.WFEngine.Task
{
    [Autowired]
    public class TaskParallel : BaseTask
    {
        public override ETaskType TaskType => ETaskType.BingXing;

        public override string GetBisData(WFTaskEntity taskEntity, string dataId, string serviceId)
        {
            //并行节点不用获取业务数据
            return string.Empty;
        }
    }
}
