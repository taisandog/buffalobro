using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

namespace Buffalo.Win32Kernel.Win32
{
    /// <summary>
    /// 该类是对注册表导入导出的操作
    /// </summary>
    public static class RegExportImport
    {

        /// <summary>
        /// 从注册表导出到文件，在导出的过程是异步的，不受操作进程管理
        /// </summary>
        /// <param name="SavingFilePath">从注册表导出的文件，如果是已存在的，会提示覆盖；
        /// 如果不存在由参数指定名字的文件，将自动创建一个。导出的文件的扩展名应当是.REG的</param>
        /// <param name="regPath">指定注册表的某一键被导出，如果指定null值，将导出整个注册表</param>
        /// <returns>成功返回0，用户中断返回1</returns>
        public static int ExportReg(string savingFilePath, string regPath)
        {
            //如果文件存在，MSG提示是否覆盖，不覆盖，中断操作
            //如果注册表路径为空，导出全部
            
            Process.Start(
                    "regedit",
                    string.Format(" /E {0} {1} ", savingFilePath, regPath));
            return 0;
        }
        /// <summary>
        /// 从文件导入的注册表
        /// </summary>
        /// <param name="SavedFilePath">指定在磁盘上存在的文件，如果指定的文件不存在，将抛出异常</param>
        /// <param name="regPath">指定注册表的键（包含在SavedFilePath文件中保存的关键字），如果该参数设置为null将导入整个SavedFilePath文件
        /// 中保存的所有关于注册表的关键字</param>
        /// <returns>成功返回0</returns>
        public static int ImportReg(string savingFilePath, string regPath)
        {
            Process.Start(
                   "regedit",
                   string.Format(" /C {0} {1}",
                   savingFilePath,
                   regPath));//线程外的

            return 0;

        }

    };
}
