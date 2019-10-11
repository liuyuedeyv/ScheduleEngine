using FD.Simple.Utils.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
namespace FD.Simple.DB.Cmd
{
    public class QueryAction : BaseAction<QueryAction>
    {
        private string fixField = "*";
        private uint pageIndex = 0;
        private uint pageSize = 0;
        private string sortField = string.Empty;
        private string defaultField = "id";

        public QueryAction(IDBContext idbContext, IFDLogger ilog, string tableName, string defaultField)
            : base(idbContext, tableName)
        {
            this.defaultField = defaultField;
        }

        public uint PageIndex { get { return this.pageIndex; } }

        public uint PageSize { get { return this.pageSize; } }

        public QueryAction Paging(uint pageIndex, uint pageSize = 10)
        {
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
            return this;
        }

        public QueryAction Sort(string sortField)
        {
            sortField = sortField.ToLower();
            if ((joinTables.Count > 0 && sortField.IndexOf(".") == -1) || (sortField.IndexOf(" asc") == -1 && sortField.IndexOf(" desc") == -1))
            {
                throw new Exception("sort field must by  'table.col asc|desc'");
            }
            this.sortField = sortField;
            return this;
        }
        public QueryAction FixField(string fields)
        {
            this.fixField = fields;
            return this;
        }

        List<JoinTable> joinTables = new List<JoinTable>();

        public QueryAction Join(JoinTable table)
        {
            this.joinTables.Add(table);
            return this;
        }
        //public QueryAction LeftJoin(string table)
        //{
        //    this.joinTables.Add(new JoinTable(table, EJoinType.Left));
        //    return this;
        //}
        //public QueryAction RightJoin(string table)
        //{
        //    this.joinTables.Add(new JoinTable(table, EJoinType.Right));
        //    return this;
        //}
        //public QueryAction InnerJoin(string table)
        //{
        //    this.joinTables.Add(new JoinTable(table, EJoinType.Inner));
        //    return this;
        //}
        public override QueryAction Where(TableFilter filter)
        {
            if (filter != null)
            {
                filter.MainTable = base.TableCode;

            }

            string joinCondition = string.Empty, filterCondition = string.Empty, tmpField = string.Empty, where = string.Empty;

            if (string.IsNullOrWhiteSpace(this.fixField))
            {
                throw new Exception("no field");
            }

            #region tables
            joinCondition = base.TableCode;
            foreach (var table in joinTables)
            {
                var tmpJoin = string.Empty;
                foreach (var item in table.Conditions)
                {
                    tmpJoin = string.Format("{0}={1}", item.Key.IndexOf('.') == -1 ? (base.TableCode + "." + item.Key) : item.Key,
                        item.Value.IndexOf('.') == -1 ? (table.Table + "." + item.Value) : item.Value);
                }
                joinCondition += string.Format(" {0} join {1} on {2} ", table.JoinType.ToString(), table.Table, tmpJoin);
            }
            #endregion

            #region field
            foreach (var col in this.fixField.Split(','))
            {
                if (col.IndexOf(".") == -1)
                {
                    tmpField += string.Format("{0}.{1},", base.TableCode, col);
                }
                else if (col.IndexOf(".") > -1 && col.IndexOf("*") > -1)
                {
                    tmpField += string.Format("{0},", col);
                }
                else
                {
                    var tmpArray = col.Split('.');
                    if (string.Compare(tmpArray[0], base.TableCode, true) == 0)
                    {
                        tmpField += string.Format("{0}.{1} as {1},", tmpArray[0], tmpArray[1]);
                    }
                    else
                    {
                        tmpField += string.Format("{0}.{1} as {0}{1},", tmpArray[0], tmpArray[1]);
                    }
                }
            }
            #endregion

            #region where
            if (filter != null)
            {
                filterCondition = filter.Build(base.ParamCollection);
                where = string.Format(" {0} ", filterCondition);
            }
            else
            {
                where = " 1=1 ";
            }
            #endregion

            string sql = string.Empty;
            #region paging
            if (this.pageIndex > 0 && this.pageSize > 0)
            {
                if (string.IsNullOrWhiteSpace(this.sortField))
                {
                    this.sortField = string.Format("{0}.{1} asc", base.TableCode, this.defaultField);
                }
                var start = (this.pageIndex - 1) * this.pageSize + 1;
                var end = this.pageSize * this.pageIndex;
                sql = base.DbContext.GetPagingFormat(new QueryModel()
                {
                    Fields = tmpField.TrimEnd(','),
                    Tables = joinCondition,
                    Where = where,
                    SortField = this.sortField,
                    Start = start,
                    End = end
                });
                base.SecondarySql = string.Format("select count(1) from {0}  where {1}", joinCondition, where);
            }
            else
            {
                sql = string.Format("select {0} from {1} where {2} {3}", tmpField.TrimEnd(','), joinCondition, where,
                    !string.IsNullOrWhiteSpace(this.sortField) ? ("order by " + this.sortField) : string.Empty);
            }
            #endregion
            base.MainSql = sql;
            return this;
        }
        public override int ExecuteNonQuery()
        {
            return this.DbContext.ExecuteNonQuery(this.MainSql, this.ParamCollection);
        }

        public object QueryScalar()
        {
            return this.DbContext.ExecuteScalar(this.MainSql, this.ParamCollection);
        }

        public IDataReader QueryReader()
        {
            return this.DbContext.ExecuteReader(this.MainSql, this.ParamCollection);
        }
        public DataSet QueryDataset()
        {
            return this.DbContext.ExecuteDataset(this.MainSql, this.ParamCollection);
        }
        public IEnumerable<T> QueryList<T>()
        {
            return this.DbContext.Query<T>(this.MainSql, this.ParamCollection);
        }
        public T QueryFirst<T>()
        {
            return QueryList<T>().FirstOrDefault();
        }
        public DBCollection<T> Query<T>()
            where T : DBEntity, new()
        {
            var coll = new DBCollection<T>();
            coll.Data = QueryList<T>();
            if (string.IsNullOrEmpty(this.SecondarySql))
            {
                coll.RecordsTotal = coll.Data.Count();
            }
            else
            {
                coll.RecordsTotal = Convert.ToInt32(this.DbContext.ExecuteScalar(this.SecondarySql, this.ParamCollection));
            }
            coll.PageIndex = this.PageIndex;
            coll.PageSize = this.PageSize;
            return coll;
        }
    }
}
