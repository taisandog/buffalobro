
using Buffalo.Winforms.UILoaderUnit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;

using System.Windows.Forms;

namespace Buffalo.Winforms
{
    public partial class FrmUIDialog : Form, IShowToolTip
    {
        public FrmUIDialog()
        {
            InitializeComponent();
            
        }
        private bool _showMask = false;
        /// <summary>
        /// 是否显示遮罩
        /// </summary>
        public bool ShowMask
        {
            get { return _showMask; }
            set { _showMask = value; }
        }

        private FrmMask _frmMask;

        /// <summary>
        /// 背景遮罩
        /// </summary>
        public FrmMask FrmMask
        {
            get
            {
                if (_frmMask == null || _frmMask.IsDisposed)
                {
                    _frmMask = new FrmMask();

                }
                return _frmMask;
            }
        }

        private FormModelBase _contentControl;

        /// <summary>
        /// 内容控件
        /// </summary>
        public FormModelBase ContentControl
        {
            get { return _contentControl; }
        }

        private bool _fullScreen;

        /// <summary>
        /// 是否全屏
        /// </summary>
        public bool FullScreen
        {
            get { return _fullScreen; }
            set { _fullScreen = value; }
        }

        /// <summary>
        /// 添加到窗体
        /// </summary>
        /// <param name="ctr"></param>
        private void AddToMain(FormModelBase ctr) 
        {
            _contentControl = ctr;
            _contentControl.OnDialogResault += _contentControl_OnDialogResault;
            _contentControl.OnShowToolTip += _contentControl_OnShowToolTip;
            ResizeForm();

            pnlMain.Controls.Add(ctr);
            ctr.Dock = DockStyle.Fill;
        }

        void _contentControl_OnShowToolTip(Control ctr, string title, string message, bool isErr) 
        {

            //UILoader.ShowToolTip(tpMessage, this, MousePosition, ctr, title, message, isErr);
            UILoader.ShowToolTip(tpMessage, this, MousePosition, ctr, title, message, isErr);
        }

        
        void _contentControl_OnDialogResault(DialogResult resault)
        {
            this.DialogResult = resault;
            this.Close();
        }

        /// <summary>
        /// 重新定义窗体大小
        /// </summary>
        private void ResizeForm() 
        {
            if (ContentControl == null) 
            {
                return;
            }
            Size ctrSize = ContentControl.Size;
            this.Size = new Size(ctrSize.Width + 25, ctrSize.Height + 60);
        }
        /// <summary>
        /// 显示窗体
        /// </summary>
        public void ShowForm(Form parentForm) 
        {
            ShowMaskForm(parentForm);
            
            if (_fullScreen)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            
            this.Show();
            this.TopMost = true;
            this.TopMost = false;

        }
        /// <summary>
        /// 显示遮罩
        /// </summary>
        private void ShowMaskForm(Form parentForm)
        {
            if (_showMask && !_fullScreen)
            {
                FrmMask.Show();
                if (parentForm != null)
                {
                    FrmMask.Location = parentForm.Location;
                    FrmMask.Size = parentForm.Size;
                }
                FrmMask.TopMost = true;
                FrmMask.TopMost = false;

            }
        }

        /// <summary>
        /// 显示对话框窗体
        /// </summary>
        public DialogResult ShowDialogForm(Form parentForm)
        {
            //ResizeForm();
            ShowMaskForm(parentForm);
            if (_fullScreen) 
            {
                this.WindowState = FormWindowState.Maximized;
            }
            
            DialogResult resault=this.ShowDialog();
            if (parentForm != null) 
            {
                parentForm.TopMost = true;
                parentForm.TopMost = false;
            }
            
            return resault;
            
        }
        /// <summary>
        /// 获取窗体
        /// </summary>
        /// <param name="ctr">要加入的控件</param>
        /// <returns></returns>
        public static FrmUIDialog UCGetForm(FormModelBase ctr, Form parentForm = null) 
        {
            FrmUIDialog frm = new FrmUIDialog();
            frm.AddToMain(ctr);
            
            return frm;
        }
        /// <summary>
        /// 获取窗体
        /// </summary>
        /// <param name="ctr">要加入的控件</param>
        /// <returns></returns>
        public static FrmUIDialog UCGetForm<T>(Form parentForm = null) where T : FormModelBase
        {
            T ctr = Activator.CreateInstance(typeof(T)) as T;
            return UCGetForm(ctr);
        }

        private void FrmUIDialog_Load(object sender, EventArgs e)
        {
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            //this.UpdateStyles();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                cp.ExStyle |= 0x02000000;

                return cp;

            }
        }
        private void FrmUIDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.ContentControl != null)
            {
                this.ContentControl.HideUI();
            }
            if (FrmMask != null && !FrmMask.IsDisposed) 
            {
                FrmMask.Close();
                FrmMask.Dispose();
            }
        }

        private void FrmUIDialog_Shown(object sender, EventArgs e)
        {
            if (this.ContentControl != null)
            {
                this.ContentControl.ShowUI();
            }
        }





        public void ShowTooltip(Control ctr, string title, string message, bool isErr)
        {
            UILoader.ShowToolTip(tpMessage, this, MousePosition, ctr, title, message, isErr);
        }
    }
}
