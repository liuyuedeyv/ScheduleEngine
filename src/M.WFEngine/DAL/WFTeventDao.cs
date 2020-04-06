using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FD.Component.DynamicDao;
using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WFEngine.Model;
using M.WorkFlow.Model;

namespace M.WFEngine.DAL
{
    [Table("wftevent")]
    public interface IWFTeventDao
    {
        [Select]
        WFTEventEntity SelectById(string id);

        [Update("waitcallback,callbackdate")]
        int UpdateCallback(WFTEventEntity entity);

        WFTEventEntity SelectByTinsId(string tinsId, string dataId);



    }

    [Autowired(typeof(DynamicDaoInterceptor))]
    public class WFTeventDao : IWFTeventDao
    {
        DataAccess _dataAccess = null;
        public WFTeventDao(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public WFTEventEntity SelectById(string id)
        {
            throw new NotImplementedException();
        }

        public WFTEventEntity SelectByTinsId(string tinsId, string dataId)
        {
            return _dataAccess.Query("wftevent").FixField("*")
                .Where(TableFilter.New().Equals("tinsid", tinsId).Equals("dataid", dataId))
                .QueryFirst<WFTEventEntity>();
        }

        public int UpdateCallback(WFTEventEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
