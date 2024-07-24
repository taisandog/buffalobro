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

///ͨ��SQL Server������v1.2

namespace Buffalo.DB.DbCommon
{
    /// <summary>
    /// �ύ��ʽ
    /// </summary>
    public enum CommitState 
    {
        /// <summary>
        /// �Զ��ύ(�ر�����)
        /// </summary>
        AutoCommit,
        /// <summary>
        /// �ֶ��ύ(�ر�����)
        /// </summary>
        UserCommit
    }

	/// <summary>
	/// ���ݿ������
	/// </summary>
	public class DataBaseOperate : IDisposable,IAsyncDisposable
	{
		
		
		/// <summary>
        /// ���ݿ����ӱ�־
		/// </summary>
        private bool IsConnected 
        {
            get 
            {

                return  ( _conn != null && _conn.State != ConnectionState.Closed);
            }
        }
        /// <summary>
        /// ֻ�����ӵı�־
        /// </summary>
        private bool IsReadConnected
        {
            get
            {

                return (_readconn != null && _readconn.State != ConnectionState.Closed);
            }
        }

        //���ݿ����Ӷ���
        private DbConnection _conn;

        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                if (!ConnectDataBase())
                {
                    throw (new ApplicationException("û�н������ݿ����ӡ�"));
                }
                return _conn;
            }
        }
        private DbConnection _readconn;

        /// <summary>
        /// ֻ�����ݿ�����
        /// </summary>
        public DbConnection ReadOnlyConnection
        {
            get
            {
                if (!ReadOnlyConnectDataBase())
                {
                    throw (new ApplicationException("û�н������ݿ����ӡ�"));
                }
                return _readconn;
            }
        }
        
        private DbCommand _comm;
        /// <summary>
        /// ֻ��������
        /// </summary>
        private DbCommand _readcomm;
        private DbTransaction _tran;
		//private IDbDataAdapter _sda;

        private int _lastAffectedRows;
        /// <summary>
        /// ǿ���������Ӳ���
        /// </summary>
        private bool _forceMasterConnection;

        private IDBAdapter _dbAdapter = null;
        
        DBInfo _db = null;
        /// <summary>
        /// �����
        /// </summary>
        MessageOutputBase _outputer;

       
        /// <summary>
        /// �����Ƿ��Ѿ�����
        /// </summary>
        private bool IsTran 
        {
            get 
            {
                return _tran != null;
            }
        }

        //�Ƿ��Զ��ر�����
        private CommitState _commitState = CommitState.AutoCommit;

        string _databaseName;

        /// <summary>
        /// ��ȡ���ݿ�����
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
                        //���������ݿ�ʧ���׳�����
                        if (!ConnectDataBase())
                        {
                            throw (new ApplicationException("û�н������ݿ����ӡ�"));
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
        /// �Ƿ��Զ��ر�����
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
        /// ǿ�������Ӳ���
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
        /// �������ݿ���Ϣ
        /// </summary>
        public DBInfo DBInfo 
        {
            get 
            {
                return _db;
            }
        }
        /// <summary>
        /// �������������������
        /// </summary>
        /// <returns></returns>
        public BatchAction StarBatchAction() 
        {
            BatchAction action = new BatchAction(this);
            return action;
        }

		/// <summary>
		/// ʵ�������ݿ���ʶ���
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
        /// ���ش� System.Data.Common.DbConnection ������Դ�ļܹ���Ϣ
        /// </summary>
        /// <returns></returns>
        public DataTable GetSchema() 
        {
            DataTable dt = null;
            try
            {
                if (!ConnectDataBase())
                {
                    throw (new ApplicationException("û�н������ݿ����ӡ�"));
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
        /// ���ش� System.Data.Common.DbConnection ������Դ�ļܹ���Ϣ
        /// </summary>
        /// <param name="collectionName">ָ��Ҫ���صļܹ�������</param>
        /// <returns></returns>
        public DataTable GetSchema(string collectionName)
        {
            DataTable dt = null;
            try
            {
                if (!ConnectDataBase())
                {
                    throw (new ApplicationException("û�н������ݿ����ӡ�"));
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
        /// ���ش� System.Data.Common.DbConnection ������Դ�ļܹ���Ϣ
        /// </summary>
        /// <param name="collectionName">ָ��Ҫ���صļܹ�������</param>
        /// <param name="restrictionValues">Ϊ����ļܹ�ָ��һ������ֵ</param>
        /// <returns></returns>
        public DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            DataTable dt = null;
            try
            {
                if (!ConnectDataBase())
                {
                    throw (new ApplicationException("û�н������ݿ����ӡ�"));
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
		/// ����ֻ�����ݿ⣬�������ݿ�����
		/// </summary>
		/// <returns>�ɹ�����true</returns>
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
		/// ����ֻ�����ݿ⣬�������ݿ�����
		/// </summary>
		/// <returns>�ɹ�����true</returns>
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
        /// �������ݿ⣬�������ݿ�����
        /// </summary>
        /// <returns>�ɹ�����true</returns>
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
        /// �������ݿ⣬�������ݿ�����
        /// </summary>
        /// <returns>�ɹ�����true</returns>
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
                        OutMessage(MessageType.OtherOper, "Connect DataBase", null, "");
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

        #region IDisposable ��Ա
        /// <summary>
        /// �ͷ�ռ����Դ
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
        /// �ر����ݿ�����
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
        /// �ر����ݿ�����
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
                    await _readcomm.DisposeAsync();
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
                    await _conn.CloseAsync();
                    await _conn.DisposeAsync();
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
        /// ���ձ�ʶλ�Զ��ر�����
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
        /// ���ձ�ʶλ�Զ��ر�����
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
        #region ��ѯ������������
        /// <summary>
        /// ���в�ѯ�ķ���,����һ��DataSet
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="sTableName">��ѯ�����ı���</param>
        /// <param name="paramList">SqlParameter���б�</param>
        /// <returns>���ؽ����</returns>
        public DataSet QueryDataSet(
			string	sql,
            ParamList paramList, Dictionary<string, bool> cacheTables)
		{

            return QueryDataSet(sql, paramList, CommandType.Text,cacheTables);
		}
        /// <summary>
        /// ��ȡ��ѯ��������
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
        /// ��ȡ��ѯ��������
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
        /// ���в�ѯ�ķ���,����һ��DataSet
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="paramList">SqlParameter���б�</param>
        /// <param name="queryCommandType">SQL�������</param>
        /// <returns>���ؽ����</returns>
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
			//���������ݿ�ʧ���׳�����
			//if (!ConnectDataBase())
			//{
			//	throw(new ApplicationException("û�н������ݿ����ӡ�"));
			//}
            DbCommand comm = GetSearchCommand();
            if (comm == null)
            {
                throw (new ApplicationException("û�н������ݿ����ӡ�"));
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
                //�������ִ�����񣬻ع�
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
        /// ���в�ѯ�ķ���,����һ��DataSet
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="paramList">SqlParameter���б�</param>
        /// <param name="queryCommandType">SQL�������</param>
        /// <returns>���ؽ����</returns>
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
            //���������ݿ�ʧ���׳�����
            //if (!ConnectDataBase())
            //{
            //	throw(new ApplicationException("û�н������ݿ����ӡ�"));
            //}
            DbCommand comm = await GetSearchCommandAsync();
            if (comm == null)
            {
                throw (new ApplicationException("û�н������ݿ����ӡ�"));
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

                    OutMessage(MessageType.Query, "DataSet", null, sql + ";" + paramInfo);
                }
                sda.Fill(dataSet);
                if (paramList != null)
                {
                    paramList.ReturnParameterValue(_comm, _db);
                }

                if (cacheTables != null && cacheTables.Count > 0)
                {
                    _db.QueryCache.SetDataSet(dataSet, cacheTables, sql, paramList, this);

                }
            }
            catch (Exception e)
            {
                //�������ִ�����񣬻ع�
                //RoolBack();
                throw new SQLRunningException(sql, paramList, _db, e);
            }
            finally
            {
                AutoClose();
            }

            return dataSet;
        }
        #endregion



        /// <summary>
        /// ���в�ѯ�ķ���������һ��DataReader���ʺ�С���ݵĶ�ȡ
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="paramList">SqlParameter���б�</param>
        /// <returns>����DataReader</returns>
        public DbDataReader Query(
			string	sql,
            ParamList paramList, Dictionary<string, bool> cachetables)
		{
			return Query(sql,paramList,CommandType.Text,cachetables);
		}

        /// <summary>
        /// ���в�ѯ�ķ���������һ��DataReader���ʺ�С���ݵĶ�ȡ
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="paramList">SqlParameter���б�</param>
        /// <param name="exeCommandType">SQL�������</param>
        /// <returns>����DataReader</returns>
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
                throw (new ApplicationException("û�н������ݿ����ӡ�"));
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

                //���뻺��
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
                //�������ִ�����񣬻ع�
                //RoolBack();
                throw new SQLRunningException(sql, paramList, _db, e);
            }
            

			return reader;
		}


        /// <summary>
        /// ���в�ѯ�ķ���������һ��DataReader���ʺ�С���ݵĶ�ȡ
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="paramList">SqlParameter���б�</param>
        /// <param name="exeCommandType">SQL�������</param>
        /// <returns>����DataReader</returns>
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
                throw (new ApplicationException("û�н������ݿ����ӡ�"));
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
                        OutMessage(MessageType.Query, "AutoCloseReader", null, sql + ";" + paramInfo);
                    }

                    reader = await comm.ExecuteReaderAsync(CommandBehavior.CloseConnection);



                }
                else
                {

                    if (_db.SqlOutputer.HasOutput)
                    {
                        OutMessage(MessageType.Query, "Reader", null, sql + ";" + paramInfo);
                    }

                    reader = await comm.ExecuteReaderAsync();
                }
                if (paramList != null)
                {
                    paramList.ReturnParameterValue(comm, _db);
                }

                //���뻺��
                if (cacheTables != null && cacheTables.Count > 0)
                {
                    DbDataReader nreader = _db.QueryCache.SetReader(reader, cacheTables, sql, paramList, this);
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
                //�������ִ�����񣬻ع�
                //RoolBack();
                throw new SQLRunningException(sql, paramList, _db, e);
            }


            return reader;
        }

        /// <summary>
        /// ���Ӱ������
        /// </summary>
        public int LastAffectedRows
        {
            get { return _lastAffectedRows; }
        }

        /// <summary>
        /// �ع�����
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
        /// �ع�����
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
                        OutMessage(MessageType.OtherOper, "RollbackTransaction", null, "");
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
        /// ִ���޸����ݿ�������޸ġ�ɾ�����޷���ֵ�Ĳ���
        /// </summary>
        /// <param name="sql">ִ�е�SQL���</param>
        /// <param name="paramList">SqlParameter���б�</param>
        /// <returns>�ɹ�ִ�з���True</returns>
        public int Execute(
			string	sql,
            ParamList paramList, Dictionary<string, bool> cachetables)
		{
			return Execute(sql,paramList,CommandType.Text,cachetables);
		}

		/// <summary>
		/// ִ���޸����ݿ�������޸ġ�ɾ�����޷���ֵ�Ĳ���
		/// </summary>
		/// <param name="sql">ִ�е�SQL���</param>
		/// <param name="paramList">SqlParameter���б�</param>
		/// <param name="queryCommandType">SQL�������</param>
		/// <returns>�ɹ�ִ�з���True</returns>
		public int Execute(
			string	sql,
			ParamList paramList,
			CommandType exeCommandType,
            Dictionary<string, bool> cacheTables)
		{
			
			if (!ConnectDataBase())
			{
				throw(new ApplicationException("û�н������ݿ�����")); 
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
                //�������ִ�����񣬻ع�
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
		/// ִ���޸����ݿ�������޸ġ�ɾ�����޷���ֵ�Ĳ���
		/// </summary>
		/// <param name="sql">ִ�е�SQL���</param>
		/// <param name="paramList">SqlParameter���б�</param>
		/// <param name="queryCommandType">SQL�������</param>
		/// <returns>�ɹ�ִ�з���True</returns>
		public async Task<int> ExecuteAsync(
            string sql,
            ParamList paramList,
            CommandType exeCommandType,
            Dictionary<string, bool> cacheTables)
        {

            if (!(await ConnectDataBaseAsync()))
            {
                throw (new ApplicationException("û�н������ݿ�����"));
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
                    OutMessage(MessageType.Execute, "NonQuery", null, sql + ";" + paramInfo);
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
                //�������ִ�����񣬻ع�
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
        /// ��������
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
        /// ��������
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
		/// ��ʼ�������ܣ�֮��ִ�е�ȫ�����ݿ���������Ҫ�����ύ������_commit����Ч
		/// </summary>
        internal bool StartTran(IsolationLevel isolationLevel)
		{
			//���������ݿ�ʧ���׳�����
			if (!ConnectDataBase())
			{
				throw(new ApplicationException("û�н������ݿ����ӡ�"));
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
            //���������ݿ�ʧ���׳�����
            if (!( await ConnectDataBaseAsync()))
            {
                throw (new ApplicationException("û�н������ݿ����ӡ�"));
            }


            if (!IsTran)
            {

                if (_db.SqlOutputer.HasOutput)
                {
                    OutMessage(MessageType.OtherOper, "BeginTransaction", null, "Level=" + isolationLevel.ToString());
                }

                _tran = await _conn.BeginTransactionAsync(isolationLevel);
                _comm.Transaction = _tran;

                return true;
            }
            return false;

        }

        /// <summary>
        /// ��ǰ�����������ύ��ʧ��ȫ���ع�
        /// </summary>
        /// <returns></returns>
        internal bool Commit()
		{
			//���û�п����������ܣ������κβ�����ֱ�ӷ��سɹ�
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
            //���û�п����������ܣ������κβ�����ֱ�ӷ��سɹ�
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
        /// �����Ϣ
        /// </summary>
        /// <param name="messType">��Ϣ����</param>
        /// <param name="type">��������</param>
        /// <param name="extendType">��չ����</param>
        /// <param name="value">ֵ</param>
        public void OutMessage(MessageType messType, string type, string extendType, string value) 
        {
            if (_outputer!=null) 
            {
                _outputer.OutPut(messType, type, extendType, value);
            }
        }
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="messType">��Ϣ����</param>
        /// <param name="type">��������</param>
        /// <param name="extendType">��չ����</param>
        /// <param name="value">ֵ</param>
        public void OutMessage(MessageType messType, MessageInfo info)
        {
            if (_outputer != null)
            {
                _outputer.OutPut(messType, info);
            }
        }

        

        /// <summary>
        /// ��Ϣ�����
        /// </summary>
        public MessageOutputBase MessageOutputer
        {
            get { return _outputer; }
        }
	}
}
