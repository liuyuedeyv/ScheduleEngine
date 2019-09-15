using System;
using System.Collections.Generic;
using System.Data;


namespace FD.Simple.DB
{
    /// <summary>
    /// 数据库操作对象
    /// </summary>
    public interface IDBContext
    {
        #region 1、基础查询方法

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="fileds"></param>
        /// <param name="tables"></param>
        /// <param name="where"></param>
        /// <param name="sortField"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        string GetPagingFormat(QueryModel model);
        /// <summary>
        /// 获取参数集合
        /// </summary>
        /// <returns></returns>
        DBParamCollection GetNewParamCollection();
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parmsValue"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string commandText, DBParamCollection parmsValue = null, CommandType cmdType = CommandType.Text);
        /// <summary>
        /// 执行并返回一个
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parmsValue"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        object ExecuteScalar(string commandText, DBParamCollection parmsValue = null, CommandType cmdType = CommandType.Text);
        IDataReader ExecuteReader(string commandText, DBParamCollection parmsValue = null, CommandType cmdType = CommandType.Text);
        DataSet ExecuteDataset(string commandText, DBParamCollection parmsValue = null, CommandType cmdType = CommandType.Text);
        #endregion

        #region 2、dapper 支持方法
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parmsValue"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(string commandText, DBParamCollection parmsValue = null, CommandType cmdType = CommandType.Text);
        /// <summary>
        /// 批量查询
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parmsValue"></param>
        /// <param name="func"></param>
        /// <param name="cmdType"></param>
        void QueryMultiple(string commandText, DBParamCollection parmsValue = null, Action<Dapper.SqlMapper.GridReader> func = null, CommandType cmdType = CommandType.Text);
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parmsValue"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        int ExecuteNonQuery2(string commandText, object parmsValue = null, CommandType cmdType = CommandType.Text);
        #endregion

        #region 3、通用DBEntity
        int Update(DBEntity entity);
        #endregion
    }
}
