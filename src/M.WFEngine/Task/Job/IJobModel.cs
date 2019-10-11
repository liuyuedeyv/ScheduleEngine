using System;
using System.Collections.Generic;
using System.Text;

namespace M.WorkFlow.Engine.Job
{
    /// <summary>
    /// 不同的任务节点需要执行的不同的任务
    /// </summary>
    public interface IJobModel
    {
        ETaskType TaskType { get; }
        /// <summary>
        /// 获取模型数据
        /// </summary>
        /// <param name="dataId"></param>
        /// <returns></returns>
        Dictionary<string, object> GetModelData(string dataId);
    }

    public enum EJobModelType
    {
        /// <summary>
        /// 业务模板
        /// </summary>
        BisWork = 1,
        /// <summary>
        /// 具体流程
        /// </summary>
        WorkFlow = 2,
        /// <summary>
        /// 流程节点
        /// </summary>
        ModelNode = 3
    }
}
