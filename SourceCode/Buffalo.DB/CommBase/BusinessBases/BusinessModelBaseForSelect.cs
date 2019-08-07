using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.BQLCommon;

namespace Buffalo.DB.CommBase.BusinessBases
{
    public class BusinessModelBaseForSelect<T> where T : EntityBase, new()
    {
        protected readonly static EntityInfoHandle _curEntityInfo = EntityInfoManager.GetEntityHandle(typeof(T));

        protected readonly static DBInfo _db = _curEntityInfo.DBInfo;

        /// <summary>
        /// ִ�в�ѯ֮ǰ�������¼�
        /// </summary>
        /// <param name="lstScope"></param>
        /// <returns></returns>
        protected virtual void OnSelect(ScopeList lstScope) 
        {
        }

        private DataBaseOperate _defaultOperate;

        /// <summary>
        /// ��ȡĬ������
        /// </summary>
        protected DataBaseOperate DefaultOperate 
        {
            get 
            {
                return _defaultOperate;
            }
        }
        /// <summary>
        /// ҵ���Ĳ�ѯ����
        /// </summary>
        public BusinessModelBaseForSelect()
        {
            _defaultOperate = _db.DefaultOperate;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        protected DBTransaction StartTransaction() 
        {

            return DefaultOperate.StartTransaction() ;
        }

        /// <summary>
        /// ��ʼ���������������
        /// </summary>
        /// <returns></returns>
        protected BatchAction StartBatchAction()
        {

            return DefaultOperate.StarBatchAction();
        }

        /// <summary>
        /// ������������ʵ��
        /// </summary>
        /// <param name="id">����</param>
        /// <returns></returns>
        public virtual T GetEntityById(object id)
        {
            DataAccessBaseForSelect<T> dao = new DataAccessBaseForSelect<T>();
            return dao.GetEntityById(id);
        }
        /// <summary>
        /// ������������ʵ��(ʹ�û���)
        /// </summary>
        /// <param name="id">����</param>
        /// <returns></returns>
        public virtual T GetByIdUseCache(object id)
        {
            DataAccessBaseForSelect<T> dao = new DataAccessBaseForSelect<T>();
            return dao.GetObjectById(id,true);
        }


        /// <summary>
        /// ֱ�Ӳ�ѯ���ݿ���ͼ
        /// </summary>
        /// <param name="tableName">������</param>
        /// <param name="lstScope">����</param>
        /// <param name="vParams">�ֶ��б�</param>
        /// <param name="lstSort">��������</param>
        /// <param name="lstSort">����</param>
        /// <returns></returns>
        public DataSet SelectTable(string tableName,  ScopeList lstScope)
        {
            OnSelect(lstScope);
            BQLDataAccessBase<T> dao = new BQLDataAccessBase<T>();
            return dao.SelectTable(tableName, lstScope);
        }

        /// <summary>
        /// ��ѯ����������ͼ
        /// </summary>
        /// <param name="table"></param>
        /// <param name="vParams"></param>
        /// <param name="lstScope"></param>
        /// <param name="objPage"></param>
        /// <returns></returns>
        public DataSet SelectTable(BQLOtherTableHandle table,  ScopeList lstScope)
        {
            OnSelect(lstScope);
            BQLDataAccessBase<T> dao = new BQLDataAccessBase<T>();
            return dao.SelectTable(table, lstScope);
        }
        
        /// <summary>
        /// ������������ʵ��
        /// </summary>
        /// <param name="lstScope">����</param>
        /// <returns></returns>
        public T GetUnique(ScopeList lstScope)
        {
            //OnSelect(lstScope);
            //DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            //T ret = null;

            //ret = entityDao.GetUnique(lstScope);
            PageContent oldPage = lstScope.PageContent;
            lstScope.PageContent = new PageContent();
            lstScope.PageContent.PageSize = 1;
            lstScope.PageContent.CurrentPage = 0;
            lstScope.PageContent.IsFillTotalRecords = false;
            List<T> lst = SelectList(lstScope);
            lstScope.PageContent = oldPage;
            if (lst.Count > 0) 
            {
                return lst[0];
            }
            return null;
        }
        #region SelectByAll
        

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="lstScope">��Χ����</param>
        /// <param name="lstSort">������������</param>
        /// <returns></returns>
        public virtual DataSet Select(ScopeList lstScope)
        {
            OnSelect(lstScope);
            DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            DataSet ret = null;
                ret = entityDao.Select(lstScope);
            return ret;
        }
        
        
        /// <summary>
        /// ����(���ؼ���)
        /// </summary>
        /// <param name="lstScope">��Χ����</param>
        /// <returns></returns>
        public virtual List<T> SelectList(ScopeList lstScope)
        {
            OnSelect(lstScope);
            DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            List<T> ret = null;
                ret = entityDao.SelectList(lstScope);
            return ret;
        }
        #endregion

        #region SelectCount
        
        /// <summary>
        /// ��ѯ����ָ�������ļ�¼����
        /// </summary>
        /// <param name="scpoeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public virtual long SelectCount(ScopeList scpoeList)
        {
            OnSelect(scpoeList);
            DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            long ret = 0;
                ret = entityDao.SelectCount(scpoeList);
            return ret;
        }
        #endregion

        #region SelectExists
        
        
        /// <summary>
        /// ��ѯ����ָ�������ļ�¼����
        /// </summary>
        /// <param name="scpoeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public virtual bool ExistsRecord(ScopeList scpoeList)
        {
            OnSelect(scpoeList);
            DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            bool ret = false;
                ret = entityDao.ExistsRecord(scpoeList);
            return ret;
        }
        #endregion
    }
}
