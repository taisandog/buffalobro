using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.ContantSearchs
{
    public class SearchConditionObject
    {
        private List<string> propertyNames=null;
        private List<string> keyNames =null;

        /// <summary>
        /// 获取属性名的集合
        /// </summary>
        public List<string> PropertyNames 
        {
            get 
            {
                return propertyNames;
            }
            set 
            {
                propertyNames = TrimNames(value);
            }
        }

        /// <summary>
        /// 获取关键字的集合
        /// </summary>
        public List<string> KeyNames
        {
            get
            {
                return keyNames;
            }
            set
            {
                keyNames = TrimNames(value);
            }
        }

        /// <summary>
        /// 缩减关键字名集合（把空键值的项去掉）
        /// </summary>
        private List<string> TrimNames(List<string> lst) 
        {

            if (lst == null)
            {
                return null;
            }
            List<string> newList = new List<string>(lst);
            int len = newList.Count;
            for (int i = len - 1; i >= 0; i--) 
            {
                if (newList[i] == "" || newList[i] == null) 
                {
                    newList.RemoveAt(i);
                }
            }
            return newList;
        }
    }
}
