using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel
{
    public class CommonRegular
    {
        /// <summary>
        /// 固定电话号码
        /// </summary>
        public const string PhoneNumber = "0\\d{2,4}-\\d{7,8}";

        /// <summary>
        /// 移动电话
        /// </summary>
        public const string MobilePhone = "((\\(\\d{3}\\))|(\\d{3}\\-))?13\\d{9}|15[89]\\d{8}$";
        /// <summary>
        /// 所有电话号码
        /// </summary>
        public const string AllPhone = "((\\(\\d{3}\\))|(\\d{3}\\-))?13\\d{9}|15[89]\\d{8}";

        /// <summary>
        /// 匹配帐号是否合法(字母开头，允许5-16字节，允许字母数字下划线)
        /// </summary>
        public const string ValidateAccount = "^\\[a-zA-Z\\]\\[a-zA-Z0-9_\\]{4,15}$";

        /// <summary>
        /// 匹配腾讯QQ号
        /// </summary>
        public const string ValidateQQ = "^\\s*[.0-9]{5,10}\\s*$";

        /// <summary>
        /// 传真号码
        /// </summary>
        public const string Fax = "^[+]{0,1}(\\d){1,3}[ ]?([-]?((\\d)|[ ]){1,12})+$";

        /// <summary>
        /// 邮政编码
        /// </summary>
        public const string MailCode = "^\\d{6}";

        /// <summary>
        /// 只能有中文
        /// </summary>
        public const string OnlyChinese = "^[\\u4E00-\\u9FA5]+$";
    }
}
