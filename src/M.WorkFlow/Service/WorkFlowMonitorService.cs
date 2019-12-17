using FD.Simple.DB;
using FD.Simple.DB.Model;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Provider;
using M.WFDesigner.Repository;
using M.WorkFlow.Model;
using System.Collections.Generic;

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
            if (string.IsNullOrWhiteSpace(pageEntity.ServiceId))
            {
                return _workFlowMonitor.GetRuningData(pageEntity);
            }
            else
            {
                return _workFlowMonitor.GetRuningDataByService(pageEntity);
            }
        }
    }
}
