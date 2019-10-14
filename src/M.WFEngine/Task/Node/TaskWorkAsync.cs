using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;
using System;
using System.Net.Http;

namespace M.WFEngine.Task
{
    [Autowired]
    public class TaskWorkAsync : BaseTask
    {
        [Autowired]
        public override DataAccess _DataAccess { get; set; }
        public override ETaskType TaskType => ETaskType.WorkAsync;

        static HttpClient httpClient = new HttpClient();
        private static readonly object asyncLock = new object();
        public override bool RunTask(WFFinsEntity fins, WFTinsEntity tinsEntity, WFTEventEntity mqEntity)
        {
            Console.WriteLine($"异步节点{tinsEntity.Taskname}开始执行……");

            //lock (asyncLock)
            //{
            var url = $@"http://localhost:5004/api/task?CallbackTag={mqEntity.ID}";


            //httpClient.Timeout = new TimeSpan(0, 0, 10);
            httpClient.GetAsync(url).GetAwaiter().GetResult();


            //}
            return false;
        }
    }
}
