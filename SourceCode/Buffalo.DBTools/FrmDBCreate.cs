using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using Microsoft.VisualStudio.Modeling.Diagrams;
using EnvDTE;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Buffalo.DBTools.HelperKernel;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLBaseFunction;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel;
using Buffalo.DB.DBCheckers;
using Buffalo.DB.PropertyAttributes;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using Buffalo.DB.CommBase.BusinessBases;
using Buffalo.DBTools.UIHelper;

namespace Buffalo.DBTools
{
    public partial class FrmDBCreate : Form
    {
        public FrmDBCreate()
        {
            InitializeComponent();
        }
        private List<ClrClass> _selectedClass = null;

        /// <summary>
        /// 选中的类
        /// </summary>
        public List<ClrClass> SelectedClass
        {
            get { return _selectedClass; }
            set { _selectedClass = value; }
        }
        //private Diagram _selectedDiagram = null;

        ///// <summary>
        ///// 选中的关系图
        ///// </summary>
        //public Diagram SelectedDiagram
        //{
        //    get { return _selectedDiagram; }
        //    set { _selectedDiagram = value; }
        //}
        //private Project _currentProject;
        ///// <summary>
        ///// 当前项目
        ///// </summary>
        //public Project CurrentProject
        //{
        //    get { return _currentProject; }
        //    set { _currentProject = value; }
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

        ///// <summary>
        ///// 选中的文档视图
        ///// </summary>
        //private ClassDesignerDocView _selectDocView;

        ///// <summary>
        ///// 选中的文档视图
        ///// </summary>
        //public ClassDesignerDocView SelectDocView
        //{
        //    get { return _selectDocView; }
        //    set { _selectDocView = value; }
        //}
        private void FrmDBCreate_Load(object sender, EventArgs e)
        {
            if (SelectedClass != null) 
            {
                try
                {
                    GetClassSQL();
                }
                catch (Exception ex) 
                {
                    FrmCompileResault.ShowCompileResault(null, ex.ToString(), "生成SQL错误");
                }
            }
            this.Text += ToolVersionInfo.ToolVerInfo;
        }

        /// <summary>
        /// 要执行的SQL
        /// </summary>
        private List<string> _lstSql;

        /// <summary>
        /// 获取类的创建语句
        /// </summary>
        /// <param name="type"></param>
        private void GetClassSQL() 
        {
            _lstSql = new List<string>();
            List<KeyWordTableParamItem> lstTable = new List<KeyWordTableParamItem>();
            DBConfigInfo dbcinfo = FrmDBSetting.GetDBConfigInfo(DesignerInfo,  "DataAccess.");
            DBInfo dbInfo = dbcinfo.CreateDBInfo();

            foreach (ClrClass curType in SelectedClass)
            {
                EntityConfig entity = new EntityConfig(curType, DesignerInfo);

                if (string.IsNullOrEmpty(entity.TableName) || !entity.IsTable)
                {
                    continue;
                }
                string typeName = null;
                Stack<EntityConfig> stkConfig = EntityConfig.GetEntity(entity, DesignerInfo);
                List<EntityParam> lstParam = new List<EntityParam>();
                List<TableRelationAttribute> lstRelation = new List<TableRelationAttribute>();
                string lastTableName = null;
                string lastSummary=null;
                while (stkConfig.Count > 0)
                {
                    EntityConfig centity = stkConfig.Pop();
                    FillParams(centity, lstParam, lstRelation);
                    lastTableName = centity.TableName;
                    lastSummary=centity.Summary;
                }
                KeyWordTableParamItem table = new KeyWordTableParamItem(lstParam, lstRelation, lastTableName, null);
                table.Description = lastSummary;
                lstTable.Add(table);
                
            }
            try
            {
                using (BatchAction ba = dbInfo.DefaultOperate.StarBatchAction())
                {
                    _lstSql = TableChecker.CheckTable(dbInfo, lstTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("生成语句失败:" + ex.Message);
            }
            ShowSql();
        }

        /// <summary>
        /// 显示SQL语句
        /// </summary>
        private void ShowSql() 
        {
            StringBuilder sbSql = new StringBuilder();
            foreach (string sql in _lstSql) 
            {
                sbSql.AppendLine(sql+";");
            }
            rtbContent.Text = sbSql.ToString();
        }

        /// <summary>
        /// 填充字段信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lstParam"></param>
        private void FillParams(EntityConfig entity, List<EntityParam> lstParam, List<TableRelationAttribute> lstRelation) 
        {
            foreach (EntityParamField param in entity.EParamFields) 
            {
                if (!param.IsGenerate) 
                {
                    continue;
                }
                
                DbType dbt=(DbType)EnumUnit.GetEnumInfoByName(typeof(DbType),param.DbType).Value;
                EntityParam pInfo = new EntityParam("",
                    param.ParamName,"",dbt,
                    param.EntityPropertyType, param.Summary, param.Length, param.AllowNull, param.ReadOnly);
                
                lstParam.Add(pInfo);
            }
            foreach (EntityRelationItem relation in entity.ERelation) 
            {
                if (relation.IsToDB && relation.IsGenerate && relation.IsParent) 
                {
                    EntityConfig parent = new EntityConfig(relation.FInfo.MemberType, DesignerInfo);
                    if (parent == null)
                    {
                        continue;
                    }
                    EntityParamField childProperty = entity.GetParamInfoByPropertyName(relation.SourceProperty);
                    if (childProperty == null) 
                    {
                        continue;
                    }
                    EntityParamField parentProperty = parent.GetParamInfoByPropertyName(relation.TargetProperty);
                    if (parentProperty == null) 
                    {
                        continue;
                    }
                    lstRelation.Add(new TableRelationAttribute("","", entity.TableName,
                        parent.TableName, childProperty.ParamName, parentProperty.ParamName, "", relation.IsParent));
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            DBConfigInfo dbcinfo = FrmDBSetting.GetDBConfigInfo(DesignerInfo, "");
            DBInfo dbInfo = dbcinfo.CreateDBInfo();
            rtbOutput.Text = "";
            if (_lstSql == null || _lstSql.Count == 0) 
            {
                return;
            }
            List<string> resault = DBChecker.ExecuteSQL(dbInfo.DefaultOperate, _lstSql);
            StringBuilder sbRet = new StringBuilder();

            foreach (string res in resault)
            {
                sbRet.AppendLine(res);
            }
            rtbOutput.Text = sbRet.ToString();
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void labHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder sbInfo = new StringBuilder();
            sbInfo.AppendLine("数据库生成语句缺少字段一般是由于多个数据项目相互引起的");
            sbInfo.AppendLine("由于本插件不可实现跨项目检测实体和数据库的映射");
            sbInfo.AppendLine("假设：B项目引用了A项目，B项目中的实体派生自A项目的实体基类，则会出现B项目生成数据库时候缺少字段的问题");
            sbInfo.AppendLine("\n");
            sbInfo.AppendLine("可以在程序启动时候执行以下语句获取准确的数据库架构升级语句:");
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("Buffalo.DB.DataBaseAdapter.DataAccessLoader.AppendModelAssembly(typeof(InfoDB).Assembly);");
            sbCode.AppendLine("MyClass.GetDBinfo().GetUpdateDBText();");
            sbCode.AppendLine("MyClass.InitDB();");
            sbCode.AppendLine("string sql = MyClass.GetDBinfo().GetUpdateDBText();");
            sbCode.AppendLine("\n");
            sbCode.AppendLine("其中InfoDB为A项目的数据库类，MyClass为B项目的数据库");
            FrmCompileResault.ShowCompileResault(sbInfo.ToString(), sbCode.ToString(), "数据库生成语句不准确怎么办?");
        }

    }
}