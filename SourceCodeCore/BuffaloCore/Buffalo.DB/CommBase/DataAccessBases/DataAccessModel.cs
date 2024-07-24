using System;
using System.Data;
using System.Data.SqlClient;
using Buffalo.DB;
using Buffalo.DB.CommBase;
using System.Collections.Generic;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using System.Threading.Tasks;

namespace Buffalo.DB.CommBase.DataAccessBases
{
    /// <summary>
    /// ���ݷ��ʲ�ģ��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataAccessModel<T> :DataAccessBaseForSelect<T>, IDataAccessModel<T> where T : EntityBase, new()
    {
        public DataAccessModel(DataBaseOperate oper):base(oper)
        {
            
        }
        public DataAccessModel():base()
        {
            
        }
        
        /// <summary>
        /// ��Χɾ����¼(����)
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="scopeList">��Χɾ���ļ���</param>
        /// <returns></returns>
        public int Delete( ScopeList scopeList)
        {
            return base.Delete(default(T),scopeList,false);
        }
        /// <summary>
        /// ��Χɾ����¼(����)
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="scopeList">��Χɾ���ļ���</param>
        /// <returns></returns>
        public Task<int> DeleteAsync(ScopeList scopeList)
        {
            return base.DeleteAsync(default(T), scopeList, false);
        }
    }
}
