using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PEReader.IMAGE_RESOURCE_DIRECTORYS
{
    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResourceType : uint
    {
        /// <summary>
        /// 无资源
        /// </summary>
        Unknow = 0x00,
        /// <summary>
        /// 光标
        /// </summary>
        Cursor = 0x01,
        /// <summary>
        /// 位图
        /// </summary>
        Bitmap = 0x02,
        /// <summary>
        /// 图标
        /// </summary>
        Icon = 0x03,
        /// <summary>
        /// 菜单
        /// </summary>
        Menu = 0x04,
        /// <summary>
        /// 对话框
        /// </summary>
        Dialog = 0x05,
        /// <summary>
        /// 字符串
        /// </summary>
        String = 0x06,
        /// <summary>
        /// 字体目录
        /// </summary>
        FontDirectory = 0x07,
        /// <summary>
        /// 字体
        /// </summary>
        Font = 0x08,
        /// <summary>
        /// 加速键
        /// </summary>
        Accelerators = 0x09,
        /// <summary>
        /// 未格式资源
        /// </summary>
        Unformatted = 0x0A,
        /// <summary>
        /// 消息表
        /// </summary>
        MessageTable = 0x0B,
        /// <summary>
        /// 光标组
        /// </summary>
        GroupCursor = 0x0C,
        /// <summary>
        /// 图标组
        /// </summary>
        GroupIcon = 0x0E,
        /// <summary>
        /// 版本信息
        /// </summary>
        VersionInformation = 0x10
    }
}
