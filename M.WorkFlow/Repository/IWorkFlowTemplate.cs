using FD.Simple.Utils.Provider;
using M.WorkFlow.Model;
using System.Collections.Generic;

namespace M.WFDesigner.Repository
{
    public interface IWorkFlowTemplate
    {
        /// <summary>
        /// 更新流程模板
        /// </summary>
        /// <param name="flowEntity"></param>
        /// <returns></returns>
        int UpdateTemplate(WFFlowEntity flowEntity);
        /// <summary>
        /// 根据流程id获取流程信息
        /// </summary>
        /// <param name="flowId"></param>
        /// <returns></returns>
        WFFlowEntity GetWFTemplate(string flowId);
        /// <summary>
        /// 根据流程服务id获取各个版本流程信息
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        List<WFFlowEntity> GetFlowsByServiceId(string serviceId);
        /// <summary>
        /// 发布流程
        /// </summary>
        /// <param name="flowId"></param>
        CommonResult<int> ReleaseFlow(string flowId);

        /// <summary>
        /// 设置当前流程 
        /// </summary>
        /// <param name="flowId"></param>
        CommonResult<int> SetCurrentFow(string serviceId, string flowId);
    }
}
