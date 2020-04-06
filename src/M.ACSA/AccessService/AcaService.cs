using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.ACSA.ResponseService;
using System;
using System.Collections.Generic;

namespace M.ACSA.AccessService
{

    public class AcaService 
    {
        [Autowired]
        public IEnumerable<IWfServiceMessage> _WfMessageService { get; set; }
        [Autowired]
        public IJsonConverter _JsonConverter { get; set; }

        [Routing(EHttpMethod.HttpPost, "acsa/healthcheck")]
        public string Healthcheck(MessageModel info)
        {
            if (!_WfMessageService.CheckServicesetting(out string msg))
            {
                return msg;
            }
            return "ok";
        }

        [MessageFormat(EMessageType.Original)]
        [Routing(EHttpMethod.HttpPost, "acsa/registerwf")]
        public string RegisterWorkflow(MessageModel info)
        {
            Console.WriteLine($"{info.ServiceId}收到消息 ：{_JsonConverter.Serialize(info)}");

            var service = _WfMessageService.GetServiceMessageInstance(info);
            if (service != null)
            {
                return service.AnswerMessage(info);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
