using System;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.MessageOutPuters;
using System.Text;
using Buffalo.DB.CommBase.BusinessBases;
using System.Data.Common;
using System.Collections.Generic;
using Buffalo.DB.Exceptions;
using System.Threading.Tasks;
using System.Data.SqlClient;

///通用SQL Server访问类v1.2

namespace Buffalo.DB.DbCommon
{
    /// <summary>
    /// 提交方式
    /// </summary>
    public enum CommitState 
    {
        /// <summary>
        /// 自动提交(关闭连接)
        /// </summary>
        AutoCommit,
        /// <summary>
        /// 手动提交(关闭连接)
        /// </summary>
        UserCommit
    }
    
	/// <summary>
	/// 数据库访问类
	/// </summary>
	public class DataBaseOperate : IDisposable,IAsyncDisposable
	{
		
		
		/// <summary>
        /// 数据库连接标志
		/// </summary>
        private bool IsConnected 
        {
            get 
            {

                return  ( _conn != null && _conn.State != ConnectionState.Closed);
            }
        }
        /// <summary>
        /// 只读连接的标志
        /// </summary>
        private bool IsReadConnected
        {
            get
            {

                return (_readconn != null && _readconn.State != ConnectionState.Closed);
            }
        }

        //数据库连接对象
        private DbConnection _conn;

        /// <summary>
        /// 数据库连接
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                if (!ConnectDataBase())
                {
                    throw (new ApplicationException("没有建立数据库连接。"));
                }
                return _conn;
            }
        }
        private DbConnection _readconn;

        /// <summary>
        /// 只读数据库连接
        /// </summary>
        public DbConnection ReadOnlyConnection
        {
            get
            {
                if (!ReadOnlyConnectDataBase())
                {
                    throw (new ApplicationException("没有建立数据库连接。"));
                }
                return _readconn;
            }
        }
        
        private DbCommand _comm;
        /// <summary>
        /// 只读的命令
        /// </summary>
        private DbCommand _readcomm;
        private DbTransaction _tran;
		//private IDbDataAdapter _sda;

        private int _lastAffectedRows;
        /// <summary>
        /// 强制用主连接操作
        /// </summary>
        private bool _forceMasterConnection;

        private IDBAdapter _dbAdapter = null;
        
        DBInfo _db = null;
        /// <summary>
        /// 输出器
        /// </summary>
        MessageOutputBase _outputer;

       
        /// <summary>
        /// 事务是否已经开启
        /// </summary>
        private bool IsTran 
        {
            get 
            {
                return _tran != null;
            }
        }

        //是否自动关闭连接
        private CommitState _commitState = CommitState.AutoCommit;

        string _databaseName;

        /// <summary>
        /// 获取数据库名字
        /// </summary>
        /// <returns></returns>
        public string DataBaseName
        {
            get
            {
                if (_databaseName == null)
                {
                    try
                    {
                        //若连接数据库失败抛出错误
                        if (!ConnectDataBase())
                        {
                            throw (new ApplicationException("没有建立数据库连接。"));
                        }
                        _databaseName = _conn.Database;
                    }
                    finally
                    {

                        AutoClose();

                    }
                }
                return _databaseName;
            }
        }

        /// <summary>
        /// 是否自动关闭连接
        /// </summary>
        public CommitState CommitState 
        {
            get 
            {
                return _commitState;
            }
            set 
            {
                _commitState = value;
            }
        }

        /// <summary>
        /// 强制主连接操作
        /// </summary>
        public bool ForceMasterConnection
        {
            get
            {
                return _forceMasterConnection;
            }
            set
            {
                _forceMasterConnection = value;
            }
        }
        /// <summary>
        /// 关联数据库信息
        /// </summary>
        public DBInfo DBInfo 
        {
            get 
            {
                return _db;
            }
        }
        /// <summary>
        /// 开启非事务的批量操作
        /// </summary>
        /// <returns></returns>
        public BatchAction StarBatchAction() 
        {
            BatchAction action = new BatchAction(this);
            return action;
        }
        
        /// <summary>
        /// 实例化数据库访问对象
        /// </summary>
        internal DataBaseOperate(DBInfo db,bool isAutoClose)
		{
            _db = db;
            _dbAdapter = db.CurrentDbAdapter;
            if (!isAutoClose) 
            {
                _commitState = CommitState.UserCommit;
            }
            _outputer = db.SqlOutputer.CreateOutput(this);
            _forceMasterConnection = false;

        }


        internal DataBaseOperate(DBInfo db)
            :this(db,true)
        { 
        }

        /// <summary>
        /// 返回此 System.Data.Common.DbConnection 的数据源的架构信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetSchema() 
        {
            DataTable dt = null;
            try
            {
                if (!ConnectDataBase())
                {
                    throw (new ApplicationException("没有建立数据库连接。"));
                }
                dt = _conn.GetSchema();
            }
            finally 
            {
                AutoClose();
            }
            return dt;
        }
        /// <summary>
        /// 返回此 System.Data.Common.DbConnection 的数据源的架构信息
        /// </summary>
        /// <param name="collectionName">指定要返回的架构的名称</param>
        /// <returns></returns>
        public DataTable GetSchema(string collectionName)
        {
            DataTable dt = null;
            try
            {
                if (!ConnectDataBase())
                {
                    throw (new ApplicationException("没有建立数据库连接。"));
                }
                dt = _conn.GetSchema(collectionName);
            }
            finally
            {
                AutoClose();
            }
            return dt;
        }
        /// <summary>
        /// 返回此 System.Data.Common.DbConnection 的数据源的架构信息
        /// </summary>
        /// <param name="collectionName">指定要返回的架构的名称</param>
        /// <param name="restrictionValues">为请求的架构指定一组限制值</param>
        /// <returns></returns>
        public DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            DataTable dt = null;
            try
            {
                if (!ConnectDataBase())
                {
                    throw (new ApplicationException("没有建立数据库连接。"));
                }
                dt = _conn.GetSchema(collectionName, restrictionValues);
            }
            finally
            {
                AutoClose();
            }
            return dt;
        }
        /// <summary>
		/// 连接只读数据库，并打开数据库连接
		/// </summary>
		/// <returns>成功返回true</returns>
		public bool ReadOnlyConnectDataBase()
        {
            if (!IsReadConnected)
            {
                try
                {
                    if (_readconn == null || _readconn.State == ConnectionState.Closed)
                    {
                        _readconn = DBConn.GetReadConnection(_db);

                    }
                    if (_readconn.State == ConnectionState.Closed)
                    {
                        _readconn.Open();
                    }
                    if (_db.SqlOutputer.HasOutput)
                    {
                        OutMessage(MessageType.OtherOper, "Connect Readonly DataBase", null, "");
                    }
                    if (_readcomm == null)
                    {
                        _readcomm = _dbAdapter.GetCommand();//**
                    }

                    _readcomm.Connection = _readconn;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return true;
        }
        /// <summary>
		/// 连接只读数据库，并打开数据库连接
		/// </summary>
		/// <returns>成功返回true</returns>
		public async Task<bool> ReadOnlyConnectDataBaseAsync()
        {
            if (!IsReadConnected)
            {
                try
                {
                    if (_readconn == null || _readconn.State == ConnectionState.Closed)
                    {
                        _readconn = DBConn.GetReadConnection(_db);

                    }
                    if (_readconn.State == ConnectionState.Closed)
                    {
                        await _readconn.OpenAsync();
                    }
                    if (_db.SqlOutputer.HasOutput)
                    {
                        await OutMessageAsync(MessageType.OtherOper, "Connect Readonly DataBase", null, "");
                    }
                    if (_readcomm == null)
                    {
                        _readcomm = _dbAdapter.GetCommand();//**
                    }

                    _readcomm.Connection = _readconn;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return true;
        }

        /// <summary>
        /// 连接数据库，并打开数据库连接
        /// </summary>
        /// <returns>成功返回true</returns>
        public bool ConnectDataBase()
		{
			if (!IsConnected)
			{
				try
				{
					if (_conn == null || _conn.State==ConnectionState.Closed)
					{
                        _conn = DBConn.GetConnection(_db);
						
					}
                    if (_conn.State == ConnectionState.Closed)
                    {
                        _conn.Open();
                    }
                    if (_db.SqlOutputer.HasOutput)
                    {
                        OutMessage(MessageType.OtherOper, "Connect DataBase", null, "");
                    }
                    if (_comm == null)
                    {
                        _comm = _dbAdapter.GetCommand();//**
                    }
                    
					_comm.Connection = _conn;
				}
				catch(Exception e)
				{
					throw e;
				}
			}
			return true;
		}
        /// <summary>
        /// 连接数据库，并打开数据库连接
        /// </summary>
        /// <returns>成功返回true</returns>
        public async Task<bool> ConnectDataBaseAsync()
        {
            if (!IsConnected)
            {
                try
                {
                    if (_conn == null || _conn.State == ConnectionState.Closed)
                    {
                        _conn = DBConn.GetConnection(_db);

                    }
                    if (_conn.State == ConnectionState.Closed)
                    {
                        await _conn.OpenAsync();
                    }
                    if (_db.SqlOutputer.HasOutput)
                    {
                        await OutMessageAsync(MessageType.OtherOper, "Connect DataBase", null, "");
                    }
                    if (_comm == null)
                    {
                        _comm = _dbAdapter.GetCommand();//**
                    }

                    _comm.Connection = _conn;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return true;
        }

        #region IDisposable 成员
        /// <summary>
        /// 释放占用资源
        /// </summary>
        public void Dispose()
		{
            Dispose(true);
            GC.SuppressFinalize(this); 
		}

        public async ValueTask DisposeAsync()
        {
            await CloseDataBaseAsync();

        }
        protected void Dispose(bool disposing)
        {
           
            if (disposing)
            {
                
            }
            CloseDataBase();
        }
        ~DataBaseOperate()
        {
            try
            {
                Dispose(false);
            }
            catch { }
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public bool CloseDataBase()
        {

            if (_comm != null)
            {
                try
                {
                    _comm.Dispose();
                }
                catch (Exception ex)
                {
                    if (_db.SqlOutputer.HasOutput)
                    {
                        OutMessage(MessageType.CacheException, ex.Message, null, ex.ToString());
                    }
                }
                finally 
                {
                    _comm = null;
                }
            }
            if (_readcomm != null)
            {
                try
                {
                    _readcomm.Dispose();
                }
                catch (Exception ex)
                {
                    if (_db.SqlOutputer.HasOutput)
                    {
                        OutMessage(MessageType.DataBaseException, ex.Message, null, ex.ToString());
                    }
                }
                finally 
                {
                    _readcomm = null;
                }
            }
            try
            {
                if (IsConnected)
                {

                    if (_db.SqlOutputer.HasOutput)
                    {
                        OutMessage(MessageType.DataBaseException, "Closed DataBase", null, "");
                    }
                    _conn.Close();
                    _conn.Dispose();
                    _dbAdapter.OnConnectionClosed(_conn, _db);
                }
                
            }
            catch (Exception ex)
            {
                if (_db.SqlOutputer.HasOutput)
                {
                    OutMessage(MessageType.DataBaseException, ex.Message, null, ex.ToString());
                }
            }
            finally
            {
                _conn = null;
                _tran = null;
            }

            try
            {
                if (IsReadConnected)
                {

                    if (_db.SqlOutputer.HasOutput)
                    {
                        OutMessage(MessageType.OtherOper, "Closed Readonly DataBase", null, "");
                    }
                    _readconn.Close();
                    _readconn.Dispose();
                    _dbAdapter.OnConnectionClosed(_readconn, _db);
                }

            }
            catch (Exception ex)
            {
                if (_db.SqlOutputer.HasOutput)
                {
                    OutMessage(MessageType.DataBaseException, ex.Message, null, ex.ToString());
                }
            }
            finally
            {
                _readconn = null;
                
            }
            return true;
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public async Task<bool> CloseDataBaseAsync()
        {

            if (_comm != null)
            {
                try
                {
                    await _comm.DisposeAsync();
                }
                catch (Exception ex)
                {
                    if (_db.SqlOutputer.HasOutput)
                    {
                        await OutMessageAsync(MessageType.CacheException, ex.Message, null, ex.ToString());
                    }
                }
                finally
                {
                    _comm = null;
                }
            }
            if (_readcomm != null)
            {
                try
                {
                    await _readcomm.DisposeAsync();
                }
                catch (Exception ex)
                {
                    if (_db.SqlOutputer.HasOutput)
                    {
                        await OutMessageAsync(MessageType.DataBaseException, ex.Message, null, ex.ToString());
                    }
                }
                finally
                {
                    _readcomm = null;
                }
            }
            try
            {
                if (IsConnected)
                {

                    if (_db.SqlOutputer.HasOutput)
                    {
                        await OutMessageAsync(MessageType.DataBaseException, "Closed DataBase", null, "");
                    }
                    await _conn.CloseAsync();
                    await _conn.DisposeAsync();
                    _dbAdapter.OnConnectionClosed(_conn, _db);
                }

            }
            catch (Exception ex)
            {
                if (_db.SqlOutputer.HasOutput)
                {
                    await OutMessageAsync(MessageType.DataBaseException, ex.Message, null, ex.ToString());
                }
            }
            finally
            {
                _conn = null;
                _tran = null;
            }

            try
            {
                if (IsReadConnected)
                {

                    if (_db.SqlOutputer.HasOutput)
                    {
                        await OutMessageAsync(MessageType.OtherOper, "Closed Readonly DataBase", null, "");
                    }
                    _readconn.Close();
                    _readconn.Dispose();
                    _dbAdapter.OnConnectionClosed(_readconn, _db);
                }

            }
            catch (Exception ex)
            {
                if (_db.SqlOutputer.HasOutput)
                {
                    await OutMessageAsync(MessageType.DataBaseException, ex.Message, null, ex.ToString());
                }
            }
            finally
            {
                _readconn = null;

            }
            return true;
        }

        /// <summary>
        /// 按照标识位自动关闭连接
        /// </summary>
        /// <returns></returns>
        internal bool AutoClose() 
        {
            if ((_commitState == CommitState.AutoCommit) && !IsTran) 
            {

                return CloseDataBase();
               
            }
            return false;
        }
        /// <summary>
        /// 按照标识位自动关闭连接
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> AutoCloseAsync()
        {
            if ((_commitState == CommitState.AutoCommit) && !IsTran)
            {

                return await CloseDataBaseAsync();
            }
            return false;
        }
        
        #endregion
        #region 查询返回所有数据
        /// <summary>
        /// 运行查询的方法,返回一个DataSet
        /// </summary>
        /// <param name="sql">要查询的SQL语句</param>
        /// <param name="sTableName">查询出来的表名</param>
        /// <param name="paramList">SqlParameter的列表</param>
        /// <returns>返回结果集</returns>
        public DataSet QueryDataSet(
			string	sql,
            ParamList paramList, Dictionary<string, bool> cacheTables)
		{

            return QueryDataSet(sql, paramList, CommandType.Text,cacheTables);
		}
        /// <summary>
        /// 获取查询的命令类
        /// </summary>
        /// <returns></returns>
		private DbCommand GetSearchCommand()
        {
            if ((!_forceMasterConnection) && (_db.HasReadOnlyConnection) && (!IsTran))
            {
                if (ReadOnlyConnectDataBase())
                {
                    return _readcomm;
                }
            }
            if (ConnectDataBase())
            {
                return _comm;
            }

            return null;
        }

        /// <summary>
        /// 获取查询的命令类
        /// </summary>
        /// <returns></returns>
        private async Task<DbCommand> GetSearchCommandAsync()
        {
            if ((!_forceMasterConnection) && (_db.HasReadOnlyConnection) && (!IsTran))
            {
                if ((await ReadOnlyConnectDataBaseAsync()))
                {
                    return _readcomm;
                }
            }
            if (await ConnectDataBaseAsync())
            {
                return _comm;
            }

            return null;
        }
        /// <summary>
        /// 运行查询的方法,返回一个DataSet
        /// </summary>
        /// <param name="sql">要查询的SQL语句</param>
        /// <param name="paramList">SqlParameter的列表</param>
        /// <param name="queryCommandType">SQL语句类型</param>
        /// <returns>返回结果集</returns>
        public DataSet QueryDataSet(
			string	sql,
			ParamList paramList,
			CommandType queryCommandType,
            Dictionary<string, bool> cacheTables
			)
		{
            if (paramList == null)
            {
                paramList = new ParamList();
            }
            DataSet dataSet = null;
            paramList = _db.CurrentDbAdapter.RebuildParamList(ref sql, paramList);
            if (cacheTables != null && cacheTables.Count > 0)
            {
                dataSet = _db.QueryCache.GetDataSet(cacheTables, sql, paramList,this);
                if (dataSet != null)
                {
                    return dataSet;
                }
            }
			//若连接数据库失败抛出错误
			//if (!ConnectDataBase())
			//{
			//	throw(new ApplicationException("没有建立数据库连接。"));
			//}
            DbCommand comm = GetSearchCommand();
            if (comm == null)
            {
                throw (new ApplicationException("没有建立数据库连接。"));
            }

            dataSet = new DataSet();
			comm.CommandType = queryCommandType;
			comm.CommandText = sql;
            IDbDataAdapter sda = _dbAdapter.GetAdapter();
			sda.SelectCommand = comm;
            string paramInfo = null;
			if(paramList!=null)
			{
                paramList.Fill(comm, _db);
                if (_db.SqlOutputer.HasOutput)
                {
                    paramInfo = paramList.GetParamString(_db,this);
                }
			}

            try
            {

                if (_db.SqlOutputer.HasOutput)
                {
                    
                    OutMessage(MessageType.Query, "DataSet", null, sql + ";" + paramInfo);
                }
                
                sda.Fill(dataSet);
                if (paramList != null)
                {
                    paramList.ReturnParameterValue(_comm, _db);
                }

                if (cacheTables != null && cacheTables.Count > 0)
                {
                    _db.QueryCache.SetDataSet(dataSet, cacheTables, sql, paramList,this);

                }
            }
            catch (Exception e)
            {
                //如果正在执行事务，回滚
                //RoolBack();
                throw new SQLRunningException(sql, paramList, _db, e);
            }
            finally 
            {
                AutoClose();
            }

            return dataSet;
		}

        /// <summary>
        /// 运行查询的方法,返回一个DataSet
        /// </summary>
        /// <param name="sql">要查询的SQL语句</param>
        /// <param name="paramList">SqlParameter的列表</param>
        /// <param name="queryCommandType">SQL语句类型</param>
        /// <returns>返回结果集</returns>
        public async Task<DataSet> QueryDataSetAsync(
            string sql,
            ParamList paramList,
            CommandType queryCommandType,
            Dictionary<string, bool> cacheTables
            )
        {
            if (paramList == null)
            {
                paramList = new ParamList();
            }
            DataSet dataSet = null;
            paramList = _db.CurrentDbAdapter.RebuildParamList(ref sql, paramList);
            if (cacheTables != null && cacheTables.Count > 0)
            {
                dataSet = _db.QueryCache.GetDataSet(cacheTables, sql, paramList, this);
                if (dataSet != null)
                {
                    return dataSet;
                }
            }
            //若连接数据库失败抛出错误
            //if (!ConnectDataBase())
            //{
            //	throw(new ApplicationException("没有建立数据库连接。"));
            //}
            DbCommand comm = await GetSearchCommandAsync();
            if (comm == null)
            {
                throw (new ApplicationException("没有建立数据库连接。"));
            }

            dataSet = new DataSet();
            comm.CommandType = queryCommandType;
            comm.CommandText = sql;
            
            IDbDataAdapter sda = _dbAdapter.GetAdapter();
            sda.SelectCommand = comm;
            string paramInfo = null;
            if (paramList != null)
            {
                paramList.Fill(comm, _db);
                if (_db.SqlOutputer.HasOutput)
                {
                    paramInfo = paramList.GetParamString(_db, this);
                }
            }

            try
            {

                if (_db.SqlOutputer.HasOutput)
                {

                    await OutMessageAsync(MessageType.Query, "DataSet", null, sql + ";" + paramInfo);
                }
                sda.Fill(dataSet);
                if (paramList != null)
                {
                    paramList.ReturnParameterValue(_comm, _db);
                }

                if (cacheTables != null && cacheTables.Count > 0)
                {
                    await _db.QueryCache.SetDataSetAsync(dataSet, cacheTables, sql, paramList,TimeSpan.MinValue, this);

                }
            }
            catch (Exception e)
            {
                //如果正在执行事务，回滚
                //RoolBack();
                throw new SQLRunningException(sql, paramList, _db, e);
            }
            finally
            {
                await AutoCloseAsync();
            }

            return dataSet;
        }
        #endregion



        /// <summary>
        /// 运行查询的方法，返回一个DataReader，适合小数据的读取
        /// </summary>
        /// <param name="sql">要查询的SQL语句</param>
        /// <param name="paramList">SqlParameter的列表</param>
        /// <returns>返回DataReader</returns>
        public DbDataReader Query(
			string	sql,
            ParamList paramList, Dictionary<string, bool> cachetables)
		{
			return Query(sql,paramList,CommandType.Text,cachetables);
		}

        /// <summary>
        /// 运行查询的方法，返回一个DataReader，适合小数据的读取
        /// </summary>
        /// <param name="sql">要查询的SQL语句</param>
        /// <param name="paramList">SqlParameter的列表</param>
        /// <param name="exeCommandType">SQL语句类型</param>
        /// <returns>返回DataReader</returns>
        public DbDataReader Query(
			string	sql,
			ParamList paramList,
			CommandType exeCommandType,
            Dictionary<string, bool> cacheTables
			)
		{
            if(paramList == null) 
            {
                paramList = new ParamList();
            }
            DbDataReader reader =null;
            paramList = _db.CurrentDbAdapter.RebuildParamList(ref sql, paramList);
            if (cacheTables != null && cacheTables.Count>0)
            {
                reader = _db.QueryCache.GetReader(cacheTables, sql, paramList,this);
                if (reader != null)
                {
                    return reader;
                }
            }

            DbCommand comm = GetSearchCommand();
            if (comm == null)
            {
                throw (new ApplicationException("没有建立数据库连接。"));
            }


            comm.CommandType = exeCommandType;
			comm.CommandText = sql;
            comm.Parameters.Clear();
            string paramInfo = null;
            if (paramList != null)
			{
                paramList.Fill(comm, _db);
                if (_db.SqlOutputer.HasOutput)
                {
                    paramInfo = paramList.GetParamString(_db,this);
                }
			}
            try
            {

                if ((_commitState == CommitState.AutoCommit) && !IsTran)
                {

                    if (_db.SqlOutputer.HasOutput)
                    {
                        OutMessage(MessageType.Query, "AutoCloseReader", null, sql + ";" + paramInfo);
                    }

                    reader = comm.ExecuteReader(CommandBehavior.CloseConnection);
                    
                    
                    
                }
                else 
                {

                    if (_db.SqlOutputer.HasOutput)
                    {
                        OutMessage(MessageType.Query, "Reader", null, sql + ";" + paramInfo);
                    }
                    
                    reader = comm.ExecuteReader();
                }
                if (paramList != null)
                {
                    paramList.ReturnParameterValue(comm, _db);
                }

                //读入缓存
                if (cacheTables != null && cacheTables.Count > 0)
                {
                    DbDataReader nreader =_db.QueryCache.SetReader(reader, cacheTables, sql, paramList,this);
                    if (nreader != null)
                    {
                        reader.Close();
                        reader = nreader;
                    }
                }
            }
            catch (Exception e)
            {
                AutoClose();
                //如果正在执行事务，回滚
                //RoolBack();
                throw new SQLRunningException(sql, paramList, _db, e);
            }
            

			return reader;
		}


        /// <summary>
        /// 运行查询的方法，返回一个DataReader，适合小数据的读取
        /// </summary>
        /// <param name="sql">要查询的SQL语句</param>
        /// <param name="paramList">SqlParameter的列表</param>
        /// <param name="exeCommandType">SQL语句类型</param>
        /// <returns>返回DataReader</returns>
        public async Task<DbDataReader> QueryAsync(
            string sql,
            ParamList paramList=null,
            CommandType exeCommandType= CommandType.Text,
            Dictionary<string, bool> cacheTables=null
            )
        {
            if (paramList == null)
            {
                paramList = new ParamList();
            }
            DbDataReader reader = null;
            paramList = _db.CurrentDbAdapter.RebuildParamList(ref sql, paramList);
            if (cacheTables != null && cacheTables.Count > 0)
            {
                reader = _db.QueryCache.GetReader(cacheTables, sql, paramList, this);
                if (reader != null)
                {
                    return reader;
                }
            }

            DbCommand comm = await GetSearchCommandAsync();
            if (comm == null)
            {
                throw (new ApplicationException("没有建立数据库连接。"));
            }


            comm.CommandType = exeCommandType;
            comm.CommandText = sql;
            comm.Parameters.Clear();
            string paramInfo = null;
            if (paramList != null)
            {
                paramList.Fill(comm, _db);
                if (_db.SqlOutputer.HasOutput)
                {
                    paramInfo = paramList.GetParamString(_db, this);
                }
            }
            try
            {

                if ((_commitState == CommitState.AutoCommit) && !IsTran)
                {

                    if (_db.SqlOutputer.HasOutput)
                    {
                        await OutMessageAsync(MessageType.Query, "AutoCloseReader", null, sql + ";" + paramInfo);
                    }

                    reader = await comm.ExecuteReaderAsync(CommandBehavior.CloseConnection);



                }
                else
                {

                    if (_db.SqlOutputer.HasOutput)
                    {
                        await OutMessageAsync(MessageType.Query, "Reader", null, sql + ";" + paramInfo);
                    }

                    reader = await comm.ExecuteReaderAsync();
                }
                if (paramList != null)
                {
                    paramList.ReturnParameterValue(comm, _db);
                }

                //读入缓存
                if (cacheTables != null && cacheTables.Count > 0)
                {
                    DbDataReader nreader = await _db.QueryCache.SetReaderAsync(reader, cacheTables, sql, paramList, this);
                    if (nreader != null)
                    {
                        await reader.CloseAsync();
                        reader = nreader;
                    }
                }
            }
            catch (Exception e)
            {
                await AutoCloseAsync();
                //如果正在执行事务，回滚
                //RoolBack();
                throw new SQLRunningException(sql, paramList, _db, e);
            }


            return reader;
        }

        /// <summary>
        /// 最后影响行数
        /// </summary>
        public int LastAffectedRows
        {
            get { return _lastAffectedRows; }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        internal bool RoolBack() 
        {
            try
            {
                if (IsTran)
                {

                    if (_db.SqlOutputer.HasOutput)
                    {
                        OutMessage(MessageType.OtherOper, "RollbackTransaction", null, "");
                    }
                    

                    _tran.Rollback();
                    _tran = null;
                    _comm.Transaction = null;
                    return true;
                }
            }
            finally
            {
                AutoClose();
            }

            return false;

        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        internal async Task<bool> RoolBackAsync()
        {
            bool ret = false;
            try
            {
                if (IsTran)
                {

                    if (_db.SqlOutputer.HasOutput)
                    {
                        await OutMessageAsync(MessageType.OtherOper, "RollbackTransaction", null, "");
                    }


                    await _tran.RollbackAsync();
                    _tran = null;
                    _comm.Transaction = null;
                    return true;
                }
            }
            finally
            {
                ret = await AutoCloseAsync();
            }

            return false;

        }
        /// <summary>
        /// 执行修改数据库操作，修改、删除等无返回值的操作
        /// </summary>
        /// <param name="sql">执行的SQL语句</param>
        /// <param name="paramList">SqlParameter的列表</param>
        /// <returns>成功执行返回True</returns>
        public int Execute(
			string	sql,
            ParamList paramList, Dictionary<string, bool> cachetables)
		{
			return Execute(sql,paramList,CommandType.Text,cachetables);
		}

		/// <summary>
		/// 执行修改数据库操作，修改、删除等无返回值的操作
		/// </summary>
		/// <param name="sql">执行的SQL语句</param>
		/// <param name="paramList">SqlParameter的列表</param>
		/// <param name="queryCommandType">SQL语句类型</param>
		/// <returns>成功执行返回True</returns>
		public int Execute(
			string	sql,
			ParamList paramList,
			CommandType exeCommandType,
            Dictionary<string, bool> cacheTables)
		{
			
			if (!ConnectDataBase())
			{
				throw(new ApplicationException("没有建立数据库连接")); 
			}
            if (paramList != null)
            {
                paramList = _db.CurrentDbAdapter.RebuildParamList(ref sql, paramList);
            }
			_comm.CommandType = exeCommandType;
			_comm.CommandText = sql;
            int ret = -1;
            _comm.Parameters.Clear();

            string paramInfo=null;
			if(paramList!=null)
			{
				paramList.Fill(_comm,_db);
                if (_db.SqlOutputer.HasOutput)
                {
                    paramInfo = paramList.GetParamString(_db,this);
                }
			}

            try
            {

                if (_db.SqlOutputer.HasOutput)
                {
                    OutMessage(MessageType.Execute, "NonQuery", null, sql + ";" + paramInfo);
                }

                ret = _comm.ExecuteNonQuery();
                _lastAffectedRows = ret;
                if (paramList != null && _comm.CommandType == CommandType.StoredProcedure)
                {
                    paramList.ReturnParameterValue(_comm, _db);
                }

                if (cacheTables != null && cacheTables.Count > 0)
                {
                    _db.QueryCache.ClearTableCache(cacheTables,this);
                }
            }
            catch (Exception e)
            {
                //如果正在执行事务，回滚
                //RoolBack();
                throw new SQLRunningException(sql, paramList, _db, e);
            }
            finally 
            {
                AutoClose();
            }

            return ret;
		}
        /// <summary>
		/// 执行修改数据库操作，修改、删除等无返回值的操作
		/// </summary>
		/// <param name="sql">执行的SQL语句</param>
		/// <param name="paramList">SqlParameter的列表</param>
		/// <param name="queryCommandType">SQL语句类型</param>
		/// <returns>成功执行返回True</returns>
		public async Task<int> ExecuteAsync(
            string sql,
            ParamList paramList,
            CommandType exeCommandType,
            Dictionary<string, bool> cacheTables)
        {

            if (!(await ConnectDataBaseAsync()))
            {
                throw (new ApplicationException("没有建立数据库连接"));
            }
            if (paramList != null)
            {
                paramList = _db.CurrentDbAdapter.RebuildParamList(ref sql, paramList);
            }
            _comm.CommandType = exeCommandType;
            _comm.CommandText = sql;
            int ret = -1;
            _comm.Parameters.Clear();

            string paramInfo = null;
            if (paramList != null)
            {
                paramList.Fill(_comm, _db);
                if (_db.SqlOutputer.HasOutput)
                {
                    paramInfo = paramList.GetParamString(_db, this);
                }
            }

            try
            {

                if (_db.SqlOutputer.HasOutput)
                {
                    await OutMessageAsync(MessageType.Execute, "NonQuery", null, sql + ";" + paramInfo);
                }

                ret = await _comm.ExecuteNonQueryAsync();
                _lastAffectedRows = ret;
                if (paramList != null && _comm.CommandType == CommandType.StoredProcedure)
                {
                    paramList.ReturnParameterValue(_comm, _db);
                }

                if (cacheTables != null && cacheTables.Count > 0)
                {
                    await _db.QueryCache.ClearTableCacheAsync(cacheTables, this);
                }
            }
            catch (Exception e)
            {
                //如果正在执行事务，回滚
                //RoolBack();
                throw new SQLRunningException(sql, paramList, _db, e);
            }
            finally
            {
                await AutoCloseAsync();
            }

            return ret;
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public DBTransaction StartTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            bool runnow = StartTran(isolationLevel);
            if (runnow)
            {
                return new DBTransaction(this);
            }
            return new DBTransaction(null);
        }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public async Task<DBTransaction> StartTransactionAsync(IsolationLevel isolationLevel= IsolationLevel.ReadCommitted)
        {
            bool runnow = await StartTranAsync(isolationLevel);
            if (runnow)
            {
                return new DBTransaction(this);
            }
            return new DBTransaction(null);
        }
        

		/// <summary>
		/// 开始事务处理功能，之后执行的全部数据库操作语句需要调用提交函数（_commit）生效
		/// </summary>
        internal bool StartTran(IsolationLevel isolationLevel)
		{
			//若连接数据库失败抛出错误
			if (!ConnectDataBase())
			{
				throw(new ApplicationException("没有建立数据库连接。"));
			}


            if (!IsTran)
            {

                if (_db.SqlOutputer.HasOutput)
                {
                    OutMessage(MessageType.OtherOper, "BeginTransaction", null, "Level=" + isolationLevel.ToString());
                }

                _tran = _conn.BeginTransaction(isolationLevel);
                _comm.Transaction = _tran;
                
                return true;
            }
            return false;

		}
        internal async Task<bool> StartTranAsync(IsolationLevel isolationLevel= IsolationLevel.ReadCommitted)
        {
            //若连接数据库失败抛出错误
            if (!( await ConnectDataBaseAsync()))
            {
                throw (new ApplicationException("没有建立数据库连接。"));
            }


            if (!IsTran)
            {

                if (_db.SqlOutputer.HasOutput)
                {
                    await OutMessageAsync(MessageType.OtherOper, "BeginTransaction", null, "Level=" + isolationLevel.ToString());
                }

                _tran = await _conn.BeginTransactionAsync(isolationLevel);
                _comm.Transaction = _tran;

                return true;
            }
            return false;

        }

        /// <summary>
        /// 当前待处理事务提交，失败全部回滚
        /// </summary>
        /// <returns></returns>
        internal bool Commit()
		{
			//如果没有开启事务处理功能，不做任何操作，直接返回成功
            if (!IsTran)
			{
				return false;
			}

            try
            {

                if (_db.SqlOutputer.HasOutput)
                {
                    OutMessage(MessageType.OtherOper, "CommitTransaction", null, "");
                }

                _tran.Commit();
                _comm.Transaction = null;
                _tran = null;
                
            }
            catch (Exception e)
            {
                //RoolBack();
                throw e;
            }
            finally 
            {
                AutoClose();
            }
			return true;
		}
        internal async Task<bool> CommitAsync()
        {
            bool ret = false;
            //如果没有开启事务处理功能，不做任何操作，直接返回成功
            if (!IsTran)
            {
                return false;
            }

            try
            {

                if (_db.SqlOutputer.HasOutput)
                {
                    await OutMessageAsync(MessageType.OtherOper, "CommitTransaction", null, "");
                }

                await _tran.CommitAsync();
                _comm.Transaction = null;
                _tran = null;

            }
            catch (Exception e)
            {
                //RoolBack();
                throw e;
            }
            finally
            {
                ret = await AutoCloseAsync();
            }
            return true;
        }

        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="messType">信息类型</param>
        /// <param name="type">具体类型</param>
        /// <param name="extendType">开展类型</param>
        /// <param name="value">值</param>
        public void OutMessage(MessageType messType, string type, string extendType, string value) 
        {
            if (_outputer!=null) 
            {
                _outputer.OutPut(messType, type, extendType, value);
            }
        }
        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="messType">信息类型</param>
        /// <param name="type">具体类型</param>
        /// <param name="extendType">开展类型</param>
        /// <param name="value">值</param>
        public void OutMessage(MessageType messType, MessageInfo info)
        {
            if (_outputer != null)
            {
                _outputer.OutPut(messType, info);
            }
        }

        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="messType">信息类型</param>
        /// <param name="type">具体类型</param>
        /// <param name="extendType">开展类型</param>
        /// <param name="value">值</param>
        public async Task OutMessageAsync(MessageType messType, string type, string extendType, string value)
        {
            if (_outputer != null)
            {
                await _outputer.OutPutAsync(messType, type, extendType, value);
            }
        }
        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="messType">信息类型</param>
        /// <param name="type">具体类型</param>
        /// <param name="extendType">开展类型</param>
        /// <param name="value">值</param>
        public async Task OutMessageAsync(MessageType messType, MessageInfo info)
        {
            if (_outputer != null)
            {
                await _outputer.OutPutAsync(messType, info);
            }
        }

        /// <summary>
        /// 信息输出器
        /// </summary>
        public MessageOutputBase MessageOutputer
        {
            get { return _outputer; }
        }
	}
}
