using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class UpdateSetParamItemList : List<UpdateSetParamItem>
    {
        /// <summary>
        /// 添加一个更新项
        /// </summary>
        /// <param name="parameter">更新项的字段</param>
        /// <param name="valueItem">更新值</param>
        public void Add(BQLParamHandle parameter, BQLValueItem valueItem) 
        {
            this.Add(new UpdateSetParamItem(parameter,valueItem));
        }
    }
}
