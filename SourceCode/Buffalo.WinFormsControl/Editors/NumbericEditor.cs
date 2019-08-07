using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace Buffalo.WinFormsControl.Editors
{
    public class NumbericEditor : TextBoxEditor
    {
        private decimal _Max = decimal.MaxValue;
        /// <summary>
        /// ����������ֵ
        /// </summary>
        [Description("����������ֵ")]
        public decimal Max
        {
            get { return _Max; }
            set
            { 
                _Max = value;
                if (_oldNumber > _Max) 
                {
                    _oldNumber = _Max;
                }
            }
        }
        private ToolTip _curToolTip = null;
        private ToolTip CurToolTip
        {
            get
            {
                if (_curToolTip == null)
                {
                    _curToolTip = new ToolTip();
                    _curToolTip.AutomaticDelay = 5000;
                    _curToolTip.ShowAlways = true;
                    _curToolTip.ToolTipTitle = "˵��:";
                }
                return _curToolTip;
            }
        }

        private decimal _Min = decimal.MinValue;
        /// <summary>
        /// ���������Сֵ
        /// </summary>
        [Description("���������Сֵ")]
        public decimal Min
        {
            get { return _Min; }
            set 
            { 
                _Min = value;
                if (_oldNumber < _Min) 
                {
                    _oldNumber = _Min;
                }
            }
        }

        private int _decimalPlaces = -1;

        /// <summary>
        /// С��λ��(С��0���ʾ�޹涨)
        /// </summary>
        [Description("С��λ��(С��0���ʾ�޹涨)")]
        public int DecimalPlaces
        {
            get { return _decimalPlaces; }
            set { _decimalPlaces = value; }
        }

        public NumbericEditor() 
            :base()
        {
            txtValue.TextChanged += new EventHandler(txtValue_TextChanged);
        }

        decimal _oldNumber = 0m;

        void txtValue_TextChanged(object sender, EventArgs e)
        {
            if (!IsCorrect(txtValue.Text) || txtValue.Text == "-")
            {
                int sel = txtValue.SelectionStart;
                if (sel > 0)
                {
                    txtValue.Text = _oldNumber.ToString();
                    txtValue.SelectionStart = sel - 1;
                }
                return;
            }
            _oldNumber = decimal.Parse(txtValue.Text);
        }

        public override object Value
        {
            get
            {
                return txtValue.Text;
            }
            set
            {
                txtValue.Text = value.ToString();
            }
        }

        public override string Text
        {
            get
            {
                return txtValue.Text;
            }
            set
            {
                txtValue.Text = value;
            }
        }

        /// <summary>
        /// ��ֵ֤�Ƿ���Ϲ淶
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsCorrect(string value) 
        {
            decimal val = 0m;
            if (!decimal.TryParse(value, out val))
            {
                CurToolTip.Show("�������Ϊ����", txtValue);
                return false;
            }
            if (_decimalPlaces >= 0)
            {
                long places = (long)Math.Pow(10, _decimalPlaces);
                decimal dec = places * Math.Abs(val);
                if (dec % 1 > 0)
                {
                    if (_decimalPlaces > 0)
                    {
                        CurToolTip.Show("����С�����ܳ���" + _decimalPlaces + "λ", txtValue);
                    }
                    else if (_decimalPlaces == 0) 
                    {
                        CurToolTip.Show("�������Ϊ����", txtValue);
                    }
                    return false;
                }
            }
            if (val > Max)
            {
                CurToolTip.Show("��������ܳ���" + Max, txtValue);
                return false;
            }
            if (val < Min)
            {
                CurToolTip.Show("������С����С��" + Min, txtValue);
                return false;
            }
            return true;
        }
    }
}
