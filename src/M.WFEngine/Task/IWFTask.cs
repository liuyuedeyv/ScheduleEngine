using M.WorkFlow.Engine.Task;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace M.WorkFlow.Engine
{
    public interface IWFTask
    {
        /// <summary>
        /// 获取开始节点信息
        /// </summary>
        /// <param name="flowId"></param>
        /// <returns></returns>
        WFTaskEntity GetStartTask(string flowId);

        /// <summary>
        /// 获取下一个执行任务，如果是并行任务返回多个
        /// </summary>
        WFTaskEntity[] GetNextTasks(WFTaskEntity preTask, WFTinsEntity tinsEntity);
        /// <summary>
        /// 获取下一个执行任务，如果是并行任务返回多个
        /// </summary>
        WFTaskEntity[] GetPreTasks(WFTaskEntity nextTask);
        /// <summary>
        /// 创建任务节点实例信息
        /// </summary>
        /// <returns></returns>
        WFTinsEntity CreateTaskIns(WFFinsEntity fins, WFTaskEntity taskEntity);
        /// <summary>
        /// 获取节点配置信息
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <returns></returns>
        BaseTask GetTaskInfo(WFTaskEntity taskEntity);
        /// <summary>
        /// 根据id获取任务节点信息
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        WFTaskEntity GetTaskById(string taskId);
        /// <summary>
        /// 根据id获取流程实例信息
        /// </summary>
        /// <param name="tinsId"></param>
        /// <returns></returns>
        WFTinsEntity GetTinsById(string tinsId);

        /// <summary>
        /// 获取某个流程中多个任务节点实例信息
        /// </summary>
        /// <param name="finsId"></param>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        IEnumerable<WFTinsEntity> GetTinssById(string finsId, string[] taskIds);
    }
}
