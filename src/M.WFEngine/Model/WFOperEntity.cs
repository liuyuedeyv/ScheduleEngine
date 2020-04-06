using System;
using System.Collections.Generic;
using System.Text;

namespace M.WFEngine.Model
{
    public class WFOperEntity
    {
        public string Id { get; set; }

        public string FlowId { get; set; }

        public string TaskId { get; set; }

        public EWFOperType OperType { get; set; }

        public string Operator { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string OperDis { get; set; }
    }

    /// <summary>
    /// 处理者类型
    /// </summary>
    public enum EWFOperType
    {
        /// <summary>
        /// 指定人员
        /// </summary>
        Person = 1
    }
}