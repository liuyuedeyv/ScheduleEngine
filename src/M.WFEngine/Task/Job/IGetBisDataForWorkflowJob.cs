using M.WorkFlow.Model;

namespace M.WFEngine.Task.Job
{
    /// <summary>
    /// 获取流程相关的业务数据
    /// </summary>
    public interface IGetBisDataForWorkflowJob
    {
        /// <summary>
        /// 业务流程Id
        /// </summary>
        string ServiceId { get; }
        /// <summary>
        /// 获取业务相关的数据
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="task"></param>
        /// <param name="tinsEntity"></param>
        /// <returns>json格式</returns>
        string GetBisData(string dataId, WFTaskEntity task);
    }


    public interface IWorkFlowEvent
    {
        /// <summary>
        /// 流程事件发生
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="dataId"></param>
        /// <param name="eventType"></param>
        void PublishWorkflowEvent(string serviceId, string dataId, EWorkflowEvent eventType);
        /// <summary>
        /// 获取业务相关的数据
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="task"></param>
        /// <param name="tinsEntity"></param>
        /// <returns>json格式</returns>
        string GetBisData(string dataId, WFTaskEntity task);
    }

    public enum EWorkflowEvent
    {
        /// <summary>
        /// 流程开始
        /// </summary>
        Start = 101,
        /// <summary>
        /// 节点执行完成
        /// </summary>
        TaskRuned = 102,
        /// <summary>
        /// 流程结束
        /// </summary>
        End = 999
    }
}
