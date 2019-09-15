using FD.Simple.Utils.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FD.Simple.DB.Cmd
{
    public class AddAction : BaseAction<AddAction>
    {
        DBEntity insertEntity;
        public AddAction(IDBContext idbContext, IFDLogger ilog, DBEntity entity) :
          base(idbContext, entity._TableCode)
        {
            this.insertEntity = entity;
        }

        public AddAction Add()
        {
            string sql = string.Empty;
            var collP = DbContext.GetNewParamCollection();

            string values = string.Empty, columns = string.Empty;
            var entityType = insertEntity.GetType();
            foreach (var col in insertEntity.ChangeField)
            {
                var param = new DBParam(col, entityType.GetProperty(col).GetValue(insertEntity, null));
                collP.Add(param);
                values += param.ParamName + ",";
                columns += col + ",";
            }
            values = values.TrimEnd(',');
            columns = columns.TrimEnd(',');

            sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", insertEntity._TableCode, columns, values);

            base.MainSql = sql;
            base.ParamCollection = collP;
            return this;
        }

        public override AddAction Where(TableFilter filter)
        {
            return this;
        }
        public override int ExecuteNonQuery()
        {
            return this.DbContext.ExecuteNonQuery(this.MainSql, this.ParamCollection);
        }
    }
}
