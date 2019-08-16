using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;

namespace Buffalo.DBTools.DocSummary.VSConfig
{
    /// <summary>
    /// VS������Ϣ
    /// </summary>
    public class VSConfigManager
    {
        private static IVSConfig _curConfig=null;
        /// <summary>
        /// ��ʼ����ǰ����
        /// </summary>
        /// <param name="curProject"></param>
        public static void InitConfig(string version)
        {
            if (_curConfig != null) 
            {
                return;
            }
            

            if (version == "8.0") 
            {
                _curConfig = new VS2005Config();
            }
            else if (version == "9.0")
            {
                _curConfig = new VS2008Config();
            }
            else if (version == "10.0")
            {
                _curConfig = new VS2010Config();
            }
            else 
            {
                _curConfig = new VS2010Config();
            }
        }
        /// <summary>
        /// ��ȡ��ǰ������
        /// </summary>
        public static IVSConfig CurConfig 
        {
            get 
            {
                return _curConfig;
            }
        }
    }
}
