using System.Data.Common;

namespace FD.Simple.DB
{
    /// <summary>
    /// 不同数据库语法差异部分处理
    /// </summary>
    /// <typeparam name="TDbCommand"></typeparam>
    internal interface IDBDifference<TDbCommand>
      where TDbCommand : DbCommand
    {

        /// <summary>
        /// 根据 DBEntity 生成sql语句
        /// </summary>
        /// <param name="dBEntity"></param>
        /// <returns></returns>
        string BuildSqlFromDBEntity(DBEntity dBEntity);
        /// <summary>
        /// 根据 DBParamCollection 转换成参数化查询
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandParameters"></param>
        void AttachParameters(TDbCommand command, DBParamCollection commandParameters);
    }
}
