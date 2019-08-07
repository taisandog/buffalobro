using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace Buffalo.WinFormsControl.Editors
{
    public partial class OnOffCheckBox : UserControl
    {
        /// <summary>
        /// �� System.Windows.Forms.CheckBox.Checked ���Ե�ֵ����ʱ������
        /// </summary>
        public event EventHandler CheckedChanged;
        /// <summary>
        /// �� System.Windows.Forms.CheckBox.CheckState ���Ե�ֵ����ʱ������
        /// </summary>
        public event EventHandler CheckStateChanged;
        public OnOffCheckBox()
        {
            InitializeComponent();
        }


        private CheckState _checkState;

        /// <summary>
        /// ��ȡ������ System.Windows.Forms.CheckBox ��״̬��
        /// </summary>
        [Bindable(true)]
        public CheckState CheckState
        {
            get
            {
                return _checkState;
            }
            set
            {
                _checkState = value;
                if (CheckStateChanged != null)
                {
                    CheckStateChanged(this, new EventArgs());
                }
                RefreashDisplay();
            }
        }




        /// <summary>
        ///  ��ȡ������һ��ֵ����ֵָʾ System.Windows.Forms.CheckBox �Ƿ���ѡ��״̬��
        /// </summary>
        [Bindable(true)]
        [DefaultValue(false)]
        [SettingsBindable(true)]
        public bool Checked
        {
            get
            {
                return CheckState == CheckState.Checked;
            }
            set
            {
                if (value)
                {
                    CheckState = CheckState.Checked;
                }
                else
                {
                    CheckState = CheckState.Unchecked;
                }
                if (CheckedChanged != null)
                {
                    CheckedChanged(this, new EventArgs());
                }
                RefreashDisplay();
            }
        }

        private OnOffButtonType _onOffType=OnOffButtonType.Quadrate;
        /// <summary>
        /// ��ť��ʽ����
        /// </summary>
        public OnOffButtonType OnOffType
        {
            get { return _onOffType; }
            set 
            { 
                _onOffType = value;
                RefreashDisplay();
            }
        }

        /// <summary>
        /// ˢ����ʾ
        /// </summary>
        private void RefreashDisplay()
        {
            if (pbDisplay == null)
            {
                return;
            }
            pbDisplay.Image = GetImage();

        }

        private Image GetImage()
        {
            if (_checkState == CheckState.Checked)
            {
                switch (_onOffType)
                {
                    case OnOffButtonType.Oblongrectangle:
                        return Buffalo.WinFormsControl.Properties.Resources.on1;
                    case OnOffButtonType.Quadrate:
                        return Buffalo.WinFormsControl.Properties.Resources.on;
                    case OnOffButtonType.BlueOblongrectangle:
                        return Buffalo.WinFormsControl.Properties.Resources.on2;
                    default:
                        return Buffalo.WinFormsControl.Properties.Resources.on;
                }
            }
            else
            {
                switch (_onOffType)
                {
                    case OnOffButtonType.Oblongrectangle:
                        return Buffalo.WinFormsControl.Properties.Resources.off1;
                    case OnOffButtonType.Quadrate:
                        return Buffalo.WinFormsControl.Properties.Resources.off;
                    case OnOffButtonType.BlueOblongrectangle:
                        return Buffalo.WinFormsControl.Properties.Resources.off2;
                    default:
                        return Buffalo.WinFormsControl.Properties.Resources.off;
                }
            }
        }

        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RefreashDisplay();
        }

        private void pbDisplay_Click(object sender, EventArgs e)
        {
            Checked = !Checked;
        }
    }

    /// <summary>
    /// ��ť��ʽ
    /// </summary>
    public enum OnOffButtonType 
    {
        /// <summary>
        /// ����
        /// </summary>
        Quadrate,
        /// <summary>
        /// ��Բ��
        /// </summary>
        Oblongrectangle,
        /// <summary>
        /// ��ɫ��Բ��
        /// </summary>
        BlueOblongrectangle
    }
}
