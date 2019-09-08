using System.Linq;
using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;

namespace M.WorkFlow.Repository
{

    [Autowired]
    public class WorkFlowTemplate : IWorkFlowTemplate
    {
        [Autowired]
        public DataAccess DataAccess { get; set; }
        [Autowired]
        public IDBContext DBContext { get; set; }

        public List<WFFlowEntity> GetFlowsByBisTemplate(string bisId)
        {
            throw new NotImplementedException();
        }

        public WFFlowEntity GetWFTemplate(string flowId)
        {
            if (string.IsNullOrWhiteSpace(flowId))
            {
                throw new ArgumentException($"{nameof(flowId)} is not null");
            }
            var filter = TableFilter.New().Equals("id", flowId);
            var query = DataAccess.Query(WFFlowEntity.TableCode).FixField("id,name").Where(filter);
            var flowEntity = query.QueryFirst<WFFlowEntity>();
            if (flowEntity != null)
            {
                filter = TableFilter.New().Equals("flowid", flowId);
                flowEntity.Links = DataAccess.Query(WFLinkEntity.TableCode).FixField("id,begintaskid,endtaskid,memo").Where(filter).QueryList<WFLinkEntity>().ToList();
                flowEntity.Tasks = DataAccess.Query(WFTaskEntity.TableCode).FixField("id,name,type,x,y").Where(filter).QueryList<WFTaskEntity>().ToList();
            }
            return flowEntity;
        }

        public int UpdateTemplate(WFFlowEntity flowEntity)
        {
            if (null == flowEntity)
            {
                throw new ArgumentException($"{nameof(flowEntity)} is not null");
            }
            using (var trans = TransHelper.BeginTrans())
            {
                foreach (var task in flowEntity?.Tasks)
                {
                    DataAccess.Update(task);
                }
                foreach (var link in flowEntity?.Links)
                {
                    DataAccess.Update(link);
                }
                trans.Commit();
            }

            return 1;
        }
    }
}
