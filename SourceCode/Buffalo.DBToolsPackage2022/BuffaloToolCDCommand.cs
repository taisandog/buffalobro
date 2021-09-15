using Buffalo.DBTools.HelperKernel;
using Buffalo.DBTools.UIHelper;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

using Microsoft.VisualStudio.Modeling.Diagrams;
using Buffalo.DBTools;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using EnvDTE80;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using System.Runtime.InteropServices;
using EnvDTE;
using System.Collections.Generic;
using System.Collections;
using Buffalo.DBTools.DocSummary;
using Buffalo.DBTools.DocSummary.VSConfig;
using System.Windows.Forms;
namespace DBTools
{
    /// <summary>
    /// Command handler
    /// </summary>
    public sealed class BuffaloToolCDCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 4129;
        /// <summary>
        /// Command ID.
        /// </summary>
        public static int[] CommandIds = { 0x1030, 0x1040, 0x1050, 0x1060, 0x1070 };
        /// <summary>
        /// Command ID.
        /// </summary>
        public static int[] CommandClassIds = { 0x2030, 0x2040, 0x2050, 0x2060, 0x2070 };
        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("4841c4b5-cdfe-4428-9237-34bd8932872f");
        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet2 = new Guid("22A4E5D6-9A91-43A9-83E2-5A67F5807BAF");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;
        

        private static DTE2 _applicationObject;
        //private static AddIn _addInInstance;
        private static IServiceProvider _serviceProvider;
        static Commands2 _commands = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="BuffaloToolCDCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private BuffaloToolCDCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            CommandID menuCommandID = null;
            MenuCommand menuItem = null;

            
            
            //OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                for (int i = 0; i < CommandIds.Length; i++)
                {
                    int item = CommandIds[i];
                    menuCommandID = new CommandID(CommandSet, item);
                    menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                    commandService.AddCommand(menuItem);

                }
                for (int i = 0; i < CommandClassIds.Length; i++)
                {
                    int item = CommandClassIds[i];
                    menuCommandID = new CommandID(CommandSet2, item);
                    menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                    commandService.AddCommand(menuItem);

                }
                //menuCommandID = new CommandID(CommandSet, CommandId);
                //menuItem = new MenuCommand(this.Execute, menuCommandID);
                //commandService.AddCommand(menuItem);
            }
            InitServices();

           
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static BuffaloToolCDCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }
        private void InitServices()
        {
            _applicationObject = ServiceProvider.GetServiceAsync(typeof(DTE)).Result as DTE2;
            _serviceProvider = new Microsoft.VisualStudio.Shell.ServiceProvider(_applicationObject as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
            try
            {
                _commands = (Commands2)_applicationObject.Commands;
                //_commitem.Install(_applicationObject, _addInInstance, _commands);

                //string err = InitBaseCode.InitBaseDll();
                //if (err != null)
                //{
                //    FrmCompileResault.ShowCompileResault(null, err, ToolVersionInfo.ToolVerInfo);
                //}
            }
            catch (Exception ex)
            {
                FrmCompileResault.ShowCompileResault(null, ex.ToString(), ToolVersionInfo.ToolVerInfo);
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in BuffaloToolCDCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new BuffaloToolCDCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
            string title = "BuffaloToolCDCommand";

            // Show a message box to prove we were here
            VsShellUtilities.ShowMessageBox(
                this.package,
                message,
                title,
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
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
        /// <summary>
        /// 类设计图信息
        /// </summary>
        public ClassDesignerInfo GetDesignerInfo()
        {
            ClassDesignerInfo designerInfo = new ClassDesignerInfo(SelectedDiagram, CurrentProject, SelectDocView);
            return designerInfo;
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
        /// 选中的文档视图
        /// </summary>
        public ClassDesignerDocView SelectDocView
        {
            get
            {
                Microsoft.VisualStudio.Shell.Interop.ISelectionContainer selectionContainer;
                Microsoft.VisualStudio.Shell.Interop.IVsMonitorSelection monitorService = _serviceProvider.GetService(typeof(Microsoft.VisualStudio.Shell.Interop.IVsMonitorSelection)) as Microsoft.VisualStudio.Shell.Interop.IVsMonitorSelection;
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
        /// 生成数据层
        /// </summary>
        private void GreanDataAccess()
        {
            ClassDesignerInfo cinfo = GetDesignerInfo();
            List<ClrClass> lstClass = GetAllClass(cinfo.SelectedDiagram);
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


        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
            MenuCommand cmd = sender as MenuCommand;
            try
            {
                //空白处
                if (cmd.CommandID.ID == 0x1030)//表到实体
                {
                    Diagram dia = SelectedDiagram;
                    if (!(dia is ShapeElement))
                    {
                        return;
                    }
                    using (FrmAllTables frmTables = new FrmAllTables())
                    {
                        frmTables.DesignerInfo = GetDesignerInfo();
                        frmTables.ShowDialog();
                    }
                }
                else if (cmd.CommandID.ID == 0x1040)//显示隐藏注释
                {
                    Diagram dia = this.SelectedDiagram;

                    if (dia != null)
                    {

                        VSConfigManager.InitConfig(_applicationObject.Version);
                        ShapeSummaryDisplayer.ShowOrHideSummary(dia, this);
                        this.SelectDocView.CurrentDesigner.ScrollDown();
                        this.SelectDocView.CurrentDesigner.ScrollUp();
                    }
                }
                else if (cmd.CommandID.ID == 0x1050)//生成数据库
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
                        st.DesignerInfo = GetDesignerInfo();
                        st.ShowDialog();
                    }
                }
                else if (cmd.CommandID.ID == 0x1060)//生成数据层
                {
                    GreanDataAccess();
                }
                else if (cmd.CommandID.ID == 0x1070)//设置参数
                {
                    string dalNamespace = GetDesignerInfo().GetNameSpace() + ".DataAccess";
                    ShapeElementMoveableCollection nestedChildShapes = SelectedDiagram.NestedChildShapes;

                    FrmDBSetting.ShowConfig(GetDesignerInfo(), dalNamespace);
                }

                //类图形

                else if (cmd.CommandID.ID == 0x2030)//配置实体
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
                }

                else if (cmd.CommandID.ID == 0x2040)//实体到表
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
                }

                else if (cmd.CommandID.ID == 0x2050)//实体到表
                {
                    SelectedShapesCollection selectedShapes = SelectedShapes;
                    if (selectedShapes == null) return;
                    for (int i = 0; i < selectedShapes.Count; i++)
                    {
                        if (!(selectedShapes.TopLevelItems[i].Shape is ClrTypeShape)) continue;
                        ClrTypeShape sp = selectedShapes.TopLevelItems[i].Shape as ClrTypeShape;
                        ClrClass curClass = sp.AssociatedType as ClrClass;
                        if (curClass == null)
                        {
                            continue;
                        }
                        EntityConfig entity = EntityConfig.GetEntityConfigByTable(curClass,
                            GetDesignerInfo());
                        entity.GenerateCode();
                    }
                }

                else if (cmd.CommandID.ID == 0x2060)//删除实体
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
                        EntityConfig entity = new EntityConfig(sp.AssociatedType,
                           GetDesignerInfo());
                        //entity.SelectDocView = SelectDocView;
                        if (MessageBox.Show("是否要删除实体:" + entity.ClassName +
                            " 及其相关的业务类？", "提示", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            EntityRemoveHelper.RemoveEntity(entity);
                        }

                    }
                }

                else if (cmd.CommandID.ID == 0x2070)//界面生成
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
                }
            }
            catch (Exception ex)
            {
                FrmCompileResault.ShowCompileResault(null, ex.ToString(), ToolVersionInfo.ToolVerInfo);
            }
        }
    }
}
