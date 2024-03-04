using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.CommBase.DataAccessBases.AliasTableMappingManagers;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordFromItem : BQLQuery
    {
        protected BQLTableHandle[] _tables;
        
        /// <summary>
        /// From�ؼ�����
        /// </summary>
        /// <param name="tables">����</param>
        /// <param name="previous">��һ���ؼ���</param>
        public KeyWordFromItem(BQLTableHandle[] tables, BQLQuery previous)
            : base(previous) 
        {
            this._tables = tables;
        }
        
        ///// <summary>
        ///// Ҫ��ѯ���ֶ�
        ///// </summary>
        //internal BQLTableHandle[] Tables 
        //{
        //    get 
        //    {
        //        return tables;
        //    }
        //}

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="jionTable">��</param>
        /// <param name="on">����</param>
        /// <returns></returns>
        public KeyWordJoinItem LeftJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "left", this);
            return item;
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="jionTable">��</param>
        /// <param name="on">����</param>
        /// <returns></returns>
        public KeyWordJoinItem LeftOuterJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "left outer", this);
            return item;
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="jionTable">��</param>
        /// <param name="on">����</param>
        /// <returns></returns>
        public KeyWordJoinItem RightJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "right", this);
            return item;
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="jionTable">��</param>
        /// <param name="on">����</param>
        /// <returns></returns>
        public KeyWordJoinItem RightOuterJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "right outer", this);
            return item;
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="jionTable">��</param>
        /// <param name="on">����</param>
        /// <returns></returns>
        public KeyWordJoinItem InnerJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "inner", this);
            return item;
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="jionTable">��</param>
        /// <param name="on">����</param>
        /// <returns></returns>
        public KeyWordJoinItem CrossJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "cross", this);
            return item;
        }
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="jionTable">��</param>
        /// <param name="on">����</param>
        /// <returns></returns>
        public KeyWordJoinItem CrossOuterJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "cross outer", this);
            return item;
        }
        /// <summary>
        /// ȫ����
        /// </summary>
        /// <param name="jionTable">��</param>
        /// <param name="on">����</param>
        /// <returns></returns>
        public KeyWordJoinItem FullJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "full", this);
            return item;
        }
        /// <summary>
        /// ȫ����
        /// </summary>
        /// <param name="jionTable">��</param>
        /// <param name="on">����</param>
        /// <returns></returns>
        public KeyWordJoinItem FullOuterJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "full outer", this);
            return item;
        }
        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="condition">����</param>
        /// <returns></returns>
        public KeyWordWhereItem Where(BQLCondition condition) 
        {
            KeyWordWhereItem item = new KeyWordWhereItem(condition, this);
            return item;
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="paramhandles"></param>
        /// <returns></returns>
        public KeyWordOrderByItem OrderBy(params BQLParamHandle[] paramhandles)
        {
            KeyWordOrderByItem item = new KeyWordOrderByItem(paramhandles, this);
            return item;
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="paramhandles"></param>
        /// <returns></returns>
        public KeyWordGroupByItem GroupBy(params BQLParamHandle[] paramhandles)
        {
            KeyWordGroupByItem item = new KeyWordGroupByItem(paramhandles, this);
            return item;
        }
        /// <summary>
        /// ��ѯ��Χ
        /// </summary>
        /// <param name="star">��ʼ����</param>
        /// <param name="totalRecord">��ʾ����</param>
        /// <returns></returns>
        public KeyWordLimitItem Limit(uint star, uint totalRecord)
        {
            KeyWordLimitItem item = new KeyWordLimitItem(star, totalRecord, this);
            return item;
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="noWait">�����ͻ�Ƿ񲻵ȴ�</param>
        /// <returns></returns>
        public KeyWorkLockUpdateItem LockUpdate(BQLLockType type) 
        {
            KeyWorkLockUpdateItem item=new KeyWorkLockUpdateItem(type, this);
            return item;
        }
        ///// <summary>
        ///// ��ѯ��Χ
        ///// </summary>
        ///// <param name="star">��ʼ����(��0��ʼ)</param>
        ///// <param name="totalRecord">��ʾ����</param>
        ///// <returns></returns>
        //public KeyWordLimitItem Limit(uint star, uint totalRecord) 
        //{
        //    KeyWordLimitItem item = new KeyWordLimitItem(star, totalRecord, this);
        //    return item;
        //}

        /// <summary>
        /// ���ر�ı�����Ϣ
        /// </summary>
        /// <param name="info"></param>
        internal override void LoadInfo(KeyWordInfomation info) 
        {
            foreach (BQLTableHandle tab in _tables) 
            {

                if (CommonMethods.IsNull(info.FromTable)) 
                {
                    BQLEntityTableHandle etab = tab as BQLEntityTableHandle;
                    if (!CommonMethods.IsNull(etab))
                    {
                        info.FromTable = etab;
                    }
                }
                BQLValueItem.DoFillInfo(tab, info);
            }
            
        }

        /// <summary>
        /// from�ؼ���
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal override void Tran(KeyWordInfomation info)
        {
            StringBuilder ret = new StringBuilder();

            StringBuilder retNoLock = new StringBuilder();
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;

            if (info.AliasManager == null)
            {
                for (int i = 0; i < _tables.Length; i++)
                {
                    BQLTableHandle table = _tables[i];
                    string tableName = table.DisplayValue(info);
                    ret.Append(tableName);
                    retNoLock.Append(tableName);
                    if (info.LockType != BQLLockType.None) //����
                    {
                        string lockSql = idba.ShowFromLockUpdate(info.LockType, info.DBInfo);
                        if (!string.IsNullOrWhiteSpace(lockSql))
                        {
                            ret.Append(" ");
                            ret.Append(lockSql);
                        }
                    }
                    if (i < _tables.Length - 1)
                    {
                        ret.Append(",");
                        retNoLock.Append(",");
                    }
                    
                }
                info.Condition.Tables.Append(ret.ToString());
                SelectCondition scon = info.Condition as SelectCondition;
                if (scon != null)
                {
                    scon.TablesNoLock.Append(retNoLock.ToString());
                }
            }
            else 
            {
                for (int i = 0; i < _tables.Length; i++)
                {
                    BQLTableHandle table = _tables[i];

                    BQLAliasHandle ahandle= info.AliasManager.GetPrimaryAliasHandle(table);
                    if (!Buffalo.Kernel.CommonMethods.IsNull(ahandle)) 
                    {
                        _tables[i] = ahandle;
                    }
                }


                KeyWordFromItem from=info.AliasManager.ToInnerTable(this,info);
                TableAliasNameManager manager = info.AliasManager;
                info.AliasManager = null;
                Stack<KeyWordFromItem> stkFrom = new Stack<KeyWordFromItem>();
                while (from != null) 
                {
                    stkFrom.Push(from);
                    from = from.Previous as KeyWordFromItem;
                }
                while (stkFrom.Count > 0)
                {
                    from = stkFrom.Pop();
                    from.Tran(info);
                }
                info.AliasManager = manager;
            }
            
            
        }



        
    }
}
