using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FD.Component.DynamicDao;
using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WFEngine.Model;

namespace M.WFEngine.DAL
{
    [Table("wfoper")]
    public interface IWFOperDao
    {
        List<WFOperEntity> SelectByTaskId(string taskId);
    }

    [Autowired(typeof(DynamicDaoInterceptor))]
    public class WFOperDao : IWFOperDao
    {
        private DataAccess _dataAccess = null;

        public WFOperDao(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public List<WFOperEntity> SelectByTaskId(string taskId)
        {
            if (string.IsNullOrWhiteSpace(taskId))
            {
                return new List<WFOperEntity>();
            }
            var filter = TableFilter.New().Equals("taskid", taskId);
            return _dataAccess.Query("wfoper").FixField("*").Where(filter).QueryList<WFOperEntity>().ToList();
        }
    }
}
