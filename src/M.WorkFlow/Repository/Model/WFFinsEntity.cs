using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FD.Simple.DB;
namespace M.WorkFlow.Model
{
    /// <summary>
    ///  流程实例扩展信息
    /// </summary>
    public partial class WFFinsEntity
    {
        /// <summary>
        /// 流程信息 绘制流程用
        /// </summary>
        [DataMember]
        public WFFlowEntity WfFlow { get; set; }
        /// <summary>
        /// 流程执行连线实例
        /// </summary>
        [DataMember]
        public List<WFLinkEntity> LinkIns { get; set; }

        /// <summary>
        /// 流程执行任务节点信息
        /// </summary>
        [DataMember]
        public List<WFTinsEntity> TaskIns { get; set; }

    }
}//end

