using FD.Simple.Utils.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FD.Simple.DB.Cmd
{
    public class DeleteAction : BaseAction<DeleteAction>
    {
        public DeleteAction(IDBContext idbContext, IFDLogger ilog, string tableName)
            : base(idbContext, tableName)
        {
        }

        public override DeleteAction Where(TableFilter filter)
        {
            string filterCondition = string.Empty, where = string.Empty;
            if (filter != null)
            {
                filterCondition = filter.Build(base.ParamCollection);
                if (!string.IsNullOrWhiteSpace(filterCondition))
                {
                    where = string.Format(" where {0} ", filterCondition);
                }
            }

            base.MainSql = string.Format("delete from {0} {1}", base.TableCode, where);
            return this;
        }
        public override int ExecuteNonQuery()
        {
            return this.DbContext.ExecuteNonQuery(this.MainSql, this.ParamCollection);
        }
    }
}
