using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordUpdateItem : BQLQuery
    {
        private BQLTableHandle table;
        
        /// <summary>
        /// Update�ؼ�����
        /// </summary>
        /// <param name="table">��</param>
        /// <param name="previous">��һ���ؼ���</param>
        public KeyWordUpdateItem(BQLTableHandle table, BQLQuery previous)
            : base(previous) 
        {
            this.table = table;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {
            //if (CommonMethods.IsNull(info.FromTable))
            //{
            //    info.FromTable = table;
            //}
            BQLValueItem.DoFillInfo(table, info);
        }
        /// <summary>
        /// Ҫ��ѯ���ֶ�
        /// </summary>
        internal BQLTableHandle Table
        {
            get 
            {
                return table;
            }
        }
        /// <summary>
        /// ���һ��set��
        /// </summary>
        /// <param name="parameter">�ֶ�</param>
        /// <param name="valueItem">ֵ</param>
        /// <returns></returns>
        public KeyWordUpdateSetItem Set(BQLParamHandle parameter, BQLValueItem valueItem)
        {
            KeyWordUpdateSetItem item = new KeyWordUpdateSetItem(parameter, valueItem, this);
            return item;
        }
        /// <summary>
        /// ���һ��set��
        /// </summary>
        /// <param name="parameter">�ֶ�</param>
        /// <param name="valueItem">ֵ</param>
        /// <returns></returns>
        public KeyWordUpdateSetItem Set(UpdateSetParamItemList lstSetItem)
        {
            KeyWordUpdateSetItem item = new KeyWordUpdateSetItem(lstSetItem, this);
            return item;
        }
        internal override void Tran(KeyWordInfomation info)
        {
            if (info.Condition == null)
            {
                info.Condition = new Buffalo.DB.QueryConditions.UpdateCondition(info.DBInfo);
                //info.ParamList = newBuffalo.DB.DbCommon.ParamList();
            }
            if (info.ParamList == null)
            {
                info.ParamList = new Buffalo.DB.DbCommon.ParamList();
            }
            string tableName = table.DisplayValue(info);
            
            info.Condition.Tables.Append(tableName);
            //return "update " + table.DisplayValue(info);
        }
    }
}
