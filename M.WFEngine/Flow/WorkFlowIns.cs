using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;
using System;

namespace M.WFEngine.Flow
{
    [Autowired]
    public class WorkFlowIns : IWorkFlowIns
    {
        [Autowired]
        public DataAccess _DataAccess { get; set; }

        public WFFinsEntity CreatFlowInstance(string serviceId, string flowId, string dataId)
        {
            WFFinsEntity fins = new WFFinsEntity();
            fins.Add();
            fins.ServiceId = serviceId;
            fins.Flowid = flowId;
            fins.Dataid = dataId;
            fins.Name = dataId;
            fins.Cdate = DateTime.Now;
            fins.Status = (int)EFlowStatus.Starting;
            _DataAccess.Update(fins);
            return fins;
        }


        public WFFinsEntity GetFlowInstance(string finsId)
        {
            var filter = TableFilter.New().Equals("ID", finsId);
            return _DataAccess.Query(WFFinsEntity.TableCode).FixField("*")
                .Where(filter).QueryFirst<WFFinsEntity>();
        }
    }
}
