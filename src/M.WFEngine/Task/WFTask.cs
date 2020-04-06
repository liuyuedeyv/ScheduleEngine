using FD.Simple.DB;
using FD.Simple.Utils;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.WFEngine.Util;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using M.WFEngine.Model;

namespace M.WFEngine.Task
{
    [Autowired]
    public class WFTask : IWFTask
    {
        [Autowired]
        public DataAccess DataAccess { get; set; }

        [Autowired]
        public IJsonConverter _JsonConverter { get; set; }

        [Autowired]
        public System.Collections.Generic.IEnumerable<IBaseTask> Tasks { get; set; }



        public WFTinsEntity CreateTaskIns(WFFinsEntity fins, WFTaskEntity taskEntity)
        {
            var taskSetting = GetTaskInfo(taskEntity);
            var tins = taskSetting.Init(fins, taskEntity);
            return tins;
        }
        public WFTaskEntity[] GetNextTasks(WFTaskEntity preTask, WFTinsEntity tinsEntity, string bisJsonData)
        {
            var filter = TableFilter.New().Equals("BEGINTASKID", preTask.ID);
            var links = DataAccess.Query(WFLinkEntity.TableCode)
                 .FixField("*").Where(filter).QueryList<WFLinkEntity>().ToList();

            if (preTask.Type != ETaskType.BingXing && links.Count() > 1)
            {
                var dataList = new List<WFJsonDataEntity>();
                dataList.Add(_JsonConverter.Deserialize<WFJsonDataEntity>(bisJsonData));
                foreach (var link in links.OrderByDescending(l => l.Priority))
                {
                    bool hitRule = false;
                    if (string.IsNullOrWhiteSpace(link.Filter))
                    {
                        hitRule = true;
                    }
                    else
                    {
                        var conditionFilter = _JsonConverter.Deserialize<TableFilter>(link.Filter);
                        hitRule = dataList.QueryDynamic(conditionFilter).Count() == 1;
                    }
                    if (hitRule)
                    {
                        links = new List<WFLinkEntity>();
                        links.Add(link);
                        break;
                    }
                }
            }

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

        public WFTaskEntity[] GetNextTasks(WFTaskEntity preTask, WFTinsEntity tinsEntity, string bisJsonData, WFCmdRunModel runModel)
        {
            //TOOD:增加处理名称来选择下一个节点
            return GetNextTasks(preTask, tinsEntity, bisJsonData);
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


            BaseTask taskInfo = null;

            foreach (var task in Tasks)
            {
                if ((task as BaseTask).TaskType == taskType)
                {
                    taskInfo = task as BaseTask;
                }
            }
            //Tasks.Where(t => (t as BaseTask).TaskType == taskType).FirstOrDefault() as BaseTask;
            if (taskInfo == null)
            {
                throw new Exception($"类型{taskType}没有实现");
            }
            return taskInfo;
        }

        public WFTaskSettingEntity GetTaskJobInfo(WFTaskEntity taskEntity)
        {
            var filter = TableFilter.New().Equals("id", taskEntity.ID);
            var settingEntity = DataAccess.Query(WFTaskSettingEntity.TableCode)
                .FixField("*").Where(filter).QueryFirst<WFTaskSettingEntity>();
            return settingEntity;
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

        public bool IsMultipleNextTask(WFTaskEntity preTask)
        {
            var filter = TableFilter.New().Equals("BEGINTASKID", preTask.ID);
            var links = DataAccess.Query(WFLinkEntity.TableCode)
                 .FixField("*").Where(filter).QueryList<WFLinkEntity>().ToList();
            return links.Count > 1;
        }
    }
}
