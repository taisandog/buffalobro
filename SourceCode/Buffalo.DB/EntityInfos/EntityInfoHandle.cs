using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.FastReflection;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase;
using Buffalo.DB.ProxyBuilder;
using Buffalo.DB.PropertyAttributes;


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
        /// <param name="entityType">实体类型</param>
        /// <param name="createInstanceHandler">实例化类的句柄</param>
        /// <param name="propertyInfoHandles">属性集合</param>
        /// <param name="tableName">对应的表名</param>
        /// <param name="baseListInfo">此对象的查询缓存集合句柄</param>
        /// <param name="connectionKey">连接字符串的键</param>
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
        /// 设置属性和映射信息
        /// </summary>
        /// <param name="propertyInfoHandles">属性信息集合</param>
        /// <param name="mappingInfoHandles">映射信息集合</param>
        /// <param name="baseListInfo">所属的基集合</param>
        internal void SetInfoHandles(Dictionary<string, EntityPropertyInfo> propertyInfoHandles,
            Dictionary<string, EntityMappingInfo> mappingInfoHandles) 
        {
            this._propertyInfoHandles = new PropertyInfoCollection(propertyInfoHandles);
            this._mappingInfoHandles = new MappingInfoCollection(mappingInfoHandles);
            
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
        /// <summary>
        /// 对应的表名名
        /// </summary>
        public string TableName
        {
            get
            {
                return _tableInfo.TableName;
            }
        }
        ///// <summary>
        ///// 连接字符串的键
        ///// </summary>
        //public string ConnectionKey
        //{
        //    get
        //    {
        //        return connectionKey;
        //    }
        //}
        /// <summary>
        /// 获取属性的信息
        /// </summary>
        public PropertyInfoCollection PropertyInfo
        {
            get 
            {
                return _propertyInfoHandles;
            }
        }

        /// <summary>
        /// 获取映射的信息
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
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
                if (_primaryProperty == null) //找出主属性
                {

                    _primaryProperty = new List<EntityPropertyInfo>();
                    foreach (EntityPropertyInfo info in _propertyInfoHandles)
                    {
                        
                        if (info.IsPrimaryKey) 
                        {
                            _primaryProperty.Add(info);
                        }
                    }
                    
                }
                return _primaryProperty;
            }

        }


        private Dictionary<string, UpdatePropertyInfo> _dicUpdateInfo = null;

        /// <summary>
        /// 初始化属性的更新信息
        /// </summary>
        private Dictionary<string, UpdatePropertyInfo> InitPropertyUpdateInfo() 
        {
            Dictionary<string, UpdatePropertyInfo> dic = new Dictionary<string, UpdatePropertyInfo>();
            foreach (EntityMappingInfo mapInfo in this.MappingInfo)
            {
                if (mapInfo.IsParent) //如果是父类实体
                {
                    dic[mapInfo.PropertyName] = new UpdatePropertyInfo(mapInfo, true);
                    dic[mapInfo.SourceProperty.PropertyName] = new UpdatePropertyInfo(mapInfo, false);
                }
            }

            
            return dic;
        }

        /// <summary>
        /// 获取本属性的关联更新信息
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public UpdatePropertyInfo GetUpdatePropertyInfo(string propertyName) 
        {
            if (_dicUpdateInfo == null) 
            {
                _dicUpdateInfo = InitPropertyUpdateInfo();
            }
            UpdatePropertyInfo ret = null;
            if (_dicUpdateInfo.TryGetValue(propertyName, out ret))
            {
                return ret;
            } 
            return null;
        }

    }
}
