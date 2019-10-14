using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.WFEngine.Task.Job;
using M.WorkFlow.Model;
using System;
using System.Net.Http;

namespace M.WFEngine.Task.Node
{
    [Autowired]
    public class TaskWorkAsyncSendMQ : BaseTask
    {
        [Autowired]
        public override DataAccess _DataAccess { get; set; }
        [Autowired]
        public override IJsonConverter _JsonConverter { get; set; }
        [Autowired]
        public override IWorkflowJobs _WFJobs { get; set; }
        public override ETaskType TaskType => ETaskType.WorkAsyncSendHttp;


        public override bool RunTask(WFTaskEntity taskEntity, WFFinsEntity fins, WFTinsEntity tinsEntity, WFTEventEntity mqEntity)
        {
            //TODO:发送数据到MQ

            return false;
        }
    }
}
