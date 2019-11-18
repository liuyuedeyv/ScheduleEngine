using System.Threading.Tasks;
namespace M.WFEngine.AccessService
{
    /// <summary>
    /// 应用接入服务
    /// </summary>
    public interface IAppAccessService
    {
        //string GetWorkflowServiceDataTemplate(string sreviceId);
        //Task<string> GetModelTaskDataTemplate(string sreviceId);

        /// <summary>
        /// 获取流程相关联的业务数据
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="dataId"></param>
        /// <returns></returns>
        Task<string> GetWorkflowServiceBisdata(string serviceId, string dataId);

        /// <summary>
        /// 获取决策节点相关联的业务数据
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="dataId"></param>
        /// <param name="objectCode">自定义对象的编码</param>
        /// <returns></returns>
        Task<string> GetModelTaskBisdata(string serviceId, string dataId, string objectCode);

        /// <summary>
        /// 获取变量相关联的业务数据
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="dataId"></param>
        /// <param name="objectCode">自定义对象的编码</param>
        /// <returns></returns>
        Task<string> GetVaribleTaskBisdata(string serviceId, string dataId, string varibles);

    }
}
