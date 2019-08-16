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
        /// ѡ�е���
        /// </summary>
        public List<ClrClass> SelectedClass
        {
            get { return _selectedClass; }
            set { _selectedClass = value; }
        }
        //private Diagram _selectedDiagram = null;

        ///// <summary>
        ///// ѡ�еĹ�ϵͼ
        ///// </summary>
        //public Diagram SelectedDiagram
        //{
        //    get { return _selectedDiagram; }
        //    set { _selectedDiagram = value; }
        //}
        //private Project _currentProject;
        ///// <summary>
        ///// ��ǰ��Ŀ
        ///// </summary>
        //public Project CurrentProject
        //{
        //    get { return _currentProject; }
        //    set { _currentProject = value; }
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

        ///// <summary>
        ///// ѡ�е��ĵ���ͼ
        ///// </summary>
        //private ClassDesignerDocView _selectDocView;

        ///// <summary>
        ///// ѡ�е��ĵ���ͼ
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
                    FrmCompileResault.ShowCompileResault(null, ex.ToString(), "����SQL����");
                }
            }
            this.Text += ToolVersionInfo.ToolVerInfo;
        }

        /// <summary>
        /// Ҫִ�е�SQL
        /// </summary>
        private List<string> _lstSql;

        /// <summary>
        /// ��ȡ��Ĵ������
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
                MessageBox.Show("�������ʧ��:" + ex.Message);
            }
            ShowSql();
        }

        /// <summary>
        /// ��ʾSQL���
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
        /// ����ֶ���Ϣ
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
                    param.EntityPropertyType, param.Summary, param.Length, param.AllowNull, param.ReadOnly, param.DefaultValue);
                
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
            try
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
            catch (Exception ex) 
            {
                FrmCompileResault.ShowError(ex.ToString(), ToolVersionInfo.ToolVerInfo);
            }
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void labHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder sbInfo = new StringBuilder();
            sbInfo.AppendLine("���ݿ��������ȱ���ֶ�һ�������ڶ��������Ŀ�໥�����");
            sbInfo.AppendLine("���ڱ��������ʵ�ֿ���Ŀ���ʵ������ݿ��ӳ��");
            sbInfo.AppendLine("���裺B��Ŀ������A��Ŀ��B��Ŀ�е�ʵ��������A��Ŀ��ʵ����࣬������B��Ŀ�������ݿ�ʱ��ȱ���ֶε�����");
            sbInfo.AppendLine("\n");
            sbInfo.AppendLine("�����ڳ�������ʱ��ִ����������ȡ׼ȷ�����ݿ�ܹ��������:");
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("Buffalo.DB.DataBaseAdapter.DataAccessLoader.AppendModelAssembly(typeof(InfoDB).Assembly);");
            sbCode.AppendLine("MyClass.GetDBinfo().GetUpdateDBText();");
            sbCode.AppendLine("MyClass.InitDB();");
            sbCode.AppendLine("string sql = MyClass.GetDBinfo().GetUpdateDBText();");
            sbCode.AppendLine("\n");
            sbCode.AppendLine("����InfoDBΪA��Ŀ�����ݿ��࣬MyClassΪB��Ŀ�����ݿ�");
            FrmCompileResault.ShowCompileResault(sbInfo.ToString(), sbCode.ToString(), "���ݿ�������䲻׼ȷ��ô��?");
        }

    }
}