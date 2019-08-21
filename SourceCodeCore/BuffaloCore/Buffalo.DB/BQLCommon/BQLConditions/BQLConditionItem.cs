using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.Kernel.Defaults;
using System.Collections;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using System.Data;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.BQLCommon
{
    public delegate string DelConditionHandle(string sourceTable, string[] paramList, DBInfo db);
    public class BQLConditionItem : BQLCondition
    {
        private DelConditionHandle _handle;
        private IEnumerable _paramList;
        private BQLQuery _query;
        private BQLValueItem _sourceHandle;
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sourceHandle">����Դ(�ֶ�)</param>
        /// <param name="paramList">�����б�</param>
        /// <param name="handle">����������</param>
        public BQLConditionItem(BQLValueItem sourceHandle, IEnumerable paramList, DelConditionHandle handle) 
        {
            this._sourceHandle = sourceHandle;
            this._handle = handle;
            this._paramList = paramList;
            this._valueDbType = DbType.Boolean;
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sourceHandle">����Դ(�ֶ�)</param>
        /// <param name="query">��ѯ</param>
        /// <param name="handle">����������</param>
        public BQLConditionItem(BQLParamHandle sourceHandle, BQLQuery query, DelConditionHandle handle)
        {
            this._sourceHandle = sourceHandle;
            this._handle = handle;
            this._query = query;
            this._valueDbType = DbType.Boolean;
        }

        internal override void FillInfo(KeyWordInfomation info)
        {
            BQLValueItem.DoFillInfo(_sourceHandle, info);
            if (_paramList != null)
            {
                List<BQLValueItem> lst = new List<BQLValueItem>();
                foreach (object item in _paramList)
                {
                    BQLValueItem value = BQLValueItem.ToValueItem(item);
                    lst.Add(value);
                    BQLValueItem.DoFillInfo(value, info);
                }
                _paramList = lst;
            }
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            if (_handle != null)
            {
                if (_query != null) 
                {
                    KeyWordConver conver = new KeyWordConver();
                    KeyWordInfomation qInfo = info.Clone() as KeyWordInfomation;
                    return "(" + _handle(_sourceHandle.DisplayValue(info), new string[] { conver.ToConver(_query, qInfo).GetSql(false) },info.DBInfo) + ")";
                }
                else if (_paramList != null)
                {
                    List<string> lstPrm = new List<string>();
                    foreach (object item in _paramList)
                    {
                        lstPrm.Add(BQLValueItem.ToValueItem(item).DisplayValue(info));
                    }
                    return "(" + _handle(_sourceHandle.DisplayValue(info), lstPrm.ToArray(),info.DBInfo) + ")";
                }
            }
            return null;
        }
    }
}
