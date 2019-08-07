using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Text;

using System.Windows.Forms;

namespace Buffalo.Winforms
{
    /// <summary>
    /// 对话框完结
    /// </summary>
    /// <param name="resault"></param>
    public delegate void DialogFinishHandle(DialogResult resault);

    /// <summary>
    /// 显示Tooltip
    /// </summary>
    /// <param name="message"></param>
    public delegate void ShowToolTipHandle(UserControl ctr,string title, string message,bool isErr) ;
    [ToolboxItem(false)]
    public class FormModelBase:UserControl
    {
        /// <summary>
        /// 对话框结果
        /// </summary>
        public event DialogFinishHandle OnDialogResault;

        /// <summary>
        /// 显示Tooltip事件
        /// </summary>
        public event ShowToolTipHandle OnShowToolTip;

        private bool _isInit = false;
        /// <summary>
        /// 显示UI
        /// </summary>
        public virtual void ShowUI()
        {
            if (!_isInit) 
            {
                OnUIInit();
                _isInit = true;
                if (this.Dock != DockStyle.Fill)
                {
                    BorderStyle = BorderStyle.FixedSingle;
                }
            }
            
        }

        /// <summary>
        /// 第一次显示UI时候触发
        /// </summary>
        public virtual void OnUIInit() 
        {

        }

        /// <summary>
        /// 隐藏控件
        /// </summary>
        public virtual void HideUI() { }

        /// <summary>
        /// 销毁控件
        /// </summary>
        public virtual void DisposeUI() { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.ParentForm != null) 
            {
                this.ParentForm.KeyPreview = true;
            }
        }

        /// <summary>
        /// 是否按Enter键跳去下一个控件
        /// </summary>
        protected virtual bool EnterAllowNext 
        {
            get 
            {
                return true;
            }
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (EnterAllowNext && keyData == Keys.Enter && this.ActiveControl != null)
            {
                if (AllowNext(this.ActiveControl))
                {
                    keyData = Keys.Tab;
                }
            }

            return base.ProcessDialogKey(keyData);
           
        }

        /// <summary>
        /// 对话框关闭
        /// </summary>
        /// <param name="resault"></param>
        protected void DialogClose(DialogResult resault) 
        {
            if (OnDialogResault != null) 
            {
                OnDialogResault(resault);
            }
        }
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="resault"></param>
        protected void ShowTooltip(UserControl ctr, string title, string message, bool isErr) 
        {
            if (OnShowToolTip != null)
            {
                OnShowToolTip(ctr,title,message,isErr);
            }
        }
        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="resault"></param>
        protected void ShowErrTooltip(string message, UserControl ctr = null)
        {
            if (ctr == null) 
            {
                ctr = this;
            }
            ShowTooltip(ctr, "错误", message, true);
        }
        //public static void ShowToolTip(ToolTip tp, Form frm, Point mousePosition, Control sender, string title, string message, bool isErr)
        //{
        //    if (isErr)
        //    {
        //        tp.ToolTipIcon = ToolTipIcon.Error;
        //    }
        //    else
        //    {
        //        tp.ToolTipIcon = ToolTipIcon.Info;
        //    }
        //    Point mousep = frm.PointToClient(mousePosition);
        //    tp.ToolTipTitle = title;
        //    //LastMsg = Infomsg;
        //    tp.IsBalloon = true;
        //    tp.Show(message, frm, mousep.X - 20, mousep.Y - 60, 2000);
        //    tp.Hide(frm);
        //    tp.Show(message, frm, mousep.X - 20, mousep.Y - 60, 2000);
        //}

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="resault"></param>
        protected void ShowTooltip(string message, UserControl ctr = null)
        {
            if (ctr == null)
            {
                ctr = this;
            }
            ShowTooltip(ctr, "提示", message, false);
        }
        /// <summary>
        /// 是否要跳到下一个控件
        /// </summary>
        /// <param name="ctr"></param>
        /// <returns></returns>
        private bool AllowNext(Control ctr)
        {

            if (this.ActiveControl is TextBoxBase)
            {
                TextBoxBase txt = this.ActiveControl as TextBoxBase;
                if (txt.Multiline && !txt.ReadOnly)
                {
                    return false;
                }
                return true;
            }
            if (this.ActiveControl is ComboBox)
            {
                return true;
            }
            if (this.ActiveControl is UpDownBase)
            {
                return true;
            }
            return false;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormModelBase
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Name = "FormModelBase";
            this.Size = new System.Drawing.Size(146, 146);
            this.ResumeLayout(false);

        }

    }
}
