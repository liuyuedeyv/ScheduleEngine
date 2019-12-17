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

        public DBCollection<WFFinsEntity> GetRuningData(PageEntity searchModel)
        {
            var filter = TableFilter.New().Equals("status", 1);
            var coll = GetRuningData(searchModel, filter);
            return coll;
        }

        public DBCollection<WFFinsEntity> GetRuningDataByService(WFMonitorSearchModel searchModel)
        {
            var filter = TableFilter.New().Equals("status", 1).Equals("serviceid", searchModel.ServiceId);
            var coll = GetRuningData(searchModel, filter);
            return coll;
        }

        private DBCollection<WFFinsEntity> GetRuningData(PageEntity searchModel, TableFilter filter)
        {
            return _dataAccess.Query("wffins").FixField("*").Paging(searchModel.PageIndex, searchModel.PageSize).Where(filter).Query<WFFinsEntity>();
        }
    }
}
