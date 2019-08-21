using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel
{
    /// <summary>
    /// ������Ļ���
    /// </summary>
    public class MassBase<T>
    {
        private static Type _curType = typeof(T);//��ǰ����
        /// <summary>
        /// ��ȡ������������������Ե���Ϣ
        /// </summary>
        /// <param name="enumType">ö������</param>
        /// <returns></returns>
        public static List<EnumInfo> GetInfos()
        {
            List<EnumInfo> dicInfos = MassManager.GetMassInfos(_curType).LstInfo;
            return dicInfos;
        }

        /// <summary>
        /// ��ȡ����ֵֵ��ע��
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
        /// ��ȡ����ֵ��Ӧ����Ϣ
        /// </summary>
        /// <param name="value">����ֵ</param>
        /// <returns></returns>
        public static EnumInfo GetInfo(object value)
        {
            return MassManager.GetInfoByValue(_curType, value);
        }


        /// <summary>
        /// ���ݳ�������ֶ�������ȡ����Ϣ
        /// </summary>
        /// <param name="name">�ֶ���</param>
        /// <returns></returns>
        public static EnumInfo GetInfoByName(string name)
        {
            return MassManager.GetInfoByName(_curType, name);
        }
    }

    /// <summary>
    /// ��������Ϣ
    /// </summary>
    public class MassInfo 
    {
        private List<EnumInfo> _lstInfo = new List<EnumInfo>();
        /// <summary>
        /// ������Ϣ����
        /// </summary>
        public List<EnumInfo> LstInfo
        {
            get { return _lstInfo; }
        }
        private Dictionary<string, EnumInfo> _dicInfos = new Dictionary<string, EnumInfo>();
        /// <summary>
        /// �����ֵ���Ϣ����
        /// </summary>
        public Dictionary<string, EnumInfo> DicInfos
        {
            get { return _dicInfos; }
        }
    }
}

//***************�÷�****************

//******����**********
//public partial class PTypes : MassBase<PTypes>
//{
//    [Description("1��")]
//    public const int P1 = 1;
//    [Description("2��")]
//    public const int P2 = 2;
//    [Description("3��")]
//    public const int P3 = 3;
//    [Description("4��")]
//    public const int P4 = 4;
//    [Description("5��")]
//    public static int P5 = 5;

//}
//******����*************

//EnumInfo i = PTypes.GetInfo(PTypes.P3);
//MessageBox.Show(i.Value+ "," + i.Description);//��ȡ����ֵ��Ӧ����Ϣ
//MessageBox.Show(PTypes.GetDescription(PTypes.P8));//��ȡ����ֵ��Ӧ��ע��

//Dictionary<string, EnumInfo> dic = PTypes.GetInfos();//��ȡ��������г���ֵ����Ϣ
//foreach (KeyValuePair<string, EnumInfo> info in dic)
//{
//    MessageBox.Show(info.Value.Description + "," + info.Value.Value.ToString());
//}

