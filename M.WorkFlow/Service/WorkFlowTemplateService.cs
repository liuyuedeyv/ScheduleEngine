using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Provider;
using M.WorkFlow.Engine;
using M.WorkFlow.Model;
using M.WorkFlow.Repository;
using System.Linq;

namespace M.WorkFlow
{
    public class WorkFlowTemplateService : BaseFoo
    {
        [Autowired]
        public IWorkFlowTemplate WorkFlowTemplate { get; set; }

        [Autowired]
        public System.Collections.Generic.IEnumerable<IBaseTask> Tasks { get; set; }


        [Routing(EHttpMethod.HttpGet, "wft/getid")]
        public string GetOneId()
        {
            return FD.Simple.Utils.DataKeyFactory.NewId();
        }


        [Routing(EHttpMethod.HttpGet, "wft/getflowinfo")]
        public WFFlowEntity GetFlowInfo(string id)
        {

            if (string.IsNullOrWhiteSpace(id))
            {
                return new WFFlowEntity();
            }
            else
            {
                return this.WorkFlowTemplate.GetWFTemplate(id);
            }
        }

        [Routing(EHttpMethod.HttpPost, "wft/updateflowinfo")]
        public CommonResult<int> UpdateFlowInfo(WFFlowEntity flowEntity)
        {
            if (flowEntity == null)
            {
                return new WarnResult("更新数据不能为空");
            }
            else
            {
                return this.WorkFlowTemplate.UpdateTemplate(flowEntity);
            }
        }

        [Routing(EHttpMethod.HttpGet, "wft/task")]
        public CommonResult<int> Task()
        {
            

            return 1;
        }
    }
}
