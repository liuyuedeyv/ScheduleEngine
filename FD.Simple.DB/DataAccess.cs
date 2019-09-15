using FD.Simple.DB.Cmd;
using FD.Simple.Utils.Logging;
using Microsoft.Extensions.Logging;
using System;
namespace FD.Simple.DB
{
    public class DataAccess
    {
        IDBContext dbContext = null;
        IFDLogger ilog = null;
        public DataAccess(IDBContext idbContext)
        {
            this.dbContext = idbContext;
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public T GetNewEntity<T>()
            where T : DBEntity, new()
        {
            var t = new T();
            t.Add();
            return t;
        }

        public UpdateAction Update(string table)
        {
            return new UpdateAction(this.dbContext, this.ilog, table);
        }
        public int Update(DBEntity dBEntity)
        {
            if (dBEntity.State == EDBEntityState.Added && string.IsNullOrWhiteSpace(dBEntity.ID))
            {
                dBEntity.ID = FD.Simple.Utils.DataKeyFactory.NewId();
            }
            return this.dbContext.Update(dBEntity);
        }
        public DeleteAction Delete(string table)
        {
            return new DeleteAction(this.dbContext, this.ilog, table);
        }
        public int Delete(DBEntity entity)
        {
            return new DeleteAction(this.dbContext, this.ilog, entity._TableCode).Where(TableFilter.New().Equals("ID", entity.ID)).ExecuteNonQuery();
        }
        public int Add(DBEntity entity)
        {
            entity.State = EDBEntityState.Added;
            return this.dbContext.Update(entity);
        }

        public QueryAction Query(string table, string defaultField = "id")
        {
            return new QueryAction(this.dbContext, this.ilog, table, defaultField);
        }
    }
}
