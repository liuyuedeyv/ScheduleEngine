using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;
using System.Collections.Generic;


namespace M.WFDesigner.Model
{
    public partial class ServicesAndFlowsEntity
    {
        public string name { get; set; }
        public List<WFFlowEntity> flows { get; set; }

    }
}
