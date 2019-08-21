using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.CommBase.BusinessBases
{
    /// <summary>
    /// ģ�Ͳ㸨����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelContext<T> where T : ThinModelBase, new()
    {
        T obj = null;
        protected internal readonly static EntityInfoHandle CurEntityInfo = EntityInfoManager.GetEntityHandle(typeof(T));
        /// <summary>
        /// ģ�Ͳ㸨����
        /// </summary>
        public ModelContext() 
        {
            obj = (T)Activator.CreateInstance(typeof(T));

        }
        private DataAccessSetBase _dal;

        /// <summary>
        /// ��ȡ���ݲ�(��������SQL)
        /// </summary>
        /// <returns></returns>
        public DataAccessSetBase GetDAL()
        {
            if (_dal == null)
            {
                EntityInfoHandle handle = EntityInfoManager.GetEntityHandle(typeof(T));
                _dal = new DataAccessSetBase(handle);
                _dal.Oper = StaticConnection.GetStaticOperate(handle.DBInfo);
            }
            return _dal;
        }
        /// <summary>
        /// ��ȡ���ݲ����
        /// </summary>
        /// <returns></returns>
        public DataAccessBase<T> GetBaseContext()
        {
            DataAccessBase<T> baseDal = new DataAccessBase<T>();
            baseDal.Oper = StaticConnection.GetDefaultOperate<T>();
            return baseDal;
        }

        /// <summary>
        /// ��ȡִ���﷨��������
        /// </summary>
        /// <returns></returns>
        public BQLDbBase GetContext()
        {
            return GetBaseContext().ContextDAL;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public DBTransaction StartTransaction()
        {
            DataAccessBase<T> baseDal = GetBaseContext();

            return baseDal.Oper.StartTransaction();
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="lstScope">��Χ����</param>
        /// <returns></returns>
        public DataSet Select(ScopeList lstScope)
        {
            obj.OnSelect(lstScope);
            return GetBaseContext().Select(lstScope);
        }


        /// <summary>
        /// ��ѯ����ָ�������ļ�¼����
        /// </summary>
        /// <param name="scpoeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public long SelectCount(ScopeList scpoeList)
        {
            obj.OnSelect(scpoeList);
            return GetBaseContext().SelectCount(scpoeList);
        }

        
        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="scpoeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public List<T> SelectList(ScopeList scpoeList)
        {
            obj.OnSelect(scpoeList);
            return GetBaseContext().SelectList(scpoeList);
        }
        /// <summary>
        /// ��ʼ���������������
        /// </summary>
        /// <returns></returns>
        public BatchAction StartBatchAction()
        {
            return GetBaseContext().Oper.StarBatchAction();
        }

        /// <summary>
        /// ��ǰʵ���ת��ֵ
        /// </summary>
        private T GetThisValue()
        {
            return this as T;
        }

        /// <summary>
        /// ��Χ����(����)
        /// </summary>
        /// <param name="lstValue">����ֵ</param>
        /// <param name="lstScope">����</param>
        /// <returns></returns>
        public int UpdateByScope(T obj , ValueSetList lstValue, ScopeList lstScope)
        {
            int ret = 0;
            bool conUpdate=this.obj.BeforeUpdateByScope(obj, lstScope, lstValue,false);
            if (conUpdate)
            {
                ret = GetBaseContext().Update(obj, lstScope, lstValue, false);
            }
            this.obj.AfterUpdateByScope(ret,obj, lstScope, lstValue, false);
            return ret;

        }
        /// <summary>
        /// ��Χ����(����)
        /// </summary>
        /// <param name="lstScope">����</param>
        /// <returns></returns>
        public int UpdateByScope(T obj, ScopeList lstScope)
        {
            int ret = 0;
            bool conUpdate = this.obj.BeforeUpdateByScope(obj, lstScope, null, false);
            if (conUpdate)
            {
                ret = GetBaseContext().Update(obj, lstScope, null, false);
            }
            this.obj.AfterUpdateByScope(ret,obj, lstScope, null, false);
            return ret;

        }

        /// <summary>
        /// ��ѯ�Ƿ��м�¼���ϴ�����
        /// </summary>
        /// <param name="scpoeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public bool ExistsRecord(ScopeList scpoeList)
        {
            obj.OnSelect(scpoeList);
            return GetBaseContext().ExistsRecord(scpoeList);
        }

        /// <summary>
        /// ��Χɾ��(����)
        /// </summary>
        /// <param name="lstScope"></param>
        /// <returns></returns>
        public int DeleteByScope(ScopeList lstScope)
        {
            int ret = 0;
            bool conUpdate = this.obj.BeforeDeleteByScope( lstScope);
            if (conUpdate)
            {
                ret = GetBaseContext().Delete(null, lstScope, false);
            }
            this.obj.AfterDeleteByScope(ret, lstScope);
            return ret;
        }

        /// <summary>
        /// ������������ʵ��
        /// </summary>
        /// <param name="id">����</param>
        /// <returns></returns>
        public T GetEntityById(object id)
        {
            ScopeList lstScope = new ScopeList();
            PrimaryKeyInfo info = id as PrimaryKeyInfo;
            if (info == null)
            {
                lstScope.AddEqual(CurEntityInfo.PrimaryProperty[0].PropertyName, id);
            }
            else 
            {
                info.FillScope(CurEntityInfo.PrimaryProperty, lstScope, true);
            }



            return GetUnique(lstScope);
        }
        /// <summary>
        /// ������������ʵ��
        /// </summary>
        /// <param name="lstScope">����</param>
        /// <returns></returns>
        public T GetUnique(ScopeList lstScope)
        {
            //obj.OnSelect(lstScope);
            lstScope.PageContent.PageSize = 1;
            lstScope.PageContent.CurrentPage = 0;
            lstScope.PageContent.IsFillTotalRecords = false;
            List<T> lst = SelectList(lstScope);
            if (lst.Count > 0)
            {
                return lst[0];
            }

            return null;
        }

    }
}
