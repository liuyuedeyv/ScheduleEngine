using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;

namespace M.WFEngine.Task.DAL
{
    [Autowired]
    public class WfTinsDal
    {
        [Autowired] //              
        private DataAccess _da { get; set; }
         
        public string GetIdByFinsIdAndTaskId(string finsId,string taskId)
        {
            string rtn = "";
            if (string.IsNullOrEmpty(finsId))
            {
                finsId = "none";//防全表查询用
            }
            var col = this._da.Query(WFTDataEntity.TableCode)
                .FixField("ID")
                .Where(TableFilter.New().Equals("FinsId", finsId).Equals("TaskId",taskId))
                .Sort("SDATE DESC")
                .Query<WFTDataEntity>();
            if (col.Data.Any())
            {
                rtn = col.Data.First().ID;
            }
            return rtn;
        }
    }
}
