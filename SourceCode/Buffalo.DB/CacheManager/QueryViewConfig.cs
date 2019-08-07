using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;
using System.IO;
using Buffalo.Kernel.FastReflection;
using Buffalo.DB.PropertyAttributes;
using Buffalo.Kernel;

namespace Buffalo.DB.CacheManager
{
    public class QueryViewConfig
    {
        private static Dictionary<string, List<string>> _dicConfig = new Dictionary<string,List<string>>();//记录有多少个视图来自该表
        

        
        /// <summary>
        /// 登记视图
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static bool RegisterView(Type viewType)
        {
            string vName = viewType.FullName;
            
            ViewRelationTables tInfo = FastInvoke.GetClassAttribute<ViewRelationTables>(viewType);
            if (tInfo == null)
            {
                return false;
            }
            bool ret = false;
            string[] names = tInfo.EntityNames;
            if (names != null)
            {
                foreach (string name in names)
                {
                    List<string> lstViewNames = null;
                    if (!_dicConfig.TryGetValue(name, out lstViewNames)) //根据表名获取该表对应的视图集合
                    {
                        lstViewNames = new List<string>();
                        using (Lock objLock = new Lock(_dicConfig))
                        {
                            _dicConfig[name] = lstViewNames;
                        }
                    }
                    lstViewNames.Add(vName);
                }
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// 根据实体名获取对应的视图集合
        /// </summary>
        /// <param name="entityName">实体名</param>
        /// <returns></returns>
        public static List<string> GetViewList(string entityName) 
        {
            
            List<string> retList = null;
            _dicConfig.TryGetValue(entityName,out retList);
            return retList;
        }
    }
}
