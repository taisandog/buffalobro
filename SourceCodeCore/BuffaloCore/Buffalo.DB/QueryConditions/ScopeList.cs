using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Buffalo.DB.ContantSearchs;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.Kernel;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.QueryConditions
{
    /// <summary>
    /// ���������ļ���
    /// </summary>
    public class ScopeList: ScopeBaseList
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
            _having = new ScopeBaseList(this);
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

        private ScopeBaseList _having;
        /// <summary>
        /// Having�־�
        /// </summary>
        public ScopeBaseList Having 
        {
            get 
            {
                return _having;
            }
        }

    }

    
}
