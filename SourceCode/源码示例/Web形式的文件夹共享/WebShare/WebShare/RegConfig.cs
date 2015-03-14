using System;
using System.Windows.Forms;
using Microsoft.Win32;
using Buffalo.Kernel;
namespace WebShare
{
	/// <summary>
	/// RegConfig 的摘要说明。
	/// </summary>
	public class RegConfig
	{
		public RegConfig()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

        private static string autoRoot = CommonMethods.GetBaseRoot("WebShare.exe -s");
        private const string keyName = "ShareMyFolder";//注册表键名
		

		/// <summary>
		/// 设置是否自启动
		/// </summary>
		public static bool IsAutoRun
		{
			get
			{
                
				RegistryKey autoKey=Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",true);
				return FindValue(autoKey,keyName);
			}
			set
			{
				RegistryKey autoKey=Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",true);
				if(value)
				{
					autoKey.SetValue(keyName,autoRoot);
				}
				else
				{
					autoKey.DeleteValue(keyName,false);
				}
			}
		}

		/// <summary>
		/// 注册表键中查找指定的项
		/// </summary>
		/// <param name="key">注册表键</param>
		/// <param name="val">项名</param>
		/// <returns></returns>
		private static bool FindValue(RegistryKey key,string val)
		{
			string[] subkeys=key.GetValueNames();
			for(int i=0;i<subkeys.Length;i++)
			{
				if(val==subkeys[i])
				{
					return true;
				}
				
			}
			return false;
		}
	}
}
