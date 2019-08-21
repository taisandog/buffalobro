using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.PropertyAttributes;
namespace TestApp.BQLEntity
{
    [DataBaseAttribute("TestDB")]
    public partial class TestDB :BQLDataBaseHandle<TestDB> 
    {
    }
    
}
