using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Buffalo.Winforms
{
    public delegate object AsynMethodHandle(object[] args);
    [ModelID("ASUI.UILoading", "加载窗体")]
    public partial class UILoading : FormModelBase
    {
        public UILoading()
        {
            InitializeComponent();
        }
        private AsynMethodHandle _anycHandle;

        /// <summary>
        /// 异步执行函数
        /// </summary>
        public AsynMethodHandle AnycHandle
        {
            get { return _anycHandle; }
            set { _anycHandle = value; }
        }

        private object[] _args;

        /// <summary>
        /// 参数
        /// </summary>
        public object[] Args
        {
            get { return _args; }
            set { _args = value; }
        }

        private object _retValue;

        /// <summary>
        /// 返回值
        /// </summary>
        public object RetValue
        {
            get { return _retValue; }
            set { _retValue = value; }
        }

        private void DoAnyc() 
        {
            try
            {
                _retValue = _anycHandle(_args);
            }
            finally 
            {
                this.Invoke((MethodInvoker)delegate
                {
                    DialogClose(DialogResult.OK);
                });
            }
        }

        private Thread _thd;

        public override void OnUIInit()
        {
            if (_anycHandle != null) 
            {
                _thd = new Thread(new ThreadStart(DoAnyc));
                _thd.Start();
            }
            base.OnUIInit();
        }

        /// <summary>
        /// 显示加载中的窗体
        /// </summary>
        /// <param name="frmParent">执行的窗体</param>
        /// <param name="handle">要执行的异步函数</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static object ShowDialog(AsynMethodHandle handle,object[] args) 
        {
            using (FrmUIDialog frm = FrmUIDialog.UCGetForm<UILoading>()) 
            {
                UILoading loadUI = frm.ContentControl as UILoading;
                loadUI.AnycHandle = handle;
                loadUI.Args = args;
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialogForm(null);
                return loadUI.RetValue;
            }
        }



        /// <summary>
        /// 初始化加载框
        /// </summary>
        /// <param name="ctrContainer"></param>
        public void InitLoading(Control ctrContainer) 
        {
            ctrContainer.Controls.Add(this);
            this.BringToFront();
            this.Hide();
        }
        /// <summary>
        /// 显示加载框
        /// </summary>
        public void ShowLoading() 
        {
            int x = (this.Parent.Size.Width - this.Width)/2;
            int y = (this.Parent.Size.Height - this.Height) / 2;
            this.Location = new Point(x, y);

            this.Show();
        }
        /// <summary>
        /// 显示加载框
        /// </summary>
        public void HideLoading()
        {
            this.Hide();
        }
    }
}
