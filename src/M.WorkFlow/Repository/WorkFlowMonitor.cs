using FD.Simple.DB;
using FD.Simple.DB.Model;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;

namespace M.WFDesigner.Repository
{
    [Autowired]
    public class WorkFlowMonitor : IWorkFlowMonitor
    {
        DataAccess _dataAccess = null;
        IDBContext _db = null;

        public WorkFlowMonitor(DataAccess dataAccess, IDBContext db)
        {
            this._dataAccess = dataAccess;
            this._db = db;
        }

        public DBCollection<WFFinsEntity> GetRuningData(WFMonitorSearchModel searchModel)
        {
            TableFilter filter = TableFilter.New().Equals("status", 1);
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

        private TableFilter BuildFilter(TableFilter filter, WFMonitorSearchModel searchModel)
        {
            if (!string.IsNullOrWhiteSpace(searchModel.DataId))
            {
                filter.Equals("dataid", searchModel.DataId);
            }
            else if (!string.IsNullOrWhiteSpace(searchModel.ServiceId))
            {
                filter.Equals("serviceid", searchModel.ServiceId);
            }
            return filter;
        }

        private DBCollection<WFFinsEntity> GetRuningData(PageEntity searchModel, TableFilter filter)
        {
            return _dataAccess.Query("wffins").FixField("*").Paging(searchModel.PageIndex, searchModel.PageSize).Where(filter).Query<WFFinsEntity>();
        }
    }
}
