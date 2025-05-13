using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using EnvDTE80;
using System.IO;
using Microsoft.VisualStudio.CommandBars;

namespace Buffalo.DBTools
{
    /// <summary>
    /// 菜单类
    /// </summary>
    public class CommandItems
    {
        private DTE2 _applicationObject;
        private AddIn _addInInstance;
        Commands2 _commands = null;
        object commandEntityTools =null;
        object commandDB = null;
        Command commandEntity = null;
        Command commandCreater = null;
        Command commandROM = null;
        Command commandSummary = null;
        Command commandDBAll = null;
        Command commandDBSet = null;
        Command commandGenDal = null;
        Command commandEntityUpdate = null;
        Command commandRemove = null;
        Command commandUI = null;
        object[] _contextGUIDS = null;

        public CommandItems() 
        {

            

        }

        /// <summary>
        /// 清除指定名称的项(删除可能VS崩溃没执行删除而残余的菜单)
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="name"></param>
        private void ClearControl(Microsoft.VisualStudio.CommandBars.CommandBar bar, string name) 
        {
            int trac = 0;
            CommandBarControl ctr = null;
            while (true)
            {
                trac++;
                if (trac >= 100) 
                {
                    return;
                }
                try
                {
                    ctr = bar.Controls[name] as CommandBarControl;
                }
                catch { }
                if (ctr == null)
                {
                    return;
                }
                ctr.Delete(Type.Missing);
                ctr = null;
            }
        }

        /// <summary>
        /// 安装菜单
        /// </summary>
        public void Install(DTE2 applicationObject, AddIn addInInstance, Commands2 commands) 
        {
            _contextGUIDS = new object[] { };
            _applicationObject = applicationObject;
            _addInInstance = addInInstance;
            _commands = commands;

            Microsoft.VisualStudio.CommandBars.CommandBar calssComm = ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)[CommandBarId.ClassDesignerContextMenu];
            Microsoft.VisualStudio.CommandBars.CommandBar designerComm = ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)[CommandBarId.ClassDiagramContextMenu];


            ClearControl(calssComm, "Buffalo助手");
            ClearControl(designerComm, "Buffalo助手");
            
            
            
            commandEntityTools = AddPopUp("Buffalo助手", calssComm, 1);
            commandDB = AddPopUp("Buffalo助手", designerComm, 1);
            commandEntity = AddToCommand("BuffaloEntityConfig", "配置实体", "配置Buffalo实体", true, 523);
            commandCreater = AddToCommand("BuffaloDBCreater", "实体到表", "通过Buffalo实体生成到数据库的表", true, 295);
            commandRemove = AddToCommand("BuffaloEntityRemove", "删除实体", "通过Buffalo实体生成到数据库的表", true, 232);
            commandROM = AddToCommand("BuffaloDBToEntity", "表到实体", "通过数据层的表生成Buffalo实体", true, 292);
            commandSummary = AddToCommand("BuffaloShowHideSummery", "显示/隐藏注释", "显示/隐藏注释", true, 192);
            commandDBAll = AddToCommand("BuffaloDBCreateAll", "生成数据库", "生成数据库", true, 577);
            commandGenDal = AddToCommand("commandGenDal", "生成数据层", "生成当前类型的数据层", true, 159);
            commandDBSet = AddToCommand("BuffaloDBSet", "设置参数", "设置数据库和生成参数", true, 611);
            commandEntityUpdate = AddToCommand("BuffaloUpdateEntityByDB", "更新实体", "把数据库的新字段更新到实体", true, 524);
            //commandUI = AddToCommand("BuffaloUI", "界面生成", "按照Buffalo模版生成界面", true, 333);
            //将对应于该命令的控件添加到类菜单
            if ((commandEntityTools != null) && (calssComm != null))
            {

                //command.AddControl(calssComm, 1);
                commandEntity.AddControl(commandEntityTools, 1);
                commandCreater.AddControl(commandEntityTools, 2);
                commandEntityUpdate.AddControl(commandEntityTools, 3);
                commandRemove.AddControl(commandEntityTools, 4);
                commandUI.AddControl(commandEntityTools, 5);
            }
            //将对应于该命令的控件添加到总类图菜单
            if ((commandDB != null) && (designerComm != null))
            {
                commandROM.AddControl(commandDB, 1);
                commandSummary.AddControl(commandDB, 2);
                commandDBAll.AddControl(commandDB, 3);
                commandGenDal.AddControl(commandDB, 4);
                commandDBSet.AddControl(commandDB, 5);
                
            }

        }

        /// <summary>
        /// 卸载菜单
        /// </summary>
        public void UnInstall() 
        {
            if (commandEntity != null)
            {
                commandEntity.Delete();
                commandEntity = null;
            }
            if (commandCreater != null)
            {
                commandCreater.Delete();
                commandCreater = null;
            }
            if (commandROM != null)
            {
                commandROM.Delete();
                commandROM = null;
            }
            if (commandSummary != null)
            {
                commandSummary.Delete();
                commandSummary = null;
            }
            if (commandRemove != null)
            {
                commandRemove.Delete();
                commandRemove = null;
            }
            if (commandDBAll != null)
            {
                commandDBAll.Delete();
                commandDBAll = null;
            }
            if (commandDBSet != null)
            {
                commandDBSet.Delete();
                commandDBSet = null;
            }
            if (commandGenDal != null) 
            {
                commandGenDal.Delete();
                commandGenDal = null;
            }
            if (commandEntityUpdate != null)
            {
                commandEntityUpdate.Delete();
                commandEntityUpdate = null;
            }
            if (commandUI != null)
            {
                commandUI.Delete();
                commandUI = null;
            }
            if (_applicationObject != null && commandDB != null)
            {
                try
                {
                    _applicationObject.Commands.RemoveCommandBar(commandDB);
                }
                catch { }
                commandDB = null;
            }
            if (_applicationObject != null && commandEntityTools != null)
            {
                try
                {
                    _applicationObject.Commands.RemoveCommandBar(commandEntityTools);
                }
                catch { }
                commandEntityTools = null;
            }

            
        }
        /// <summary>
        /// 添加命令
        /// </summary>
        /// <param name="name"></param>
        /// <param name="buttonText"></param>
        /// <param name="tooltip"></param>
        /// <param name="msoButton"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private Command AddToCommand(string name, string buttonText, string tooltip, bool msoButton, object bitmap)
        {
            return _commands.AddNamedCommand2(_addInInstance, name, buttonText, tooltip, msoButton, bitmap,
                ref _contextGUIDS, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled,
                (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
        }

        /// <summary>
        /// 添加弹出菜单
        /// </summary>
        /// <param name="buttonText"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private object AddPopUp(string buttonText, Microsoft.VisualStudio.CommandBars.CommandBar parent, int postion)
        {
            object command = _commands.AddCommandBar(buttonText, 
                vsCommandBarType.vsCommandBarTypeMenu, parent, postion);
            return command;
        }
       
    }
}
