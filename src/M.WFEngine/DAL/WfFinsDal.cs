using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;

namespace M.WFEngine.Flow.DAL
{
    [Autowired]
    public class WfFinsDal
    {
        [Autowired] //              
        public DataAccess _da { get; set; }
        
        public WFFinsEntity Get(string id)
        {
            var col = this._da.Query(WFFinsEntity.TableCode)
                .FixField("*")
                .Where(TableFilter.New().Equals("ID", id))
                .Query<WFFinsEntity>();
            return col.Data.FirstOrDefault();
        }

        public string GetIdByDataIdAndFlowId(string dataid,string flowId)
        {
            string rtn = "";
            if (string.IsNullOrEmpty(dataid))
            {
                dataid = "none";//如果没有值则给个默认值,防止全表查询
            }
            var col = this._da.Query(WFFinsEntity.TableCode)
                .FixField("ID")
                .Where(TableFilter.New().Equals("DATAID", dataid).Equals("FlOWID",flowId))
                .Query<WFFinsEntity>();
            if(col.Data.Any())
            {
                rtn = col.Data.First().ID;
            }
            return rtn;
        }
    }
}
