using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Buffalo.WebKernel.ARTDialog
{
    /// <summary>
    /// 提示框样式
    /// </summary>
    public enum DialogSkin
    {
        [Description("aero")]
        Aero=1,
        [Description("black")]
        Black = 2,
        [Description("blue")]
        Blue = 3,
        [Description("chrome")]
        Chrome = 4,
        [Description("default")]
        Default = 5,
        [Description("green")]
        Green = 6,
        [Description("idialog")]
        Idialog = 7,
        [Description("opera")]
        Opera = 8,
        [Description("simple")]
        Simple = 9,
        [Description("twitter")]
        Twitter = 10,
    }
}
