using System;
namespace Buffalo.DB.DataBaseAdapter.IDbAdapters
{
    public interface IMathFunctions
    {
        string DoAbs(string[] values);
        string DoAcos(string[] values);
        string DoAsin(string[] values);
        string DoAtan(string[] values);
        string DoAtan2(string[] values);
        string DoCeil(string[] values);
        string DoCos(string[] values);
        string DoExp(string[] values);
        string DoFloor(string[] values);
        string DoLn(string[] values);
        string DoLog10(string[] values);
        string DoPower(string[] values);
        string DoRandom(string[] values);
        string DoRound(string[] values);
        string DoSign(string[] values);
        string DoSin(string[] values);
        string DoSqrt(string[] values);
        string DoTan(string[] values);
        string IndexOf(string[] values);
        string SubString(string[] values);
        string DoMod(string[] values);
        string BitAND(string[] values);
        string BitOR(string[] values);
        string BitXOR(string[] values);
        string BitNot(string[] values);
        
    }
}
