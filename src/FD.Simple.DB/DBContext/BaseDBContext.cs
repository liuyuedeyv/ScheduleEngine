using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace FD.Simple.DB
{
    public abstract class BaseDBContext<TDbConnection, TDbCommand, TDbDataAdapter, TDbParameter> : IDBContext, IDBDifference<TDbCommand>
        where TDbCommand : System.Data.Common.DbCommand, new()
        where TDbConnection : System.Data.Common.DbConnection, new()
        where TDbDataAdapter : System.Data.Common.DbDataAdapter, new()
        where TDbParameter : System.Data.Common.DbParameter, new()
    {
        DBContextModel _dbContextModel = null;
        protected BaseDBContext(DBContextModel dBContextModel)
        {
            this._dbContextModel = dBContextModel;
        }

        #region IDBDifference接口  不同数据库差异特性
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
        public abstract string GetPagingFormat(QueryModel model);
        /// <summary>
        /// 根据 DBEntity 生成sql语句
        /// </summary>
        /// <param name="dBEntity"></param>
        /// <returns></returns>
        public abstract string BuildSqlFromDBEntity(DBEntity dBEntity);
        /// <summary>
        /// 根据 DBParamCollection 转换成参数化查询
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandParameters"></param>
        public abstract void AttachParameters(TDbCommand command, DBParamCollection commandParameters);
        #endregion

        #region IDBContext接口

        #region 1、origin ado.net method 基础查询方法
        /// <summary>
        /// 获取一个新的 参数集合
        /// </summary>
        /// <returns></returns>
        public abstract DBParamCollection GetNewParamCollection();

        protected DBParamCollection CreateParamCollection(string preFix, string stringPlus = "+")
        {
            return new DBParamCollection(preParam: preFix, stringPlus: stringPlus);
        }

        private void PrepareCommand(TDbCommand command, TDbConnection connection, CommandType commandType, string commandText, DBParamCollection commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            command.CommandTimeout = _dbContextModel.CommandTimeout;

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }


        public int ExecuteNonQuery(string commandText, DBParamCollection parmsValue = null, System.Data.CommandType cmdType = CommandType.Text)
        {
            if (_dbContextModel == null || _dbContextModel.ConnectionString == null || _dbContextModel.ConnectionString.Length == 0) throw new ArgumentNullException("connectionString");
            // Create & open a SqlConnection, and dispose of it after we are done
            using (TDbConnection connection = new TDbConnection())
            {
                connection.ConnectionString = this._dbContextModel.ConnectionString;
                connection.Open();
                // Call the overload that takes a connection in place of the connection string
                var cmd = new TDbCommand();
                PrepareCommand(cmd, connection, cmdType, commandText, parmsValue, out bool mustCloseConnection);
                int retval = cmd.ExecuteNonQuery();
                foreach (TDbParameter dp in cmd.Parameters)
                {
                    if (dp.Direction != ParameterDirection.Input)
                    {
                        foreach (DBParam dbParam in parmsValue)
                        {
                            if (dbParam.ParamName == dp.ParameterName)
                            {
                                dbParam.ParamValue = dp.Value;
                                break;
                            }
                        }
                    }

                }
                cmd.Parameters.Clear();
                if (mustCloseConnection)
                    connection.Close();

                return retval;
            }
        }

        public object ExecuteScalar(string commandText, DBParamCollection parmsValue = null, System.Data.CommandType cmdType = CommandType.Text)
        {
            if (_dbContextModel == null || _dbContextModel.ConnectionString == null || _dbContextModel.ConnectionString.Length == 0) throw new ArgumentNullException("connectionString");
            using (var connection = new TDbConnection())
            {
                connection.ConnectionString = this._dbContextModel.ConnectionString;
                connection.Open();

                var cmd = new TDbCommand();

                PrepareCommand(cmd, connection, cmdType, commandText, parmsValue, out bool mustCloseConnection);
                object retval;
                // Execute the command & return the results
                retval = cmd.ExecuteScalar();
                // Detach the SqlParameters from the command object, so they can be used again
                foreach (TDbParameter dp in cmd.Parameters)
                {
                    if (dp.Direction != ParameterDirection.Input)
                    {
                        foreach (DBParam dbParam in parmsValue)
                        {
                            if (dbParam.ParamName == dp.ParameterName)
                            {
                                dbParam.ParamValue = dp.Value;
                                break;
                            }
                        }
                    }

                }
                cmd.Parameters.Clear();

                if (mustCloseConnection)
                    connection.Close();

                return retval;
            }
        }

        public System.Data.IDataReader ExecuteReader(string commandText, DBParamCollection parmsValue = null, System.Data.CommandType cmdType = CommandType.Text)
        {
            if (_dbContextModel == null || _dbContextModel.ConnectionString == null || _dbContextModel.ConnectionString.Length == 0) throw new ArgumentNullException("connectionString");

            TDbConnection connection = null;
            try
            {
                connection = new TDbConnection
                {
                    ConnectionString = this._dbContextModel.ConnectionString
                };
                connection.Open();
                // Call the overload that takes a connection in place of the connection string
                var cmd = new TDbCommand();
                PrepareCommand(cmd, connection, cmdType, commandText, parmsValue, out bool mustCloseConnection);

                var dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);


                if (mustCloseConnection)
                    connection.Close();

                return dataReader;
            }
            catch (Exception ex)
            {
                if (connection != null)
                    connection.Close();
                throw ex;
            }
        }

        public virtual System.Data.DataSet ExecuteDataset(string commandText, DBParamCollection parmsValue = null, System.Data.CommandType cmdType = CommandType.Text)
        {
            if (_dbContextModel == null || _dbContextModel.ConnectionString == null || _dbContextModel.ConnectionString.Length == 0) throw new ArgumentNullException("connectionString");

            using (var connection = new TDbConnection())
            {
                connection.ConnectionString = this._dbContextModel.ConnectionString;
                connection.Open();

                var cmd = new TDbCommand();
                PrepareCommand(cmd, connection, cmdType, commandText, parmsValue, out bool mustCloseConnection);

                // System.Data.SqlClient.SqlDataAdapter

                // Create the DataAdapter & DataSet

                using (var da = new TDbDataAdapter())
                {
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();

                    // Fill the DataSet using default values for DataTable names, etc
                    da.Fill(ds);

                    if (mustCloseConnection)
                        connection.Close();
                    // Return the dataset
                    return ds;
                }
            }
        }

        #endregion

        #region 2、dapper extend method
        public IEnumerable<T> Query<T>(string commandText, DBParamCollection parmsValue = null, CommandType cmdType = CommandType.Text)
        {
            if (_dbContextModel == null || _dbContextModel.ConnectionString == null || _dbContextModel.ConnectionString.Length == 0) throw new ArgumentNullException("connectionString");

            using (var connection = new TDbConnection())
            {
                connection.ConnectionString = this._dbContextModel.ConnectionString;
                connection.Open();


                DynamicParameters param = new DynamicParameters();
                if (parmsValue != null)
                {
                    foreach (var p in parmsValue)
                    {
                        //TODO:兼容处理，后续应该去掉
                        DbType d = DbType.AnsiString;
                        switch (p.ParamType)
                        {
                            case EColType.NVARCHAR:
                                d = DbType.String;
                                break;
                            case EColType.VARCHAR:
                                d = DbType.AnsiString;
                                break;
                            case EColType.NUMERIC:
                                d = DbType.Double;
                                break;
                            case EColType.DATETIME:
                                d = DbType.DateTime;
                                break;
                        }
                        param.Add(p.ParamName, p.ParamValue, p.ParamDbType ?? d, p.Direction, p.ParamLength);
                    }
                }
                IEnumerable<T> list = connection.Query<T>(commandText, param, commandType: cmdType, commandTimeout: _dbContextModel.CommandTimeout);
                foreach (var item in list)
                {
                    if (item is DBEntity)
                    {
                        (item as DBEntity).InitReady = true;
                    }
                }
                return list;
            }
        }


        public void QueryMultiple(string commandText, DBParamCollection parmsValue = null, Action<SqlMapper.GridReader> func = null, CommandType cmdType = CommandType.Text)
        {
            if (_dbContextModel == null || _dbContextModel.ConnectionString == null || _dbContextModel.ConnectionString.Length == 0) throw new ArgumentNullException("connectionString");

            DynamicParameters param = new DynamicParameters();
            if (parmsValue != null)
            {
                foreach (var p in parmsValue)
                {
                    param.Add(p.ParamName, p.ParamValue);
                }
            }

            using (var connection = new TDbConnection())
            {
                connection.ConnectionString = this._dbContextModel.ConnectionString;
                connection.Open();

                var multipleReader = connection.QueryMultiple(commandText, param, commandType: cmdType, commandTimeout: _dbContextModel.CommandTimeout);
                func?.Invoke(multipleReader);
            }
        }


        public int ExecuteNonQuery2(string commandText, object parmsValue, System.Data.CommandType cmdType = CommandType.Text)
        {
            if (_dbContextModel == null || _dbContextModel.ConnectionString == null || _dbContextModel.ConnectionString.Length == 0) throw new ArgumentNullException("connectionString");
            // Create & open a SqlConnection, and dispose of it after we are done
            using (TDbConnection connection = new TDbConnection())
            {
                connection.ConnectionString = this._dbContextModel.ConnectionString;
                connection.Open();

                return connection.Execute(commandText, parmsValue);
            }
        }


        #endregion

        #region 3、通用DBEntity
        public int Update(DBEntity dBEntity)
        {
            using (var connection = new TDbConnection())
            {
                connection.ConnectionString = this._dbContextModel.ConnectionString;
                connection.Open();
                var sql = BuildSqlFromDBEntity(dBEntity);
                if (string.IsNullOrWhiteSpace(sql))
                {
                    return 0;
                }
                else
                {
                    var count = connection.Execute(sql, dBEntity, commandTimeout: _dbContextModel.CommandTimeout);
                    return count;
                }
            }
        }
        #endregion

        #endregion
    }
}
