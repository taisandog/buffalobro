using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.PropertyAttributes;
namespace TestLib.BQLEntity
{
    [DataBaseAttribute("YMRDB")]
    public partial class YMRDB :BQLDataBaseHandle<YMRDB> 
    {
    }
    
}
