using System.Linq;
using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WFDesigner.Repository.Contacts;
using M.WorkFlow.Model;

namespace M.WFDesigner.Repository
{
    [Autowired]
    public class WFTEventMsgRepository:IWftEventMsgRepository
    {
        [Autowired] //              
        private DataAccess _DataAccess { get; set; }

        public WFTEventMsgEntity GetById(string id)
        {
            var col = this._DataAccess.Query(WFTEventMsgEntity.TableCode)
                .FixField("*")
                .Where(TableFilter.New().Equals("ID", id))
                .Query<WFTEventMsgEntity>();
            return col.Data.FirstOrDefault();
        }


        public DBCollection<WFTEventMsgEntity> GetList(TableFilter tf)
        {
            var col = this._DataAccess.Query(WFTEventMsgEntity.TableCode)
                .FixField("*")
                .Where(tf)
                .Query<WFTEventMsgEntity>();
            return col;
        }

    }
}