using FD.Simple.DB;
using FD.Simple.Utils.Serialize;
using M.WFEngine.Service;
using M.WFEngine.Task.Job;
using M.WorkFlow.Model;
using System.Linq;
using System.Net.Http;

namespace M.WFEngine.Task
{
    public abstract class BaseTask : IBaseTask
    {
        public abstract DataAccess _DataAccess { get; set; }

        public abstract IJsonConverter _JsonConverter { get; set; }

        public abstract IWorkflowJobs _WFJobs { get; set; }

        public abstract ETaskType TaskType { get; }

        /// <summary>
        /// 执行具体任务
        /// </summary>
        public virtual bool RunTask(WFTaskEntity taskEntity, WFFinsEntity fins, WFTinsEntity tinsEntity, WFTEventEntity mqEntity)
        {
            System.Console.WriteLine($"{taskEntity.Type}节点{tinsEntity.Taskname}开始执行……");

            return true;
        }
        /// <summary>
        /// 获取业务数据
        /// </summary>
        public virtual string GetBisData(WFTaskEntity taskEntity, string dataId, string serviceId)
        {
            //var job = _WFJobs.GetBisDataForWorkflowJobs.Where(f => f.ServiceId == fins.ServiceId).FirstOrDefault();
            //if (null != job)
            //{
            //    var jsonDasta = job.GetBisData(fins.Dataid, task);
            //    WFTDataEntity dataEntity = new WFTDataEntity();
            //    dataEntity.State = EDBEntityState.Added;
            //    dataEntity.Flowid = fins.Flowid;
            //    dataEntity.Finsid = fins.ID;
            //    dataEntity.Taskid = task.ID;
            //    //dataEntity.Tinsid = tinsEntity.ID;
            //    dataEntity.Cdate = System.DateTime.Now;
            //    dataEntity.JsonData = jsonDasta;
            //    _DataAccess.Update(dataEntity);
            //}

            var info = new FlowInfo()
            {
                EventType = "1",
                DataId = dataId,
                ServiceId = serviceId
            };
            HttpClient client = new HttpClient();
            HttpContent httpContent = new StringContent(_JsonConverter.Serialize(info), System.Text.Encoding.UTF8, "application/json");
            var jsonData = client.PostAsync("http://localhost:5004/acsa/registerwf", httpContent).GetAwaiter().GetResult().Content.ReadAsStringAsync().Result;

            WFTDataEntity dataEntity = new WFTDataEntity();
            dataEntity.State = EDBEntityState.Added;
            dataEntity.Flowid = taskEntity.Flowid;
            //dataEntity.Finsid = fins.ID;
            dataEntity.Taskid = taskEntity.ID;
            //dataEntity.Tinsid = tinsEntity.ID;
            dataEntity.Cdate = System.DateTime.Now;
            dataEntity.JsonData = jsonData;
            _DataAccess.Update(dataEntity);

            return jsonData;
        }

        /// <summary>
        /// 初始任务节点程实例信息
        /// </summary>
        public virtual WFTinsEntity Init(WFFinsEntity fins, WFTaskEntity taskEntity)
        {
            WFTinsEntity tins = new WFTinsEntity();
            tins.Add();
            tins.Finsid = fins.ID;
            tins.Flowid = taskEntity.Flowid;
            tins.Taskid = taskEntity.ID;
            tins.Taskname = taskEntity.Name;
            tins.Sdate = System.DateTime.Now;
            if (taskEntity.Type == ETaskType.End || taskEntity.Type == ETaskType.Start)
            {
                tins.Edate = System.DateTime.Now;
            }
            _DataAccess.Update(tins);

            return tins;
        }
        /// <summary>
        /// 流转到下一步
        /// </summary>
        public virtual void CreateJob(WFFinsEntity fins, WFTinsEntity tinsEntity, bool needWaitCallback)
        {
            //插入MQ，流程监控启动
            WFTEventEntity nextEntity = new WFTEventEntity();
            nextEntity.Add();
            nextEntity.Flowid = tinsEntity.Flowid;
            nextEntity.Finsid = tinsEntity.Finsid;
            nextEntity.Taskid = tinsEntity.Taskid;
            nextEntity.Dataid = fins.Dataid;
            nextEntity.Tinsid = tinsEntity.ID;
            nextEntity.Status = 0;
            nextEntity.Waitcallback = needWaitCallback ? 1 : 0;
            nextEntity.Cdate = System.DateTime.Now;

            _DataAccess.Update(nextEntity);
        }
    }
}
