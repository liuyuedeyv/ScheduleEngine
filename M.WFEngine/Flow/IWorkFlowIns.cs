using M.WorkFlow.Model;

namespace M.WFEngine.Flow
{
    public interface IWorkFlowIns
    {
        /// <summary>
        /// 创建流程实例
        /// </summary>
        /// <param name="flowId"></param>
        /// <param name="dataId"></param>
        /// <returns></returns>
        WFFinsEntity CreatFlowInstance(string serviceId, string flowId, string dataId);
        /// <summary>
        /// 获取流程实例信息
        /// </summary>
        /// <param name="finsId"></param>
        /// <returns></returns>
        WFFinsEntity GetFlowInstance(string finsId);
    }
}
