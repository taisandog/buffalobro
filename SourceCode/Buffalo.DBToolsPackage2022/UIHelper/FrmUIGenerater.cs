using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using EnvDTE;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using System.Xml;
using Buffalo.DBTools.HelperKernel;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Buffalo.WinFormsControl.Editors;
using Buffalo.Kernel.FastReflection;
using System.Security.AccessControl;
using Buffalo.DBTools.UIHelper.ModelLoader;
using System.Reflection;
using Buffalo.Win32Kernel;
using Buffalo.DBToolsPackage;
using DBTools;

namespace Buffalo.DBTools.UIHelper
{
    public partial class FrmUIGenerater : Form
    {
        public FrmUIGenerater()
        {
            InitializeComponent();
        }
        private EntityInfo _curEntityInfo;
        /// <summary>
        /// 当前实体的信息
        /// </summary>
        public EntityInfo CurEntityInfo
        {
            get { return _curEntityInfo; }
            set { _curEntityInfo = value; }
        }

        //private object _currentEntity;

        private UIModelItem _classInfo = null;

        private UIConfigItem _config;
        /// <summary>
        /// 配置信息
        /// </summary>
        public UIConfigItem Config
        {
            get { return _config; }
        }

        private Dictionary<string, EditorBase> _classUIConfig = null;
        private Dictionary<string, EditorBase> _propertyUIConfig = null;

        private Dictionary<string, ConfigItem> _configClassInfo = null;
        private Dictionary<string, ConfigItem> _configItemInfo = null;
        #region 绑定信息
        /// <summary>
        /// 绑定属性信息
        /// </summary>
        private void BindItems(List<UIModelItem> lstItems) 
        {
           
           
            BindingList<UIModelItem> lst = new BindingList<UIModelItem>(lstItems);

            gvMember.DataSource = lst;
            BindProjects();
        }

        private List<Project> _lstProjects;

        /// <summary>
        /// 绑定项目集合
        /// </summary>
        public void BindTargetProjects(List<Project> lstProjects) 
        {
            List<ComboBoxItem> lst = new List<ComboBoxItem>();
            foreach(Project objProject in lstProjects)
            {
                
                ComboBoxItem item = new ComboBoxItem(objProject.Name, objProject);
                lst.Add(item);
            }
            _lstProjects = lstProjects;
            cmbProjects.BindValue(lst);
        }

        /// <summary>
        /// 绑定项目信息
        /// </summary>
        private void BindProjects() 
        {
            List<UIProject> lstPorject = _config.Projects;
            List<ComboBoxItem> lst = new List<ComboBoxItem>();
            foreach (UIProject objProject in lstPorject)
            {

                ComboBoxItem item = new ComboBoxItem(objProject.Name, objProject);
                lst.Add(item);
            }
            cmbModels.BindValue(lst);
        }

        /// <summary>
        /// 创建类级别信息
        /// </summary>
        private void CreateClassItem() 
        {
            _classUIConfig = new Dictionary<string, EditorBase>();
            List<ConfigItem> lstItem = _config.ClassItems;
            pnlClassConfig.Controls.Clear();
            _configClassInfo = new Dictionary<string, ConfigItem>();
            for (int i = 0; i < lstItem.Count; i++)
            {
                ConfigItem curItem=lstItem[i];
                _configClassInfo[curItem.Name] = curItem;
                EditorBase editor = NewItem(curItem);

                pnlClassConfig.Controls.Add(editor);
                //tabPanel.Controls.SetChildIndex(editor, i);
                editor.OnValueChange += new ValueChangeHandle(editorClass_OnValueChange);
                _classUIConfig[editor.BindPropertyName] = editor;
            }
        }
        void editorClass_OnValueChange(object sender, object oldValue, object newValue)
        {
            EditorBase editor = sender as EditorBase;
            if (editor == null)
            {
                return;
            }
            
            _classInfo.CheckItem[editor.BindPropertyName] = editor.Value;

        }
        /// <summary>
        /// 创建控件
        /// </summary>
        private void CreateItems() 
        {
            _propertyUIConfig = new Dictionary<string, EditorBase>();
            List<ConfigItem> lstItem = _config.ConfigItems;
            tabPanel.ColumnCount = 2;
            tabPanel.RowCount = (int)Math.Ceiling((double)lstItem.Count / (double)2)+1;
            tabPanel.RowStyles.Clear();
            tabPanel.ColumnStyles.Clear();
            AddCellStyle();
            _configItemInfo = new Dictionary<string, ConfigItem>();
            for (int i = 0; i < lstItem.Count; i++) 
            {
                ConfigItem curItem = lstItem[i];
                _configItemInfo[curItem.Name] = curItem;
                EditorBase editor = NewItem(curItem);
                int col = i % 2;
                int row = i / 2;
                tabPanel.Controls.Add(editor, col, row);
                tabPanel.Controls.SetChildIndex(editor, i);
                editor.OnValueChange += new ValueChangeHandle(editor_OnValueChange);
                editor.Dock = DockStyle.Left;
                _propertyUIConfig[editor.BindPropertyName] = editor;
            }
        }
        /// <summary>
        /// 添加单元格样式
        /// </summary>
        private void AddCellStyle()
        {
            tabPanel.RowStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tabPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        }

        /// <summary>
        /// 创建新的设置项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private EditorBase NewItem(ConfigItem item) 
        {
            EditorBase editor = item.GetEditor();
            
            editor.BindPropertyName = item.Name;
            editor.LableText = item.Summary;
            editor.LableFont = new Font(editor.LableFont.FontFamily, 9, FontStyle.Bold);
            editor.LableWidth=80;
            editor.Width = 250;
            
            return editor;
        }

        void editor_OnValueChange(object sender, object oldValue, object newValue)
        {
            EditorBase editor = sender as EditorBase;
            if (editor == null) 
            {
                return;
            }
            if(_currentItem==null)
            {
                return;
            }
            _currentItem.CheckItem[editor.BindPropertyName] = editor.Value;

        }
        #endregion



        /// <summary>
        /// 加载信息
        /// </summary>
        private void LoadInfo() 
        {
            if (_curEntityInfo == null)
            {
                return;
            }
            XmlDocument doc = LoadConfig();
            _config = new UIConfigItem(doc, _curEntityInfo.DesignerInfo);



            List<UIModelItem> lstItems = _curEntityInfo.Propertys;

            foreach (UIModelItem item in lstItems)
            {
                item.InitDefaultValue(_config.ConfigItems, CurEntityInfo, CurEntityInfo.DesignerInfo.CurrentProject, item);
            }
        }

        private void FrmUIGenerater_Load(object sender, EventArgs e)
        {
            
            gvMember.AutoGenerateColumns = false;
            try
            {
                
                LoadInfo();
            }
            catch (Exception ex) 
            {
                FrmCompileResault.ShowCompileResault(null, ex.ToString(), "加载模版失败");
                this.Close();
            }
            CreateClassItem();
            CreateItems();
            LoadItemCache();
            LoadClassItemCache();
            BindUIModleInfo(_classUIConfig, _classInfo);
            BindItems(_curEntityInfo.Propertys);
            this.Text = "UI界面生成-" + _curEntityInfo.ClassName+ToolVersionInfo.ToolVerInfo;
            
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private UIModelItem _currentItem = null;

        private void gvMember_CurrentCellChanged(object sender, EventArgs e)
        {
            if(gvMember.CurrentCell==null)
            {

                return;
            }
            _currentItem = gvMember.Rows[gvMember.CurrentCell.RowIndex].DataBoundItem as UIModelItem;
            if (_propertyUIConfig != null && _currentItem != null)
            {
                BindUIModleInfo(_propertyUIConfig, _currentItem);
            }
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            SaveItemInfos();
            SaveClassItemInfo();

            if (cmbModels.Value!=null)
            {
                UIProject project = cmbModels.Value as UIProject;
                if (project != null) 
                {
                    try
                    {
                        Project selectedProject = cmbProjects.Value as Project;
                        project.GenerateCode(_curEntityInfo, _config,selectedProject, GetProperty(false), _classInfo);
                        this.Close();
                    }
                    catch (CompileException cex) 
                    {
                        FrmCompileResault.ShowCompileResault(cex.Code, cex.ToString(),"编译错误");
                    }
                    catch (Exception ex)
                    {
                        FrmCompileResault.ShowError( ex.ToString(), "编译错误");
                        return;
                    }
                }
                
            }
        }
        #region 保存和加载
        private string _modelPath;
        /// <summary>
        /// 模版根目录
        /// </summary>
        private string ModelPath
        {
            get
            {
                FileInfo file = new FileInfo(_curEntityInfo.DesignerInfo.CurrentProject.FileName);
                string directory = file.DirectoryName;
                directory = directory + "\\.bmodels\\";
                return directory;
            }
        }

        /// <summary>
        /// 检测文件
        /// </summary>
        private XmlDocument LoadConfig()
        {
            string directory = ModelPath;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string xml = "UIConfigItem.xml";
            string fileName = directory + xml;
            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, Models.uiconfigitem);
            }

            string dvModel = directory + "DataView.bm";
            if (!File.Exists(dvModel))
            {
                File.WriteAllText(dvModel, Models.dataview);
            }

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(fileName);

            return xmldoc;
        }

        /// <summary>
        /// 获取项
        /// </summary>
        /// <param name="isAll">是否全部项</param>
        /// <returns></returns>
        private List<UIModelItem> GetProperty(bool isAll) 
        {
            BindingList<UIModelItem> lst = gvMember.DataSource as BindingList<UIModelItem>;
            List<UIModelItem> lstRet = new List<UIModelItem>(lst.Count);

            EntityConfig entity = new EntityConfig(CurEntityInfo.ClassType, CurEntityInfo.DesignerInfo);
            foreach (UIModelItem item in lst)
            {
                if ((!isAll) && (!item.IsGenerate))
                {
                    continue;
                }

                lstRet.Add(item);
            }
            return lstRet;
        }

        /// <summary>
        /// 保存项选择信息
        /// </summary>
        private void SaveItemInfos() 
        {
            XmlDocument doc = EntityMappingConfig.NewXmlDocument();
            XmlNode root = doc.CreateElement("root");
            doc.AppendChild(root);

            
            List<UIModelItem> lst = GetProperty(true);
            foreach (UIModelItem item in lst)
            {

                XmlNode inode = doc.CreateElement("modelitem");
                root.AppendChild(inode);
                item.WriteNode(inode);
            }
            string directory = ModelPath + "gencache\\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                File.SetAttributes(directory, FileAttributes.Hidden);
            }
            string fileName = directory + "\\" + _curEntityInfo.FullName + ".cache.xml";
            doc.Save(fileName);
        }

        /// <summary>
        /// 保存类信息
        /// </summary>
        private void SaveClassItemInfo() 
        {
            XmlDocument doc = EntityMappingConfig.NewXmlDocument();
            XmlNode root = doc.CreateElement("root");
            doc.AppendChild(root);

            XmlAttribute attRoot = doc.CreateAttribute("model");
            UIProject uproject = cmbModels.Value as UIProject;
            if (uproject != null)
            {
                attRoot.InnerText = uproject.Name;
                root.Attributes.Append(attRoot);
            }

            attRoot = doc.CreateAttribute("project");
            Project project = cmbProjects.Value as Project;
            if (project != null)
            {
                attRoot.InnerText = project.Name;
                root.Attributes.Append(attRoot);
            }


            Dictionary<string,object> div = _classInfo.CheckItem;
            _classInfo.WriteNode(root);

            string directory = ModelPath + "\\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                File.SetAttributes(directory, FileAttributes.Hidden);
            }
            string fileName = directory + "\\classinfo.cache.xml";
            doc.Save(fileName);
        }

        /// <summary>
        /// 加载属性项信息
        /// </summary>
        private void LoadClassItemCache() 
        {
            _classInfo = new UIModelItem();
            _classInfo.InitDefaultValue(_config.ClassItems, CurEntityInfo, CurEntityInfo.DesignerInfo.CurrentProject,null);
            string fileName =ModelPath+ "\\classinfo.cache.xml";
            if (!File.Exists(fileName)) 
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(fileName);
            }
            catch { return; }
            XmlNodeList nodes = doc.GetElementsByTagName("root");
            if (nodes.Count <= 0) 
            {
                return;
            }
            XmlNode nodeRoot = nodes[0];

            XmlAttribute attRoot = nodeRoot.Attributes["model"];
            if (attRoot != null) 
            {
                string mname=attRoot.InnerText;
                if (!string.IsNullOrEmpty(mname))
                {
                    foreach (UIProject upro in _config.Projects)
                    {
                        if (upro.Name == mname)
                        {
                            cmbModels.Value = upro;
                            break;
                        }
                    }
                }
            }

            attRoot = nodeRoot.Attributes["project"];
            if (attRoot != null)
            {
                string pname = attRoot.InnerText;
                if (!string.IsNullOrEmpty(pname))
                {
                    foreach (Project ipro in _lstProjects)
                    {
                        if (ipro.Name == pname)
                        {
                            cmbProjects.Value = ipro;
                            break;
                        }
                    }
                }
            }
            
            _classInfo.ReadItem(nodeRoot, _configClassInfo);
            
        }


        /// <summary>
        /// 加载属性项信息
        /// </summary>
        private void LoadItemCache()
        {
            string directory = ModelPath + "gencache\\";
            string fileName = directory + _curEntityInfo.FullName + ".cache.xml";

            if (!File.Exists(fileName))
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(fileName);
            }
            catch { return; }
            XmlNodeList nodes = doc.GetElementsByTagName("modelitem");


            List<UIModelItem> lstCurPropertys = _curEntityInfo.Propertys;
            List<UIModelItem> lstNewPropertys = new List<UIModelItem>(lstCurPropertys.Count);
            for (int j=0;j<nodes.Count;j++)
            {
                XmlNode node = nodes[j];
                XmlAttribute att=node.Attributes["name"];
                if(att==null)
                {
                    continue;
                }
                string name=att.InnerText;
                if(name==null)
                {
                    continue;
                }
                for (int i=lstCurPropertys.Count-1;i>=0;i--) 
                {
                    UIModelItem item = lstCurPropertys[i];
                    if (item.PropertyName == name) 
                    {
                        att = node.Attributes["isgen"];
                        if (att != null)
                        {
                            item.IsGenerate = att.InnerText == "1";
                        }

                        item.ReadItem(node,_configItemInfo);
                        lstNewPropertys.Add(item);
                        lstCurPropertys.RemoveAt(i);
                        break;
                    }
                }
                
            }
            _curEntityInfo.Propertys = lstNewPropertys;
            
        }
        /// <summary>
        /// 绑定UI信息
        /// </summary>
        /// <param name="dicControl"></param>
        /// <param name="item"></param>
        private void BindUIModleInfo(Dictionary<string, EditorBase> dicControl, UIModelItem item) 
        {
            
            Dictionary<string, object> dic=item.CheckItem;

            List<EditorBase> lstEditor = new List<EditorBase>(dicControl.Count);
            foreach (KeyValuePair<string, EditorBase> kvp in dicControl) 
            {
                lstEditor.Add(kvp.Value);
            }
            object value=null;
            foreach (EditorBase editor in lstEditor) 
            {
                string key = editor.BindPropertyName;
                if (item.CheckItem.TryGetValue(key, out value))
                {
                    editor.Value = value;
                }
                else 
                {
                    editor.Reset();
                }
            }

        }

        

        #endregion
        private void labInfo_Click(object sender, EventArgs e)
        {

        }

        //private void gvProject_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex < 0) 
        //    {
        //        return;
        //    }
        //    string colName = gvProject.Columns[e.ColumnIndex].Name;
        //    if (colName == "colRefreash") 
        //    {
        //        UIProject item = gvProject.Rows[e.RowIndex].DataBoundItem as UIProject;
        //        item.ClearCache(_curEntityInfo);
        //    }


        //}


        private void btnReFreash_Click(object sender, EventArgs e)
        {
            CodeGenCache.Clear();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            
            if (gvMember.SelectedRows.Count == 0) 
            {
                return;
            }
            int index = gvMember.SelectedRows[0].Index;
            if (index >= gvMember.Rows.Count-1) 
            {
                return;
            }
            BindingList<UIModelItem> lst = gvMember.DataSource as BindingList<UIModelItem>;
            if (lst == null)
            {
                return;
            }
            UIModelItem tmp = lst[index];
            lst[index] = lst[index + 1];
            lst[index + 1] = tmp;
            gvMember.Rows[index + 1].Selected = true;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (gvMember.SelectedRows.Count == 0)
            {
                return;
            }
            int index = gvMember.SelectedRows[0].Index;
            if (index <= 0)
            {
                return;
            }
            BindingList<UIModelItem> lst = gvMember.DataSource as BindingList<UIModelItem>;
            if (lst == null)
            {
                return;
            }
            UIModelItem tmp = lst[index];
            lst[index] = lst[index - 1];
            lst[index - 1] = tmp;
            gvMember.Rows[index - 1].Selected = true;
        }

        

    }
}