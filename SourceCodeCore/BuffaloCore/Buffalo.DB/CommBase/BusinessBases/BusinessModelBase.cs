using System;
using System.Data;
using System.Data.SqlClient;
using Buffalo.DB;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.CacheManager;
using System.Threading.Tasks;

namespace Buffalo.DB.CommBase.BusinessBases
{
    public abstract class BusinessModelBase<T> : BusinessModelBaseForSelect<T>
        where T : EntityBase, new()
    {
        public BusinessModelBase()
		{
			
		}
        /// <summary>
        /// ���º����ʱ���жϸö����Ƿ��Ѿ�����
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="oper">�������Ӷ���</param>
        /// <returns>û�д��ھͷ���null�����򷵻�һ��ֵ</returns>
        protected virtual object Exists(T entity) 
        {
            return null;
        }
        /// <summary>
        /// ���º����ʱ���жϸö����Ƿ��Ѿ�����
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="oper">�������Ӷ���</param>
        /// <returns>û�д��ھͷ���null�����򷵻�һ��ֵ</returns>
        protected virtual async Task<object> ExistsAsync(T entity)
        {
            return null;
        }

        /// <summary>
        /// ɾ��ʱ���жϸö����Ƿ񻹴����Ӽ�¼
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="oper">�������Ӷ���</param>
        /// <returns>û�д��ھͷ���null�����򷵻�һ��ֵ</returns>
        protected virtual object HasChild(T entity,ScopeList lstScope) 
        {
            return null;
        }
        /// <summary>
        /// ɾ��ʱ���жϸö����Ƿ񻹴����Ӽ�¼
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="oper">�������Ӷ���</param>
        /// <returns>û�д��ھͷ���null�����򷵻�һ��ֵ</returns>
        protected virtual async Task<object> HasChildAsync(T entity, ScopeList lstScope)
        {
            return null;
        }
       
        /// <summary>
        /// ɾ��ʱ���жϸö����Ƿ񻹴����Ӽ�¼
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="oper">�������Ӷ���</param>
        /// <returns>û�д��ھͷ���null�����򷵻�һ��ֵ</returns>
        protected virtual object HasChildById(object id)
        {
            return null;
        }
        /// <summary>
        /// ɾ��ʱ���жϸö����Ƿ񻹴����Ӽ�¼
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="oper">�������Ӷ���</param>
        /// <returns>û�д��ھͷ���null�����򷵻�һ��ֵ</returns>
        protected virtual async Task<object> HasChildByIdAsync(object id)
        {
            return null;
        }
        /// <summary>
        ///  �˴�ִ�е�Ӱ������
        /// </summary>
        protected int _affectedRows=-1;

        /// <summary>
        /// �˴�ִ�е�Ӱ������
        /// </summary>
        public int AffectedRows
        {
            get
            {
                return _affectedRows;
            }
        }





        #region Update
       
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="scorpList">��Χ���µ��б� ��Ϊnullʱ�� entity.���� ������Ч</param>
        /// <param name="lstValue">setʵ�� ���б�ĸ���ֵ������entity</param>
        /// <param name="optimisticConcurrency">��������</param>
        /// <returns>null:�������,��Ϊnull:����ʧ��</returns>
        public virtual object Update(T entity, ScopeList scorpList=null, ValueSetList lstValue = null, bool optimisticConcurrency=false)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = null;
            if (entity != null)
            {
                ret = Exists(entity);
                if (ret != null)
                {
                    return ret;
                }
            }
            _affectedRows = entityDao.Update(entity, scorpList, lstValue, optimisticConcurrency);
            return ret;

        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="scorpList">��Χ���µ��б� ��Ϊnullʱ�� entity.���� ������Ч</param>
        /// <param name="lstValue">setʵ�� ���б�ĸ���ֵ������entity</param>
        /// <param name="optimisticConcurrency">��������</param>
        /// <returns>null:�������,��Ϊnull:����ʧ��</returns>
        public virtual async Task<object> UpdateAsync(T entity, ScopeList scorpList = null, ValueSetList lstValue = null, bool optimisticConcurrency = false)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = null;
            if (entity != null)
            {
                ret = await ExistsAsync(entity);
                if (ret != null)
                {
                    return ret;
                }
            }

            _affectedRows = await entityDao.UpdateAsync(entity, scorpList, lstValue, optimisticConcurrency);


            return ret;

        }
       
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="lst">�����б�</param>
        /// <returns>null:�������,��Ϊnull:����ʧ��</returns>
        public virtual object Update(List<T> lst)
        {
            _affectedRows = 0;

            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = null;
            
            foreach (T entity in lst)
            {
                ret = Exists(entity);
                if (ret != null)
                {
                    continue;
                }

                _affectedRows += entityDao.Update(entity);

            }

            return ret;

        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="lst">�����б�</param>
        /// <returns>null:�������,��Ϊnull:����ʧ��</returns>
        public virtual async Task<object> UpdateAsync(List<T> lst)
        {
            _affectedRows = 0;

            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = null;

            foreach (T entity in lst)
            {
                ret = ExistsAsync(entity);
                if (ret != null)
                {
                    continue;
                }

                _affectedRows += await entityDao.UpdateAsync(entity,null,null,false);

            }

            return ret;

        }

        #endregion

        #region Insert
        
        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="setList">set��ֵ</param>
        /// <param name="fillIdentity">�Ƿ��������������</param>
        /// <returns>null:�������,��Ϊnull:����ʧ��</returns>
        public virtual object Insert(T entity,ValueSetList setList=null, bool fillIdentity = false)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = Exists(entity);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = entityDao.Insert(entity, setList, fillIdentity);

            return ret;
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="setList">set��ֵ</param>
        /// <param name="fillIdentity">�Ƿ��������������</param>
        /// <returns>null:�������,��Ϊnull:����ʧ��</returns>
        public virtual async Task<object> InsertAsync(T entity, ValueSetList setList = null, bool fillIdentity = false)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = await ExistsAsync(entity);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = await entityDao.InsertAsync(entity, setList, fillIdentity);

            return ret;
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="isConcurrency">�Ƿ�����</param>
        /// <returns>null:ɾ�����,��Ϊnull:ɾ��ʧ��</returns>
        public virtual object Delete(T entity,bool isConcurrency = false)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = HasChild(entity,null);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = entityDao.Delete(entity, null, isConcurrency);
            return ret;
        }
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="isConcurrency">�Ƿ�����</param>
        /// <returns>null:ɾ�����,��Ϊnull:ɾ��ʧ��</returns>
        public virtual async Task<object> DeleteAsync(T entity, bool isConcurrency = false)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = await HasChildAsync(entity, null);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = await entityDao.DeleteAsync(entity, null, isConcurrency);
            return ret;
        }

        /// <summary>
        /// ��ձ�
        /// </summary>

        public void TruncateTable()
        {
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            _affectedRows = entityDao.TruncateTable();

        }
        /// <summary>
        /// ��ձ�
        /// </summary>

        public async Task TruncateTableAsync()
        {
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            _affectedRows = await entityDao.TruncateTableAsync();

        }

        /// <summary>
        /// ����IDɾ����¼
        /// </summary>
        /// <param name="id">Ҫɾ���ļ�¼ID</param>
        /// <returns>null:ɾ�����,��Ϊnull:ɾ��ʧ��</returns>
        public virtual object DeleteById(object id)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = HasChildById(id);
            if (ret != null)
            {
                return ret;
            }
            _affectedRows = entityDao.DeleteById(id);

            return ret;
        }
        /// <summary>
        /// ����IDɾ����¼
        /// </summary>
        /// <param name="id">Ҫɾ���ļ�¼ID</param>
        /// <returns>null:ɾ�����,��Ϊnull:ɾ��ʧ��</returns>
        public virtual async Task<object> DeleteByIdAsync(object id)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = await HasChildByIdAsync(id);
            if (ret != null)
            {
                return ret;
            }
            _affectedRows = await entityDao.DeleteByIdAsync(id);

            return ret;
        }
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="lstScope">����ɾ������������</param>
        /// <returns>null:ɾ�����,��Ϊnull:ɾ��ʧ��</returns>
        public virtual object Delete(ScopeList lstScope)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();


            object ret = HasChild(null,lstScope);
            if (ret != null)
            {
                return ret;
            }

            
            _affectedRows = entityDao.Delete(lstScope);
            return ret;
        }
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="lstScope">����ɾ������������</param>
        /// <returns>null:ɾ�����,��Ϊnull:ɾ��ʧ��</returns>
        public virtual async Task<object> DeleteAsync(ScopeList lstScope)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();


            object ret = await HasChildAsync(null, lstScope);
            if (ret != null)
            {
                return ret;
            }


            _affectedRows = await entityDao.DeleteAsync(lstScope);
            return ret;
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="lst">���ݼ���</param>
        /// <returns>����0:ɾ�����,С��0:ɾ��ʧ��</returns>
        public virtual object Delete(List<T> lst)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object res = null;
            
            foreach (T entity in lst)
            {
                res = HasChild(entity, null);
                if (res != null)
                {
                    continue;
                }
                _affectedRows += entityDao.Delete(entity);
            }

            return res;
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="lst">���ݼ���</param>
        /// <returns>����0:ɾ�����,С��0:ɾ��ʧ��</returns>
        public virtual async Task<object> DeleteAsync(List<T> lst)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object res = null;

            foreach (T entity in lst)
            {
                res = await HasChildAsync(entity, null);
                if (res != null)
                {
                    continue;
                }
                _affectedRows += await entityDao.DeleteAsync(entity);
            }

            return res;
        }
        #endregion
    }
}