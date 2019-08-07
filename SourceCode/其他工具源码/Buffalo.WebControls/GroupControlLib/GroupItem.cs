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
        /// �ؼ�
        /// </summary>
        [Description("�ؼ�"),
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
        /// ������(GridView)
        /// </summary>
        [Description("������(GridView)"),
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
        /// Ҫ���ƵĿؼ���
        /// </summary>
        [Description("Ҫ���ƵĿؼ���"),
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
