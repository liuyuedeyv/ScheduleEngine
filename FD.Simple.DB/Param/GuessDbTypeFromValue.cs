using System.Data;
using System.Collections.Generic;
namespace FD.Simple.DB
{
    internal static class GuessDbType
    {
        static readonly Dictionary<System.Type, DbType> DotnetTypeToDbTypeMapping = new Dictionary<System.Type, DbType>
        {
            [typeof(System.Char)] = DbType.AnsiStringFixedLength,
            [typeof(System.String)] = DbType.AnsiString,
            [typeof(System.Boolean)] = DbType.Boolean,
            [typeof(System.Byte)] = DbType.Byte,
            [typeof(System.SByte)] = DbType.SByte,
            [typeof(System.Int16)] = DbType.Int16,
            [typeof(System.UInt16)] = DbType.Int16,
            [typeof(System.Int32)] = DbType.Int32,
            [typeof(System.UInt32)] = DbType.Int32,
            [typeof(System.Int64)] = DbType.Int64,
            [typeof(System.UInt64)] = DbType.Int64,
            [typeof(System.Single)] = DbType.Single,
            [typeof(System.Double)] = DbType.Double,
            [typeof(System.Decimal)] = DbType.Decimal,
            [typeof(System.DateTime)] = DbType.DateTime,
            [typeof(System.DateTimeOffset)] = DbType.DateTime
        };

        /// <summary>
        /// 从传入Value猜测数据类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DbType GuessDBTypeFromValue(this object value)
        {
            DbType dbType = DbType.AnsiString;
            if (!DotnetTypeToDbTypeMapping.TryGetValue(value.GetType(), out dbType))
            {
                throw new System.Exception("不支持的参数类型");
            }
            return dbType;
        }
    }
}
