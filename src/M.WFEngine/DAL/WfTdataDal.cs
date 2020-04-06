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
    public class WfTdataDal
    {
        [Autowired] //              
        public DataAccess _da { get; set; }

        public void Add(string flowId,string finsid,string taskId,string tinsId,string jsonData)
        {
            TableFilter tf=TableFilter.New().Equals("FlowID",flowId).Equals("FinsId", finsid).Equals("TinsId", tinsId);
            //查重,
            var col = GetByFlowIdAndTaskId(tf);
            if (col.Data.Any(o => o.JsonData == jsonData))
            {
                return;//如果有记录则不再插入数据
            }
            WFTDataEntity dataEntity = new WFTDataEntity();
            dataEntity.State = EDBEntityState.Added;
            dataEntity.Flowid = flowId;
            dataEntity.Finsid = finsid;
            dataEntity.Taskid = taskId;
            dataEntity.Tinsid = tinsId;
            dataEntity.Cdate = System.DateTime.Now;
            dataEntity.JsonData = jsonData;
            this._da.Update(dataEntity); 
        }

        public DBCollection<WFTDataEntity> GetByFlowIdAndTaskId(TableFilter tf)
        {
            var col = this._da.Query(WFTDataEntity.TableCode)
                .FixField("*")
                .Where(tf)
                .Query<WFTDataEntity>();
            return col;
        }
    }
}
