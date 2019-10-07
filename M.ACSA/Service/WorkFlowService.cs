using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Provider;
using M.WorkFlow.Engine;
using System.Linq;

namespace M.WorkFlow
{
    public class WorkFlowService : BaseFoo
    {
        [Autowired]
        public IWorkFlow _WorkFlow { get; set; }

        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="dataId"></param>
        /// <returns></returns>
        [Routing(EHttpMethod.HttpGet, "wft/start")]
        public CommonResult<int> StartWF(string serviceId, string dataId)
        {
            if (string.IsNullOrWhiteSpace(serviceId) || string.IsNullOrWhiteSpace(dataId))
            {
                return new WarnResult("dataId  and flowId is not null");
            }


            return _WorkFlow.Start(serviceId, dataId);
        }
        /// <summary>
        /// 异步任务回调方法
        /// </summary>
        /// <param name="flowId"></param>
        /// <param name="dataId"></param>
        /// <returns></returns>

        [Routing(EHttpMethod.HttpGet, "wft/callback")]
        public CommonResult<int> CallbackWF(string mqId)
        {
            if (string.IsNullOrWhiteSpace(mqId))
            {
                return new WarnResult("mqId is not null");
            }

            return _WorkFlow.Callback(mqId);
        }
    }
}
