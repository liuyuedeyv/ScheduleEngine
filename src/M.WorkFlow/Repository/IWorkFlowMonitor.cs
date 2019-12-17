using FD.Simple.Utils.Provider;
using M.WorkFlow.Model;
using System.Collections.Generic;
using FD.Simple.DB;

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
        /// <returns></returns>
        DBCollection<WFFinsEntity> GetRuningData(FD.Simple.DB.Model.PageEntity searchModel);
        /// <summary>
        /// 获取流程中的数据
        /// </summary>
        /// <param name="serviceId">流程服务ID</param>
        /// <returns></returns>
        DBCollection<WFFinsEntity> GetRuningDataByService(WFMonitorSearchModel searchModel);
    }
}
