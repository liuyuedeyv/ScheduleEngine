using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApiClient;

namespace M.RemoteService.Controllers
{
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public static System.Collections.Concurrent.ConcurrentQueue<string> queueIds = new System.Collections.Concurrent.ConcurrentQueue<string>();


        // GET api/values
        [HttpGet]
        [HttpPost]
        [Route("api/task")]
        public ActionResult<IEnumerable<string>> Get([FromForm]RemoteMethodInfo info)
        {
            if (!string.IsNullOrWhiteSpace(info?.CallbackTag))
            {
                queueIds.Enqueue(info?.CallbackTag);
            }
            return new string[] { info?.CallbackTag, info?.CustomTag };
        }


        [HttpPost]
        [Route("healthinfo")]
        public ActionResult<IEnumerable<string>> healthinfo([FromForm] RemoteMethodInfo info)
        {
            return new string[] { info?.CallbackTag, info?.CustomTag };
        }
    }
    public class RemoteMethodInfo
    {
        /// <summary>
        /// 回调标识
        /// </summary>
        public string CallbackTag { get; set; }
        /// <summary>
        /// 自定义标识
        /// </summary>
        public string CustomTag { get; set; }
    }

    public class CallbackService : BackgroundService
    {
        private readonly ILogger<CallbackService> _logger;

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("后台任务开始启动^");
            return base.StartAsync(cancellationToken);
        }

        public CallbackService(ILogger<CallbackService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogWarning($"GracePeriodManagerService is starting.");

            stoppingToken.Register(() =>
                _logger.LogWarning($" GracePeriod background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogWarning($"GracePeriod task doing background work.count:" + ValuesController.queueIds.Count);

                // This eShopOnContainers method is querying a database table
                // and publishing events into the Event Bus (RabbitMQ / ServiceBus)
                CheckConfirmedGracePeriodOrders();

                await Task.Delay(1, stoppingToken);
            }

            _logger.LogWarning($"GracePeriod background task is stopping.");
        }
        static IWFCallback wFCallback = null;

        private async void CheckConfirmedGracePeriodOrders()
        {
            _logger.LogWarning("记录数：" + ValuesController.queueIds.Count);
            if (ValuesController.queueIds.Count > 0)
            {
                if (ValuesController.queueIds.TryDequeue(out string id))
                {
                    try
                    {
                        _logger.LogWarning($"取出id{id}");
                        if (wFCallback == null)
                        {
                            wFCallback = HttpApi.Resolve<IWFCallback>();
                        }
                        var result = await wFCallback.Call(id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }
                }
            }
        }
    }
}
