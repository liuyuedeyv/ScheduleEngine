using FD.Simple.Utils.Provider;

namespace M.WFEngine.Flow
{
    public interface IWorkFlow
    {
        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="flowId"></param>
        /// <param name="dataId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        int Start(string flowId, string dataId,string name);
        /// <summary>
        /// 执行任务的回调
        /// </summary>
        /// <param name="mqId"></param>
        /// <returns></returns>
        int Callback(string mqId);
        /// <summary>
        /// 处理后台任务事件
        /// </summary>
        /// <param name="batchCount">批次预期数量</param>
        /// <returns>实际取到可执行任务数量</returns>
        int ProcessWftEvent(uint batchCount);
        /// <summary>
        /// 异步任务回调 失败后调用方法,包含异常信息
        /// </summary>
        /// <param name="mqId">mq异步Id</param>
        /// <param name="errorMsg">错误信息</param>
        /// <returns></returns>
        int CallbackForTaskError(string mqId, string errorMsg);
        /// <summary>
        /// 废弃
        /// </summary>
        /// <param name="mqId"></param>
        /// <returns></returns>
        CommonResult<int> GiveUp(string mqId,string reason);
    }
}
