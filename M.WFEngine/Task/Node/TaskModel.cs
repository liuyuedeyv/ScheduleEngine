using System;
using System.Collections.Generic;
using System.Text;
using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;

namespace M.WorkFlow.Engine.Task
{
    /// <summary>
    /// 决策节点
    /// </summary>
    [Autowired]
    public class TaskModel : BaseTask
    {
        [Autowired]
        public override DataAccess _DataAccess { get; set; }
        public override ETaskType TaskType => ETaskType.Model;

        public override bool RunTask(WFFinsEntity fins, WFTinsEntity tinsEntity, WFMQEntity mqEntity)
        {
            Console.WriteLine($"决策节点{tinsEntity.Taskname}开始执行……");
            //调取远方模型进行预算，返回结果
            return true;
        }
    }
}
