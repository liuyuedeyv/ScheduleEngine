using FD.Simple.Utils.Agent;
using System;
using System.Collections.Generic;
using System.Text;

namespace M.ACSA.ResponseService.Custom
{
    [Autowired]
    public class WFService3 : BaseWfServiceMessage
    {
        public override EResponseMessageType AccessMessageType => EResponseMessageType.GetVariable;
        public override string ObjectDisname => $"获取变量计算信息";
        public override string ObjectCode => "acsa_003";

        public override string WFServcieId
        {
            get { return "00001PF1OUJQJ0000A01"; }
        }

        /// <summary>
        /// 决策节点获取业务数据的方法
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override string AnswerMessage(MessageModel info)
        {
            //增加自己逻辑，此处返回简单逻辑即可

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("{@Exchange}", "e1");
            // 增加其他变量

            return _JsonConverter.Serialize(dic);
        }

    }
}
