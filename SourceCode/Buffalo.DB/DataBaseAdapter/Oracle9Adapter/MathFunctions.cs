using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.DataBaseAdapter.Oracle9Adapter
{
    public class MathFunctions :Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.MathFunctions
    {
        public override string DoCeil(string[] values)
        {
            return "ceil(" + values[0] + ")";
        }
        public override string DoAtan2(string[] values)
        {
            return " atan2(" + values[0] + ")";
        }
        public override string DoLn(string[] values)
        {
            return " ln(" + values[0] + ")";
        }

        public override string DoLog10(string[] values)
        {
            return " log(10," + values[0] + ")";
        }

        public override string DoRandom(string[] values)
        {
            return " sys.dbms_random.value(0,1)";
        }

        public override string IndexOf(string[] values)
        {
            return " instr(" + values[1] + "," + values[0] + "," + values[2] + ")";
        }

        public override string SubString(string[] values)
        {
            return " substr(" + values[0] + "," + values[1] + "," + values[2] + ")";
        }

        public override string DoMod(string[] values)
        {
            return "MOD(" + values[0] + ", " + values[1] + ")";
        }
        public override string BitAND(string[] values)
        {
            return "BITAND(" + values[0] + "," + values[1] + ")";
        }
        public override string BitOR(string[] values)
        {
            return "((" + values[0] + " + " + values[1] + ") - BITAND(" + values[0] + ", " + values[1] + "))";
        }
        public override string BitXOR(string[] values)
        {
            return "((" + values[0] + " + " + values[1] + ") - BITAND(" + values[0] + ", " + values[1] + ") * 2)";
        }
        public override string BitNot(string[] values)
        {
            return "(-1 - " + values[0] + ")";
        }
    }
}
