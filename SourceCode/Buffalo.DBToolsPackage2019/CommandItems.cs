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
    /// �˵���
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
        /// ���ָ�����Ƶ���(ɾ������VS����ûִ��ɾ��������Ĳ˵�)
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
        /// ��װ�˵�
        /// </summary>
        public void Install(DTE2 applicationObject, AddIn addInInstance, Commands2 commands) 
        {
            _contextGUIDS = new object[] { };
            _applicationObject = applicationObject;
            _addInInstance = addInInstance;
            _commands = commands;

            Microsoft.VisualStudio.CommandBars.CommandBar calssComm = ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)[CommandBarId.ClassDesignerContextMenu];
            Microsoft.VisualStudio.CommandBars.CommandBar designerComm = ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)[CommandBarId.ClassDiagramContextMenu];


            ClearControl(calssComm, "Buffalo����");
            ClearControl(designerComm, "Buffalo����");
            
            
            
            commandEntityTools = AddPopUp("Buffalo����", calssComm, 1);
            commandDB = AddPopUp("Buffalo����", designerComm, 1);
            commandEntity = AddToCommand("BuffaloEntityConfig", "����ʵ��", "����Buffaloʵ��", true, 523);
            commandCreater = AddToCommand("BuffaloDBCreater", "ʵ�嵽��", "ͨ��Buffaloʵ�����ɵ����ݿ�ı�", true, 295);
            commandRemove = AddToCommand("BuffaloEntityRemove", "ɾ��ʵ��", "ͨ��Buffaloʵ�����ɵ����ݿ�ı�", true, 232);
            commandROM = AddToCommand("BuffaloDBToEntity", "��ʵ��", "ͨ�����ݲ�ı�����Buffaloʵ��", true, 292);
            commandSummary = AddToCommand("BuffaloShowHideSummery", "��ʾ/����ע��", "��ʾ/����ע��", true, 192);
            commandDBAll = AddToCommand("BuffaloDBCreateAll", "�������ݿ�", "�������ݿ�", true, 577);
            commandGenDal = AddToCommand("commandGenDal", "�������ݲ�", "���ɵ�ǰ���͵����ݲ�", true, 159);
            commandDBSet = AddToCommand("BuffaloDBSet", "���ò���", "�������ݿ�����ɲ���", true, 611);
            commandEntityUpdate = AddToCommand("BuffaloUpdateEntityByDB", "����ʵ��", "�����ݿ�����ֶθ��µ�ʵ��", true, 524);
            commandUI = AddToCommand("BuffaloUI", "��������", "����Buffaloģ�����ɽ���", true, 333);
            //����Ӧ�ڸ�����Ŀؼ���ӵ���˵�
            if ((commandEntityTools != null) && (calssComm != null))
            {

                //command.AddControl(calssComm, 1);
                commandEntity.AddControl(commandEntityTools, 1);
                commandCreater.AddControl(commandEntityTools, 2);
                commandEntityUpdate.AddControl(commandEntityTools, 3);
                commandRemove.AddControl(commandEntityTools, 4);
                commandUI.AddControl(commandEntityTools, 5);
            }
            //����Ӧ�ڸ�����Ŀؼ���ӵ�����ͼ�˵�
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
        /// ж�ز˵�
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
        /// �������
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
        /// ��ӵ����˵�
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
