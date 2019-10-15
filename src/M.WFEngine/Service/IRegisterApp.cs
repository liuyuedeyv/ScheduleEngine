using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace SendMsgDemo
{

    /// <summary>
    /// 注册到应用端
    /// </summary>
    public interface IRegisterApp
    {
        //string GetWorkflowServiceDataTemplate(string sreviceId);

        Task<string> GetWorkflowServiceBisdata(string serviceId, string dataId);

        //Task<string> GetModelTaskDataTemplate(string sreviceId);

        Task<string> GetModelTaskBisdata(string serviceId, string dataId);
    }
    public class RegisterApp : IRegisterApp
    {
        IHttpClientFactory _httpClientFactory;
        private string _url = "http://localhost:5003/acsa/registerwf";
        public RegisterApp(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> GetModelTaskBisdata(string serviceId, string dataId)
        {
            return await GetRemoteBisdata(serviceId, dataId, EMsgType.GetModelTaskBisdata);
        }

        public async Task<string> GetWorkflowServiceBisdata(string serviceId, string dataId)
        {
            return await GetRemoteBisdata(serviceId, dataId, EMsgType.GetWorkflowServiceBisdata);
        }

        private async Task<string> GetRemoteBisdata(string serviceId, string dataId, EMsgType msgType)
        {
            var client = _httpClientFactory.CreateClient();
            MessageModel model = new MessageModel()
            {
                ServiceId = serviceId,
                DataId = dataId,
                MsgType = msgType
            };
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var result = await client.PostAsync(_url, content);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsStringAsync();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
