using System;
using System.Data;

namespace FD.Simple.DB
{
    /// <summary>
    /// 数据库参数
    /// </summary>
    public class DBParam
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <param name="dbType">如果使用Sqlite 数据库，此列不建议赋值</param>
        /// <param name="length"></param>
        public DBParam(string paramName, object value, DbType? dbType = null, int length = 255, ParameterDirection direction = ParameterDirection.Input)
        {
            if (direction == ParameterDirection.Input && (value == null || value == DBNull.Value))
            {
                throw new ArgumentNullException("value");
            }
            this.Direction = direction;
            this.ParamName = paramName.Replace('.', '_');
            this.ParamValue = value;
            this.ParamDbType = dbType == null ? value.GuessDBTypeFromValue() : dbType;
            this.ParamLength = length;
        }
        [System.Obsolete("已过期，请使用    public DBParam(string paramName, object value, DbType dbType, int length = 255) 来初始化参数")]
        public DBParam(string paramName, object value, EColType type, int length)
        {
            this.ParamName = paramName.Replace('.', '_');
            this.ParamType = type;
            this.ParamValue = value;
            this.ParamLength = length;
        }

        public string ParamName { get; set; }
        public string ParamNameLike { get { return this.ParamName + "+ '%' "; ; } }
        [System.Obsolete("已过期，请使用ParamDbType来替代")]
        public EColType ParamType { get; set; }
        public DbType? ParamDbType { get; set; }
        public object ParamValue { get; set; }
        public int ParamLength { get; set; }

        public System.Data.ParameterDirection Direction = ParameterDirection.Input;
    }
}