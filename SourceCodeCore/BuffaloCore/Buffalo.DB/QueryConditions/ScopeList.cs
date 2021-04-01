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
    /// 查找条件的集合
    /// </summary>
    public class ScopeList: ScopeBaseList
    {
        /// <summary>
        /// 查找条件的集合
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
        /// 获取真正要显示的属性集合
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
        /// 查询时候要分组哪些属性的集合
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
        /// 是否使用缓存
        /// </summary>
        public bool UseCache
        {
            get { return _useCache; }
            set { _useCache = value; }
        }
        private ShowEntityCollection _showEntity =null;

        /// <summary>
        /// 要显示的实体
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
        /// 要显示的子实体属性名
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
        /// 查询时候要显示哪些属性的集合
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
        /// 查询时候要隐藏哪些属性的集合
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
        /// 排序
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
        /// 分页信息
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
        /// 是否有分页
        /// </summary>
        public bool HasPage 
        {
            get 
            {
                return (_pageContent != null && _pageContent.PageSize > 0);
            }
        }

        /// <summary>
        /// 判断是否有排序
        /// </summary>
        public bool HasSort 
        {
            get
            {
                return (_sortList != null && _sortList.Count > 0);
            }
        }

        

        /// <summary>
        /// 把范围集合转成XML字符串
        /// </summary>
        /// <returns></returns>
        public string ToXmlString() 
        {
            return ScopeXmlExtend.GetScopeXmlString(this);
        }

        /// <summary>
        /// 根据XML加载范围集合项
        /// </summary>
        /// <param name="xmlString">xml字符串</param>
        /// <returns></returns>
        public void LoadXML(string xmlString)
        {
            ScopeXmlExtend.LoadScopeItems(xmlString,this);
        }

        private ScopeBaseList _having;
        /// <summary>
        /// Having字句
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
