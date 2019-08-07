using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Buffalo.Win32Kernel.Win32
{
    public class MessageBoxEx
    {
        private MessageBoxEx() { }
        private static int _defaultLCID = Thread.CurrentThread.CurrentCulture.LCID;

        /// <summary>
        /// Ä¬ÈÏÇøÓòID
        /// </summary>
        public static int DefaultLCID
        {
            get { return MessageBoxEx._defaultLCID; }
            set { MessageBoxEx._defaultLCID = value; }
        }


        public static DialogResult Show(string text)
        {
            return Show(WindowsAPI.GetActiveWindow(), text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, _defaultLCID);
        }

        public static DialogResult Show(IWin32Window owner, string text)
        {
            return Show(owner, text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, _defaultLCID);
        }

        public static DialogResult Show(string text, string caption)
        {
            return Show(WindowsAPI.GetActiveWindow(), text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, _defaultLCID);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            return Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, _defaultLCID);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            return Show(WindowsAPI.GetActiveWindow(), text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, _defaultLCID);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            return Show(owner, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, _defaultLCID);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(WindowsAPI.GetActiveWindow(), text, caption, buttons, icon, MessageBoxDefaultButton.Button1, _defaultLCID);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(owner.Handle, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, _defaultLCID);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Show(WindowsAPI.GetActiveWindow(), text, caption, buttons, icon, defaultButton, _defaultLCID);
        }

       
        //**************************





        public static DialogResult Show(string text, int lcid) 
        {
            return Show(WindowsAPI.GetActiveWindow(), text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, lcid);
        }

        public static DialogResult Show(IWin32Window owner, string text, int lcid) 
        {
            return Show(owner, text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, lcid);
        }

        public static DialogResult Show(string text, string caption, int lcid) 
        {
            return Show(WindowsAPI.GetActiveWindow(), text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, lcid);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, int lcid) 
        {
            return Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, lcid);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, int lcid) 
        {
            return Show(WindowsAPI.GetActiveWindow(), text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, lcid);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, int lcid) 
        {
            return Show(owner, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, lcid);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, int lcid) 
        {
            return Show(WindowsAPI.GetActiveWindow(), text, caption, buttons, icon, MessageBoxDefaultButton.Button1, lcid);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, int lcid) 
        {
            return Show(owner.Handle, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, lcid);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, int lcid) 
        {
            return Show(WindowsAPI.GetActiveWindow(), text, caption, buttons, icon, defaultButton, lcid);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, int lcid) 
        {
            return (DialogResult)WindowsAPI.MessageBoxEx(owner.Handle, text, caption, (int)buttons | (int)defaultButton , lcid);
        }
        public static DialogResult Show(IntPtr handle, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, int lcid)
        {
            return (DialogResult)WindowsAPI.MessageBoxEx(handle, text, caption, (int)buttons | (int)defaultButton, lcid);
        }


        

 

    }
}

//**************************************Ê¾Àý*************************
//CultureInfo info = CultureInfo.GetCultureInfo("en-US");
//            MessageBoxEx.DefaultLCID = info.LCID;
//            if (MessageBoxEx.Show("shit", "aa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) 
//            {
//                richTextBox1.Text = "aaaa";
//            }