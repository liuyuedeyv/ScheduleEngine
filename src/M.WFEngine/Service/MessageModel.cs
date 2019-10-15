using System;
using System.Collections.Generic;
using System.Text;

namespace SendMsgDemo
{
    public class MessageModel
    {
        public string ServiceId { get; set; }
        public string DataId { get; set; }
        public EMsgType MsgType { get; set; }
    }

    public enum EMsgType
    {
        /// <summary>
        /// 获取流程业务数据
        /// </summary>
        GetWorkflowServiceBisdata,
        /// <summary>
        /// 获取决策节点业务数据
        /// </summary>
        GetModelTaskBisdata
    }
}
