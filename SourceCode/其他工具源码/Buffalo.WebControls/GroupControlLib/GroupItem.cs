using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Buffalo.WebControls.GroupControlLib
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class GroupItem
    {
        private string itemContorl;
        /// <summary>
        /// 控件
        /// </summary>
        [Description("控件"),
        NotifyParentProperty(true),
        TypeConverter(typeof(ControlIDConverter))
        ]
        public string ItemContorl
        {
            get
            {

                return itemContorl;
            }
            set
            {
                itemContorl = value;
            }
        }

        private int columnIndex;
        /// <summary>
        /// 列索引(GridView)
        /// </summary>
        [Description("列索引(GridView)"),
        NotifyParentProperty(true),
        ]
        public int ColumnIndex
        {
            get
            {

                return columnIndex;
            }
            set
            {
                columnIndex = value;
            }
        }

        private string contorlName;
        /// <summary>
        /// 要控制的控件名
        /// </summary>
        [Description("要控制的控件名"),
        NotifyParentProperty(true),
        ]
        public string ContorlName
        {
            get
            {
                return contorlName;
            }
            set
            {
                contorlName = value;
            }
        }
    }
}
