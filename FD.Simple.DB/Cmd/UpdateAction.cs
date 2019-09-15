using FD.Simple.Utils.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FD.Simple.DB.Cmd
{
    public class UpdateAction : BaseAction<UpdateAction>
    {
        private Dictionary<string, object> UpdateColumns;

        public UpdateAction(IDBContext idbContext, IFDLogger ilog, string tableName) :
            base(idbContext, tableName)
        {
            UpdateColumns = new Dictionary<string, object>();
        }

        public UpdateAction Set(string colName, object value)
        {
            this.UpdateColumns.Add(colName, value);
            return this;
        }

        public override UpdateAction Where(TableFilter filter)
        {
            if (this.UpdateColumns.Count == 0)
            {
                throw new Exception("no update columns");
            }

            #region update columns
            string columnStr = string.Empty;
            foreach (var col in this.UpdateColumns)
            {
                var p = new DBParam(col.Key, col.Value);
                ParamCollection.Add(p);
                columnStr += string.Format(" {0} ={1},", col.Key, p.ParamName);
            }
            columnStr = columnStr.TrimEnd(',');
            #endregion

            #region where
            string filterCondition = string.Empty, where = string.Empty;
            if (filter != null)
            {
                filterCondition = filter.Build(base.ParamCollection);
                if (!string.IsNullOrWhiteSpace(filterCondition))
                {
                    where = string.Format(" where {0} ", filterCondition);
                }
            }
            #endregion

            base.MainSql = string.Format("update {0} set {1} {2}", base.TableCode, columnStr, where);
            return this;
        }

        public override int ExecuteNonQuery()
        {
            return this.DbContext.ExecuteNonQuery(this.MainSql, this.ParamCollection);
        }
    }
}
