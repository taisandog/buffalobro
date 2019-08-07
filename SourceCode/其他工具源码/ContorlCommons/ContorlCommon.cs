using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Buffalo.WebKernel.WebCommons.ContorlCommons
{
    public class ContorlCommon
    {
        /// <summary>
        /// ����ɫת��HTML��ɫ����
        /// </summary>
        /// <param name="color">��ɫ</param>
        /// <returns></returns>
        public static string ToColorString(Color color)
        {
            if (color.IsEmpty) 
            {
                return "";
            }
            StringBuilder colorStr = new StringBuilder(10);
            colorStr.Append("#");
            string strColor = "";

            strColor = color.R.ToString("X");
            if (strColor.Length < 2)
            {
                strColor = "0" + strColor;
            }
            colorStr.Append(strColor);

            strColor = color.G.ToString("X");
            if (strColor.Length < 2)
            {
                strColor = "0" + strColor;
            }
            colorStr.Append(strColor);

            strColor = color.B.ToString("X");
            if (strColor.Length < 2)
            {
                strColor = "0" + strColor;
            }
            colorStr.Append(strColor);
            return colorStr.ToString();
        }
        const string hexElements = "1234567890abcdef";
        /// <summary>
        /// ����ɫ�ַ���ת������ɫ
        /// </summary>
        /// <param name="colorString">��ɫ�ַ���</param>
        /// <returns></returns>
        public static Color ColorStringToColor(string colorString)
        {
            if (colorString == null)
            {
                return Color.Empty;
            }
            string colorStr = colorString.ToLower();
            int[] colorPart = new int[3];
            StringBuilder sbTmp = new StringBuilder(2);
            int curIndex = 0;
            foreach (char chr in colorStr)
            {
                if (hexElements.IndexOf(chr) > 0)
                {
                    sbTmp.Append(chr);
                }
                if (sbTmp.Length >= 2)
                {
                    colorPart[curIndex] = Convert.ToInt32(sbTmp.ToString(), 16);
                    if (curIndex > colorPart.Length - 1)
                    {
                        break;
                    }
                    curIndex++;
                    sbTmp.Remove(0, sbTmp.Length);
                }
            }
            return Color.FromArgb(colorPart[0], colorPart[1], colorPart[2]);
        }
    }
}
