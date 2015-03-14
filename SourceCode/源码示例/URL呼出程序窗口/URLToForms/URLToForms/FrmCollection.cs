using System;
using System.Collections.Generic;
using System.Text;

namespace URLToForms
{
    public class FrmCollection
    {
        private static Dictionary<string, Type> _dic = InitFrms();

        private static Dictionary<string, Type> InitFrms() 
        {
            Dictionary<string, Type> dic = new Dictionary<string, Type>();
            dic["10000"] = typeof(FrmQuery);
            dic["10001"] = typeof(FrmEdit);
            return dic;
        }

        public static Type GetFrmByID(string id) 
        {
            Type ret = null;
            if (_dic.TryGetValue(id, out ret)) 
            {
                return ret;
            }
            return null;
        }
    }
}
