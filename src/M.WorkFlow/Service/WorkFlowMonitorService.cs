using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Provider;
using M.WFDesigner.Repository;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;
using M.WFDesigner.Model;

namespace M.WFDesigner.Service
{
    public class WorkFlowMonitorService : BaseFoo
    {

        IWorkFlowMonitor _workFlowMonitor;
        public WorkFlowMonitorService(IWorkFlowMonitor workFlowMonitor)
        {
            _workFlowMonitor = workFlowMonitor;
        }
        /// <summary>
        /// 读取正在运行的流程信息
        /// </summary>
        /// <param name="pageEntity"></param>
        /// <returns></returns>
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

        #region 读取流程实例信息

        [Routing(EHttpMethod.All, "wfm/getwfinsexecinfo")]
        public CommonResult<WFFinsEntity> GetWfInsExecInfo(string finsId)
        {
            if (string.IsNullOrWhiteSpace(finsId))
            {
                return new WarnResult("finsId 不能为空");
            }


            return _workFlowMonitor.GetWFFinsExecInfo(finsId); 
        }


        #endregion

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
