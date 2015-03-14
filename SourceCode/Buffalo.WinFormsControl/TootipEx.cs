using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Buffalo.Kernel.Win32;

namespace Buffalo.WinFormsControl
{

    public class TootipEx:ToolTip
    {
        private bool _hasClose;
        /// <summary>
        /// 是否有关闭按钮
        /// </summary>
        public bool HasClose
        {
            get { return _hasClose; }
            set { _hasClose = value; }
        }


        protected override CreateParams CreateParams
        {
            get
            {
                int style = 0;
                if (_hasClose) 
                {
                    style |= (int)ToolTipStyles.TTS_CLOSE;
                }
                if (IsBalloon) 
                {
                    style |= (int)ToolTipStyles.TTS_BALLOON;
                    IsBalloon = false;
                }
                if (ShowAlways) 
                {
                    style |= (int)ToolTipStyles.TTS_ALWAYSTIP;
                    ShowAlways = false;
                }
                CreateParams ret=base.CreateParams;
                ret.Style = ret.Style | style;
                return ret;
            }
        }
    }
}
