using System;
using System.Collections.Generic;
using System.Text;
using WebApiClient;

namespace M.WFEngine.Service
{

    public interface IAppService : IHttpApi
    {
        [WebApiClient.Attributes.HttpGet("acsa/registerwf")]
        ITask<string> Call(FlowInfo info);
    }

    public class FlowInfo
    {
        public string EventType { get; set; }

        public string ServiceId { get; set; }
        public string DataId { get; set; }

        public string Body { get; set; }
    }
}
