using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;
using Buffalo.Kernel;
using System.Windows.Forms;

namespace Buffalo.Win32Kernel.Win32
{
    public class ProcessUrlRegistry
    {
        /// <summary>
        /// ���ע���
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static bool CheckReg(string processName) 
        {
            RegistryKey hkml = Registry.ClassesRoot.OpenSubKey(processName);
            return hkml != null;
        }

        /// <summary>
        /// ע�����
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="fileName"></param>
        public static void RegistryTo(string processName, string fileName)
        {
            //yocartoon�ڵ�
            RegistryKey processKey = Registry.ClassesRoot.OpenSubKey(processName, true);
            if (processKey == null)
            {
                processKey = Registry.ClassesRoot.CreateSubKey(processName);
                processKey.SetValue("", processName+" Protocol", RegistryValueKind.String);
                processKey.SetValue("URL Protocol", fileName, RegistryValueKind.String);
                //defaultIcon�ڵ�
                RegistryKey defaultIcon = processKey.OpenSubKey("DefaultIcon", true);
                if (defaultIcon == null)
                {
                    defaultIcon = processKey.CreateSubKey("DefaultIcon");
                }
                defaultIcon.SetValue("", fileName + ",1", RegistryValueKind.String);
                //shell�ڵ�
                RegistryKey shell = processKey.OpenSubKey("shell", true);
                if (shell == null)
                {
                    shell = processKey.CreateSubKey("shell");
                }
                shell.SetValue("", "", RegistryValueKind.String);
                //open�ڵ�
                RegistryKey open = shell.OpenSubKey("open", true);
                if (open == null)
                {
                    open = shell.CreateSubKey("open");
                }
                open.SetValue("", "", RegistryValueKind.String);
                //command�ڵ�
                RegistryKey command = open.OpenSubKey("command", true);
                if (command == null)
                {
                    command = open.CreateSubKey("command");
                }
                command.SetValue("", "\"" + fileName + "\"" + " " + "\"%1" + "\"", RegistryValueKind.String);


            }
        }
    }
}
