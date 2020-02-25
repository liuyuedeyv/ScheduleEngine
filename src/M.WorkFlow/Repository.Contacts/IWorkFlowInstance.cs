using FD.Simple.Utils.Provider;
using M.WorkFlow.Model;
using System.Collections.Generic;

namespace M.WFDesigner.Repository
{
    /// <summary>
    ///  流程实例相关
    /// </summary>
    public interface IWorkFlowInstance
    {
        /// <summary>
        /// 获取任务节点实例信心
        /// </summary>
        /// <param name="insId"></param>
        /// <returns></returns>
        List<WFTinsEntity> GetTinsList(string finsId); 

    }
}
