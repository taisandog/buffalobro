using System;
using System.Data;
using System.Data.SqlClient;
using Buffalo.DB;
using Buffalo.DB.CommBase;
using System.Collections.Generic;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;

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
        ///// <summary>
        ///// ������������ʵ��
        ///// </summary>
        ///// <param name="lstScope">����</param>
        ///// <returns></returns>
        //public virtual new T GetUnique(ScopeList lstScope)
        //{
        //    return base.GetUnique(lstScope);
        //}
        /// <summary>
        /// ������������ʵ��
        /// </summary>
        /// <param name="id">����</param>
        /// <returns></returns>
        public virtual T GetEntityById(object id)
        {
            return base.GetObjectById(id,false);
        }
        
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="lstScope">��Χ����</param>
        /// <returns></returns>
        public virtual new DataSet Select(ScopeList lstScope)
        {
            return base.Select(lstScope);
        }
        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="scpoeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public virtual new List<T> SelectList(ScopeList scpoeList)
        {
            return base.SelectList(scpoeList);
        }
       
        /// <summary>
        /// ��ѯ����ָ�������ļ�¼����
        /// </summary>
        /// <param name="scpoeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public virtual new long SelectCount(ScopeList scpoeList)
        {
            return base.SelectCount(scpoeList);
        }
        
        /// <summary>
        /// ��ѯ����ָ�������ļ�¼����
        /// </summary>
        /// <param name="scpoeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public virtual new  bool ExistsRecord(ScopeList scpoeList)
        {
            return base.ExistsRecord(scpoeList);
        }
    }
}
