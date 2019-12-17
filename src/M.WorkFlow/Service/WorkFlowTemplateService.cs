using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Provider;
using M.WFDesigner.Model;
using M.WFDesigner.Repository;
using M.WorkFlow.Model;
using System.Collections.Generic;

namespace M.WFDesigner.Service
{
    public class WorkFlowTemplateService : BaseFoo
    {
        [Autowired]
        public IWorkFlowTemplate WorkFlowTemplate { get; set; }


        [Routing(EHttpMethod.HttpGet, "wft/getid")]
        public string GetOneId()
        {
            return FD.Simple.Utils.DataKeyFactory.NewId();
        }

        [Routing(EHttpMethod.HttpGet, "wft/getallflows")]
        public CommonResult<List<WFFlowEntity>> GetFlowsByServiceIdo(string serviceId)
        {
            if (string.IsNullOrWhiteSpace(serviceId))
            {
                return new WarnResult("serviceId is not null");
            }
            else
            {
                return this.WorkFlowTemplate.GetFlowsByServiceId(serviceId);
            }
        }

        [Routing(EHttpMethod.HttpPost, "wft/getallservcie")]
        public CommonResult<Dictionary<string, ServicesAndFlowsEntity>> GetAllServcies()
        {
            Dictionary<string, ServicesAndFlowsEntity> serviceMap = new Dictionary<string, ServicesAndFlowsEntity>();
            List<WFServiceEntity> services = this.WorkFlowTemplate.GetAllService();
            foreach (var service in services)
            {
                ServicesAndFlowsEntity sfEntity = new ServicesAndFlowsEntity();
                sfEntity.name = service.Name;
                sfEntity.flows = this.WorkFlowTemplate.GetFlowsByServiceId(service.ID);
                serviceMap.Add(service.ID, sfEntity);
            }
            return serviceMap;

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


        [Routing(EHttpMethod.HttpPost, "wft/releaseflow")]
        public CommonResult<int> ReleaseFlow(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return 0;
            }
            else
            {
                return this.WorkFlowTemplate.ReleaseFlow(id);
            }
        }

        [Routing(EHttpMethod.HttpPost, "wft/setCurrent")]
        public CommonResult<int> SetCurrentFlow(string serviceId, string flowId)
        {
            if (string.IsNullOrWhiteSpace(serviceId) || string.IsNullOrWhiteSpace(flowId))
            {
                return 0;
            }
            else
            {
                return this.WorkFlowTemplate.SetCurrentFow(serviceId, flowId);
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
