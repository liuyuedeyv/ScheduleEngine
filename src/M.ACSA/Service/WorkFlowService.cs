using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using System;
using System.Collections.Generic;
using System.Text;

namespace M.ACSA.Service
{

    public class WorkflowService : BaseFoo
    {
        [Autowired]
        public IJsonConverter _JsonConverter { get; set; }

        [MessageFormat(EMessageType.Original)]
        [Routing(EHttpMethod.HttpPost, "acsa/registerwf")]
        public string RegisterWorkflow(FlowInfo info)
        {
            Console.WriteLine($"{info.ServiceId}收到消息 ：{info.Body}");


            if (info.EventType == "1")
            {
                Dictionary<string, string> dicData = new Dictionary<string, string>();
                dicData.Add("dataid", info.DataId);

                return _JsonConverter.Serialize(dicData);
            }

            return string.Empty;
        }

    }

    public class FlowInfo : IFooParameter
    {
        public string EventType { get; set; }

        public string ServiceId { get; set; }
        public string DataId { get; set; }

        public string Body { get; set; }
    }
}
