using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 常量类的基类
    /// </summary>
    public class MassBase<T>
    {
        private static Type _curType = typeof(T);//当前类型
        /// <summary>
        /// 获取本常量类里边所有属性的信息
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static List<EnumInfo> GetInfos()
        {
            List<EnumInfo> dicInfos = MassManager.GetMassInfos(_curType).LstInfo;
            return dicInfos;
        }

        /// <summary>
        /// 获取常量值值的注释
        /// </summary>
        /// <param name="objEnum"></param>
        /// <returns></returns>
        public static string GetDescription(object value)
        {
            EnumInfo info = GetInfo(value);
            if (info != null)
            {
                return info.Description;
            }
            return null;
        }
        /// <summary>
        /// 获取常量值对应的信息
        /// </summary>
        /// <param name="value">常量值</param>
        /// <returns></returns>
        public static EnumInfo GetInfo(object value)
        {
            return MassManager.GetInfoByValue(_curType, value);
        }


        /// <summary>
        /// 根据常量类的字段名来获取其信息
        /// </summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public static EnumInfo GetInfoByName(string name)
        {
            return MassManager.GetInfoByName(_curType, name);
        }
    }

    /// <summary>
    /// 常量类信息
    /// </summary>
    public class MassInfo 
    {
        private List<EnumInfo> _lstInfo = new List<EnumInfo>();
        /// <summary>
        /// 常量信息集合
        /// </summary>
        public List<EnumInfo> LstInfo
        {
            get { return _lstInfo; }
        }
        private Dictionary<string, EnumInfo> _dicInfos = new Dictionary<string, EnumInfo>();
        /// <summary>
        /// 常量字典信息集合
        /// </summary>
        public Dictionary<string, EnumInfo> DicInfos
        {
            get { return _dicInfos; }
        }
    }
}

//***************用法****************

//******声明**********
//public partial class PTypes : MassBase<PTypes>
//{
//    [Description("1号")]
//    public const int P1 = 1;
//    [Description("2号")]
//    public const int P2 = 2;
//    [Description("3号")]
//    public const int P3 = 3;
//    [Description("4号")]
//    public const int P4 = 4;
//    [Description("5号")]
//    public static int P5 = 5;

//}
//******调用*************

//EnumInfo i = PTypes.GetInfo(PTypes.P3);
//MessageBox.Show(i.Value+ "," + i.Description);//获取常量值对应的信息
//MessageBox.Show(PTypes.GetDescription(PTypes.P8));//获取常量值对应的注释

//Dictionary<string, EnumInfo> dic = PTypes.GetInfos();//获取类里边所有常量值的信息
//foreach (KeyValuePair<string, EnumInfo> info in dic)
//{
//    MessageBox.Show(info.Value.Description + "," + info.Value.Value.ToString());
//}

