using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.WFEngine.AccessService;
using M.WFEngine.Flow.DAL;
using M.WFEngine.Util;
using M.WorkFlow.Model;
using M.WFEngine.Task.DAL;

namespace M.WFEngine.Task
{
    [Autowired]
    public abstract class BaseTask : IBaseTask
    {
        [Autowired]
        public DataAccess _DataAccess { get; set; }
        [Autowired]
        public IJsonConverter _JsonConverter { get; set; }
        [Autowired]
        public IAppAccessService _IAppAccessService { get; set; }

        public abstract ETaskType TaskType { get; }

        [Autowired] //              
        public WfTdataDal _WfTdataDal { get; set; }

        [Autowired] //              
        public WfFinsDal _WfFinsDal { get; set; }

        [Autowired] //              
        public WfTinsDal _WftinsDal { get; set; }

        /// <summary>
        /// 执行具体任务
        /// </summary>
        public virtual bool RunTask(WFTaskEntity taskEntity, WFFinsEntity fins, WFTinsEntity tinsEntity, WFTEventEntity mqEntity)
        {
            //System.Console.WriteLine($"{taskEntity.Type}节点{tinsEntity.Taskname}开始执行……");

            return true;
        }
        /// <summary>
        /// 获取业务数据
        /// </summary>
        public virtual string GetBisData(WFTaskEntity taskEntity, string dataId, string serviceId, EAccessMessageType messageType)
        {
            //调取远方模型进行，返回结果
            string jsonData = string.Empty;
            //根据serviceId 和dataid 获取 finsid 和tinsid

            var finisid = this._WfFinsDal.GetIdByDataIdAndFlowId(dataId, taskEntity.Flowid);
            var tinsid = this._WftinsDal.GetIdByFinsIdAndTaskId(finisid, taskEntity.ID);
            
            if (messageType == EAccessMessageType.GetVariable)
            {
                jsonData = _IAppAccessService.GetVaribleTaskBisdata(serviceId, dataId, taskEntity.GetTemplateInfo()).GetAwaiter().GetResult();
            }
            else
            {
                jsonData = _IAppAccessService.GetWorkflowServiceBisdata(serviceId, dataId).GetAwaiter().GetResult();
            } 
            _WfTdataDal.Add(taskEntity.Flowid,finisid, taskEntity.ID,tinsid, jsonData); 
          
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
