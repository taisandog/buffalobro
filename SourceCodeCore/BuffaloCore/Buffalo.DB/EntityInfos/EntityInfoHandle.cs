using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.FastReflection;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase;
using Buffalo.DB.ProxyBuilder;
using Buffalo.DB.PropertyAttributes;
using System.Collections.Concurrent;
using System.Threading;

namespace Buffalo.DB.EntityInfos
{


    /// <summary>
    /// 类的信息
    /// </summary>
    public class EntityInfoHandle 
    {
        
        private Type _entityType;
        //private string connectionKey;
        private CreateInstanceHandler _createInstanceHandler;
        private PropertyInfoCollection _propertyInfoHandles;
        private MappingInfoCollection _mappingInfoHandles;
        private List<EntityPropertyInfo> _primaryProperty;//主属性
        private DBInfo _dbInfo;
        private CreateInstanceHandler _createProxyInstanceHandler;
        private TableAttribute _tableInfo;
        /// <summary>
        /// 类的信息
        /// </summary>
        /// <param name="entityType">类的类型</param>
        /// <param name="createInstanceHandler">创建类的反射信息</param>
        /// <param name="tableInfo">关联表信息</param>
        /// <param name="db">关联数据库信息</param>
        internal EntityInfoHandle(Type entityType, CreateInstanceHandler createInstanceHandler,
             TableAttribute tableInfo, DBInfo db) 
        {
            this._entityType = entityType;
            this._createInstanceHandler = createInstanceHandler;

            this._tableInfo = tableInfo;
            //this.connectionKey = connectionKey;
            this._dbInfo = db;
            
        }

        /// <summary>
        /// 初始化主键信息
        /// </summary>
        private void InitPrimaryProperty()
        {
            List<EntityPropertyInfo> lst = new List<EntityPropertyInfo>();
            foreach (EntityPropertyInfo info in _propertyInfoHandles)
            {

                if (info.IsPrimaryKey)
                {
                    lst.Add(info);
                }
            }
            _primaryProperty = lst;
        }

        /// <summary>
        /// 获取是否需要延迟加载
        /// </summary>
        /// <returns></returns>
        public bool GetNeedLazy() 
        {
            if (_dbInfo.AllowLazy == LazyType.User) 
            {
                return _tableInfo.AllowLazy;
            }

            return _dbInfo.AllowLazy==LazyType.Enable;
            
        }

        /// <summary>
        /// 设置属性信息和关联映射信息
        /// </summary>
        /// <param name="propertyInfoHandles">属性信息集合</param>
        /// <param name="mappingInfoHandles">关联映射信息集合</param>
        internal void SetInfoHandles(Dictionary<string, EntityPropertyInfo> propertyInfoHandles,
            Dictionary<string, EntityMappingInfo> mappingInfoHandles) 
        {
            this._propertyInfoHandles = new PropertyInfoCollection(propertyInfoHandles);
            this._mappingInfoHandles = new MappingInfoCollection(mappingInfoHandles);
            
        }
        /// <summary>
        /// 初始化信息
        /// </summary>
        internal void InitInfo()
        {
            InitPrimaryProperty();
            InitPropertyUpdateInfo();
        }

        /// <summary>
        /// 初始化代理类
        /// </summary>
        internal void InitProxyType(EntityProxyBuilder proxyBuilder) 
        {
            _proxyType = proxyBuilder.CreateProxyType(_entityType);
            _createProxyInstanceHandler = FastInvoke.GetInstanceCreator(_proxyType);
        }

        /// <summary>
        /// 当前所属数据库的信息
        /// </summary>
        public DBInfo DBInfo 
        {
            get 
            {
                return _dbInfo;
            }
        }
        /// <summary>
        /// 本实体的类型
        /// </summary>
        public Type EntityType 
        {
            get 
            {
                return _entityType;
            }
        }
        /// <summary>
        /// 注释
        /// </summary>
        public string Description
        {
            get
            {
                return _tableInfo.Description;
            }
        }

        private ThreadLocal<string> _curTableName = new ThreadLocal<string>();

        /// <summary>
        /// 切换当前使用的表名,null则切换回默认
        /// </summary>
        public string SelectedTableName 
        {
            get 
            {
                return _curTableName.Value;
            }
            set 
            {
                if (string.IsNullOrWhiteSpace(value)) 
                {
                    _curTableName.Value = null;
                    return;
                }
                _curTableName.Value = value;
            }
        }

        /// <summary>
        /// 对应的表名名
        /// </summary>
        public string TableName
        {
            get
            {
                string selTableName = _curTableName.Value;
                if (selTableName != null) 
                {
                    return selTableName;
                }
                return _tableInfo.TableName;
            }
        }
        
        /// <summary>
        /// 获取属性的信息
        /// </summary>
        public PropertyInfoCollection PropertyInfo
        {
            get 
            {
                if (_propertyInfoHandles == null)
                {
                    InitInfo();
                }
                return _propertyInfoHandles;
            }
        }

        /// <summary>
        /// 映射信息集合
        /// </summary>
        public MappingInfoCollection MappingInfo
        {
            get
            {
                return _mappingInfoHandles;
            }
        }

        private Type _proxyType;
        /// <summary>
        /// 代理类
        /// </summary>
        public Type ProxyType
        {
            get 
            { 

                return _proxyType; 
            }
        }
        

        /// <summary>
        /// 返回此类型的实例
        /// </summary>
        /// <returns></returns>
        public object CreateInstance() 
        {
            if (_createInstanceHandler != null) 
            {
                return _createInstanceHandler.Invoke();
            }
            return null;
        }
        /// <summary>
        /// 返回此类型的代理类实例
        /// </summary>
        /// <returns></returns>
        public object CreateProxyInstance()
        {
            if (_createProxyInstanceHandler != null)
            {
                return _createProxyInstanceHandler.Invoke();
            }
            return null;
        }
        /// <summary>
        /// 返回此类型的代理类实例(用于查询)
        /// </summary>
        /// <returns></returns>
        public object CreateSelectProxyInstance()
        {
            object obj = CreateProxyInstance();
            //EntityBase eobj = obj as EntityBase;
            //eobj.PrimaryKeyChange();
            return obj;
        }
        /// <summary>
        /// 主属性
        /// </summary>
        public List<EntityPropertyInfo> PrimaryProperty
        {
            get
            {
                if (_primaryProperty == null)
                {
                    InitInfo();
                }
                return _primaryProperty;
            }

        }


        private ConcurrentDictionary<string, UpdatePropertyInfo> _dicUpdateInfo = null;

        /// <summary>
        /// 初始化属性的更新信息
        /// </summary>
        private void InitPropertyUpdateInfo() 
        {
            ConcurrentDictionary<string, UpdatePropertyInfo> dic = new ConcurrentDictionary<string, UpdatePropertyInfo>();
            foreach (EntityMappingInfo mapInfo in this.MappingInfo)
            {
                if (mapInfo.IsParent) //如果是父类实体
                {
                    dic[mapInfo.PropertyName] = new UpdatePropertyInfo(mapInfo, true);
                    if (mapInfo.SourceProperty == null) 
                    {
                        throw new MissingMemberException("关联属性:" + mapInfo.PropertyName + "，并没有正确配置源属性或目标属性");
                    }
                    dic[mapInfo.SourceProperty.PropertyName] = new UpdatePropertyInfo(mapInfo, false);
                }
            }


            _dicUpdateInfo = dic;
        }

        /// <summary>
        /// 获取本属性的关联更新信息
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public UpdatePropertyInfo GetUpdatePropertyInfo(string propertyName) 
        {
            
            UpdatePropertyInfo ret = null;
            if (_dicUpdateInfo.TryGetValue(propertyName, out ret))
            {
                return ret;
            } 
            return null;
        }

    }
}
