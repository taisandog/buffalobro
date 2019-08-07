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
        /// 检查注册表
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static bool CheckReg(string processName) 
        {
            RegistryKey hkml = Registry.ClassesRoot.OpenSubKey(processName);
            return hkml != null;
        }

        /// <summary>
        /// 注册程序
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="fileName"></param>
        public static void RegistryTo(string processName, string fileName)
        {
            //yocartoon节点
            RegistryKey processKey = Registry.ClassesRoot.OpenSubKey(processName, true);
            if (processKey == null)
            {
                processKey = Registry.ClassesRoot.CreateSubKey(processName);
                processKey.SetValue("", processName+" Protocol", RegistryValueKind.String);
                processKey.SetValue("URL Protocol", fileName, RegistryValueKind.String);
                //defaultIcon节点
                RegistryKey defaultIcon = processKey.OpenSubKey("DefaultIcon", true);
                if (defaultIcon == null)
                {
                    defaultIcon = processKey.CreateSubKey("DefaultIcon");
                }
                defaultIcon.SetValue("", fileName + ",1", RegistryValueKind.String);
                //shell节点
                RegistryKey shell = processKey.OpenSubKey("shell", true);
                if (shell == null)
                {
                    shell = processKey.CreateSubKey("shell");
                }
                shell.SetValue("", "", RegistryValueKind.String);
                //open节点
                RegistryKey open = shell.OpenSubKey("open", true);
                if (open == null)
                {
                    open = shell.CreateSubKey("open");
                }
                open.SetValue("", "", RegistryValueKind.String);
                //command节点
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
