using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Data.PostgreSQL
{
    public class MathFunctions :Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.MathFunctions
    {
        public override string IndexOf(string[] values)
        {
            return " locate(" + values[0] + "," + values[1] + "," + values[2] + ")";
        }

        public override string DoAtan2(string[] values)
        {
            return " atan2(" + values[0] + ")";
        }
        public override string DoLog10(string[] values)
        {
            return "log(10," + values[0] + ")";
        }
    }
}
