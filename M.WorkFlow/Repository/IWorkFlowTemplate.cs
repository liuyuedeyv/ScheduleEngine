using M.WorkFlow.Model;
using System.Collections.Generic;

namespace M.WorkFlow.Repository
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
        /// 根据业务模板id获取流程信息
        /// </summary>
        /// <param name="bisId"></param>
        /// <returns></returns>
        List<WFFlowEntity> GetFlowsByBisTemplate(string bisId);
    }
}
