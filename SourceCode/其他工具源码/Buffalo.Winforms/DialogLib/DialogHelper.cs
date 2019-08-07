
using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Buffalo.Winforms.DialogLib
{
    public class DialogHelper
    {
       

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="mess">消息</param>
        /// <param name="showMask">显示遮罩</param>
        public static void ShowMessage(string mess,Form parentForm=null, string title = "提   示")
        {
            using (FrmUIDialog frm = FrmUIDialog.UCGetForm<UCMessage>()) 
            {
                frm.Text = title;
                frm.StartPosition = FormStartPosition.CenterParent;
                UCMessage ctr = frm.ContentControl as UCMessage;
                ctr.SetMessage(mess);
                frm.ShowDialogForm(parentForm);
                
            }
        }
        /// <summary>
        /// 显示错误
        /// </summary>
        /// <param name="mess">消息</param>
        /// <param name="showMask">显示遮罩</param>
        public static void ShowError(string mess, Form parentForm = null, string title = "错   误")
        {
            using (FrmUIDialog frm = FrmUIDialog.UCGetForm<UCMessage>())
            {
                frm.Text = title;
                frm.StartPosition = FormStartPosition.CenterParent;
                UCMessage ctr = frm.ContentControl as UCMessage;
                ctr.SetError(mess);
                frm.ShowDialogForm(parentForm);
            }
        }
        /// <summary>
        /// 显示错误
        /// </summary>
        /// <param name="mess">消息</param>
        /// <param name="showMask">显示遮罩</param>
        public static void ShowSuccess(string mess, Form parentForm = null, string title = "操作成功")
        {
            using (FrmUIDialog frm = FrmUIDialog.UCGetForm<UCMessage>())
            {
                frm.Text = title;
                frm.StartPosition = FormStartPosition.CenterParent;
                UCMessage ctr = frm.ContentControl as UCMessage;
                ctr.SetSuccess(mess);
                frm.ShowDialogForm(parentForm);
            }
        }
        /// <summary>
        /// 显示问题
        /// </summary>
        /// <param name="mess">问题</param>
        /// <param name="showMask">显示遮罩</param>
        public static bool ShowConfirm(string mess, Form parentForm = null, string title = "问   题")
        {

            using (FrmUIDialog frm = FrmUIDialog.UCGetForm<UCConfirm>())
            {
                frm.Text = title;
                frm.StartPosition = FormStartPosition.CenterParent;
                UCConfirm ctr = frm.ContentControl as UCConfirm;
                ctr.SetConfirm(mess);
                return frm.ShowDialogForm(parentForm) == DialogResult.Yes;
            }
        }

        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <typeparam name="T">控件类型</typeparam>
        /// <returns></returns>
        public static FrmUIDialog GetForm<T>(string title) where T : FormModelBase
        {
            FrmUIDialog frm = FrmUIDialog.UCGetForm<T>();
            
            frm.Text = title;
            return frm;
        }
        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <param name="ctr">控件</param>
        /// <returns></returns>
        public static FrmUIDialog GetForm(FormModelBase ctr, string title) 
        {
            FrmUIDialog frm = FrmUIDialog.UCGetForm(ctr);
            
            return frm;
        }

        /// <summary>
        /// 显示输入框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public static string ShowInput(string title, string label, char passwordChar = '\0') 
        {
            return InputModel.ShowInput(title, label,passwordChar);
        }
    }
}
