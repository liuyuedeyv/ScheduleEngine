using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace M.WFEngine.Task
{
    /// <summary>
    /// 决策节点
    /// </summary>
    [Autowired]
    public class TaskModel : BaseTask
    {
        [Autowired]
        public override DataAccess _DataAccess { get; set; }
        [Autowired]
        public override IJsonConverter _JsonConverter { get; set; }
        public override ETaskType TaskType => ETaskType.Model;
        [Autowired]
        public IHttpClientFactory _httpClientFactory { get; set; }

        public override string GetBisData(WFTaskEntity taskEntity, string dataId, string serviceId)
        {
            //调取远方模型进行预算，返回结果
            var client = _httpClientFactory.CreateClient();
            var url = $@"http://localhost:5003/acsa/registerwf";

            FlowInfo flowInfo = new FlowInfo();
            flowInfo.Body = "123";
            flowInfo.DataId = dataId;
            var httpconent = new StringContent(_JsonConverter.Serialize(flowInfo), Encoding.UTF8, "apllication/json");
            var result = client.PutAsync(url, httpconent).GetAwaiter().GetResult();

            var jsonData = _JsonConverter.Serialize(result.Content);
            WFTDataEntity dataEntity = new WFTDataEntity();
            dataEntity.State = EDBEntityState.Added;
            dataEntity.Flowid = taskEntity.Flowid;
            dataEntity.Finsid = "";
            dataEntity.Taskid = taskEntity.ID;
            dataEntity.Tinsid = "";
            dataEntity.Cdate = System.DateTime.Now;
            dataEntity.JsonData = jsonData;
            _DataAccess.Update(dataEntity);

            return jsonData;
        }
    }

    public class FlowInfo
    {
        public string EventType { get; set; }

        public string ServiceId { get; set; }
        public string DataId { get; set; }

        public string Body { get; set; }
    }
}
