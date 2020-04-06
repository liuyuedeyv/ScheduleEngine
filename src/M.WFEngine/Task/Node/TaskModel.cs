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
using M.WFEngine.Flow.DAL;
using M.WFEngine.Task.DAL;

namespace M.WFEngine.Task
{
    /// <summary>
    /// 决策节点
    /// </summary>
    //[Autowired]
    public class TaskModel : BaseTask
    {
        IJsonConverter _jsonConverter;
        public TaskModel(IJsonConverter jsonConverter)
        {
            _jsonConverter = jsonConverter;
        }

        [Autowired] //              
        private WfTdataDal _WfTdataDal { get; set; }

        [Autowired] //              
        private WfFinsDal _WfFinsDal { get; set; }

        [Autowired] //              
        private WfTinsDal _WftinsDal { get; set; }
        public override ETaskType TaskType => ETaskType.Model;
        public override string GetBisData(WFTaskEntity taskEntity, string dataId, string serviceId, EAccessMessageType messageType)
        {
            //从tasksetting获取配置的远程对象代码，调取远方模型进行预算，返回结果
            string jsonData = "{}";
            if (!string.IsNullOrWhiteSpace(taskEntity.Setting))
            {
                var finisid = this._WfFinsDal.GetIdByDataIdAndFlowId(dataId,taskEntity.Flowid);
                var tinsid = this._WftinsDal.GetIdByFinsIdAndTaskId(finisid, taskEntity.ID);

                var settingModel = _jsonConverter.Deserialize<ObjectSettingModel>(taskEntity.Setting);
                if (settingModel != null)
                {
                    jsonData = _IAppAccessService.GetModelTaskBisdata(serviceId, dataId, settingModel.ObjectCode).GetAwaiter().GetResult();

                }
                _WfTdataDal.Add(taskEntity.Flowid,finisid, taskEntity.ID,tinsid, jsonData);
               
            }
            return jsonData;
        }
    }

    public class ObjectSettingModel
    {
        public string ObjectCode { get; set; }
    }
}
