using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWorkLockUpdateItem : BQLQuery
    {
        private BQLLockType _type = BQLLockType.None;
        /// <summary>
        /// 锁更新
        /// </summary>
        /// <param name="isNoWait">是否不堵塞</param>
        /// <param name="previous"></param>
        public KeyWorkLockUpdateItem(BQLLockType lockType, BQLQuery previous) : base(previous)
        {
            _type = lockType;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {
            info.LockType = _type;

        }

        internal override void Tran(KeyWordInfomation info)
        {
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            string lockSql = idba.LockUpdate(_type, info.DBInfo);
            if (!string.IsNullOrWhiteSpace(lockSql))
            {
                info.Condition.LockUpdate.Append(" ");
                info.Condition.LockUpdate.Append(lockSql);
            }
        }
    }
    /// <summary>
    /// 锁类型
    /// </summary>
    public enum BQLLockType 
    {
        /// <summary>
        /// 不加锁
        /// </summary>
        None=0,
        /// <summary>
        /// 锁Update
        /// </summary>
        LockUpdate=1,
        /// <summary>
        ///  锁Update，不堵塞，如果出错则直接返回错误
        /// </summary>
        LockUpdateNoWait = 2,
        /// <summary>
        ///  锁Update，跳过被锁的记录，返回后边的记录
        /// </summary>
        LockUpdateSkipLock = 3
    }
}
