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
    /// �����Ϣ
    /// </summary>
    public class EntityInfoHandle 
    {
        
        private Type _entityType;
        //private string connectionKey;
        private CreateInstanceHandler _createInstanceHandler;
        private PropertyInfoCollection _propertyInfoHandles;
        private MappingInfoCollection _mappingInfoHandles;
        private List<EntityPropertyInfo> _primaryProperty;//������
        private DBInfo _dbInfo;
        private CreateInstanceHandler _createProxyInstanceHandler;
        private TableAttribute _tableInfo;
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="entityType">�������</param>
        /// <param name="createInstanceHandler">������ķ�����Ϣ</param>
        /// <param name="tableInfo">��������Ϣ</param>
        /// <param name="db">�������ݿ���Ϣ</param>
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
        /// ��ʼ��������Ϣ
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
        /// ��ȡ�Ƿ���Ҫ�ӳټ���
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
        /// ����������Ϣ�͹���ӳ����Ϣ
        /// </summary>
        /// <param name="propertyInfoHandles">������Ϣ����</param>
        /// <param name="mappingInfoHandles">����ӳ����Ϣ����</param>
        internal void SetInfoHandles(Dictionary<string, EntityPropertyInfo> propertyInfoHandles,
            Dictionary<string, EntityMappingInfo> mappingInfoHandles) 
        {
            this._propertyInfoHandles = new PropertyInfoCollection(propertyInfoHandles);
            this._mappingInfoHandles = new MappingInfoCollection(mappingInfoHandles);
            
        }
        /// <summary>
        /// ��ʼ����Ϣ
        /// </summary>
        internal void InitInfo()
        {
            InitPrimaryProperty();
            InitPropertyUpdateInfo();
        }

        /// <summary>
        /// ��ʼ��������
        /// </summary>
        internal void InitProxyType(EntityProxyBuilder proxyBuilder) 
        {
            _proxyType = proxyBuilder.CreateProxyType(_entityType);
            _createProxyInstanceHandler = FastInvoke.GetInstanceCreator(_proxyType);
        }

        /// <summary>
        /// ��ǰ�������ݿ����Ϣ
        /// </summary>
        public DBInfo DBInfo 
        {
            get 
            {
                return _dbInfo;
            }
        }
        /// <summary>
        /// ��ʵ�������
        /// </summary>
        public Type EntityType 
        {
            get 
            {
                return _entityType;
            }
        }
        /// <summary>
        /// ע��
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
        /// �л���ǰʹ�õı���,null���л���Ĭ��
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
        /// ��Ӧ�ı�����
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
        /// ��ȡ���Ե���Ϣ
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
        /// ӳ����Ϣ����
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
        /// ������
        /// </summary>
        public Type ProxyType
        {
            get 
            { 

                return _proxyType; 
            }
        }
        

        /// <summary>
        /// ���ش����͵�ʵ��
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
        /// ���ش����͵Ĵ�����ʵ��
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
        /// ���ش����͵Ĵ�����ʵ��(���ڲ�ѯ)
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
        /// ������
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
        /// ��ʼ�����Եĸ�����Ϣ
        /// </summary>
        private void InitPropertyUpdateInfo() 
        {
            ConcurrentDictionary<string, UpdatePropertyInfo> dic = new ConcurrentDictionary<string, UpdatePropertyInfo>();
            foreach (EntityMappingInfo mapInfo in this.MappingInfo)
            {
                if (mapInfo.IsParent) //����Ǹ���ʵ��
                {
                    dic[mapInfo.PropertyName] = new UpdatePropertyInfo(mapInfo, true);
                    if (mapInfo.SourceProperty == null) 
                    {
                        throw new MissingMemberException("��������:" + mapInfo.PropertyName + "����û����ȷ����Դ���Ի�Ŀ������");
                    }
                    dic[mapInfo.SourceProperty.PropertyName] = new UpdatePropertyInfo(mapInfo, false);
                }
            }


            _dicUpdateInfo = dic;
        }

        /// <summary>
        /// ��ȡ�����ԵĹ���������Ϣ
        /// </summary>
        /// <param name="propertyName">������</param>
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
