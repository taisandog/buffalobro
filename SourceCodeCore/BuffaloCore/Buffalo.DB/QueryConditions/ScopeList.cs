using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Buffalo.DB.ContantSearchs;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.Kernel;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.QueryConditions
{
    /// <summary>
    /// ���������ļ���
    /// </summary>
    public class ScopeList:List<Scope>
    {
        /// <summary>
        /// ���������ļ���
        /// </summary>
        public ScopeList() 
        {
            _showEntity = new ShowEntityCollection(this);
            _groupBy = new ScopePropertyCollection(this);
            _showProperty = new ScopePropertyCollection(this);
            _hideProperty = new ScopePropertyCollection(this);
            _showChild = new ShowChildCollection();
        }

        

        /// <summary>
        /// ��ȡ����Ҫ��ʾ�����Լ���
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public List<BQLParamHandle> GetShowProperty(BQLTableHandle table)
        {
            List<BQLParamHandle> propertys = new List<BQLParamHandle>();
            if (_showProperty != null && _showProperty.Count > 0)
            {
                foreach (BQLParamHandle param in _showProperty)
                {

                    propertys.Add(param);

                }
                return propertys;
            }



            BQLEntityTableHandle eTable = table as BQLEntityTableHandle;
            if (!CommonMethods.IsNull(eTable))
            {
                foreach (EntityPropertyInfo info in eTable.GetEntityInfo().PropertyInfo)
                {
                    string name = info.PropertyName;
                    bool hidden = false;
                    foreach (BQLParamHandle param in _hideProperty)
                    {
                        BQLEntityParamHandle eph = param as BQLEntityParamHandle;
                        if (!CommonMethods.IsNull(eph))
                        {
                            if (eph.PInfo.PropertyName == name)
                            {
                                hidden = true;
                                break;
                            }
                        }
                        

                    }
                    if (!hidden)
                    {
                        propertys.Add(eTable[info.PropertyName]);
                    }
                }
                return propertys;

            }
            if (!CommonMethods.IsNull(table))
            {
                propertys.Add(table._);
            }
            return propertys;
        }

        private ScopePropertyCollection _groupBy =null;

        /// <summary>
        /// ��ѯʱ��Ҫ������Щ���Եļ���
        /// </summary>
        public ScopePropertyCollection GroupBy
        {
            get
            {
                return _groupBy;
            }
        }
        private bool _useCache;
        /// <summary>
        /// �Ƿ�ʹ�û���
        /// </summary>
        public bool UseCache
        {
            get { return _useCache; }
            set { _useCache = value; }
        }
        private ShowEntityCollection _showEntity =null;

        /// <summary>
        /// Ҫ��ʾ��ʵ��
        /// </summary>
        public ShowEntityCollection ShowEntity 
        {
            get 
            {
                return _showEntity;
            }
        }
        private ShowChildCollection _showChild = null;

        /// <summary>
        /// Ҫ��ʾ����ʵ��������
        /// </summary>
        public ShowChildCollection ShowChild
        {
            get
            {
                return _showChild;
            }
        }
        private ScopePropertyCollection _showProperty = null;

        /// <summary>
        /// ��ѯʱ��Ҫ��ʾ��Щ���Եļ���
        /// </summary>
        public ScopePropertyCollection ShowProperty
        {
            get
            {
                return _showProperty;
            }
        }
        private ScopePropertyCollection _hideProperty = null;

        /// <summary>
        /// ��ѯʱ��Ҫ������Щ���Եļ���
        /// </summary>
        public ScopePropertyCollection HideProperty
        {
            get
            {
                return _hideProperty;
            }
        }


        private bool _hasInner = false;

        /// <summary>
        /// �ж��Ƿ��ж���ѯ
        /// </summary>
        internal bool HasInner 
        {
            get
            {
                return _hasInner;
            }
            set 
            {
                _hasInner = value;
            }
        }
        private SortList _sortList ;

        /// <summary>
        /// ����
        /// </summary>
        public SortList OrderBy 
        {
            get 
            {
                if(_sortList==null)
                {
                    _sortList=new SortList(this);
                }
                return _sortList;
            }
            set 
            {
                _sortList = value;
            }
        }

        private PageContent _pageContent;

        /// <summary>
        /// ��ҳ��Ϣ
        /// </summary>
        public PageContent PageContent 
        {
            get 
            {
                if (_pageContent == null) 
                {
                    _pageContent = new PageContent();
                }
                return _pageContent;
            }
            set 
            {
                _pageContent = value;
            }
        }

        /// <summary>
        /// �Ƿ��з�ҳ
        /// </summary>
        public bool HasPage 
        {
            get 
            {
                return (_pageContent != null && _pageContent.PageSize > 0);
            }
        }

        /// <summary>
        /// �ж��Ƿ�������
        /// </summary>
        public bool HasSort 
        {
            get
            {
                return (_sortList != null && _sortList.Count > 0);
            }
        }

        /// <summary>
        /// ����������ķ�Χ
        /// </summary>
        /// <param name="lstScope">��������</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddScopeList(ScopeList lstScope, ConnectType conntype)
        {
            this.Add(new Scope(null, lstScope, null, ScopeType.Scope, conntype));
            return true;
        }

        /// <summary>
        /// ����������ķ�Χ
        /// </summary>
        /// <param name="where">����</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool Add(BQLCondition where, ConnectType conntype)
        {
            _hasInner = true;
            this.Add(new Scope(null, where, null, ScopeType.Condition, conntype));
            return true;
        }

        /// <summary>
        /// �ж��Ƿ���
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private bool IsInnerTable(BQLEntityParamHandle property) 
        {
            return !CommonMethods.IsNull(property.BelongEntity.GetParentTable());
        }

        /// <summary>
        /// ����µķ�Χ
        /// </summary>
        /// <param name="property">����</param>
        /// <param name="minValue">��Сֵ</param>
        /// <param name="maxValue">���ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddBetween(BQLEntityParamHandle property, object minValue, object maxValue, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property.Between(minValue, maxValue);
                return Add(where, conntype);
            }

            return AddBetween(property.PInfo.PropertyName, minValue, maxValue, conntype);

        }
        
        /// <summary>
        /// ����µķ�Χ
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="minValue">��Сֵ</param>
        /// <param name="maxValue">���ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddBetween(string propertyName, object minValue, object maxValue, ConnectType conntype) 
        {
            this.Add(new Scope(propertyName, minValue, maxValue, ScopeType.Between, conntype));
            return true;
        }
        /// <summary>
        /// ����µķ�Χ
        /// </summary>
        /// <param name="property">����</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddContains(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property.Contains(value);
                return Add(where, conntype);
            }
            else 
            {
                return AddContains(property, value, conntype);
            }
        }
        /// <summary>
        /// ����µķ�Χ
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddContains(string propertyName, object value, ConnectType conntype)
        {
            this.Add(new Scope(propertyName, value, null, ScopeType.Contains, conntype));
            return true;
        }
        /// <summary>
        /// ����µĴ�������
        /// </summary>
        /// <param name="property">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddMore(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property > value;
                return Add(where, conntype);
            }
            else 
            {
                return AddMore(property.PInfo.PropertyName, value, conntype);
            }
        }
        /// <summary>
        /// ����µĴ�������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddMore(string propertyName, object value, ConnectType conntype)
        {
            this.Add(new Scope(propertyName, value, null, ScopeType.More, conntype));
            return true;
        }


        /// <summary>
        /// ����µĴ��ڵ�������
        /// </summary>
        /// <param name="property">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddMoreThen(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property >= value;
                return Add(where, conntype);
            }
            else 
            {
                return AddMoreThen(property.PInfo.PropertyName, value, conntype);
            }
        }

        /// <summary>
        /// ����µĴ��ڵ�������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddMoreThen(string propertyName, object value, ConnectType conntype)
        {
            this.Add(new Scope(propertyName,value, null, ScopeType.MoreThen, conntype));
            return true;
        }

        /// <summary>
        /// ����µ�С�ڵ�������
        /// </summary>
        /// <param name="property">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddLessThen(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property <= value;
                return Add(where, conntype);
            }
            else 
            {
                return AddLessThen(property.PInfo.PropertyName, value, conntype);
            }
        }
        /// <summary>
        /// ����µ�С�ڵ�������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddLessThen(string propertyName, object value, ConnectType conntype)
        {

            this.Add(new Scope(propertyName, value, null, ScopeType.LessThen, conntype));
            return true;
        }
        /// <summary>
        /// ����µ�С������
        /// </summary>
        /// <param name="property">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddLess(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property < value;
                return Add(where, conntype);
            }
            else 
            {
                return AddLess(property.PInfo.PropertyName, value, conntype);
            }
        }

        /// <summary>
        /// ����µ�С������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddLess(string propertyName, object value, ConnectType conntype)
        {

            this.Add(new Scope(propertyName,value, null, ScopeType.Less, conntype));
            return true;
        }
        /// <summary>
        /// ����µ�In����(�������Ϊ�գ��򷵻�1=2)
        /// </summary>
        /// <param name="property">������</param>
        /// <param name="valuesCollection">ֵ����</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddIn(BQLEntityParamHandle property, IEnumerable valuesCollection, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                
                BQLCondition where = property.In(valuesCollection);
                return Add(where, conntype);
            }
            else 
            {
                return AddIn(property.PInfo.PropertyName, valuesCollection, conntype);
            }
        }

        /// <summary>
        /// ����µ�In����(�������Ϊ�գ��򷵻�1=2)
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="valuesCollection">ֵ����</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddIn(string propertyName, IEnumerable valuesCollection, ConnectType conntype)
        {

            this.Add(new Scope(propertyName, valuesCollection, null, ScopeType.IN, conntype));
            return true;
        }

        /// <summary>
        /// ����µ�NotIn����(�������Ϊ�գ��򷵻�1=1)
        /// </summary>
        /// <param name="property">������</param>
        /// <param name="valuesCollection">ֵ����</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddNotIn(BQLEntityParamHandle property, IEnumerable valuesCollection, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property.NotIn(valuesCollection);
                return Add(where, conntype);
            }
            else 
            {
                return AddNotIn(property.PInfo.PropertyName, valuesCollection, conntype);
            }
        }

        /// <summary>
        /// ����µ�NotIn����(�������Ϊ�գ��򷵻�1=1)
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="valuesCollection">ֵ����</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddNotIn(string propertyName, IEnumerable valuesCollection, ConnectType conntype)
        {

            this.Add(new Scope(propertyName, valuesCollection, null, ScopeType.NotIn, conntype));
            return true;
        }

        /// <summary>
        /// ����µĲ���������
        /// </summary>
        /// <param name="property">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddNotEqual(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property != value;
                return Add(where, conntype);
            }
            else 
            {
                return AddNotEqual(property.PInfo.PropertyName, value, conntype);
            }
        }

        /// <summary>
        /// ����µĲ���������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddNotEqual(string propertyName, object value, ConnectType conntype)
        {

            this.Add(new Scope(propertyName, value, null, ScopeType.NotEqual, conntype));
            return true;
        }

        /// <summary>
        /// ����µĵ�������
        /// </summary>
        /// <param name="property">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddEqual(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property == value;
                return Add(where, conntype);
            }
            else 
            {
                return AddEqual(property.PInfo.PropertyName, value, conntype);
            }
        }

        /// <summary>
        /// ����µĵ�������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddEqual(string propertyName, object value,ConnectType conntype)
        {

            this.Add(new Scope(propertyName, value, null, ScopeType.Equal, conntype));
            return true;
        }

        /// <summary>
        /// ����µ�Like����
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddLike(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property.Like(value);
                return Add(where, conntype);
            }
            else 
            {
                return AddLike(property.PInfo.PropertyName, value, conntype);
            }
        }

        /// <summary>
        /// ����µ�Like����
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddLike(string propertyName, object value, ConnectType conntype)
        {

            this.Add(new Scope(propertyName, value, null, ScopeType.Like, conntype));
            return true;
        }


        /// <summary>
        /// ����µ�StarWith����
        /// </summary>
        /// <param name="property">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddStarWith(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property.StarWith(value);
                return Add(where, conntype);
            }
            else 
            {
                return AddStarWith(property.PInfo.PropertyName, value, conntype);
            }
        }

        /// <summary>
        /// ����µ�StarWith����
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddStarWith(string propertyName, object value, ConnectType conntype)
        {

            this.Add(new Scope(propertyName, value, null, ScopeType.StarWith, conntype));
            return true;
        }

        /// <summary>
        /// ����µ�EndWith����
        /// </summary>
        /// <param name="property">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddEndWith(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property.EndWith(value);
                return Add(where, conntype);
            }
            else 
            {
                return AddEndWith(property.PInfo.PropertyName, value, conntype);
            }
        }

        /// <summary>
        /// ����µ�EndWith����
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddEndWith(string propertyName, object value, ConnectType conntype)
        {

            this.Add(new Scope(propertyName, value, null, ScopeType.EndWith, conntype));
            return true;
        }
        #region And����
        /// <summary>
        /// ����µķ�Χ
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="minValue">��Сֵ</param>
        /// <param name="maxValue">���ֵ</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddBetween(string propertyName, object minValue, object maxValue)
        {

            return AddBetween(propertyName,minValue,maxValue,ConnectType.And);
        }
        /// <summary>
        /// ����µķ�Χ
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddContains(string propertyName, object value)
        {
            this.Add(new Scope(propertyName, value, null, ScopeType.Contains, ConnectType.And));
            return true;
        }
        /// <summary>
        /// ����µĴ�������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddMore(string propertyName, object value)
        {
            return AddMore(propertyName, value, ConnectType.And); ;
        }

        /// <summary>
        /// ����µĴ��ڵ�������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddMoreThen(string propertyName, object value)
        {
            return AddMoreThen(propertyName,value,ConnectType.And);
        }

        /// <summary>
        /// ����µ�С�ڵ�������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddLessThen(string propertyName, object value)
        {
            return AddLessThen(propertyName,value,ConnectType.And);
        }

        /// <summary>
        /// ����µ�С������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddLess(string propertyName, object value)
        {
            return AddLess(propertyName,value,ConnectType.And);
        }

        /// <summary>
        /// ����µ�In����(�������Ϊ�գ��򷵻�1=2)
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="valuesCollection">ֵ����</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddIn(string propertyName, IEnumerable valuesCollection)
        {
            return AddIn(propertyName, valuesCollection, ConnectType.And);
        }
        /// <summary>
        /// ����µ�NotIn����(�������Ϊ�գ��򷵻�1=1)
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="valuesCollection">ֵ����</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddNotIn(string propertyName, IEnumerable valuesCollection)
        {
            return AddNotIn(propertyName, valuesCollection, ConnectType.And);
        }
        /// <summary>
        /// ����µĲ���������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddNotEqual(string propertyName, object value)
        {
            return AddNotEqual(propertyName, value, ConnectType.And);
        }

        /// <summary>
        /// ����µĵ�������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddEqual(string propertyName, object value)
        {
            return AddEqual(propertyName, value, ConnectType.And) ;
        }
        /// <summary>
        /// ����µĵ�������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddLike(string propertyName, object value)
        {
            return AddLike(propertyName, value, ConnectType.And);
        }
        
        

        /// <summary>
        /// ����µķ�Χ
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="minValue">��Сֵ</param>
        /// <param name="maxValue">���ֵ</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddBetween(BQLEntityParamHandle property, object minValue, object maxValue)
        {

            return AddBetween(property, minValue, maxValue, ConnectType.And);
        }
        /// <summary>
        /// ����µķ�Χ
        /// </summary>
        /// <param name="property">����</param>
        /// <param name="value">��Сֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddContains(BQLEntityParamHandle property, object value)
        {
            
                return AddContains(property, value, ConnectType.And);
            
        }
        /// <summary>
        /// ����µĴ�������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddMore(BQLEntityParamHandle property, object value)
        {
            return AddMore(property, value, ConnectType.And); ;
        }

        /// <summary>
        /// ����µĴ��ڵ�������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddMoreThen(BQLEntityParamHandle property, object value)
        {
            return AddMoreThen(property, value, ConnectType.And);
        }

        /// <summary>
        /// ����µ�С�ڵ�������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddLessThen(BQLEntityParamHandle property, object value)
        {
            return AddLessThen(property, value, ConnectType.And);
        }

        /// <summary>
        /// ����µ�С������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddLess(BQLEntityParamHandle property, object value)
        {
            return AddLess(property, value, ConnectType.And);
        }

        /// <summary>
        /// ����µ�In����(�������Ϊ�գ��򷵻�1=2)
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="valuesCollection">ֵ����</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddIn(BQLEntityParamHandle property, IEnumerable valuesCollection)
        {
            return AddIn(property, valuesCollection, ConnectType.And);
        }
        /// <summary>
        /// ����µ�NotIn����(�������Ϊ�գ��򷵻�1=1)
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="valuesCollection">ֵ����</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddNotIn(BQLEntityParamHandle property, IEnumerable valuesCollection)
        {
            return AddNotIn(property, valuesCollection, ConnectType.And);
        }
        /// <summary>
        /// ����µĲ���������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddNotEqual(BQLEntityParamHandle property, object value)
        {
            return AddNotEqual(property, value, ConnectType.And);
        }

        /// <summary>
        /// ����µĵ�������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddEqual(BQLEntityParamHandle property, object value)
        {
            return AddEqual(property, value, ConnectType.And);
        }
        /// <summary>
        /// ����µĵ�������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddLike(BQLEntityParamHandle property, object value)
        {
            return AddLike(property, value, ConnectType.And);
        }

        /// <summary>
        /// ����µ�StarWith����
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddStarWith(BQLEntityParamHandle property, object value)
        {
            return AddStarWith(property, value, ConnectType.And);
        }

        /// <summary>
        /// ����µ�StarWith����
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddStarWith(string propertyName, object value)
        {
            return AddStarWith(propertyName, value, ConnectType.And);
        }

        /// <summary>
        /// ����µ�EndWith����
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddEndWith(BQLEntityParamHandle property, object value)
        {
            return AddEndWith(property, value, ConnectType.And);
            
        }

        /// <summary>
        /// ����µ�EndWith����
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddEndWith(string propertyName, object value)
        {
            return AddEndWith(propertyName, value, ConnectType.And);
        }
        /// <summary>
        /// ����������ķ�Χ
        /// </summary>
        /// <param name="lstScope">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool AddScopeList(ScopeList lstScope)
        {
            if (lstScope.HasInner) 
            {
                HasInner = lstScope.HasInner;
            }
            this.Add(new Scope(null, lstScope, null, ScopeType.Scope, ConnectType.And));
            return true;
        }

        /// <summary>
        /// ����������ķ�Χ
        /// </summary>
        /// <param name="where">����</param>
        /// <param name="conntype">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public bool Add(BQLCondition where)
        {
            this.Add(where, ConnectType.And);
            return true;
        }
        #endregion 

        /// <summary>
        /// �ѷ�Χ����ת��XML�ַ���
        /// </summary>
        /// <returns></returns>
        public string ToXmlString() 
        {
            return ScopeXmlExtend.GetScopeXmlString(this);
        }

        /// <summary>
        /// ����XML���ط�Χ������
        /// </summary>
        /// <param name="xmlString">xml�ַ���</param>
        /// <returns></returns>
        public void LoadXML(string xmlString)
        {
            ScopeXmlExtend.LoadScopeItems(xmlString,this);
        }
    }
}
