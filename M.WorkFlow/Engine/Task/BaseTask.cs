using FD.Simple.DB;
using M.WorkFlow.Model;

namespace M.WorkFlow.Engine
{
    public interface IBaseTask
    {

    }
    public abstract class BaseTask : IBaseTask
    {
        public abstract DataAccess _DataAccess { get; set; }

        public abstract ETaskType TaskType { get; }
        /// <summary>
        /// 执行具体任务
        /// </summary>
        public abstract bool RunTask(WFFinsEntity fins, WFTinsEntity tinsEntity);
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
            WFMQEntity nextEntity = new WFMQEntity();
            nextEntity.Add();
            nextEntity.Flowid = tinsEntity.Flowid;
            nextEntity.Finsid = tinsEntity.Finsid;
            nextEntity.Taskid = tinsEntity.Taskid;
            nextEntity.Tinsid = tinsEntity.ID;
            nextEntity.Status = 0;
            nextEntity.Waitcallback = needWaitCallback ? 1 : 0;
            nextEntity.Cdate = System.DateTime.Now;

            _DataAccess.Update(nextEntity);
        }
    }
}
