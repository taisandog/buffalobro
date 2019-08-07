using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.WinFormsControl.DataGridViewEx
{
    public class DataGridViewPercentColumn:System.Windows.Forms.DataGridViewColumn
    {

        public DataGridViewPercentColumn()
        {
            base.CellTemplate = new DataGridViewPercentCell();
        }

        private bool _showTotal = false;

        /// <summary>
        /// �Ƿ���ֵ�����ʾ����
        /// </summary>
        public bool ShowTotal
        {
            get
            {
                return _showTotal;
            }
            set
            {
                _showTotal = value;
            }
        }

        private decimal _TotalCount;
        /// <summary>
        /// ������
        /// </summary>
        public decimal TotalCount
        {
            get { return _TotalCount; }
            set { _TotalCount = value; }
        }

        private string _valueForamt;

        /// <summary>
        /// ֵ�ĸ�ʽ��
        /// </summary>
        public string ValueForamt
        {
            get { return _valueForamt; }
            set { _valueForamt = value; }
        }
    }
}
