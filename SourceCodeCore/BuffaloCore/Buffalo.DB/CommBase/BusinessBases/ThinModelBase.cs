using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.BQLCommon;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.CommBase.BusinessBases
{
    /// <summary>
    /// С�ͼܹ�����
    /// </summary>
    public class ThinModelBase:EntityBase
    {
        private DataAccessSetBase _dal;

        /// <summary>
        /// ��ȡ�������ݲ�
        /// </summary>
        /// <returns></returns>
        private DataAccessSetBase GetBaseDataAccess() 
        {
            if (_dal == null) 
            {
                EntityInfoHandle handle=EntityInfoManager.GetEntityHandle(CH.GetRealType(this));
                _dal = new DataAccessSetBase(handle);
                _dal.Oper = StaticConnection.GetStaticOperate(handle.DBInfo);
            }
            return _dal;
        }

        #region �����޸�
        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert() 
        {
            return Insert(null,true);
        }

        /// <summary>
        /// ��ѯʱ�򴥷�
        /// </summary>
        /// <param name="lstScope"></param>
        /// <returns></returns>
        protected internal virtual void OnSelect(ScopeList lstScope) 
        {
            
        }

        /// <summary>
        /// ��Χ����ǰ����
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="scorpList">��Χ��������</param>
        /// <param name="lstValue">��ֵ</param>
        /// <param name="optimisticConcurrency">�Ƿ��ò�����</param>
        /// <returns>�Ƿ����ִ�и���</returns>
        protected internal virtual bool BeforeUpdateByScope(EntityBase entity, ScopeList scorpList, ValueSetList lstValue, bool optimisticConcurrency)
        {
            return true;
        }


        /// <summary>
        /// ��Χ���º󴥷�
        /// </summary>
        /// <param name="affected">Ӱ������</param>
        /// <param name="entity">ʵ��</param>
        /// <param name="scorpList">��Χ��������</param>
        /// <param name="lstValue">��ֵ</param>
        /// <param name="optimisticConcurrency">�Ƿ��ò�����</param>
        protected internal virtual void AfterUpdateByScope(int affected, EntityBase entity, ScopeList scorpList, ValueSetList lstValue, bool optimisticConcurrency)
        {

        }

        /// <summary>
        /// ��Χɾ��ǰ����
        /// </summary>
        /// <param name="scorpList">��Χ��������</param>
        /// <returns>�Ƿ����ִ�и���</returns>
        protected internal virtual bool BeforeDeleteByScope(ScopeList scorpList)
        {
            return true;
        }


        /// <summary>
        /// ��Χɾ���󴥷�
        /// </summary>
        /// <param name="affected">Ӱ������</param>
        /// <param name="scorpList">��Χ��������</param>
        protected internal virtual void AfterDeleteByScope(int affected, ScopeList scorpList)
        {

        }

        /// <summary>
        /// ����ʵ�岢���ID
        /// </summary>
        /// <param name="fillPrimaryKey">�Ƿ����ʵ��</param>
        /// <returns></returns>
        public virtual int Insert(bool fillPrimaryKey)
        {
            return Insert(null, fillPrimaryKey);
        }
        /// <summary>
        /// ����ʵ�岢���ID
        /// </summary>
        /// <param name="fillPrimaryKey">�Ƿ����ʵ��</param>
        /// <returns></returns>
        public virtual int Insert(ValueSetList setList, bool fillPrimaryKey)
        {
            DataAccessSetBase dal = GetBaseDataAccess();

            return dal.DoInsert(this, setList, fillPrimaryKey);
        }
        /// <summary>
        /// ����ʵ��
        /// </summary>
        ///  <param name="optimisticConcurrency">�Ƿ񲢷�����</param>
        /// <returns></returns>
        public int Update(bool optimisticConcurrency) 
        {
            return Update(null, optimisticConcurrency);
        }
        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <param name="lstValue">ǿ������ֵ</param>
        ///  <param name="optimisticConcurrency">�Ƿ񲢷�����</param>
        /// <returns></returns>
        public virtual int Update(ValueSetList lstValue,bool optimisticConcurrency)
        {
            DataAccessSetBase dal = GetBaseDataAccess();
            foreach (EntityPropertyInfo epPk in dal.EntityInfo.PrimaryProperty)
            {
                object id = epPk.GetValue(this);
                if (DefaultType.IsDefaultValue(id))
                {
                    throw new Exception("����:" + epPk.PropertyName + "��ֵ����Ϊ��");
                }
            }
            return dal.Update(this, null, lstValue, optimisticConcurrency);
        }
        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <returns></returns>
        public int Update()
        {
            return Update(null,false);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="optimisticConcurrency">�Ƿ񲢷�����</param>
        /// <returns></returns>
        public virtual int Delete(bool optimisticConcurrency) 
        {
            DataAccessSetBase dal = GetBaseDataAccess();
            ScopeList lstScope = new ScopeList();
            foreach (EntityPropertyInfo pInfo in dal.EntityInfo.PrimaryProperty)
            {
                object id = pInfo.GetValue(this);
                if (DefaultType.IsDefaultValue(id))
                {
                    throw new Exception("����:" + pInfo.PropertyName + "��ֵ����Ϊ��");
                }
                lstScope.AddEqual(pInfo.PropertyName, id);
            }
            
            
            return dal.Delete(this, lstScope, optimisticConcurrency);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <returns></returns>
        public int Delete()
        {
            return Delete(false);
        }


        
        #endregion
    }
}
