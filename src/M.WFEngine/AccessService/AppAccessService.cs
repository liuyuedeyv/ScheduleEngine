using FD.Simple.DB;
using FD.Simple.Utils;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace M.WFEngine.AccessService
{
    [Autowired]
    public class AppAccessService : IAppAccessService
    {
        DataAccess _dataAccess;
        IHttpClientFactory _httpClientFactory;

        public AppAccessService(IHttpClientFactory httpClientFactory, DataAccess dataAccess)
        {
            _httpClientFactory = httpClientFactory;
            _dataAccess = dataAccess;
        }
        public async Task<string> GetModelTaskBisdata(string serviceId, string dataId, string objectCode)
        {
            return await GetRemoteBisdata(serviceId, dataId, EAccessMessageType.GetModelTaskBisdata, objectCode);
        }

        public async Task<string> GetWorkflowServiceBisdata(string serviceId, string dataId)
        {
            return await GetRemoteBisdata(serviceId, dataId, EAccessMessageType.GetWorkflowServiceBisdata, null);
        }

        public async Task<string> GetVaribleTaskBisdata(string serviceId, string dataId, string varibles)
        {
            return await GetRemoteBisdata(serviceId, dataId, EAccessMessageType.GetVariable, varibles);
        }
        private async Task<string> GetRemoteBisdata(string serviceId, string dataId, EAccessMessageType msgType, string objectCode)
        {
            var client = _httpClientFactory.CreateClient();
            MessageModel model = new MessageModel()
            {
                ServiceId = serviceId,
                DataId = dataId,
                MsgType = msgType,
                ObjectCode = objectCode,
                Body = objectCode
            };
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var url = GetAppAccessUrl(serviceId);
            var result = await client.PostAsync(url, content);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsStringAsync();
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 从数据库或者缓存中获取流程服务的接入url信息
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        private string GetAppAccessUrl(string serviceId)
        {
            var filter = TableFilter.New().Equals("id", serviceId);
            var serviceEntity = _dataAccess.Query(WFServiceEntity.TableCode).FixField("id,wfappid").Where(filter).QueryFirst<WFServiceEntity>();

            filter = TableFilter.New().Equals("id", serviceEntity.WfappId);
            return _dataAccess.Query("wfapp").FixField("url").Where(filter).QueryScalar().ConvertTostring();
        }

    }
}
