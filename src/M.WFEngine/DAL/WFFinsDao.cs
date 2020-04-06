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
    [Table("wffins")]
    public interface IWFFinsDao
    {
        [Update("status,edate")]
        int Reject(WFFinsEntity entity);
    }


    public class WFFinsDao : IWFFinsDao
    {
        public int Reject(WFFinsEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
