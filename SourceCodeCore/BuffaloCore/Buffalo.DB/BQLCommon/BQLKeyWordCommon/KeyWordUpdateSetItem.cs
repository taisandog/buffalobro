using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordUpdateSetItem : BQLQuery
    {
        UpdateSetParamItemList lstItems = null;

        /// <summary>
        /// Set�ؼ�����
        /// </summary>
        /// <param name="parameter">Ҫ���µ��ֶ�</param>
        /// <param name="valueItem">ֵ</param>
        /// <param name="previous">��һ��</param>
        public KeyWordUpdateSetItem(BQLParamHandle parameter, BQLValueItem valueItem, BQLQuery previous)
            : base(previous) 
        {
            lstItems = new UpdateSetParamItemList();
            lstItems.Add(parameter, valueItem);
        }
        /// <summary>
        /// Set�ؼ�����
        /// </summary>
        /// <param name="lstItems">Ҫ���µ����</param>
        /// <param name="previous">��һ���ؼ���</param>
        public KeyWordUpdateSetItem(UpdateSetParamItemList lstItems, BQLQuery previous)
            : base(previous) 
        {
            this.lstItems = lstItems;
        }

        internal override void LoadInfo(KeyWordInfomation info)
        {
        }

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="condition">����</param>
        /// <returns></returns>
        public KeyWordWhereItem Where(BQLCondition condition)
        {
            KeyWordWhereItem item = new KeyWordWhereItem(condition, this);
            return item;
        }
        /// <summary>
        /// ���һ��set��
        /// </summary>
        /// <param name="parameter">�ֶ�</param>
        /// <param name="valueItem">ֵ</param>
        /// <returns></returns>
        public KeyWordUpdateSetItem _(BQLParamHandle parameter, BQLValueItem valueItem)
        {
            lstItems.Add(parameter, valueItem);
            return this;
        }

        internal override void Tran(KeyWordInfomation info)
        {
            StringBuilder sb = new StringBuilder();
            for(int i=0;i<lstItems.Count;i++)  
            {
                UpdateSetParamItem item = lstItems[i];
                string value=null;
                if (CommonMethods.IsNull(item.ValueItem))
                {
                    value = "null";
                }
                else 
                {
                    item.ValueItem.ValueDbType = item.Parameter.ValueDbType;
                    value=item.ValueItem.DisplayValue(info);
                }
                info.Infos.IsShowTableName = false;
                sb.Append(item.Parameter.DisplayValue(info) + "=" + value);
                info.Infos.IsShowTableName = true;
                if (i < lstItems.Count - 1) 
                {
                    sb.Append(",");
                }
            }
            info.Condition.UpdateSetValue.Append(sb.ToString());
            //return " set " + sb.ToString();
        }
    }
}
