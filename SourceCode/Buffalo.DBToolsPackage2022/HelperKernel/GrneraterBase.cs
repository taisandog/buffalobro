using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DBTools.ROMHelper;
using System.IO;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// ����������
    /// </summary>
    public class GrneraterBase
    {
        //protected EntityConfig _entity;

        ///// <summary>
        ///// ʵ����Ϣ
        ///// </summary>
        //public EntityConfig Entity
        //{
        //    get
        //    {
        //        return _entity;
        //    }
        //}

        private KeyWordTableParamItem _table;

        /// <summary>
        /// ����Ϣ
        /// </summary>
        public KeyWordTableParamItem Table
        {
            get { return _table; }
        }

        //private Project _currentProject;

        ///// <summary>
        ///// ��ǰ��Ŀ
        ///// </summary>
        //public Project CurrentProject 
        //{
        //    get 
        //    {
        //        return _currentProject;
        //    }
        //}
        private ClassDesignerInfo _designerInfo;

        /// <summary>
        /// �����ͼ��Ϣ
        /// </summary>
        public ClassDesignerInfo DesignerInfo
        {
            get { return _designerInfo; }
            set { _designerInfo = value; }
        }
        private string _entityBaseTypeName;

        /// <summary>
        /// ʵ��Ļ���
        /// </summary>
        public string EntityBaseTypeName 
        {
            get 
            {
                return _entityBaseTypeName;
            }
        }

        /// <summary>
        /// ��ͼ�ļ�
        /// </summary>
        public string ClassDesignerFileName 
        {
            get 
            {
                return DesignerInfo.SelectDocView.DocData.FileName;
            }
        }

        /// <summary>
        /// ��Ҫ���ɵ���·��Ŀ¼
        /// </summary>
        public string GenerateBasePath 
        {
            get
            {
                FileInfo classFile = new FileInfo(ClassDesignerFileName);
                return classFile.DirectoryName;
            }
        }

        private string _entityBaseTypeShortName;

        /// <summary>
        /// ʵ��Ļ�����
        /// </summary>
        public string EntityBaseTypeShortName 
        {
            get 
            {
                return _entityBaseTypeShortName;
            }
        }
        private string _entityFileName;

        /// <summary>
        /// ʵ���ļ���
        /// </summary>
        public string EntityFileName 
        {
            get
            {
                return _entityFileName;
            }
        }

        private string _entityNamespace;

        /// <summary>
        /// ʵ�������ռ�
        /// </summary>
        public string EntityNamespace
        {
            get
            {
                return _entityNamespace;
            }
        }

        private string _BQLEntityNamespace;
        /// <summary>
        /// BQLʵ��������ռ�
        /// </summary>
        public string BQLEntityNamespace 
        {
            get 
            {
                return _BQLEntityNamespace;
            }
        }

        private string _className;
        /// <summary>
        /// ʵ������
        /// </summary>
        public string ClassName
        {
            get
            {
                return _className;
            }
        }


        private string _businessNamespace;
        /// <summary>
        /// ҵ��������ռ�
        /// </summary>
        public string BusinessNamespace
        {
            get
            {
                return _businessNamespace;
            }
        }
        private string _baseNamespace;
        /// <summary>
        /// ���������ռ�
        /// </summary>
        public string BaseNamespace
        {
            get
            {
                return _baseNamespace;
            }
        }
        private string _dataAccessNamespace;
        /// <summary>
        /// ���ݲ������ռ�
        /// </summary>
        public string DataAccessNamespace
        {
            get
            {
                return _dataAccessNamespace;
            }
        }

        private string _DBName;

        /// <summary>
        /// ���ݿ���
        /// </summary>
        public string DBName 
        {
            get 
            {
                return _DBName;
            }
        }

        private DBConfigInfo _dbConfig;

        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public DBConfigInfo BbConfig
        {
            get { return _dbConfig; }
            set { _dbConfig = value; }
        }
        List<string> _lstGenericArgs =null;
        /// <summary>
        /// �Է��͸���Ĵ���ֵ
        /// </summary>
        public List<string> GenericArgs
        {
            get { return _lstGenericArgs; }
        }
        Dictionary<string, List<string>> _dicGenericInfo;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public Dictionary<string, List<string>> GenericInfo
        {
            get { return _dicGenericInfo; }
        }
        Dictionary<string, List<string>> _dicBaseGenericInfo;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public Dictionary<string, List<string>> BaseGenericInfo
        {
            get { return _dicGenericInfo; }
        }
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        private string GetBaseTypeShortName(string baseType) 
        {
            baseType = GetBaseTypeName(baseType);
            int lastDot = baseType.LastIndexOf('.');
            if (lastDot >= 0)
            {
                return baseType.Substring(lastDot + 1, baseType.Length - lastDot - 1);
            }
            return baseType;
        }
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        private string GetBaseTypeNameSpace(string baseType)
        {
            baseType = GetBaseTypeName(baseType);
            int lastDot = baseType.LastIndexOf('.');
            if (lastDot >= 0)
            {
                return baseType.Substring(0,lastDot);
            }
            return baseType;
        }
        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <returns></returns>
        private string GetBaseTypeName(string baseType) 
        {
            int genTypeIndex = baseType.IndexOf('<');
            if (genTypeIndex >= 0)
            {
                baseType = baseType.Substring(0, genTypeIndex);
            }
            return baseType;
        }

        public GrneraterBase(DBEntityInfo entity,ClassDesignerInfo info) 
        {
            _table = entity.ToTableInfo();
            _className = entity.ClassName;
            DesignerInfo = info;
            _entityBaseTypeName = GetBaseTypeName(entity.BaseType);
           

            _entityBaseTypeShortName =GetBaseTypeShortName( entity.BaseType);
           
            _entityFileName = entity.FileName;
            _entityNamespace = entity.EntityNamespace;
            _baseNamespace = GetBaseTypeNameSpace(entity.BaseType);
            _BQLEntityNamespace = entity.EntityNamespace + ".BQLEntity";
            _businessNamespace = entity.EntityNamespace + ".Business";
            _dataAccessNamespace = entity.EntityNamespace + ".DataAccess";
            _DBName = entity.CurrentDBConfigInfo.DbName;
            _dbConfig = entity.CurrentDBConfigInfo;
        }

        /// <summary>
        /// ��ʽ������
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string FormatName(string name) 
        {
            return name.Replace(" ", "_");
        }

        public GrneraterBase(EntityConfig entity)
        {
            //_entity = entity;
            //_tableName = entity.TableName;
            _table = entity.ToTableInfo();
            DesignerInfo = entity.DesignerInfo;
            _entityBaseTypeName = GetBaseTypeName(entity.BaseTypeName);
            _entityBaseTypeShortName = GetBaseTypeShortName(entity.BaseTypeName);
            _baseNamespace = GetBaseTypeNameSpace(entity.BaseTypeName);
            _entityFileName = entity.FileName;
            _entityNamespace = entity.Namespace;
            //_summary = entity.Summary;
            _className = entity.ClassName;
            _BQLEntityNamespace = entity.Namespace + ".BQLEntity";
            _businessNamespace = entity.Namespace + ".Business";
            _dataAccessNamespace = entity.Namespace + ".DataAccess";
            _DBName = entity.CurrentDBConfigInfo.DbName;
            _dbConfig = entity.CurrentDBConfigInfo;
            _dicGenericInfo = entity.GenericInfo;

            _lstGenericArgs = entity.GenericArgs;

        }

        
    }
}
