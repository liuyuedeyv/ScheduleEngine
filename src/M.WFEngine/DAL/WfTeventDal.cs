using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;

namespace M.WFEngine.DAL
{
    [Autowired]
    public class WfTEventDal
    {
        [Autowired] //              
        private DataAccess _da { get; set; }

        public WFTEventEntity Get(string id)
        {
            var col = this._da.Query(WFTEventEntity.TableCode)
                .FixField("*")
                .Where(TableFilter.New().Equals("ID",id))
                .Query<WFTEventEntity>();
            return col.Data.FirstOrDefault();
        }
    }
}
