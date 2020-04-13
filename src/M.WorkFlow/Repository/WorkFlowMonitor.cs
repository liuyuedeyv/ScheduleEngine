using System;
using System.Collections.Generic;
using System.Linq;
using FD.Simple.DB;
using FD.Simple.DB.Model;
using FD.Simple.Utils.Agent;
using M.WFDesigner.RefData;
using M.WFDesigner.Repository.Contacts;
using M.WFEngine.Flow;
using M.WorkFlow.Model;

namespace M.WFDesigner.Repository
{
    [Autowired]
    public class WorkFlowMonitor : IWorkFlowMonitor
    {
        DataAccess _dataAccess = null;
        IDBContext _db = null;
        WFServiceTableRefdataRepository _wfServiceTableRefdata; 

        [Autowired] //              
        private IWorkFlowInstance _WorkFlowInstance { get; set; }

        [Autowired] //              
        private IWorkFlowTemplate _workFlowTemplate { get; set; }

        [Autowired] //              
        private IWftEventRepository _wftEventRepository { get; set; }

        [Autowired] //              
        private IWftEventMsgRepository _wftEventMsgRepository { get; set; }
        public WorkFlowMonitor(DataAccess dataAccess, IDBContext db,WFServiceTableRefdataRepository wfServiceTableRefdata)
        {
            this._dataAccess = dataAccess;
            this._db = db;
            this._wfServiceTableRefdata = wfServiceTableRefdata;
        }

        public DBCollection<WFFinsEntity> GetRuningData(WFMonitorSearchModel searchModel)
        {
            TableFilter filter = TableFilter.New();

            BuildFilter(filter, searchModel);
            var coll = GetRuningData(searchModel, filter);
            return coll;
        }

        public DBCollection<WFTEventEntity> GetWaitcallbackData(string finsId)
        {
            if (string.IsNullOrWhiteSpace(finsId))
            {
                return new DBCollection<WFTEventEntity>();
            }
            var filter = TableFilter.New().Equals("finsid", finsId).Equals("waitcallback", 1);
            return _dataAccess.Query("wftevent").FixField("id,taskid,tinsid,processdate").Where(filter).Query<WFTEventEntity>();
        }

        public WFFinsEntity GetWFFinsExecInfo(string finsId)
        {
            var col = this._dataAccess.Query(WFFinsEntity.TableCode)
                .FixField("*")
                .Where(TableFilter.New().Equals("ID", finsId))
                .Query<WFFinsEntity>();
            if (!col.Data.Any())
            {
                throw  new Exception($"{finsId}数据不存在");
            }
            var en = col.Data.FirstOrDefault();
            en.WfFlow = this._workFlowTemplate.GetWFTemplate(en.Flowid);
            en.TaskIns = _WorkFlowInstance.GetTinsList(finsId);

            TableFilter tf = TableFilter.New().Equals("FINSID", en.ID);
            var eventArr = this._wftEventRepository.GetList(tf);//查询异步回调信息 
            en.LinkIns = new List<WFLinkEntity>();
            for (int i = 0; i < en.TaskIns.Count; i++)
            {
                 
                WFTinsEntity startTIns = en.TaskIns[i];
                if (startTIns.Edate.ToString("yyyy-MM-dd") == "0001-01-01")//#如果没有结束时间
                {
                    //TODOWHP 待完善	on 2020-03-28 09:41:14
                    //eventArr.Data.All(o=>o.Tinsid=en.ID)
                } 
            }

            foreach (WFLinkEntity link in en.WfFlow.Links)
            {
                if (en.TaskIns.Any(o => o.Taskid == link.Begintaskid) && en.TaskIns.Any(o => o.Taskid == link.Endtaskid))
                {
                    en.LinkIns.Add(link);
                }
            }
           

           
            return en;
        }

        private TableFilter BuildFilter(TableFilter filter, WFMonitorSearchModel searchModel)
        {
            if (!string.IsNullOrWhiteSpace(searchModel.DataId))
            {
                filter.Equals("dataid", searchModel.DataId);
            }
            if (!string.IsNullOrWhiteSpace(searchModel.ServiceId))
            {
                filter.Equals("serviceid", searchModel.ServiceId);
            }
            if(!string.IsNullOrEmpty(searchModel.Name))
            {
                filter.LikeLeft("name", searchModel.Name);
            }
            if (searchModel.Status!=-999&&searchModel.Status!=null)
            {
                filter.Equals("status", searchModel.Status);
            }
            return filter;
        }

        private DBCollection<WFFinsEntity> GetRuningData(PageEntity searchModel, TableFilter filter)
        {
            DBCollection<WFFinsEntity> col = _dataAccess.Query("wffins").FixField("*")
                .Paging(searchModel.PageIndex, searchModel.PageSize)
                .Sort("CDATE DESC")
                .Where(filter).Query<WFFinsEntity>();
            foreach (WFFinsEntity en in col.Data)
            {
                en.RefData.Add("serviceidname", this._wfServiceTableRefdata.GetData(en.ServiceId).Name);
                en.RefData.Add("statusName", ((EFlowStatus)en.Status).ToString());
            }
            return col;
        }
    }
}
