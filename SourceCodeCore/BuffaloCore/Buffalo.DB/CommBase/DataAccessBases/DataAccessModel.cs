using System;
using System.Data;
using System.Data.SqlClient;
using Buffalo.DB;
using Buffalo.DB.CommBase;
using System.Collections.Generic;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;

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

        /////<summary>
        /////�����޸ļ�¼
        /////</summary>
        /////<param name="entity">ʵ��</param>
        /////<returns></returns>
        //public int ConcurrencyUpdate(T entity)
        //{
        //    return base.Update(entity, null, true);
        //}
        
        

        ///<summary>
        ///�޸ļ�¼
        ///</summary>
        ///<param name="entity">ʵ��</param>
        ///<returns></returns>
        public int Update(T entity)
        {
            return Update(entity, null,null, true);
        }
        ///<summary>
        ///�޸ļ�¼
        ///</summary>
        ///<param name="entity">ʵ��</param>
        /// <param name="scopeList">��Χ���µļ���</param>
        ///<returns></returns>
        public int Update(T entity,ScopeList scopeList)
        {
            return Update(entity, scopeList,null,false);
        }

        /// <summary>
        /// �޸ļ�¼
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="scopeList">��Χ���µļ���</param>
        /// <param name="valueList">ֵ����</param>
        /// <param name="optimisticConcurrency">�Ƿ񲢷�</param>
        /// <returns></returns>
        public int Update(T entity, ScopeList scopeList, ValueSetList valueList, bool optimisticConcurrency) 
        {
            return base.Update(entity, scopeList, valueList, optimisticConcurrency);
        }

        /// <summary>
        /// �����¼
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <returns></returns>
        public new int Insert(T entity, bool fillIdentity)
        {
            return base.Insert(entity,null, fillIdentity);
        }

        /// <summary>
        /// ɾ����¼
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="scopeList">��Χɾ���ļ���</param>
        /// <returns></returns>
        public int Delete(T entity)
        {
            return base.Delete(entity, null,true);
        }

        ///// <summary>
        ///// ����ɾ��
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public int ConcurrencyDelete(T entity)
        //{
        //    return base.Delete(entity, null, true);
        //}
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
    }
}
