using M.WorkFlow.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace M.WorkFlow.Engine
{
    public interface IWorkFlow
    {
        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="flowId"></param>
        /// <param name="dataId"></param>
        /// <returns></returns>
        int Start(string flowId, string dataId);
        /// <summary>
        /// 执行任务的回调
        /// </summary>
        /// <param name="mqId"></param>
        /// <returns></returns>
        int Callback(string mqId);
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
        /// <summary>
        /// 处理后台任务事件
        /// </summary>
        /// <param name="batchCount">批次预期数量</param>
        /// <returns>实际取到可执行任务数量</returns>
        int ProcessWftEvent(uint batchCount);
    }
}
