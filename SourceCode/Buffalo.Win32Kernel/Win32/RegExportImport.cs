using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

namespace Buffalo.Win32Kernel.Win32
{
    /// <summary>
    /// �����Ƕ�ע����뵼���Ĳ���
    /// </summary>
    public static class RegExportImport
    {

        /// <summary>
        /// ��ע��������ļ����ڵ����Ĺ������첽�ģ����ܲ������̹���
        /// </summary>
        /// <param name="SavingFilePath">��ע��������ļ���������Ѵ��ڵģ�����ʾ���ǣ�
        /// ����������ɲ���ָ�����ֵ��ļ������Զ�����һ�����������ļ�����չ��Ӧ����.REG��</param>
        /// <param name="regPath">ָ��ע����ĳһ�������������ָ��nullֵ������������ע���</param>
        /// <returns>�ɹ�����0���û��жϷ���1</returns>
        public static int ExportReg(string savingFilePath, string regPath)
        {
            //����ļ����ڣ�MSG��ʾ�Ƿ񸲸ǣ������ǣ��жϲ���
            //���ע���·��Ϊ�գ�����ȫ��
            
            Process.Start(
                    "regedit",
                    string.Format(" /E {0} {1} ", savingFilePath, regPath));
            return 0;
        }
        /// <summary>
        /// ���ļ������ע���
        /// </summary>
        /// <param name="SavedFilePath">ָ���ڴ����ϴ��ڵ��ļ������ָ�����ļ������ڣ����׳��쳣</param>
        /// <param name="regPath">ָ��ע���ļ���������SavedFilePath�ļ��б���Ĺؼ��֣�������ò�������Ϊnull����������SavedFilePath�ļ�
        /// �б�������й���ע���Ĺؼ���</param>
        /// <returns>�ɹ�����0</returns>
        public static int ImportReg(string savingFilePath, string regPath)
        {
            Process.Start(
                   "regedit",
                   string.Format(" /C {0} {1}",
                   savingFilePath,
                   regPath));//�߳����

            return 0;

        }

    };
}
