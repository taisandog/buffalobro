using System;
using System.Data;
using System.Collections.Generic;
using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.QueryConditions;
using ManagementLib.BQLEntity;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.BQLCommon.BQLBaseFunction;
namespace ManagementLib
{
	
	/// <summary>
	///  
	/// </summary>
	public partial class SampleInfo
	{
        public static DataSet Search(string[] searchs, PageContent objPage) 
        {
            ScopeList lstScope = new ScopeList();
            lstScope.PageContent = objPage;
            double per = Math.Pow(2, searchs.Length);
            
            lstScope.ShowProperty.Add(Management.SampleInfo.SampleName.As("SampleName"));
            lstScope.ShowProperty.Add(Management.SampleInfo.SamplingTerrace.As("SamplingTerrace"));
            lstScope.ShowProperty.Add(Management.SampleInfo.SampleNumber.As("SampleNumber"));
            BQLValueItem perValue = BQLValueItem.ToValueItem(0);
            BQLCondition nameCon=BQLCondition.FalseValue;
            foreach(string search in searchs)
            {
                nameCon=nameCon|Management.SampleInfo.SampleName.Like(search);
                perValue = perValue + BQL.Case().When(BQL.ToParam("SampleName").IndexOf(search, 0) <= 0).Then(0).Else(1).End * per;
                per /= 2;
            }
            lstScope.ShowProperty.Add(perValue.As("per1"));
            lstScope.Add(nameCon);
            lstScope.OrderBy.Add(perValue.As("").DESC);

            per = Math.Pow(2, searchs.Length);
            perValue = BQLValueItem.ToValueItem(0);
            nameCon = BQLCondition.FalseValue;
            foreach (string search in searchs)
            {
                nameCon = nameCon | Management.SampleInfo.SamplingTerrace.Like(search);
                perValue = perValue + BQL.Case().When(BQL.ToParam("SamplingTerrace").IndexOf(search, 0) <= 0).Then(0).Else(1).End * per;
                per /= 2;
            }
            lstScope.ShowProperty.Add(perValue.As("per2"));
            lstScope.Add(nameCon,ConnectType.OR);
            lstScope.OrderBy.Add(perValue.As("").DESC);

            per = Math.Pow(2, searchs.Length);
            perValue = BQLValueItem.ToValueItem(0);
            nameCon = BQLCondition.FalseValue;
            foreach (string search in searchs)
            {
                nameCon = nameCon | Management.SampleInfo.SampleNumber.Like(search);
                perValue = perValue + BQL.Case().When(BQL.ToParam("SampleNumber").IndexOf(search, 0) <= 0).Then(0).Else(1).End * per;
                per /= 2;
            }
            lstScope.ShowProperty.Add(perValue.As("per3"));
            lstScope.Add(nameCon, ConnectType.OR);
            lstScope.OrderBy.Add(perValue.As("").DESC);
            
            //lstScope.OrderBy.Add(BQL.ToParam("per1").DESC);
            //lstScope.OrderBy.Add(BQL.ToParam("per2").DESC);
            //lstScope.OrderBy.Add(BQL.ToParam("per3").DESC);
            return GetContext().Select(lstScope);
        }
	}
}
