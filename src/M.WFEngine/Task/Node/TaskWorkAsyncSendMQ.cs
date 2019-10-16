using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;
using System;
namespace M.WFEngine.Task.Node
{
    [Autowired]
    public class TaskWorkAsyncSendMQ : BaseTask
    {
        public override ETaskType TaskType => ETaskType.WorkAsyncSendHttp;


        public override bool RunTask(WFTaskEntity taskEntity, WFFinsEntity fins, WFTinsEntity tinsEntity, WFTEventEntity mqEntity)
        {
            //TODO:发送数据到MQ
            Console.WriteLine("发送数据到MQ……");
            return false;
        }
    }
}
