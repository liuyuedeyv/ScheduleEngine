using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;

namespace M.WFDesigner.Repository
{
    [Autowired]
    public class WorkFlowInstance:IWorkFlowInstance
    {

        [Autowired]
        public DataAccess DataAccess { get; set; }
        [Autowired]
        public IDBContext DBContext { get; set; }

        public List<WFTinsEntity> GetTinsList(string finsId)
        {
            var col = this.DataAccess.Query(WFTinsEntity.TableCode)
                .FixField("*")
                .Where(TableFilter.New().Equals("FINSID", finsId))
                .Query<WFTinsEntity>();
            return col.Data.ToList();
        }
 
    }
}
