using FD.Simple.Utils.Agent;
using System;
using System.Collections.Generic;
using System.Text;

namespace M.ACSA.ResponseService.Custom
{
    [Autowired]
    public class WFService1 : BaseWfServiceMessage
    {
        public override EResponseMessageType AccessMessageType => EResponseMessageType.GetWorkflowServiceBisdata;
        public override string ObjectDisname => $"业务流程{WFServcieId}通用获取业务数据";
        public override string ObjectCode => "acsa_001";

        public override string WFServcieId
        {
            get { return "00001PF1OUJQJ0000A01"; }
        }


        /// <summary>
        /// 流程通用的获取业务数据的方法
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override string AnswerMessage(MessageModel info)
        {
            //增加自己逻辑
            return _JsonConverter.Serialize(info);
        }

    }
}
