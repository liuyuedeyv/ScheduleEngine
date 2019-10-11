using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace M.WorkFlow.Model
{
    public partial class WFFlowEntity : DBEntity, IFooParameter
    {
        /// <summary>
        /// 工作流连线
        /// </summary>
        [DataMember]
        public List<WFLinkEntity> Links { get; set; }

        /// <summary>
        /// 工作流任务节点
        /// </summary>
        [DataMember]
        public List<WFTaskEntity> Tasks { get; set; }

        /// <summary>
        /// 当前流程
        /// </summary>
        [DataMember]
        public int CurrentTag { get; set; }
    }
}
