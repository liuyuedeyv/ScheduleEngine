using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.ACSA.Msg;
using System;
using System.Collections.Generic;
using System.Text;

namespace M.ACSA.Service
{

    public class WorkflowService : BaseFoo
    {
        [Autowired]
        public IJsonConverter _JsonConverter { get; set; }

        [Autowired]
        public IEnumerable<IMessage> _WfMessages { get; set; }

        [MessageFormat(EMessageType.Original)]
        [Routing(EHttpMethod.HttpPost, "acsa/registerwf")]
        public string RegisterWorkflow(MessageModel info)
        {
            Console.WriteLine($"{info.ServiceId}收到消息 ：{info.ToString()}");

            return _JsonConverter.Serialize(info);
        }


    }

    public class FlowInfo : IFooParameter
    {
        public string EventType { get; set; }

        public string ServiceId { get; set; }
        public string DataId { get; set; }

        public string Body { get; set; }
    }

    public class MessageModel : IFooParameter
    {
        public string ServiceId { get; set; }
        public string DataId { get; set; }
        public EMsgType MsgType { get; set; }
    }

    public enum EMsgType
    {
        /// <summary>
        /// 获取流程业务数据
        /// </summary>
        GetWorkflowServiceBisdata,
        /// <summary>
        /// 获取决策节点业务数据
        /// </summary>
        GetModelTaskBisdata
    }
}
