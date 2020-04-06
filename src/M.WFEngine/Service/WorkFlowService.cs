using System;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Provider;
using M.WFEngine.Flow;
using M.WFEngine.Model;

namespace M.WFEngine.Service
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
        public virtual CommonResult<int> StartWF(string serviceId, string dataId, string name)
        {
            if (string.IsNullOrWhiteSpace(serviceId) || string.IsNullOrWhiteSpace(dataId))
            {
                return new WarnResult("dataId  and flowId is not null");
            }


            return _WorkFlow.Start(serviceId, dataId, name);
        }
        /// <summary>
        /// 异步任务回调方法
        /// </summary> 
        /// <returns></returns>

        [Routing(EHttpMethod.HttpGet, "wft/callback")]
        public virtual CommonResult<int> CallbackWF(string mqId)
        {
            if (string.IsNullOrWhiteSpace(mqId))
            {
                return new WarnResult("mqId is not null");
            }

            return _WorkFlow.Callback(mqId);
        }

        /// <summary>
        /// 处理人处理任务
        /// </summary> 
        /// <returns></returns>

        [Routing(EHttpMethod.HttpPost, "wft/run")]
        public virtual CommonResult<int> RunWF(WFCmdRunModel runModel)
        {
            if (string.IsNullOrWhiteSpace(runModel?.OinsId))
            {
                throw new Exception($"Oinsid 不能为空");
            }
            if (string.IsNullOrWhiteSpace(runModel?.CmdId))
            {
                throw new Exception($"Cmdid 不能为空");
            }

            return _WorkFlow.Run(runModel);
        }

        [Routing(EHttpMethod.HttpGet, "wft/giveup")]
        public virtual CommonResult<int> giveup(string mqId, string reason)
        {
            if (string.IsNullOrWhiteSpace(mqId))
            {
                return new WarnResult("mqId is not null");
            }

            return _WorkFlow.GiveUp(mqId, reason);
        }

        /// <summary>
        /// 异步任务回调 失败后调用方法,包含异常信息
        /// </summary>
        /// <param name="mqId">mq异步Id</param>
        /// <param name="errorMsg">错误信息</param>
        /// <returns></returns>

        [Routing(EHttpMethod.HttpGet, "wft/callbackfortaskerror")]
        public virtual CommonResult<int> CallbackWFForTaskError(string mqId, string errorMsg)
        {
            if (string.IsNullOrWhiteSpace(mqId))
            {
                return new WarnResult("mqId is not null");
            }

            if (string.IsNullOrWhiteSpace(errorMsg))
            {
                return new WarnResult("errorMsg is not null");
            }

            try
            {
                return _WorkFlow.CallbackForTaskError(mqId, errorMsg);

            }
            catch (Exception e)
            {
                return new WarnResult(e.Message);
            }
        }

        [Routing(EHttpMethod.HttpGet, "wft/manualProcess")]
        public int ManualProcessWftevent()
        {
            return _WorkFlow.ProcessWftEvent(20);
        }


        //[Routing(EHttpMethod.HttpGet, "wft/appname")]
        //public   string GetAppName(string mqId);
    }
}
