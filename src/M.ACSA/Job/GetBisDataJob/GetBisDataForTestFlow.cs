using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;

namespace M.ACSA.Job.GetBisDataJob
{
    [Autowired]
    public class GetBisDataForTestFlow : M.WFEngine.Task.Job.IGetBisDataForWorkflowJob
    {
        [Autowired]
        public IJsonConverter _JsonConverter { get; set; }

        public string ServiceId { get { return "00001PF1OUJQJ0000A01"; } }
        public string GetBisData(string dataId, WFTaskEntity task)
        {
            Console.WriteLine($"业务案件{dataId}获取业务数据……");
            //TODO:从远端获取业务数据
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("dataid", dataId);
            return _JsonConverter.Serialize(data);
        }
    }
}
