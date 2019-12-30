using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Provider;
using M.WFDesigner.Repository;
using M.WorkFlow.Model;
using System;

namespace M.WFDesigner.Service
{
    public class WorkFlowMonitorService : BaseFoo
    {

        IWorkFlowMonitor _workFlowMonitor;
        public WorkFlowMonitorService(IWorkFlowMonitor workFlowMonitor)
        {
            _workFlowMonitor = workFlowMonitor;
        }

        [Routing(EHttpMethod.HttpPost, "wfm/getruningdata")]
        public CommonResult<DBCollection<WFFinsEntity>> GetFlowsByServiceIdo(WFMonitorSearchModel pageEntity)
        {
            if (pageEntity == null)
            {
                pageEntity = new WFMonitorSearchModel();
            }
            pageEntity.PageSize = Math.Min(pageEntity.PageSize, 100);

            return _workFlowMonitor.GetRuningData(pageEntity);
        }

        [Routing(EHttpMethod.HttpGet, "wfm/getwaitcallbackdata")]
        public CommonResult<DBCollection<WFTEventEntity>> GetWaitCallbackData(string finsId)
        {
            if (string.IsNullOrWhiteSpace(finsId))
            {
                return new WarnResult("finsId 不能为空");
            }

            return _workFlowMonitor.GetWaitcallbackData(finsId);
        }
    }
}
