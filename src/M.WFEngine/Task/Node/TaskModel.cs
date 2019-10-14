using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.WFEngine.Task.Job;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M.WFEngine.Task
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
        public override IJsonConverter _JsonConverter { get; set; }
        [Autowired]
        public override IWorkflowJobs _WFJobs { get; set; }

        public override ETaskType TaskType => ETaskType.Model;


        public override string GetBisData(WFTaskEntity taskEntity, string dataId, string serviceId)
        {
            //调取远方模型进行预算，返回结果

            var job = _WFJobs.Jobs.Where(j => j.TaskType == this.TaskType).FirstOrDefault();
            //job.Exe("");

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("dataid", dataId);

            var jsonData = _JsonConverter.Serialize(data);
            WFTDataEntity dataEntity = new WFTDataEntity();
            dataEntity.State = EDBEntityState.Added;
            dataEntity.Flowid = taskEntity.Flowid;
            dataEntity.Finsid = "";
            dataEntity.Taskid = taskEntity.ID;
            dataEntity.Tinsid = "";
            dataEntity.Cdate = System.DateTime.Now;
            dataEntity.JsonData = jsonData;
            _DataAccess.Update(dataEntity);

            return jsonData;
        }
    }
}
