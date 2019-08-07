using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using System.Data;

namespace Buffalo.DB.DataBaseAdapter.AccessAdapter
{

    public class CommonFunction :ICommonFunction
    {
        public virtual string IsNull(string[] values)
        {
            //return "isNull(" + values[0] + "," + values[1] + ")";
            return "IIF(isnull(" + values[0] + ")," + values[1] + "," + values[0] + ")";
        }

        public virtual string Distinct(string[] values)
        {
            StringBuilder sbValues = new StringBuilder();
            foreach (string value in values) 
            {
                sbValues.Append(value + ",");
            }

            if (sbValues.Length > 0) 
            {
                sbValues.Remove(sbValues.Length - 1, 1);
            }

            return "distinct " + sbValues.ToString() +" ";
        }




        public string Len(string[] values)
        {
            return "Len(" + values[0] + ")";
        }

        
    }
}
