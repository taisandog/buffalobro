using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

/** 
 * @原作者:苏飞—Perky Su
 * @创建时间:2010-02-04 02:05
 * @链接:http://www.cnblogs.com/sufei/archive/2010/02/04/1663125.html
 * @说明:实体属性对比器
*/
namespace Buffalo.Win32Kernel.DataGridViewUnit
{
    /// <summary>
    /// 可排序绑定的集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BindingCollection<T> : BindingList<T>
    {
        private bool isSorted;
        private PropertyDescriptor sortProperty;
        private ListSortDirection sortDirection;
        public BindingCollection() { }
        public BindingCollection(IEnumerable<T> lst)
        {
            AddRange(lst);
        }

        /// <summary>
        /// 添加一个集合
        /// </summary>
        /// <param name="lst"></param>
        public void AddRange(IEnumerable<T> lst) 
        {
            foreach (T obj in lst)
            {
                Add(obj);
            }
        }

        protected override bool IsSortedCore
        {
            get { return isSorted; }
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirection; }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return sortProperty; }
        }

        protected override bool SupportsSearchingCore
        {
            get { return true; }
        }

        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            List<T> items = this.Items as List<T>;

            if (items != null)
            {
                ObjectPropertyCompare<T> pc = new ObjectPropertyCompare<T>(property, direction);
                items.Sort(pc);
                isSorted = true;
            }
            else
            {
                isSorted = false;
            }

            sortProperty = property;
            sortDirection = direction;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore()
        {
            isSorted = false;
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
        //排序
        public void Sort(PropertyDescriptor property, ListSortDirection direction)
        {
            this.ApplySortCore(property, direction);
        }
    }
}
