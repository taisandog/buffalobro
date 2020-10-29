using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel.Defaults;
using System.Text;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.MessageOutPuters;
//ParamList v1.1
namespace Buffalo.DB.DbCommon
{
	
	/// <summary>
	/// SqlParameter���б�
	/// </summary>
	public class ParamList:List<DBParameter>
	{
        ///// <summary>
        ///// �Ѿ����ڵ����ݿ�ֵ
        ///// </summary>
        //private Dictionary<string, DBParameter> _dicExistsValue = new Dictionary<string, DBParameter>();

        /// <summary>
        /// �µļ���
        /// </summary>
        /// <returns></returns>
        private string NewKeyName(DBInfo db) 
        {
            string pName = "P" + this.Count;
            return db.CurrentDbAdapter.FormatParamKeyName(pName);
        }

        /// <summary>
        /// �µ�ֵ��
        /// </summary>
        /// <returns></returns>
        private string NewValueKeyName(DBInfo db)
        {
            string pName = "P" + this.Count;
            return db.CurrentDbAdapter.FormatValueName(pName);
        }

        //private Dictionary<string, bool> _tables;
        ///// <summary>
        ///// �����������ı�
        ///// </summary>
        //public Dictionary<string, bool> Tables
        //{
        //    get { return _tables; }
        //    internal set { _tables = value; }
        //}
        ///// <summary>
        ///// ��ʼ���������Ϣ
        ///// </summary>
        ///// <returns></returns>
        //internal bool SetCacheTable(string tabelName) 
        //{
        //    if (_tables == null)
        //    {
        //        _tables = new Dictionary<string, bool>();
        //    }
        //    _tables[tabelName] = true;
        //    return true;
        //}
        /// <summary>
        /// �µ����ݿ�ֵ
        /// </summary>
        /// <param name="type">���ݿ�����</param>
        /// <param name="paramValue">ֵ����</param>
        /// <returns></returns>
        public DBParameter NewParameter(DbType type, object paramValue, DBInfo db)
        {
            string pKeyName = null;



            DBParameter prm = null;

            pKeyName = NewKeyName(db);
            string valueName = NewValueKeyName(db);
            prm = AddNew(pKeyName, type, paramValue);
            prm.ValueName = valueName;

            return prm;
        }


		/// <summary>
        /// ����һ���µ�IDataParameter����ӵ��б�
		/// </summary>
        /// <param name="paramName">IDataParameterҪӳ�����������</param>
		/// <param name="type">��������</param>
		/// <param name="paramValue">������ֵ</param>
        public DBParameter AddNew(string paramName, DbType type, object paramValue)
		{
			return AddNew(paramName,type,paramValue,ParameterDirection.Input);
        }
        /// <summary>
        /// ����һ���µ�IDataParameter����ӵ��б�
		/// </summary>
        /// <param name="paramName">IDataParameterҪӳ�����������</param>
		/// <param name="type">��������</param>
		/// <param name="paramValue">������ֵ�����Ϊ����Ĳ����������Ϊnull��</param>
		/// <param name="paramDir">��������������</param>
        public DBParameter AddNew(string paramName, DbType type, object paramValue, ParameterDirection paramDir)
		{
            if (paramValue is Enum)
            {
                paramValue = DefaultValue.ConvertEnumUnderlyingValue(paramValue);
            }
            DBParameter newParam = new DBParameter(paramName, type, paramValue, paramDir);
            this.Add (newParam);
            return newParam;
		}
		
		/// <summary>
		/// ���б���ߵ�SqlParameter�ӵ�SqlCommand���
		/// </summary>
		/// <param name="comm">Ҫ�ӽ�ȥ��SqlCommand</param>
		public void Fill(IDbCommand comm,DBInfo db)
		{
            StringBuilder ret = new StringBuilder(500);
            comm.Parameters.Clear();


            for (int i = 0; i < this.Count; i++)
            {
                DBParameter prm = this[i];
                IDataParameter dPrm = GetParameter(prm,db);
                comm.Parameters.Add(dPrm);
            }

            //if (db.SqlOutputer.HasOutput)
            //{
            //    return GetParamString(db);
            //}

            //return null;
		}
        /// <summary>
        /// ��ȡparam��ߵ�ֵ����ʾ�ַ���
        /// </summary>
        /// <returns></returns>
        public string GetParamString(DBInfo db,DataBaseOperate oper)
        {
            bool showBinary=false;
            int hideTextLength = 0;
            MessageOutputBase msg=oper.MessageOutputer;
            if (msg != null) 
            {
                showBinary = msg.ShowBinary;
                hideTextLength = msg.HideTextLength;
            }
            return GetParamString(db, showBinary, hideTextLength);
        }
        /// <summary>
        /// ��ȡparam��ߵ�ֵ����Ϣ
        /// </summary>
        /// <param name="db">���ݿ�</param>
        /// <param name="showBinary">�Ƿ���ʾbyte[]</param>
        /// <param name="hideTextLength">���ַ������ȴ������ֵʱ��������ֵ</param>
        /// <returns></returns>
        public string GetParamString(DBInfo db, bool showBinary,int hideTextLength) 
        {
            StringBuilder ret = new StringBuilder(500);
            for (int i = 0; i < this.Count; i++)
            {
                DBParameter prm = this[i];
                IDataParameter dPrm = GetParameter(prm, db);

                string value = null;
                if (dPrm.Value != null)
                {
                    if (dPrm.DbType == DbType.Binary && !showBinary)
                    {
                        Array val = dPrm.Value as Array;
                        if (val != null)
                        {
                            value = "[Binary len=" + val.Length + "]";
                        }
                        else
                        {
                            value = "[Binary null]";
                        }
                    }
                    else if (hideTextLength > 0)
                    {
                        string strValue = dPrm.Value as string;
                        if (strValue != null && strValue.Length > hideTextLength) 
                        {
                            value = "[String len=" + strValue.Length + "]";
                        }
                    }
                   
                }
                if (value == null)
                {
                    value = DataAccessCommon.FormatValue(dPrm.Value, dPrm.DbType, db);
                }
                ret.Append(dPrm.ParameterName);
                ret.Append("(");
                ret.Append(dPrm.DbType);
                ret.Append(")");
                ret.Append("=");
                ret.Append(value);
                ret.Append(",");
            }

            if (ret.Length > 0) 
            {
                ret.Remove(ret.Length - 1, 1);
            }
            return ret.ToString();
        }

        /// <summary>
        /// ��ȡʵ�����ݿ���ֶα���
        /// </summary>
        /// <param name="prm"></param>
        /// <returns></returns>
        private IDataParameter GetParameter(DBParameter prm, DBInfo db)
        {
            object value = prm.Value;
            if (value is Guid)
            {
                value = Buffalo.Kernel.CommonMethods.GuidToString((Guid)value);
            }
            DbType dbType = prm.DbType;
            if (dbType == DbType.Guid) 
            {
                dbType = DbType.AnsiString;
            }
            return db.CurrentDbAdapter.GetDataParameter(prm.ParameterName, dbType, value, prm.Direction);
        }

        /// <summary>
        /// ���ر�����ֵ
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="db"></param>
        public void ReturnParameterValue(IDbCommand comm, DBInfo db) 
        {
            for (int i = 0; i < this.Count; i++)
            {
                DBParameter prm = this[i];
                if (prm.Direction != ParameterDirection.Input) 
                {
                    IDataParameter dPrm = comm.Parameters[i] as IDataParameter;
                    if (dPrm != null) 
                    {
                        prm.Value = dPrm.Value;
                    }
                }
            }
        }

		/// <summary>
		/// ��ȡ�ڼ���SqlParameter
		/// </summary>
        public DBParameter this[string paramName]
		{
			get
			{
                for (int i = 0; i < this.Count; i++) 
                {
                    DBParameter tmpParam = this[i];
                    if(tmpParam.ParameterName==paramName)
                    {
                        return tmpParam;
                    }
                }
                return null;
			}
			
		}

        


	}
}
