using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using System.Collections;
using System.IO;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Project;
using EnvDTE;
using Buffalo.DB.CommBase;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Windows.Forms;
using Buffalo.DB.CommBase.BusinessBases;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.PropertyAttributes;
using Buffalo.Kernel;
using System.Data;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DBCheckers;
using Buffalo.DBTools.ROMHelper;
using Buffalo.DBTools.UIHelper;

namespace Buffalo.DBTools.HelperKernel
{
    public class EntityConfig
    {


        private List<string> _sourceCode = null;

        private EntityParamFieldCollection _eParamFields = new EntityParamFieldCollection();
        /// <summary>
        /// 实体字段
        /// </summary>
        public EntityParamFieldCollection EParamFields
        {
            get { return _eParamFields; }
        }
        private EntityRelationCollection _eRelation = new EntityRelationCollection();
        /// <summary>
        /// 映射属性
        /// </summary>
        public EntityRelationCollection ERelation
        {
            get { return _eRelation; }
        }
        private bool _allowLazy=true;

        /// <summary>
        /// 允许延迟加载
        /// </summary>
        public bool AllowLazy
        {
            set { _allowLazy = value; }
            get { return _allowLazy; }
        }
        //ClassDesignerDocView _selectDocView;
        ///// <summary>
        ///// 选择的文档
        ///// </summary>
        //public ClassDesignerDocView SelectDocView
        //{
        //    get { return _selectDocView; }
        //    set { _selectDocView = value; }
        //}

        private ClrType _classType;

        
        private CodeElementPosition _cp;
        private string _tableName;
        private string _className;
        private string _namespace;
        //private Project _currentProject;
        //private Diagram _currentDiagram;
        private DBConfigInfo _currentDBConfigInfo;
        private Encoding _fileEncoding;

        private bool _useCache;
        /// <summary>
        /// 使用缓存
        /// </summary>
        public bool UseCache
        {
            get { return _useCache; }
            set { _useCache = value; }
        }
        /// <summary>
        /// 类文件编码
        /// </summary>
        public Encoding FileEncoding
        {
            get { return _fileEncoding; }
            
        }
        /// <summary>
        /// 数据库配置信息
        /// </summary>
        public DBConfigInfo CurrentDBConfigInfo
        {
            get 
            {
                InitDBConfig();
                return _currentDBConfigInfo;
            }
        }

        /// <summary>
        /// 初始化数据库信息
        /// </summary>
        public void InitDBConfig() 
        {
            if (_currentDBConfigInfo == null)
            {
                _currentDBConfigInfo = FrmDBSetting.GetDBConfigInfo(DesignerInfo, Namespace + ".DataAccess");
            }
        }
        ///// <summary>
        ///// 当前类图
        ///// </summary>
        //public Diagram CurrentDiagram
        //{
        //    get { return _currentDiagram; }
        //}


        ///// <summary>
        ///// 当前工程
        ///// </summary>
        //public Project CurrentProject
        //{
        //    get { return _currentProject; }
            
        //}
        private ClassDesignerInfo _designerInfo;

        /// <summary>
        /// 类设计图信息
        /// </summary>
        public ClassDesignerInfo DesignerInfo
        {
            get { return _designerInfo; }
            set { _designerInfo = value; }
        }
        /// <summary>
        /// 关联类型
        /// </summary>
        public ClrType ClassType
        {
            get { return _classType; }
        }

        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace
        {
            get { return _namespace; }
        }
        

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName
        {
            get { return _className; }
            set { _className = value; }
        }

        private ClrType _baseType;

        /// <summary>
        /// 基类 
        /// </summary>
        public ClrType BaseType
        {
            get { return _baseType; }
            set { _baseType = value; }
        }

        private string _baseTypeName;


        /// <summary>
        /// 基名称
        /// </summary>
        public string BaseTypeName 
        {
            get
            {

                return _baseTypeName;
            }
        }

        private List<string> _interfaces;

        /// <summary>
        /// 接口
        /// </summary>
        public List<string> Interfaces
        {
            get { return _interfaces; }
            set { _interfaces = value; }
        }

        private string _fileName;

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }

        private string _summary;

        /// <summary>
        /// 注释
        /// </summary>
        public string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }
        private bool _isTable = false;
        /// <summary>
        /// 判断是否表
        /// </summary>
        public bool IsTable
        {
            get { return _isTable; }
            set { _isTable = value; }
        }
        private Dictionary<string, CodeElementPosition> _properties;

        /// <summary>
        /// 类包含的属性集合
        /// </summary>
        public Dictionary<string, CodeElementPosition> Properties
        {
            get { return _properties; }
        }

        DBConfigInfo _dbInfo;
        /// <summary>
        /// 数据库信息
        /// </summary>
        public DBConfigInfo DbInfo
        {
            get
            {
                if (_dbInfo == null)
                {
                    _dbInfo = FrmDBSetting.GetDBConfigInfo(DesignerInfo, DesignerInfo.GetNameSpace() + ".DataAccess");


                }
                return _dbInfo;
            }
        }
        private Dictionary<string, CodeElementPosition> _methods;

        /// <summary>
        /// 类包含的函数集合
        /// </summary>
        public Dictionary<string, CodeElementPosition> Methods
        {
            get { return _methods; }
        }

        private Dictionary<string, CodeElementPosition> _fields;

        /// <summary>
        /// 类包含的字段集合
        /// </summary>
        public Dictionary<string, CodeElementPosition> Fields
        {
            get { return _fields; }
        }
        /// <summary>
        /// 初始化类配置
        /// </summary>
        /// <param name="classShape">类图形</param>
        /// <param name="project">所属项目</param>
        public EntityConfig(ClrType ctype, ClassDesignerInfo info) 
        {
            _designerInfo = info;
            //_classShape = classShape;
            _classType = ctype;
            
            FillClassInfo();
            InitField();
            InitPropertys();
            InitMethods();
            FillBaseTypeGenericArgs(ctype, _lstGenericArgs);
            //_currentProject = project;
            //_currentDiagram = currentDiagram;
            
        }

        /// <summary>
        /// 获取字段对应的属性
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, EntityParamField> GetParamMapField() 
        {
            Dictionary<string, EntityParamField> dic = new Dictionary<string, EntityParamField>();
            EntityConfig curType = this;
            while (curType != null)
            {

                foreach (EntityParamField field in curType.EParamFields)
                {
                    if (!string.IsNullOrEmpty(field.ParamName))
                    {
                        dic[field.ParamName] = field;
                    }
                }
                if (!IsSystemType(curType.BaseType))
                {
                    try
                    {
                        curType = new EntityConfig(curType.BaseType, DesignerInfo);
                    }
                    catch
                    {
                        curType = null;
                    }
                }
                else 
                {
                    curType = null;
                }
            }
            return dic;
        }
        /// <summary>
        /// 获取关系对应的属性
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, EntityRelationItem> GetRelationmMapField()
        {
            Dictionary<string, EntityRelationItem> dic = new Dictionary<string, EntityRelationItem>();
            EntityConfig curType = this;
            while (curType != null)
            {
                foreach (EntityRelationItem field in curType.ERelation)
                {
                    string key = field.TypeName + ":" + field.SourceProperty + ":" + field.TargetProperty;
                    if (!string.IsNullOrEmpty(key))
                    {
                        dic[key] = field;
                    }
                }
                if (!IsSystemType(curType.BaseType))
                {
                    try
                    {
                        curType = new EntityConfig(curType.BaseType, DesignerInfo);
                    }
                    catch
                    {
                        curType = null;
                    }
                }
                else
                {
                    curType = null;
                }
            }
            return dic;
        }
        private List<EntityParam> _dbParams;
        /// <summary>
        /// 数据库所属的此类不存在的字段
        /// </summary>
        public List<EntityParam> DbParams
        {
            get { return _dbParams; }
        }
        private List<TableRelationAttribute> _dbRelations;
        /// <summary>
        /// 数据库所属的此类不存在的关系
        /// </summary>
        public List<TableRelationAttribute> DbRelations
        {
            get { return _dbRelations; }
        }
        /// <summary>
        /// 更新类信息
        /// </summary>
        /// <param name="ctype"></param>
        /// <param name="project"></param>
        /// <param name="currentDiagram"></param>
        /// <returns></returns>
        public static EntityConfig GetEntityConfigByTable(ClrType ctype,
           ClassDesignerInfo desinfo) 
        {

            EntityConfig entity = new EntityConfig(ctype, desinfo);
            //entity.DesignerInfo.SelectDocView = selectDocView;
            if (string.IsNullOrEmpty(entity.TableName)) 
            {
                return null;
            }
            DBInfo db=entity.DbInfo.CreateDBInfo();
            List<string> selTab = new List<string>();
            selTab.Add(entity.TableName);
            List<DBTableInfo> lstGen = TableChecker.GetTableInfo(db, selTab);
            if (lstGen.Count > 0) 
            {
                DBTableInfo info = lstGen[0];
                Dictionary<string, EntityParamField> dicParam = entity.GetParamMapField();
                entity._dbParams=new List<EntityParam>();
                foreach (EntityParam prm in info.Params) 
                {
                    string paramName = prm.ParamName;
                    if (dicParam.ContainsKey(paramName)) 
                    {
                        continue;
                    }
                    entity._dbParams.Add(prm);

                }
                Dictionary<string, EntityRelationItem> dicRelation = entity.GetRelationmMapField();
                entity._dbRelations = new List<TableRelationAttribute>();
                foreach (TableRelationAttribute tr in info.RelationItems)
                {

                    string key = EntityFieldBase.ToPascalName(tr.TargetTable) + ":" + EntityFieldBase.ToPascalName(tr.SourceName) + ":" + EntityFieldBase.ToPascalName(tr.TargetName);
                    if (dicRelation.ContainsKey(key))
                    {
                        continue;
                    }
                    entity._dbRelations.Add(tr);

                }
                
            }
            return entity;
        } 

        /// <summary>
        /// 获取父类信息
        /// </summary>
        /// <param name="ctype"></param>
        /// <returns></returns>
        public static ClrType GetBaseClass(ClrType ctype,out string typeName) 
        {
            InheritanceTypeRefMoveableCollection col = ctype.InheritanceTypeRefs;
            typeName = "System.Object";
            if (col != null && col.Count > 0)
            {
                typeName = col[0].Name;
                ClrType baseType = col[0].ClrType;
                
                return baseType;
            }

            return null;
            
        }

        /// <summary>
        /// 填充基类的传入泛型参数
        /// </summary>
        private static void FillBaseTypeGenericArgs(ClrType ctype, List<string> lstArgs)
        {
            if (EntityConfig.IsSystemType(ctype))
            {
                return;
            }
            InheritanceTypeRefMoveableCollection col = ctype.InheritanceTypeRefs;
            if (col != null && col.Count > 0)
            {
                string args = col[0].TypeArguments;
                if (!string.IsNullOrEmpty(args))
                {
                    GetBaseTypeGenericArgs(args, lstArgs);
                }
            }
            
        }

        /// <summary>
        /// 基类的传入泛型参数
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="lstArgs">参数集合</param>
        /// <returns></returns>
        private static void GetBaseTypeGenericArgs(string args,List<string> lstArgs) 
        {
            string genericParam = args.Trim('<', '>', ' ');
            string[] itemParts = genericParam.Split(',');
            foreach (string strItem in itemParts)
            {
                lstArgs.Add(strItem);
            }
        }

        /// <summary>
        /// 获取ClrClass的全名
        /// </summary>
        /// <param name="cls"></param>
        /// <returns></returns>
        public static string GetFullName(ClrClass cls) 
        {
            return cls.OwnerNamespace.Name + "." + cls.Name;
        }

        /// <summary>
        /// 填充类信息
        /// </summary>
        private void FillClassInfo() 
        {
            ClrType ctype = _classType;
            _className = ctype.Name;

            _baseType = GetBaseClass(ctype,out _baseTypeName);

            _fileName = GetFileName(ctype, out _cp);
            EnvDTE.ProjectItem classItem = GetProjectItemByFileName(DesignerInfo, _fileName);
            //foreach (CodeElementPosition cp in ctype.SourceCodePositions)
            //{
            //    if (cp.FileName.IndexOf(".extend.cs") <0)
            //    {
            //        _fileName = cp.FileName;

            //        _cp = cp;
            //        break;
            //    }
            //}
            _namespace = ctype.OwnerNamespace.Name;
            _summary = ctype.DocSummary;
            _tableName =EntityFieldBase.ToCamelName(_className);
            _lstSource = CodeFileHelper.ReadFile(FileName);
            _fileEncoding = CodeFileHelper.GetFileEncoding(_fileName);
            if (ctype.Generic) 
            {
                InitGeneric(ctype,_dicGenericInfo);
            }
        }
        Dictionary<string, List<string>> _dicGenericInfo = new Dictionary<string, List<string>>();
        /// <summary>
        /// 泛型信息
        /// </summary>
        public Dictionary<string, List<string>> GenericInfo
        {
            get { return _dicGenericInfo; }
        }

        List<string> _lstGenericArgs = new List<string>();
        /// <summary>
        /// 对泛型父类的传入值
        /// </summary>
        public List<string> GenericArgs
        {
            get { return _lstGenericArgs; }
        }

        /// <summary>
        /// 初始化泛型信息
        /// </summary>
        /// <param name="ctype">类型</param>
        internal static void InitGeneric(ClrType ctype, Dictionary<string, List<string>> dicGenericInfo) 
        {
            string headCode = ctype.OuterText.DeclarationOuterText.Substring(0, ctype.OuterText.DeclarationHeaderLength);
            string genericParameters = ctype.TypeParameters;
            AppendGenericParam(genericParameters, dicGenericInfo);
            List<int> lstIndex = new List<int>();
            string tag=" where ";
            int curIndex = 0;
            do
            {
                curIndex = headCode.IndexOf(tag, curIndex);
                
                if (curIndex >= 0)
                {
                    lstIndex.Add(curIndex);
                    curIndex++;
                }
            } while (curIndex >= 0);

            if (lstIndex.Count <= 0) 
            {
                return;
            }
            string code = null;
            for (int i = 0; i < lstIndex.Count; i++) 
            {

                if (i < lstIndex.Count - 1)
                {
                    code = headCode.Substring(lstIndex[i] + tag.Length, lstIndex[i + 1] - (lstIndex[i] + tag.Length));
                }
                else 
                {
                    code = headCode.Substring(lstIndex[i] + tag.Length, headCode.Length - (lstIndex[i] + tag.Length));
                    int classBegin = code.IndexOf("{");
                    if (classBegin > 0) 
                    {
                        code = code.Substring(0, classBegin);
                    }
                    
                }
                AppendToKeys(code,dicGenericInfo);
            }
            
        }
        /// <summary>
        /// 添加泛型信息
        /// </summary>
        /// <param name="genericParam"></param>
        private static void AppendGenericParam(string genericParam, Dictionary<string, List<string>> dicGenericInfo) 
        {
            genericParam = genericParam.Trim('<', '>', ' ');
            string[] itemParts = genericParam.Split(',');
            foreach (string strItem in itemParts)
            {
                dicGenericInfo[strItem.Trim()] = null;
            }
        }

        /// <summary>
        /// 加到泛型集合
        /// </summary>
        /// <param name="code"></param>
        private static void AppendToKeys(string code, Dictionary<string, List<string>> dicGenericInfo) 
        {
            code = code.Replace("\r\n", "");
            code = code.Replace("\r", "");
            code = code.Replace("\n", "");

            string[] typeParts = code.Split(':');
            if (typeParts.Length != 2) 
            {
                return;
            }
            string key = typeParts[0];
            string items = typeParts[1];

            string[] itemParts = items.Split(',');
            List<string> lstItem = new List<string>();
            foreach (string strItem in itemParts) 
            {
                lstItem.Add(strItem.Trim());
            }

            dicGenericInfo[key] = lstItem;
        }

        /// <summary>
        /// 获取实体所在的文件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetFileName(ClrType ctype, out CodeElementPosition ocp) 
        {
            ocp = null;
            foreach (CodeElementPosition cp in ctype.SourceCodePositions)
            {
                if (cp.FileName.IndexOf(".extend.cs") < 0)
                {
                    ocp = cp;
                    
                    return cp.FileName;
                }
            }
            return null;
        }

        private bool _hasConfig;
        /// <summary>
        /// 此实体是否存在配置(BEM)
        /// </summary>
        public bool HasConfig
        {
            get { return _hasConfig; }
        }

        /// <summary>
        /// 初始化字段信息
        /// </summary>
        private void InitField()
        {
            List<ClrField> lstFields = GetAllMember<ClrField>(_classType, false);
            _fields = new Dictionary<string, CodeElementPosition>();
            for (int j = 0; j < lstFields.Count; j++)
            {
                ClrField field = lstFields[j];
                if (field == null)
                {
                    continue;
                }
                if (field.SourceCodePositions == null) 
                {
                    continue;
                }
                foreach (CodeElementPosition cp in field.SourceCodePositions)
                {
                    if (!IsManyOne(field))
                    {
                        EntityParamField epf = new EntityParamField(cp, field, this);
                        epf.AllowNull = EntityFieldBase.IsNullProperty(field.MemberTypeShortName);
                        _eParamFields.Add(epf);
                    }
                    else 
                    {
                        
                        EntityRelationItem erf = new EntityRelationItem(cp, field, this);
                        _eRelation.Add(erf);
                    }

                    _fields[field.Name] = cp;
                }
                _eParamFields.SortItem();
                _eRelation.SortItem();
            }
            _hasConfig=EntityMappingConfig.LoadConfigInfo(this);


        }

        /// <summary>
        /// 判断是否多对一
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsManyOne(Member source) 
        {
            DataTypeInfos info = EntityFieldBase.GetTypeInfo(source);


            return info==null;
        }
        private List<string> _allPropertyNames;

        /// <summary>
        /// 所有字段名集合
        /// </summary>
        public List<string> AllPropertyNames 
        {
            get 
            {
                if (_allPropertyNames == null)
                {
                    _allPropertyNames = new List<string>();
                    List<ClrProperty> lstProperty = GetAllMember<ClrProperty>(_classType, true);
                    foreach (ClrProperty prot in lstProperty)
                    {
                        if (!IsManyOne(prot))
                        {
                            _allPropertyNames.Add(prot.Name);
                        }
                    }
                }
                return _allPropertyNames;
            }
        }

        /// <summary>
        /// 初始化函数
        /// </summary>
        private void InitMethods() 
        {
            List<ClrMethod> lstMethod = GetAllMember<ClrMethod>(_classType, false);
            _methods = new Dictionary<string, CodeElementPosition>();
            for (int j = 0; j < lstMethod.Count; j++)
            {
                object tm = lstMethod[j];

                ClrMethod method = tm as ClrMethod;
                if (method == null)
                {
                    continue;
                }
                foreach (CodeElementPosition cp in method.SourceCodePositions)
                {
                    _methods[method.Name] = cp;

                }

            }
        }

        /// <summary>
        /// 初始化属性信息
        /// </summary>
        private void InitPropertys()
        {
            List<ClrProperty> lstProperty = GetAllMember<ClrProperty>(_classType, false);
            _properties = new Dictionary<string, CodeElementPosition>();
            for (int j = 0; j < lstProperty.Count; j++)
            {
                object tm = lstProperty[j];

                ClrProperty property = tm as ClrProperty;
                if (property == null)
                {
                    continue;
                }
                foreach (CodeElementPosition cp in property.SourceCodePositions)
                {
                    _properties[property.Name] = cp;
                   
                }

            }


        }


       

        /// <summary>
        /// 切出开头的空格数
        /// </summary>
        /// <returns></returns>
        public static string CutSpace(string str) 
        {
            StringBuilder sbRet = new StringBuilder(str.Length);
            foreach (char chr in str) 
            {
                if (chr == ' ')
                {
                    sbRet.Append(chr);
                }
                else 
                {
                    break;
                }
            }
            return sbRet.ToString();
        }

        /// <summary>
        /// 生成扩展代码
        /// </summary>
        private void GenerateExtenCode() 
        {
            BQLEntityGenerater bqlEntity = new BQLEntityGenerater(this);
            GenerateExtendCode();
            if (_currentDBConfigInfo.Tier == 3)
            {
                Generate3Tier g3t = new Generate3Tier(this);
                g3t.GenerateBusiness();
                if (!string.IsNullOrEmpty(this.TableName))
                {
                    g3t.GenerateIDataAccess();
                    g3t.GenerateDataAccess();
                    g3t.GenerateBQLDataAccess();
                }
            }
            bqlEntity.GenerateBQLEntityDB();
            bqlEntity.GenerateBQLEntity();
            EntityMappingConfig.SaveXML(this);
        }

        /// <summary>
        /// 获取所有字段
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, ClrField> AllField() 
        {
            List<ClrField> lstFields = GetAllMember<ClrField>(this._classType, false);

            Dictionary<string, ClrField> dicFields = new Dictionary<string, ClrField>();
            foreach (ClrField objField in lstFields)
            {
                dicFields[objField.Name] = objField;
            }
            return dicFields;
        }
        private List<string> _lstSource = null;
        /// <summary>
        /// 生成代码
        /// </summary>
        public void GenerateCode() 
        {
            //InitDBConfig();

            
            List<string> lstTarget = new List<string>(_lstSource.Count);
            bool isUsing = true;
            Dictionary<string, bool> dicUsing = new Dictionary<string, bool>();

            Dictionary<int, CodeElementPosition> dicNeedVirtual = NeedVirtual();

            for (int i = 0; i < _lstSource.Count; i++) 
            {
                string str=_lstSource[i];
                if (i == _cp.StartLine-1) 
                {
                    if (str.IndexOf("class")>0) 
                    {
                        if (str.IndexOf(" partial ") < 0) 
                        {
                            str = str.Replace("class", "partial class");
                        }
                    }
                    lstTarget.Add(str);
                }
                else if (i == _cp.EndLine - 1) 
                {
                    string space = CutSpace(str) + "    ";
                    foreach (EntityParamField param in _eParamFields) 
                    {
                        if (param.IsGenerate) 
                        {
                            param.AddSource(lstTarget, space);
                        }
                    }
                    foreach (EntityRelationItem relation in _eRelation)
                    {
                        if (relation.IsGenerate)
                        {
                            relation.AddSource(lstTarget, space);
                        }
                    }

                    if( _dbParams!=null)
                    {
                        foreach (EntityParam param in _dbParams)
                        {
                            StringBuilder sb = new StringBuilder();
                            DBEntityInfo.AppendFieldInfo(param, sb);
                            lstTarget.Add(sb.ToString());
                            
                        }
                    }
                    if (_dbRelations != null) 
                    {
                        foreach (TableRelationAttribute er in _dbRelations)
                        {
                            StringBuilder sb = new StringBuilder();
                            DBEntityInfo.FillRelationsInfo(er, sb);
                            lstTarget.Add(sb.ToString());

                        }
                    }


                    AddContext(lstTarget);
                    lstTarget.Add(str);
                    
                }
                
                else if (isUsing && str.IndexOf("namespace " + Namespace) >= 0)
                {
                    AddSqlCommonUsing(dicUsing, lstTarget);
                    lstTarget.Add(str);
                    isUsing = false;
                }
                else
                {
                    if (isUsing)
                    {
                        if (str.IndexOf("using ") >= 0)
                        {
                            dicUsing[str.Trim()] = true;
                        }
                    }
                    if (dicNeedVirtual.ContainsKey(i+1))
                    {
                        str=VirtualProperty(str);
                    }
                    
                    lstTarget.Add(str);
                }
            }

            CodeFileHelper.SaveFile(FileName, lstTarget);
            GenerateExtenCode();
            

        }

        static string[] _modifier ={ "protected internal ","internal protected ","public ", "private ", "protected ", "internal " };


        /// <summary>
        /// Virtual属性
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string VirtualProperty(string str)
        {
            StringBuilder ret = new StringBuilder();

            if (str.IndexOf(" virtual ") < 0)
            {
                
                
                string line=null;
                bool hasAdd = false;
                using (StringReader strRead = new StringReader(str))
                {
                    while ((line = strRead.ReadLine()) != null)
                    {
                        if (hasAdd) 
                        {
                            ret.AppendLine(line);
                            continue;
                        }
                        if (CommonMethods.IsNullOrWhiteSpace(line))
                        {
                            ret.AppendLine(line);
                            continue;
                        }

                        if (line.Trim().IndexOf('[') == 0)
                        {
                            ret.AppendLine(line);
                            continue;
                        }

                        string newLine = line.TrimStart(' ');
                        string space = CutSpace(line);
                        foreach (string mod in _modifier)
                        {
                            if (newLine.StartsWith(mod))
                            {
                                ret.Append(space);
                                ret.Append(mod);
                                ret.Append("virtual ");
                                ret.Append(newLine.Substring(mod.Length));
                                hasAdd = true;
                                return ret.ToString();
                            }
                        }
                        ret.Append(space);
                        ret.Append("virtual ");
                        ret.Append(newLine);
                        hasAdd = true;
                    }
                    
                }
                
                return ret.ToString().TrimEnd('\r','\n');
            }
            return str;

        }
            
        /// <summary>
        /// 获取需要Virtual的属性
        /// </summary>
        /// <returns></returns>
        private Dictionary<int,CodeElementPosition> NeedVirtual() 
        {
            Dictionary<int, CodeElementPosition> lst = new Dictionary<int, CodeElementPosition>();
            CodeElementPosition cp=null;
            foreach (EntityParamField param in _eParamFields)
            {
                if (param.IsGenerate)
                {
                    if (_properties.TryGetValue(param.PropertyName, out cp)) 
                    {
                        int line = cp.StartLine;

                        lst[line] = cp;
                    }
                }
            }
            foreach (EntityRelationItem relation in _eRelation)
            {
                if (relation.IsGenerate)
                {
                    if (_properties.TryGetValue(relation.PropertyName, out cp))
                    {
                        lst[cp.StartLine] = cp;
                    }
                }
            }
            return lst;
        }

        /// <summary>
        /// 添加到代码
        /// </summary>
        /// <param name="lstTarget"></param>
        private void AddContext(List<string> lstTarget)
        {
            if (CurrentDBConfigInfo.Tier != 1 || string.IsNullOrEmpty(TableName))
            {
                return;
            }
            string strField = "_____baseContext";

            string strPropertie = "GetContext";

            if (!ExistsMember<ClrField>( _classType,strField,false))
            {
                lstTarget.Add("        private static ModelContext<" + ClassName + "> " + strField + "=new ModelContext<" + ClassName + ">();");
            }

            if (!ExistsMember<ClrMethod>(_classType, strPropertie, false))
            {
                string hasNew = "";
                if (ExistsMember<ClrMethod>(_classType, strPropertie, true)) 
                {
                    hasNew = "new ";
                }

                lstTarget.Add("        /// <summary>");
                lstTarget.Add("        /// 获取查询关联类");
                lstTarget.Add("        /// </summary>");
                lstTarget.Add("        /// <returns></returns>");
                lstTarget.Add("        public " + hasNew + "static ModelContext<" + ClassName + "> " + strPropertie + "() ");
                lstTarget.Add("        {");
                lstTarget.Add("            return " + ClassName + "." + strField + ";");
                lstTarget.Add("        }");
            }

        }

        /// <summary>
        /// 通过属性名获取信息
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public EntityParamField GetParamInfoByPropertyName(string propertyName)
        {
            Stack<EntityConfig> stkEntity = GetEntity(this, DesignerInfo);
            while (stkEntity.Count > 0)
            {
                EntityConfig curEntity = stkEntity.Pop();
                foreach (EntityParamField item in curEntity.EParamFields)
                {
                    if (item.PropertyName == propertyName)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取类的自身和基类集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Stack<EntityConfig> GetEntity(EntityConfig entity, ClassDesignerInfo info) 
        {
            Stack<EntityConfig> stkConfig = new Stack<EntityConfig>();

            ClrType curType = entity.ClassType;
            string typeName;
            while (curType != null)
            {

                
                stkConfig.Push(entity);
                curType = EntityConfig.GetBaseClass(curType, out typeName);
                if (EntityConfig.IsSystemType(curType))
                {
                    break;
                }
                entity = new EntityConfig(curType, info);
            }
            return stkConfig;
        }

        private static readonly string[] BaseTypes ={ 
            typeof(EntityBase).FullName, typeof(object).FullName,"Buffalo.DB.CommBase.BusinessBases.ThinModelBase" };

        /// <summary>
        /// 是否系统类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSystemType(ClrType type) 
        {
            if (type == null) 
            {
                return true;
            }
            return IsSystemTypeName(type.Name);
            
        }

        

        /// <summary>
        /// 是否系统类名
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static bool IsSystemTypeName(string typeName) 
        {
            if (string.IsNullOrEmpty(typeName)) 
            {
                return true;
            }
            foreach (string basetypeName in BaseTypes)
            {
                if (basetypeName.Equals(typeName))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取所有成员
        /// </summary>
        /// <typeparam name="T">成员类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="fillBase">是否级联父类</param>
        /// <returns></returns>
        public static List<T> GetAllMember<T>( ClrType type,bool fillBase) where T : Member
        {
            List<T> lst = new List<T>();
            Dictionary<string, bool> dicExistsPropertyName = new Dictionary<string, bool>();
            FillAllMember<T>(lst,dicExistsPropertyName, type, fillBase);
            return lst;
        }

        /// <summary>
        /// 获取该类的所有成员
        /// </summary>
        /// <typeparam name="T">成员类型</typeparam>
        /// <param name="lst">集合</param>
        /// <param name="type">类型</param>
        /// <param name="fillBase">是否级联父类</param>
        public static void FillAllMember<T>(List<T> lst, Dictionary<string, bool> dicExistsPropertyName,
            ClrType type, bool fillBase) where T : Member
        {
            
            if (fillBase)
            {
                InheritanceTypeRefMoveableCollection col = type.InheritanceTypeRefs;
                if (col != null && col.Count > 0)
                {
                    string baseType = col[0].TypeTypeName;


                    bool isBaseType = IsSystemTypeName(baseType);

                    if (!isBaseType)
                    {
                        ClrType btype=col[0].ClrType;
                        if (btype != null)
                        {
                            FillAllMember<T>(lst, dicExistsPropertyName, btype, fillBase);
                        }
                    }

                }
            }
           

            foreach (object pro in type.Members) 
            {
                T cPro=pro as T;

                if (cPro != null && !dicExistsPropertyName.ContainsKey(cPro.Name) &&!cPro.IsStatic)
                {
                    dicExistsPropertyName[cPro.Name] = true;
                    lst.Add(cPro);
                }
            }
            
        }
        /// <summary>
        /// 判断是否存在成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="menberName"></param>
        /// <returns></returns>
        public static bool ExistsMember<T>(ClrType type, string menberName) where T : Member
        {
            return ExistsMember<T>(type, menberName, true);
        }
        /// <summary>
        /// 判断是否存在成员
        /// </summary>
        /// <returns></returns>
        public static bool ExistsMember<T>(ClrType type,string menberName,bool checkBase) where T : Member
        {
            bool hasName = false;

            //检查基类
            if (checkBase)
            {
                InheritanceTypeRefMoveableCollection col = type.InheritanceTypeRefs;
                if (col != null && col.Count > 0)
                {
                    string baseType = col[0].TypeTypeName;
                    bool isBaseType = IsSystemTypeName(baseType);
                    if (!isBaseType)
                    {
                        ClrType btype = col[0].ClrType;
                        if (btype != null)
                        {
                            hasName = ExistsMember<T>(btype, menberName);
                        }
                    }
                }
            }
            if (hasName) 
            {
                return hasName;
            }
            //检查本类
            foreach (object pro in type.Members)
            {
                T cPro = pro as T;

                if (cPro!=null)
                {
                    if (cPro.Name == menberName) 
                    {
                        return true;
                    }
                }
            }
            return false;
        }
       

        /// <summary>
        /// 通过属性名查找属性配置信息
        /// </summary>
        /// <param name="stkEntity">实体集合</param>
        /// <param name="propertyName">属性</param>
        /// <returns></returns>
        public static EntityParamField FindParamInfoByName(Stack<EntityConfig> stkEntity,string propertyName) 
        {
            foreach (EntityConfig entity in stkEntity) 
            {
                EntityParamField param = entity.EParamFields.FindByPropertyName(propertyName);
                if (param != null) 
                {
                    return param;
                }
            }
            return null;
        }
        /// <summary>
        /// 通过属性名查找映射属性配置信息
        /// </summary>
        /// <param name="stkEntity">实体集合</param>
        /// <param name="propertyName">映射属性</param>
        /// <returns></returns>
        public static EntityRelationItem FindRelInfoByName(Stack<EntityConfig> stkEntity, string propertyName)
        {
            foreach (EntityConfig entity in stkEntity)
            {
                EntityRelationItem rel = entity.ERelation.FindByPropertyName(propertyName);
                if (rel != null)
                {
                    return rel;
                }
            }
            return null;
        }
        /// <summary>
        /// 所需的using
        /// </summary>
        private static string[] _needUsing ={ 
            "using System.Collections.Generic;" ,"using Buffalo.DB.CommBase;",
            "using Buffalo.Kernel.Defaults;","using Buffalo.DB.PropertyAttributes;",
            "using System.Data;","using Buffalo.DB.CommBase.BusinessBases;"
        };

        /// <summary>
        /// 添加类库必要的using
        /// </summary>
        /// <param name="dicUsing">已有的using集合</param>
        /// <param name="lstTarget">目标源码</param>
        private void AddSqlCommonUsing(Dictionary<string, bool> dicUsing, List<string> lstTarget) 
        {
            foreach (string usingStr in _needUsing) 
            {
                if (!dicUsing.ContainsKey(usingStr)) 
                {
                    lstTarget.Add(usingStr);
                }
            }
        }

        /// <summary>
        /// 生成扩展类代码文件
        /// </summary>
        private void GenerateExtendCode()
        {
            FileInfo fileInfo = new FileInfo(FileName);
            string fileName = fileInfo.DirectoryName + "\\" + fileInfo.Name.Replace(".cs", ".extend.cs");
            if (File.Exists(fileName)) 
            {
                return;
            }

            string model = Models.UserEntity;
            List<string> codes = new List<string>();
            using (StringReader reader = new StringReader(model)) 
            {
                string tmp = null;
                while((tmp=reader.ReadLine())!=null)
                {
                    tmp = tmp.Replace("<%=EntityNamespace%>", Namespace);
                    tmp = tmp.Replace("<%=Summary%>", _summary);


                    string classFullName = null;
                    if (ClassType.Generic)
                    {
                        classFullName=ClassType.GenericTypeName;
                        
                    }
                    else
                    {
                        classFullName=ClassName;
                    }
                    tmp = tmp.Replace("<%=ClassFullName%>", classFullName);
                    codes.Add(tmp);
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem classItem = GetProjectItemByFileName(DesignerInfo, _cp.FileName);
            //DTEHelper.AddFileToProjectItem(item, NHBFilePath, 3);
            EnvDTE.ProjectItem newit = classItem.ProjectItems.AddFromFile(fileName);
            //EnvDTE.ProjectItem newit = _currentProject.ProjectItems.AddFromFile(fileName);
            
            newit.Properties.Item("BuildAction").Value = (int)BuildAction.Code;
        }

        /// <summary>
        /// 获取项目项
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static EnvDTE.ProjectItem GetProjectItemByFileName(ClassDesignerInfo info, string fileName)
        {
            Project project = info.CurrentProject;
            foreach (EnvDTE.ProjectItem item in project.ProjectItems)
            {
                EnvDTE.ProjectItem res = GetProjectItemByFileName(item, fileName);
                if (res != null)
                {
                    return res;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取文件所属的项目项
        /// </summary>
        /// <param name="item">项</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static EnvDTE.ProjectItem GetProjectItemByFileName(EnvDTE.ProjectItem item, string fileName)
        {
            if (item.get_FileNames(0).ToLower() == fileName.ToLower()) return item;
            if (item.ProjectItems.Count == 0) return null;
            foreach (EnvDTE.ProjectItem i in item.ProjectItems)
            {
                EnvDTE.ProjectItem res = GetProjectItemByFileName(i, fileName);
                if (res != null)
                {
                    return res;
                }
            }
            return null;
        }
        
        /// <summary>
        /// 当前表到表信息的转换
        /// </summary>
        /// <returns></returns>
        public KeyWordTableParamItem ToTableInfo() 
        {
            KeyWordTableParamItem table = new KeyWordTableParamItem(TableName, null);
            table.Description = Summary;
            table.IsView = false;
            FillParams(table);
            FillRelation(table);
            return table;
        }

        /// <summary>
        /// 填充关系信息
        /// </summary>
        /// <param name="table"></param>
        private void FillRelation(KeyWordTableParamItem table) 
        {
            table.RelationItems=new List<TableRelationAttribute>();
            foreach (EntityRelationItem er in ERelation) 
            {
                if (!er.IsGenerate) 
                {
                    continue;
                }
                table.RelationItems.Add(er.GetRelationInfo());
            }
            if (_dbRelations != null)
            {
                table.RelationItems.AddRange(_dbRelations);
            }
        }

        /// <summary>
        /// 填充字段信息
        /// </summary>
        /// <param name="table"></param>
        private void FillParams(KeyWordTableParamItem table) 
        {
            table.Params = new List<EntityParam>();
            foreach (EntityParamField field in EParamFields) 
            {
                if (!field.IsGenerate) 
                {
                    continue;
                }
                table.Params.Add(field.ToParamInfo());
            }
            if (_dbParams != null) 
            {
                table.Params.AddRange(_dbParams);
            }
        }

    }
}
