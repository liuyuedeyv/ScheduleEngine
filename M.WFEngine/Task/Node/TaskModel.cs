using System;
using System.Collections.Generic;
using System.Text;
using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Engine.Job;
using M.WorkFlow.Model;
using System.Linq;
using FD.Simple.Utils.Serialize;

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
        [Autowired]
        public IJsonConverter _JsonConverter { get; set; }
        [Autowired]
        public IEnumerable<IJob> Jobs { get; set; }
        public override ETaskType TaskType => ETaskType.Model;


        public override bool RunTask(WFFinsEntity fins, WFTinsEntity tinsEntity, WFTEventEntity mqEntity)
        {
            Console.WriteLine($"决策节点{tinsEntity.Taskname}开始执行……");
            //调取远方模型进行预算，返回结果

            var job = Jobs.Where(j => j.TaskType == this.TaskType).FirstOrDefault();
            //job.Exe("");

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("dataid", fins.Dataid);
            WFTDataEntity dataEntity = new WFTDataEntity();
            dataEntity.State = EDBEntityState.Added;
            dataEntity.Flowid = fins.Flowid;
            dataEntity.Finsid = fins.ID;
            dataEntity.Taskid = tinsEntity.Taskid;
            dataEntity.Tinsid = tinsEntity.ID;
            dataEntity.Cdate = System.DateTime.Now;
            dataEntity.JsonData = _JsonConverter.Serialize(data);
            _DataAccess.Update(dataEntity);

            return true;
        }
    }
}
