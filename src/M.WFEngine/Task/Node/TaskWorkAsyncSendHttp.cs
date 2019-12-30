using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace M.WFEngine.Task
{
    [Autowired]
    public class TaskWorkAsyncSendHttp : BaseTask
    {

        IJsonConverter _jsonConverter;
        IHttpClientFactory _httpClientFactory;
        public TaskWorkAsyncSendHttp(IHttpClientFactory httpClientFactory, IJsonConverter jsonConverter)
        {
            _httpClientFactory = httpClientFactory;
            _jsonConverter = jsonConverter;
        }

        public override ETaskType TaskType => ETaskType.WorkAsyncSendHttp;

        public override bool RunTask(WFTaskEntity taskEntity, WFFinsEntity fins, WFTinsEntity tinsEntity, WFTEventEntity enventEntity)
        {
            // 根据tasksetting 节点的配置信息读取要发送到的远端地址
            //可以是多个地址
            if (!string.IsNullOrWhiteSpace(taskEntity.Setting))
            {
                var list = _jsonConverter.Deserialize<List<SendHttpModel>>(taskEntity.Setting);
                if (list != null)
                {
                    Dictionary<string, string> dicPostdata = null;
                    foreach (var item in list)
                    {
                        //Console.WriteLine($"发送数据到远端{item.Url}");

                        dicPostdata = new Dictionary<string, string>();
                        dicPostdata.Add("callbackTag", enventEntity.ID);
                        dicPostdata.Add("customTag", item.CustomTag);
                        dicPostdata.Add("dataId", enventEntity.Dataid);
                        var content = new FormUrlEncodedContent(dicPostdata);
                        var httpClient = _httpClientFactory.CreateClient();
                        try
                        {
                            var response = httpClient.PostAsync(item.Url, content).GetAwaiter().GetResult();
                            if (response.IsSuccessStatusCode)
                            {
                                response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                            }
                            else
                            {
                                var a = "";
                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            throw new HttpRequestException($"{ex.Message}，RunTask url：{item.Url} content:{_jsonConverter.Serialize(dicPostdata) }");
                        }
                    }
                    return false;
                }
            }
            return true;
        }
    }

    public class SendHttpModel
    {
        public string Url { get; set; }

        public string CustomTag { get; set; }
    }
}
