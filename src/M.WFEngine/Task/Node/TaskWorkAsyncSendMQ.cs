using FD.Component.RabbitMQ;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;

namespace M.WFEngine.Task
{
    //[Autowired]
    public class TaskWorkAsyncSendMQ : BaseTask
    {
        /// <summary>
        /// 信道服务
        /// </summary>
        [Autowired]
        public IChannelService _ChannelService { get; set; }

        public override ETaskType TaskType => ETaskType.WorkAsyncSendMQ;

        public override bool NeedWaitCallbackWhenCreateJob => true;

        public override bool RunTask(WFTaskEntity taskEntity, WFFinsEntity fins, WFTinsEntity tinsEntity, WFTEventEntity enventEntity)
        {
            //TODO:发送数据到MQ
            // 根据tasksetting 节点的配置信息读取要发送到的远端地址
            //可以是多个地址
            if (!string.IsNullOrWhiteSpace(taskEntity.Setting))
            {
                var obj = _JsonConverter.Deserialize<SendMQModel>(taskEntity.Setting);
                if (obj != null)
                {
                    var dicPostdata = new Dictionary<string, string>();
                    dicPostdata.Add("callbackTag", enventEntity.ID);
                    dicPostdata.Add("customTag", obj.CustomTag);
                    dicPostdata.Add("dataId", enventEntity.Dataid);
                    byte[] body = System.Text.Encoding.UTF8.GetBytes(_JsonConverter.Serialize(dicPostdata));
                    _ChannelService.SendMsg(model =>
                    {
                        model.BasicPublish(obj.Exchange, obj.RoutingKey ?? "", body, "");
                    }, obj.ServerName);
                }
            }
            return false;
        }
    }
    public class SendMQModel
    {
        /// <summary>
        /// MQ server地址
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// Exchange名称
        /// </summary>
        public string Exchange { get; set; }
        /// <summary>
        /// Routingkey
        /// </summary>
        public string RoutingKey { get; set; }
        /// <summary>
        /// 自定义参数
        /// </summary>
        public string CustomTag { get; set; }
    }
}
