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
    /// ֻ��Select�������ݷ��ʲ�ģ��
    /// </summary>
    public class DataAccessBaseForSelect<T> : DataAccessBase<T>, IViewDataAccess<T> where T : EntityBase, new()
    {
        /// <summary>
        /// ֻ��Select�������ݷ��ʲ�ģ��
        /// </summary>
        /// <param name="oper"></param>
        public DataAccessBaseForSelect(DataBaseOperate oper)
        {
            Oper = oper;
            
            
            
        }
        /// <summary>
        /// ֻ��Select�������ݷ��ʲ�ģ��
        /// </summary>
        public DataAccessBaseForSelect()
        {
            Oper = StaticConnection.GetDefaultOperate<T>();
        }

        /// <summary>
        /// ������������ʵ��
        /// </summary>
        /// <param name="id">����</param>
        /// <returns></returns>
        public virtual T GetEntityById(object id, bool isSearchByCache = false)
        {
            return base.GetObjectById(id, isSearchByCache);
        }

        /// <summary>
        /// ������������ʵ��
        /// </summary>
        /// <param name="id">����</param>
        /// <returns></returns>
        public virtual Task<T> GetEntityByIdAsync(object id, bool isSearchByCache = false)
        {
            return base.GetObjectByIdAsync(id, isSearchByCache);
        }

    }
}
