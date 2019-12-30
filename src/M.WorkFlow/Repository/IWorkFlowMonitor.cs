using FD.Simple.DB;
using M.WorkFlow.Model;

namespace M.WFDesigner.Repository
{
    /// <summary>
    /// 流程监控
    /// </summary>
    public interface IWorkFlowMonitor
    {
        /// <summary>
        /// 获取流程中的数据
        /// </summary>
        /// <param name="serviceId">流程服务ID</param>
        /// <returns></returns>
        DBCollection<WFFinsEntity> GetRuningData(WFMonitorSearchModel searchModel);

        /// <summary>
        /// 根据流程实例id获取等待回调数据
        /// </summary>
        /// <param name="finsId"></param>
        /// <returns></returns>
        DBCollection<WFTEventEntity> GetWaitcallbackData(string finsId);
    }
}