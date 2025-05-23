using System;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using System.Runtime.InteropServices;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DBTools.HelperKernel;
using Buffalo.DBTools.DocSummary;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using System.Collections;
using Buffalo.DBTools.DocSummary.VSConfig;
using System.Windows.Forms;
using System.Text;
using System.IO;
using Buffalo.DBTools.ROMHelper;
using Buffalo.DBTools.UIHelper;

namespace Buffalo.DBTools
{
	/// <summary>用于实现外接程序的对象。</summary>
	/// <seealso class='IDTExtensibility2' />
    [GuidAttribute("6BA92A5A-614E-4f6c-88D5-0733F04648C2"), ProgId("Buffalo.DBTools.Connect")]
    public class Connect : IDTCommandTarget
    {
        /// <summary>实现外接程序对象的构造函数。请将您的初始化代码置于此方法内。</summary>
        public Connect()
        {
        }

        private DTE2 _applicationObject;
        //private AddIn _addInInstance;
        private IServiceProvider m_serviceProvider;
        Commands2 _commands = null;

        CommandItems _commitem = new CommandItems();
        

        /// <summary>
        /// 类设计图信息
        /// </summary>
        public ClassDesignerInfo GetDesignerInfo()
        {
            ClassDesignerInfo designerInfo = new ClassDesignerInfo(SelectedDiagram, CurrentProject, SelectDocView);
            return designerInfo;
        }
        /// <summary>
        /// 是否此命令
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        private bool IsCommand(string commandName, string modelName)
        {
            string cmd = this.GetType().FullName + "." + modelName;
            return cmd.Equals(commandName);
        }
        /// <summary>
        /// 通过类名找到类
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="selectedDiagram">类图</param>
        /// <returns></returns>
        public static ClrClass FindClrClassByName(string className, Diagram selectedDiagram)
        {
            List<ClrClass> lstClass = GetAllClass(selectedDiagram);
            foreach (ClrClass cls in lstClass) 
            {
                if (EntityConfig.GetFullName(cls) == className) 
                {
                    return cls;
                }
            }
            return null;
        }


        

        /// <summary>实现 IDTExtensibility2 接口的 OnAddInsUpdate 方法。当外接程序集合已发生更改时接收通知。</summary>
        /// <param term='custom'>特定于宿主应用程序的参数数组。</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>实现 IDTExtensibility2 接口的 OnStartupComplete 方法。接收宿主应用程序已完成加载的通知。</summary>
        /// <param term='custom'>特定于宿主应用程序的参数数组。</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>实现 IDTExtensibility2 接口的 OnBeginShutdown 方法。接收正在卸载宿主应用程序的通知。</summary>
        /// <param term='custom'>特定于宿主应用程序的参数数组。</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
            
        }

        /// <summary>实现 IDTCommandTarget 接口的 QueryStatus 方法。此方法在更新该命令的可用性时调用</summary>
        /// <param term='commandName'>要确定其状态的命令的名称。</param>
        /// <param term='neededText'>该命令所需的文本。</param>
        /// <param term='status'>该命令在用户界面中的状态。</param>
        /// <param term='commandText'>neededText 参数所要求的文本。</param>
        /// <seealso class='Exec' />
        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
        {
            if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {

                if (commandName == "BuffaloEntityConfig.Connect" ||
                    commandName == "BuffaloDBCreater.Connect")
                {
                    SelectedShapesCollection selectedShapes = SelectedShapes;
                    if (selectedShapes == null)
                    {
                        status = (vsCommandStatus)vsCommandStatus.vsCommandStatusUnsupported |
                            vsCommandStatus.vsCommandStatusInvisible;
                        return;
                    }
                    bool findClass = false;
                    for (int i = 0; i < selectedShapes.Count; i++)
                    {
                        if (!(selectedShapes.TopLevelItems[i].Shape is ClrTypeShape))
                        {
                            continue;
                        }

                        ClrTypeShape sp = selectedShapes.TopLevelItems[i].Shape as ClrTypeShape;
                        if (!(sp.AssociatedType is ClrClass))
                        {
                            continue;
                        }
                        status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported |
                            vsCommandStatus.vsCommandStatusEnabled;
                        findClass = true;
                        break;
                    }
                    if (findClass == false)
                    {
                        status = (vsCommandStatus)vsCommandStatus.vsCommandStatusUnsupported |
                            vsCommandStatus.vsCommandStatusInvisible;
                        return;
                    }
                }

                status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported |
                    vsCommandStatus.vsCommandStatusEnabled;

            }
        }

        /// <summary>
        /// 当前项目
        /// </summary>
        public Project CurrentProject
        {
            get
            {
                Project prj = null;
                if (_applicationObject.SelectedItems.Count == 0) return null;
                EnvDTE.SelectedItem item = _applicationObject.SelectedItems.Item(1);
                if (item.Project != null)
                {
                    prj = item.Project;
                }
                else
                {
                    prj = item.ProjectItem.ProjectItems.ContainingProject;
                }

                //
                return prj;
            }
        }

        /// <summary>
        /// 获取所有项目
        /// </summary>
        public List<Project> AllProjects 
        {
            get 
            {
                List<Project> lstProject = new List<Project>();
                Solution2 soln = _applicationObject.Solution as Solution2;
                IEnumerator ienum = soln.Projects.GetEnumerator();

                while (ienum.MoveNext()) 
                {
                    Project objPro = ienum.Current as Project;
                    if (objPro != null)
                    {
                        try
                        {
                            string filename = objPro.FileName;
                            if (!string.IsNullOrEmpty(filename))
                            {
                                lstProject.Add(objPro);
                            }
                        }
                        catch { }
                       
                    }
                }
                return lstProject;
            }
        }

        
        /// <summary>
        /// 选中的图形
        /// </summary>
        public SelectedShapesCollection SelectedShapes
        {
            get
            {
                ClassDesignerDocView docView = SelectDocView;
                if (docView == null)
                {
                    return null;
                }

                return docView.CurrentDesigner.Selection;
            }
        }



        /// <summary>
        /// 选中的图
        /// </summary>
        public Diagram SelectedDiagram
        {
            get
            {
                ClassDesignerDocView docView = SelectDocView;
                if (docView == null)
                {
                    return null;
                }
                return docView.CurrentDiagram;
            }
        }

        /// <summary>
        /// 选中的文档视图
        /// </summary>
        public ClassDesignerDocView SelectDocView
        {
            get
            {
                Microsoft.VisualStudio.Shell.Interop.ISelectionContainer selectionContainer;
                Microsoft.VisualStudio.Shell.Interop.IVsMonitorSelection monitorService = m_serviceProvider.GetService(typeof(Microsoft.VisualStudio.Shell.Interop.IVsMonitorSelection)) as Microsoft.VisualStudio.Shell.Interop.IVsMonitorSelection;
                if (monitorService != null)
                {
                    IntPtr ppHier, ppSC;
                    uint pitemid;
                    Microsoft.VisualStudio.Shell.Interop.IVsMultiItemSelect ppMIS;
                    Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(monitorService.GetCurrentSelection(out ppHier, out pitemid, out ppMIS, out ppSC));
                    if (ppSC != IntPtr.Zero)
                    {
                        selectionContainer = Marshal.GetObjectForIUnknown(ppSC) as Microsoft.VisualStudio.Shell.Interop.ISelectionContainer;
                        //ISelectionService SelectionContainer = selectionContainer as ISelectionService;
                        return selectionContainer as Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.ClassDesignerDocView;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 获取所有类
        /// </summary>
        /// <returns></returns>
        public static List<ClrClass> GetAllClass(Diagram selectedDiagram) 
        {
            ShapeElementMoveableCollection nestedChildShapes = selectedDiagram.NestedChildShapes;
            List<ClrClass> lstClass = new List<ClrClass>();
            IEnumerable shapes = nestedChildShapes as IEnumerable;
            if (shapes == null)
            {
                return null;
            }
            foreach (ShapeElement element in shapes)
            {
                ClrClass classType = SummaryShape.GetClass(element);
                if (classType != null)
                {
                    lstClass.Add(classType);
                }

            }
            return lstClass;
        }

        /// <summary>实现 IDTCommandTarget 接口的 Exec 方法。此方法在调用该命令时调用。</summary>
        /// <param term='commandName'>要执行的命令的名称。</param>
        /// <param term='executeOption'>描述该命令应如何运行。</param>
        /// <param term='varIn'>从调用方传递到命令处理程序的参数。</param>
        /// <param term='varOut'>从命令处理程序传递到调用方的参数。</param>
        /// <param term='handled'>通知调用方此命令是否已被处理。</param>
        /// <seealso class='Exec' />
        public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
        {
            handled = false;
            if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
            {
                try
                {
                    if (IsCommand(commandName, "BuffaloEntityConfig"))
                    {
                        SelectedShapesCollection selectedShapes = SelectedShapes;
                        if (selectedShapes == null) return;
                        for (int i = 0; i < selectedShapes.Count; i++)
                        {
                            if (!(selectedShapes.TopLevelItems[i].Shape is ClrTypeShape)) continue;
                            ClrTypeShape sp = selectedShapes.TopLevelItems[i].Shape as ClrTypeShape;
                            if (!(sp.AssociatedType is ClrClass))
                            {
                                continue;
                            }
                            using (FrmClassDesigner st = new FrmClassDesigner())
                            {
                                Diagram selDiagram = SelectedDiagram;
                                st.SelectedClass = sp;
                                st.DesignerInfo = GetDesignerInfo();
                                st.ShowDialog();
                            }
                        }
                        handled = true;
                        return;
                    }
                    else if (IsCommand(commandName, "BuffaloDBCreater"))
                    {
                        SelectedShapesCollection selectedShapes = SelectedShapes;
                        if (selectedShapes == null) return;
                        List<ClrClass> lstClass = new List<ClrClass>();
                        for (int i = 0; i < selectedShapes.Count; i++)
                        {
                            if (!(selectedShapes.TopLevelItems[i].Shape is ClrTypeShape))
                            {
                                continue;
                            }
                            ClrTypeShape sp = selectedShapes.TopLevelItems[i].Shape as ClrTypeShape;
                            ClrClass classType = sp.AssociatedType as ClrClass;
                            if (classType == null)
                            {
                                continue;
                            }
                            lstClass.Add(classType);
                        }
                        using (FrmDBCreate st = new FrmDBCreate())
                        {
                            Diagram selDiagram = SelectedDiagram;
                            st.SelectedClass = lstClass;
                            //st.SelectDocView = SelectDocView;
                            //st.CurrentProject = CurrentProject;
                            //st.SelectedDiagram = selDiagram;
                            st.DesignerInfo = GetDesignerInfo();
                            st.ShowDialog();
                        }
                        handled = true;
                        return;
                    }

                    else if (IsCommand(commandName, "BuffaloDBToEntity"))
                    {
                        Diagram dia = SelectedDiagram;
                        if (!(dia is ShapeElement))
                        {
                            return;
                        }
                        using (FrmAllTables frmTables = new FrmAllTables())
                        {
                            //frmTables.SelectedDiagram = dia;
                            //frmTables.SelectDocView = SelectDocView;
                            //frmTables.CurrentProject = CurrentProject;
                            frmTables.DesignerInfo = GetDesignerInfo();
                            frmTables.ShowDialog();
                        }

                    }

                    else if (IsCommand(commandName, "BuffaloShowHideSummery"))
                    {
                        Diagram dia = this.SelectedDiagram;

                        if (dia != null)
                        {

                            VSConfigManager.InitConfig(_applicationObject.Version);
                            ShapeSummaryDisplayer.ShowOrHideSummary(dia, this);
                            this.SelectDocView.CurrentDesigner.ScrollDown();
                            this.SelectDocView.CurrentDesigner.ScrollUp();

                            handled = true;
                        }

                    }
                    else if (IsCommand(commandName, "BuffaloDBCreateAll"))
                    {
                        
                        
                        List<ClrClass> lstClass = GetAllClass(SelectedDiagram);
                        if (lstClass == null) 
                        {
                            return;
                        }
                        using (FrmDBCreate st = new FrmDBCreate())
                        {
                            Diagram selDiagram = SelectedDiagram;
                            st.SelectedClass = lstClass;
                            //st.SelectDocView = SelectDocView;
                            //st.CurrentProject = CurrentProject;
                            //st.SelectedDiagram = selDiagram;
                            st.DesignerInfo = GetDesignerInfo();
                            st.ShowDialog();
                        }
                        handled = true;
                        return;

                    }
                    else if (IsCommand(commandName, "BuffaloDBSet")) 
                    {
                        string dalNamespace = GetDesignerInfo().GetNameSpace() + ".DataAccess";
                        ShapeElementMoveableCollection nestedChildShapes = SelectedDiagram.NestedChildShapes;

                        FrmDBSetting.ShowConfig(GetDesignerInfo(), dalNamespace);
                    }
                    else if (IsCommand(commandName, "BuffaloEntityRemove"))
                    {
                        SelectedShapesCollection selectedShapes = SelectedShapes;
                        if (selectedShapes == null) return;
                        for (int i = 0; i < selectedShapes.Count; i++)
                        {
                            
                            if (!(selectedShapes.TopLevelItems[i].Shape is ClrTypeShape)) continue;
                            ClrTypeShape sp = selectedShapes.TopLevelItems[i].Shape as ClrTypeShape;
                            if (!(sp.AssociatedType is ClrClass))
                            {
                                continue;
                            }
                            EntityConfig entity=new EntityConfig(sp.AssociatedType,
                               GetDesignerInfo());
                            //entity.SelectDocView = SelectDocView;
                            if(MessageBox.Show("是否要删除实体:"+entity.ClassName+
                                " 及其相关的业务类？","提示",MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question)==DialogResult.Yes)
                            {
                                EntityRemoveHelper.RemoveEntity(entity);
                            }
                            
                        }
                    }

                    else if (IsCommand(commandName, "BuffaloUpdateEntityByDB")) 
                    {
                        SelectedShapesCollection selectedShapes = SelectedShapes;
                        if (selectedShapes == null) return;
                        for (int i = 0; i < selectedShapes.Count; i++)
                        {
                            if (!(selectedShapes.TopLevelItems[i].Shape is ClrTypeShape)) continue;
                            ClrTypeShape sp = selectedShapes.TopLevelItems[i].Shape as ClrTypeShape;
                            ClrClass curClass=sp.AssociatedType as ClrClass;
                            if (curClass==null)
                            {
                                continue;
                            }
                            EntityConfig entity = EntityConfig.GetEntityConfigByTable(curClass,
                                GetDesignerInfo());
                            entity.GenerateCode();
                        }
                        handled = true;
                        return;
                    }
                    if (IsCommand(commandName, "BuffaloUI"))
                    {
                        SelectedShapesCollection selectedShapes = SelectedShapes;
                        if (selectedShapes == null) return;
                        for (int i = 0; i < selectedShapes.Count; i++)
                        {
                            if (!(selectedShapes.TopLevelItems[i].Shape is ClrTypeShape)) continue;
                            ClrTypeShape sp = selectedShapes.TopLevelItems[i].Shape as ClrTypeShape;
                            if (!(sp.AssociatedType is ClrClass))
                            {
                                continue;
                            }
                            using (FrmUIGenerater st = new FrmUIGenerater())
                            {
                                Diagram selDiagram = SelectedDiagram;
                                st.CurEntityInfo = new EntityInfo(sp.AssociatedType, GetDesignerInfo());
                                st.BindTargetProjects(AllProjects);
                                st.ShowDialog();
                            }
                        }
                        handled = true;
                        return;
                    }
                    if (IsCommand(commandName, "commandGenDal"))
                    {
                        GreanDataAccess();
                        return;
                    }
                }
                catch (Exception ex)
                {

                    FrmCompileResault.ShowCompileResault(null, ex.ToString(), ToolVersionInfo.ToolVerInfo);

                }
            }
        }


        /// <summary>
        /// 生成数据层
        /// </summary>
        private void GreanDataAccess()
        {
            ClassDesignerInfo cinfo = GetDesignerInfo();
            List<ClrClass> lstClass = Connect.GetAllClass(cinfo.SelectedDiagram);
            using (FrmProcess frmLoading = FrmProcess.ShowProcess())
            {
                frmLoading.UpdateProgress(0, 0, "正在生成类");

                for (int i = 0; i < lstClass.Count; i++)
                {
                    ClrClass cls = lstClass[i];
                    EntityConfig entity = new EntityConfig(cls, cinfo);
                    Generate3Tier g3t = new Generate3Tier(entity);
                    if (!string.IsNullOrEmpty(entity.TableName))
                    {
                        g3t.GenerateIDataAccess();
                        g3t.GenerateDataAccess();
                        g3t.GenerateBQLDataAccess();
                    }
                    frmLoading.UpdateProgress(i + 1, lstClass.Count, "正在生成");
                }
            }
        }

    }
}