using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class UpdateSetParamItemList : List<UpdateSetParamItem>
    {
        /// <summary>
        /// ���һ��������
        /// </summary>
        /// <param name="parameter">��������ֶ�</param>
        /// <param name="valueItem">����ֵ</param>
        public void Add(BQLParamHandle parameter, BQLValueItem valueItem) 
        {
            this.Add(new UpdateSetParamItem(parameter,valueItem));
        }
    }
}
