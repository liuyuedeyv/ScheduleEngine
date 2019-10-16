using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using System;
using System.Collections.Generic;
using System.Text;

namespace M.ACSA.ResponseService
{
    [Autowired]
    public abstract class BaseWfServiceMessage : IWfServiceMessage
    {
        [Autowired]
        public IJsonConverter _JsonConverter { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public abstract EResponseMessageType AccessMessageType { get; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public abstract string ObjectDisname { get; }
        /// <summary>
        /// 对象编码，不能重复
        /// </summary>
        public abstract string ObjectCode { get; }
        /// <summary>
        /// 流程服务ID
        /// </summary>
        public virtual string WFServcieId { get; }


        public virtual string AnswerMessage(MessageModel info)
        {
            return _JsonConverter.Serialize(info);
        }
    }
}
