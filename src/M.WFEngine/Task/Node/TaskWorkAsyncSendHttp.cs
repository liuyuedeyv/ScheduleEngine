using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.WFEngine.Task.Job;
using M.WorkFlow.Model;
using System;
using System.Net.Http;

namespace M.WFEngine.Task
{
    [Autowired]
    public class TaskWorkAsyncSendHttp : BaseTask
    {
        [Autowired]
        public override DataAccess _DataAccess { get; set; }
        [Autowired]
        public override IJsonConverter _JsonConverter { get; set; }
        [Autowired]
        public override IWorkflowJobs _WFJobs { get; set; }

        public override ETaskType TaskType => ETaskType.WorkAsyncSendHttp;

        static HttpClient httpClient = new HttpClient();
        private static readonly object asyncLock = new object();
        public override bool RunTask(WFTaskEntity taskEntity, WFFinsEntity fins, WFTinsEntity tinsEntity, WFTEventEntity mqEntity)
        {
            Console.WriteLine($"异步节点{tinsEntity.Taskname}开始执行……");

            var settingEntity = taskEntity.SettingEntity;

            //lock (asyncLock)
            //{
            var url = $@"http://localhost:5005/api/task?CallbackTag={mqEntity.ID}";

            //1、callbacktag
            //2、dataid
            //3、

            //httpClient.Timeout = new TimeSpan(0, 0, 10);
            httpClient.GetAsync(url).GetAwaiter().GetResult();


            //}
            return false;
        }
    }
}
