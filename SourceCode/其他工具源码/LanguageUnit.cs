using Buffalo.Kernel;
using MediaAaronDataLib.Business;
using ServerUnit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MediaAaronDataLib
{
    public class LanguageUnit
    {
        private static int _selectedLanguage = 0;
        private const string SelectKey = "$$LanguageUnit.Language";

        private ResourceManager _resource;
        private CultureInfo _culture;

        /// <summary>
        /// 语言工具
        /// </summary>
        /// <param name="resource"></param>
        public LanguageUnit(ResourceManager resource, CultureInfo culture,LanguageType language)
        {
            _resource = resource;
            _culture = culture;
            _language = language;
        }
        
        /// <summary>
        /// 创建多语言数据层
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="language"></param>
        /// <returns></returns>
        public static T CreateBusiness<T>(LanguageType language) where T : ILanguageBusiness
        {
            T bo = (T)Activator.CreateInstance(typeof(T));
            bo.LoadLanguage(language);
            return bo;
        }
        /// <summary>
        /// 创建多语言数据层
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateBusiness<T>() where T : ILanguageBusiness
        {
            return CreateBusiness<T>(SelectedLanguage);
        }
        /// <summary>
        /// 当前的语言
        /// </summary>
        private LanguageType _language;

        /// <summary>
        /// 当前的语言
        /// </summary>
        public LanguageType Language
        {
            get { return _language; }
            set { _language = value; }
        }


        /// <summary>
        /// 选中的语言
        /// </summary>
        public static LanguageType SelectedLanguage 
        {
            get 
            {
                int val = 0;
                if(CommonMethods.IsWebContext)
                {
                    object oval = HttpContext.Current.Session[SelectKey];
                    if(oval!=null)
                    {
                        val=Convert.ToInt32(oval);
                    }
                }
                else 
                {
                    val = _selectedLanguage;
                }
                return (LanguageType)val;
            }
            set 
            {
                if (CommonMethods.IsWebContext)
                {
                    HttpContext.Current.Session[SelectKey] = (int)value;
                }
                else
                {
                    _selectedLanguage = (int)value;
                }
            }
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetText(string key)
        {
            LanguageType type = _language;
            string skey=null;
            switch (type) 
            {
                case LanguageType.Chinese:
                    skey = key + "_CN";
                    break;
                case LanguageType.English:
                    skey = key + "_EN";
                    break;
                default:
                    skey = key + "_CN";
                    break;
            }
            return _resource.GetString(skey, _culture);
        }

    }

    /// <summary>
    /// 语言类型
    /// </summary>
    public enum LanguageType 
    {
        
        /// <summary>
        /// 中文
        /// </summary>
        Chinese=0,
        /// <summary>
        /// 英文
        /// </summary>
        English=1
    }
}
