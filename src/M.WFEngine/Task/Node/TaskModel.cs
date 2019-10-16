using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.WFEngine.AccessService;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace M.WFEngine.Task
{
    /// <summary>
    /// 决策节点
    /// </summary>
    [Autowired]
    public class TaskModel : BaseTask
    {
        IJsonConverter _jsonConverter;
        public TaskModel(IJsonConverter jsonConverter)
        {
            _jsonConverter = jsonConverter;
        }

        public override ETaskType TaskType => ETaskType.Model;
        public override string GetBisData(WFTaskEntity taskEntity, string dataId, string serviceId)
        {
            //从tasksetting获取配置的远程对象代码，调取远方模型进行预算，返回结果
            string jsonData = "{}";
            if (!string.IsNullOrWhiteSpace(taskEntity.Setting))
            {
                var settingModel = _jsonConverter.Deserialize<ObjectSettingModel>(taskEntity.Setting);
                if (settingModel != null)
                {
                    jsonData = _IAppAccessService.GetModelTaskBisdata(serviceId, dataId, settingModel.ObjectCode).GetAwaiter().GetResult();

                }
                WFTDataEntity dataEntity = new WFTDataEntity();
                dataEntity.State = EDBEntityState.Added;
                dataEntity.Flowid = taskEntity.Flowid;
                dataEntity.Finsid = "";
                dataEntity.Taskid = taskEntity.ID;
                dataEntity.Tinsid = "";
                dataEntity.Cdate = System.DateTime.Now;
                dataEntity.JsonData = jsonData;
                _DataAccess.Update(dataEntity);
            }
            return jsonData;
        }
    }

    public class ObjectSettingModel
    {
        public string ObjectCode { get; set; }
    }
}
