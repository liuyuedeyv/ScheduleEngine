using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Net.Http;

namespace M.WorkFlow.Test
{
    public class WorkFlowMonitorHttpTest
    {
        string host = "http://10.50.135.21:5002";
        IHttpClientFactory _httpClientFactory;
        public WorkFlowMonitorHttpTest(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Fact]
        public async void Test1()
        {
            var url = $"{host}/wfm/getruningdata";
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent("");
            var respone = await client.PostAsync(url, content);
            respone.Content.ToString();
            //client.PostAsync()
        }
    }
}
