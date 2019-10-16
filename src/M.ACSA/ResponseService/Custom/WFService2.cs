using FD.Simple.Utils.Agent;
using System;
using System.Collections.Generic;
using System.Text;

namespace M.ACSA.ResponseService.Custom
{
    [Autowired]
    public class WFService2 : BaseWfServiceMessage
    {
        public override EResponseMessageType AccessMessageType => EResponseMessageType.GetModelTaskBisdata;
        public override string ObjectDisname => $"获取是电话判断逻辑的信息";
        public override string ObjectCode => "acsa_002";

        /// <summary>
        /// 决策节点获取业务数据的方法
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override string AnswerMessage(MessageModel info)
        {
            //增加自己逻辑，此处返回简单逻辑即可
            return _JsonConverter.Serialize(info);
        }

    }
}
