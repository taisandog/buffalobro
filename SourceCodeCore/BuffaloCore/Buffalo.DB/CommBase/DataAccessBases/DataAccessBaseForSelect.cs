using System;
using System.Data;
using System.Data.SqlClient;
using Buffalo.DB;
using Buffalo.DB.CommBase;
using System.Collections.Generic;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using System.Threading.Tasks;

namespace Buffalo.DB.CommBase.DataAccessBases
{
    /// <summary>
    /// 只有Select语句的数据访问层模版
    /// </summary>
    public class DataAccessBaseForSelect<T> : DataAccessBase<T>, IViewDataAccess<T> where T : EntityBase, new()
    {
        /// <summary>
        /// 只有Select语句的数据访问层模版
        /// </summary>
        /// <param name="oper"></param>
        public DataAccessBaseForSelect(DataBaseOperate oper)
        {
            Oper = oper;
            
            
            
        }
        /// <summary>
        /// 只有Select语句的数据访问层模版
        /// </summary>
        public DataAccessBaseForSelect()
        {
            Oper = StaticConnection.GetDefaultOperate<T>();
        }

        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual T GetEntityById(object id, bool isSearchByCache = false)
        {
            return base.GetObjectById(id, isSearchByCache);
        }

        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual Task<T> GetEntityByIdAsync(object id, bool isSearchByCache = false)
        {
            return base.GetObjectByIdAsync(id, isSearchByCache);
        }

    }
}
