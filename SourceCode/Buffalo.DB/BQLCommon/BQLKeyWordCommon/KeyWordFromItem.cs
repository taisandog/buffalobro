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
        /// From关键字项
        /// </summary>
        /// <param name="tables">表集合</param>
        /// <param name="previous">上一个关键字</param>
        public KeyWordFromItem(BQLTableHandle[] tables, BQLQuery previous)
            : base(previous) 
        {
            this._tables = tables;
        }
        
        ///// <summary>
        ///// 要查询的字段
        ///// </summary>
        //internal BQLTableHandle[] Tables 
        //{
        //    get 
        //    {
        //        return tables;
        //    }
        //}

        /// <summary>
        /// 左连接
        /// </summary>
        /// <param name="jionTable">表</param>
        /// <param name="on">条件</param>
        /// <returns></returns>
        public KeyWordJoinItem LeftJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "left", this);
            return item;
        }
        /// <summary>
        /// 左外连接
        /// </summary>
        /// <param name="jionTable">表</param>
        /// <param name="on">条件</param>
        /// <returns></returns>
        public KeyWordJoinItem LeftOuterJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "left outer", this);
            return item;
        }
        /// <summary>
        /// 右连接
        /// </summary>
        /// <param name="jionTable">表</param>
        /// <param name="on">条件</param>
        /// <returns></returns>
        public KeyWordJoinItem RightJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "right", this);
            return item;
        }
        /// <summary>
        /// 右外连接
        /// </summary>
        /// <param name="jionTable">表</param>
        /// <param name="on">条件</param>
        /// <returns></returns>
        public KeyWordJoinItem RightOuterJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "right outer", this);
            return item;
        }
        /// <summary>
        /// 内连接
        /// </summary>
        /// <param name="jionTable">表</param>
        /// <param name="on">条件</param>
        /// <returns></returns>
        public KeyWordJoinItem InnerJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "inner", this);
            return item;
        }
        /// <summary>
        /// 交叉连接
        /// </summary>
        /// <param name="jionTable">表</param>
        /// <param name="on">条件</param>
        /// <returns></returns>
        public KeyWordJoinItem CrossJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "cross", this);
            return item;
        }
        /// <summary>
        /// 交叉外连接
        /// </summary>
        /// <param name="jionTable">表</param>
        /// <param name="on">条件</param>
        /// <returns></returns>
        public KeyWordJoinItem CrossOuterJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "cross outer", this);
            return item;
        }
        /// <summary>
        /// 全连接
        /// </summary>
        /// <param name="jionTable">表</param>
        /// <param name="on">条件</param>
        /// <returns></returns>
        public KeyWordJoinItem FullJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "full", this);
            return item;
        }
        /// <summary>
        /// 全连接
        /// </summary>
        /// <param name="jionTable">表</param>
        /// <param name="on">条件</param>
        /// <returns></returns>
        public KeyWordJoinItem FullOuterJoin(BQLTableHandle joinTable, BQLCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "full outer", this);
            return item;
        }
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public KeyWordWhereItem Where(BQLCondition condition) 
        {
            KeyWordWhereItem item = new KeyWordWhereItem(condition, this);
            return item;
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="paramhandles"></param>
        /// <returns></returns>
        public KeyWordOrderByItem OrderBy(params BQLParamHandle[] paramhandles)
        {
            KeyWordOrderByItem item = new KeyWordOrderByItem(paramhandles, this);
            return item;
        }
        /// <summary>
        /// 分组
        /// </summary>
        /// <param name="paramhandles"></param>
        /// <returns></returns>
        public KeyWordGroupByItem GroupBy(params BQLParamHandle[] paramhandles)
        {
            KeyWordGroupByItem item = new KeyWordGroupByItem(paramhandles, this);
            return item;
        }
        /// <summary>
        /// 查询范围
        /// </summary>
        /// <param name="star">开始条数</param>
        /// <param name="totalRecord">显示条数</param>
        /// <returns></returns>
        public KeyWordLimitItem Limit(uint star, uint totalRecord)
        {
            KeyWordLimitItem item = new KeyWordLimitItem(star, totalRecord, this);
            return item;
        }
        /// <summary>
        /// 锁定行
        /// </summary>
        /// <param name="noWait">如果冲突是否不等待</param>
        /// <returns></returns>
        public KeyWorkLockUpdateItem LockUpdate(BQLLockType type) 
        {
            KeyWorkLockUpdateItem item=new KeyWorkLockUpdateItem(type, this);
            return item;
        }
        ///// <summary>
        ///// 查询范围
        ///// </summary>
        ///// <param name="star">开始条数(从0开始)</param>
        ///// <param name="totalRecord">显示条数</param>
        ///// <returns></returns>
        //public KeyWordLimitItem Limit(uint star, uint totalRecord) 
        //{
        //    KeyWordLimitItem item = new KeyWordLimitItem(star, totalRecord, this);
        //    return item;
        //}

        /// <summary>
        /// 加载表的别名信息
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
        /// from关键字
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
                    if (info.LockType != BQLLockType.None) //加锁
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
