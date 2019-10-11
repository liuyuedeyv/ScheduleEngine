using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace FD.Simple.DB.DBContext
{
    public class SqlServerDBContext : BaseDBContext<SqlConnection, SqlCommand, SqlDataAdapter, SqlParameter>
    {
        public SqlServerDBContext(DBContextModel dBContextModel) : base(dBContextModel)
        {

        }
        static readonly Dictionary<EColType, DbType> _ecolTypeToDbTypeMapping = new Dictionary<EColType, DbType>()
        {
            [EColType.NVARCHAR] = DbType.String,
            [EColType.VARCHAR] = DbType.AnsiString,
            [EColType.NUMERIC] = DbType.Double,
            [EColType.DATETIME] = DbType.DateTime
        };
        static readonly Dictionary<DbType, DbType> _specialDbTypeMapping = new Dictionary<DbType, DbType>()
        {
            [DbType.SByte] = DbType.Int16,
            [DbType.UInt16] = DbType.Int32,
            [DbType.UInt32] = DbType.Int64,
            [DbType.UInt64] = DbType.AnsiString,
            [DbType.VarNumeric] = DbType.AnsiString
        };

        public override DBParamCollection GetNewParamCollection()
        {
            return base.CreateParamCollection("@");
        }

        public override string GetPagingFormat(QueryModel m)
        {
            var sql = string.Format(@"SELECT t.*  FROM (SELECT ROW_NUMBER() OVER (ORDER BY {3}) AS RowNumber,{0}
                                        FROM {1} where {2} ) t WHERE t.RowNumber BETWEEN {4} AND {5}",
                m.Fields, m.Tables, m.Where, m.SortField, m.Start, m.End);
            return sql;
        }
        public override string BuildSqlFromDBEntity(DBEntity dBEntity)
        {
            if (dBEntity.ChangeField.Count == 0 && (dBEntity.State == EDBEntityState.Added || dBEntity.State == EDBEntityState.Modified))
            {
                return string.Empty;
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (dBEntity.State == EDBEntityState.Added)
            {
                var tempSql = "";
                if (dBEntity.Suffix != null && dBEntity.Suffix.Trim() != "")
                {
                    tempSql = string.Format("INSERT INTO {0} ([{1}]) VALUES ({2})", dBEntity._TableCode, string.Join("],[", dBEntity.ChangeField), "@" + string.Join(dBEntity.Suffix + ",@", dBEntity.ChangeField));
                    tempSql = tempSql.Insert(tempSql.Length - 1, dBEntity.Suffix);
                    sb.Append(tempSql);
                }
                else
                {
                    sb.AppendFormat("INSERT INTO {0} ([{1}]) VALUES ({2})", dBEntity._TableCode, string.Join("],[", dBEntity.ChangeField), "@" + string.Join(dBEntity.Suffix + ",@", dBEntity.ChangeField));
                }
            }
            else if (dBEntity.State == EDBEntityState.Modified)
            {
                sb.AppendFormat("UPDATE  {0} SET ", dBEntity._TableCode);
                foreach (var filedCode in dBEntity.ChangeField)
                {
                    if (filedCode == "ID")
                    {
                        continue;
                    }
                    sb.AppendFormat(" [{0}]=@{0}{1},", filedCode, dBEntity.Suffix);
                }
                sb.Remove(sb.Length - 1, 1);
                sb = sb.Append("  WHERE ID=@ID" + dBEntity.Suffix);
            }
            else if (dBEntity.State == EDBEntityState.Deleted)
            {
                sb.AppendFormat(" DELETE FROM  {0} WHERE ID=@ID{1}", dBEntity._TableCode, dBEntity.Suffix);
            }
            return sb.ToString();
        }
        public override void AttachParameters(SqlCommand command, DBParamCollection commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters == null)
            {
                return;
            }
            foreach (var p in commandParameters)
            {
                if (p != null)
                {
                    // Check for derived output value with no value assigned
                    if ((p.Direction == ParameterDirection.InputOutput ||
                        p.Direction == ParameterDirection.Input) &&
                        (p.ParamValue == null))
                    {
                        p.ParamValue = DBNull.Value;
                    }
                    DbType dbType = DbType.AnsiString;
                    //如果没用设置ParamDbType，则以ParmaType为准
                    if (p.ParamDbType == null)
                    {
                        _ecolTypeToDbTypeMapping.TryGetValue(p.ParamType, out dbType);
                    }
                    else
                    {
                        if (!_specialDbTypeMapping.TryGetValue((DbType)p.ParamDbType, out dbType))
                        {
                            dbType = (DbType)p.ParamDbType;
                        }
                    }
                    command.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = p.ParamName,
                        Value = p.ParamValue,
                        Direction = p.Direction,
                        DbType = dbType,
                        Size = p.ParamLength
                    });
                }
            }
        }
    }
}
