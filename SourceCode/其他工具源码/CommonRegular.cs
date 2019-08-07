using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel
{
    public class CommonRegular
    {
        /// <summary>
        /// �̶��绰����
        /// </summary>
        public const string PhoneNumber = "0\\d{2,4}-\\d{7,8}";

        /// <summary>
        /// �ƶ��绰
        /// </summary>
        public const string MobilePhone = "((\\(\\d{3}\\))|(\\d{3}\\-))?13\\d{9}|15[89]\\d{8}$";
        /// <summary>
        /// ���е绰����
        /// </summary>
        public const string AllPhone = "((\\(\\d{3}\\))|(\\d{3}\\-))?13\\d{9}|15[89]\\d{8}";

        /// <summary>
        /// ƥ���ʺ��Ƿ�Ϸ�(��ĸ��ͷ������5-16�ֽڣ�������ĸ�����»���)
        /// </summary>
        public const string ValidateAccount = "^\\[a-zA-Z\\]\\[a-zA-Z0-9_\\]{4,15}$";

        /// <summary>
        /// ƥ����ѶQQ��
        /// </summary>
        public const string ValidateQQ = "^\\s*[.0-9]{5,10}\\s*$";

        /// <summary>
        /// �������
        /// </summary>
        public const string Fax = "^[+]{0,1}(\\d){1,3}[ ]?([-]?((\\d)|[ ]){1,12})+$";

        /// <summary>
        /// ��������
        /// </summary>
        public const string MailCode = "^\\d{6}";

        /// <summary>
        /// ֻ��������
        /// </summary>
        public const string OnlyChinese = "^[\\u4E00-\\u9FA5]+$";
    }
}
