using System.Linq;
using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WFDesigner.Repository.Contacts;
using M.WorkFlow.Model;

namespace M.WFDesigner.Repository
{
    [Autowired]
    public class WfteventRepository:IWftEventRepository
    {
        [Autowired] //              
        private DataAccess _DataAccess { get; set; }

        public  WFTEventEntity GetById(string id)
        {
            var col = this._DataAccess.Query(WFTEventEntity.TableCode)
                .FixField("*")
                .Where(TableFilter.New().Equals("ID",id))
                .Query<WFTEventEntity>();
            return col.Data.FirstOrDefault();
        }


        public DBCollection<WFTEventEntity> GetList(TableFilter tf)
        {
            var col = this._DataAccess.Query(WFTEventEntity.TableCode)
                .FixField("*")
                .Where(tf)
                .Query<WFTEventEntity>();
            return col;
        }

    }
}
