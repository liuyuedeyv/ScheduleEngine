using System.Data;
using System.Linq;
using Autofac.Features.Indexed;
using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Model;

namespace M.WFDesigner.RefData
{
    public class WFServiceTableRefdataRepository : RefdataRepository<WFServiceEntity>
    {

         
        protected override string CachePre => ERefCachePre.WFS.ToString();

        DataAccess _da;
        public WFServiceTableRefdataRepository(ICache cache,IIndex<string, DataAccess> daArr)
            : base(cache)
        {
            this._da = daArr["fastdev"]; 
        }
        public WFServiceEntity GetData(string key)
        {
            return base.GetData(key, InitData);
        }
        private WFServiceEntity InitData(string key)
        {
            var col = this._da.Query(WFServiceEntity.TableCode)
                .FixField("*")
                .Where(TableFilter.New().Equals("ID", key))
                .Query<WFServiceEntity>(); 
            return col.Data.FirstOrDefault();
        }
    }
}
