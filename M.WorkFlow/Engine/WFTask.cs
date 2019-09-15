using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
        WFTaskEntity[] GetNextTasks(WFTaskEntity preTask);
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

    [Autowired]
    public class WFTask : IWFTask
    {
        [Autowired]
        public DataAccess DataAccess { get; set; }

        [Autowired]
        public System.Collections.Generic.IEnumerable<IBaseTask> Tasks { get; set; }



        public WFTinsEntity CreateTaskIns(WFFinsEntity fins, WFTaskEntity taskEntity)
        {
            var taskSetting = GetTaskInfo(taskEntity);
            var tins = taskSetting.Init(fins, taskEntity);
            return tins;
        }
        public WFTaskEntity[] GetNextTasks(WFTaskEntity preTask)
        {
            //TODO:判断如果并行节点返回多个后续节点，如果是普通节点则返回一个
            var filter = TableFilter.New().Equals("BEGINTASKID", preTask.ID);
            var links = DataAccess.Query(WFLinkEntity.TableCode)
                 .FixField("*").Where(filter).QueryList<WFLinkEntity>();

            if (links.Count() == 1)
            {
                filter = TableFilter.New().Equals("ID", string.Join(',', links.Select(l => l.Endtaskid).ToArray()));
            }
            else
            {
                filter = TableFilter.New().In("ID", string.Join(',', links.Select(l => l.Endtaskid).ToArray()));
            }
            var tasks = DataAccess.Query(WFTaskEntity.TableCode).FixField("*")
                 .Where(filter).QueryList<WFTaskEntity>();
            return tasks.ToArray();
        }

        public WFTaskEntity[] GetPreTasks(WFTaskEntity nextTask)
        {
            var filter = TableFilter.New().Equals("ENDTASKID", nextTask.ID);
            var links = DataAccess.Query(WFLinkEntity.TableCode)
                 .FixField("*").Where(filter).QueryList<WFLinkEntity>();

            filter = TableFilter.New().In("ID", string.Join(',', links.Select(l => l.Begintaskid).ToArray()));
            var tasks = DataAccess.Query(WFTaskEntity.TableCode).FixField("*")
                 .Where(filter).QueryList<WFTaskEntity>();
            return tasks.ToArray();
        }

        public WFTaskEntity GetStartTask(string flowId)
        {
            var filter = TableFilter.New().Equals("flowid", flowId).Equals("type", 1);
            var taskEntity = DataAccess.Query(WFTaskEntity.TableCode)
                .FixField("*").Where(filter).QueryFirst<WFTaskEntity>();
            return taskEntity;
        }

        public WFTaskEntity GetTaskById(string taskId)
        {
            var filter = TableFilter.New().Equals("id", taskId);
            var taskEntity = DataAccess.Query(WFTaskEntity.TableCode)
                .FixField("*").Where(filter).QueryFirst<WFTaskEntity>();
            return taskEntity;
        }

        public BaseTask GetTaskInfo(WFTaskEntity taskEntity)
        {
            ETaskType taskType = taskEntity.Type;
            var taskInfo = Tasks.Where(t => (t as BaseTask).TaskType == taskType).FirstOrDefault() as BaseTask;
            if (taskInfo == null)
            {
                throw new Exception($"类型{taskType}没有实现");
            }
            return taskInfo;
        }

        public WFTinsEntity GetTinsById(string tinsId)
        {
            var filter = TableFilter.New().Equals("id", tinsId);
            var tinsEntity = DataAccess.Query(WFTinsEntity.TableCode)
                .FixField("*").Where(filter).QueryFirst<WFTinsEntity>();
            return tinsEntity;
        }

        public IEnumerable<WFTinsEntity> GetTinssById(string finsId, string[] taskIds)
        {
            var filter = TableFilter.New().Equals("FINSID", finsId).In("taskid", string.Join(',', taskIds));
            var tinsEntitys = DataAccess.Query(WFTinsEntity.TableCode)
                .FixField("*").Where(filter).QueryList<WFTinsEntity>();
            return tinsEntitys;
        }
    }
}
