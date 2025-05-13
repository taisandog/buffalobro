using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AddInSetup
{
    /// <summary>
    /// 帮助文档项
    /// </summary>
    public class HelpDocItem
    {
        /// <summary>
        /// 帮助文档项
        /// </summary>
        public HelpDocItem()
        {

        }
        /// <summary>
        /// 帮助文档项
        /// </summary>
        /// <param name="title"></param>
        /// <param name="path"></param>
        public HelpDocItem(string title,string path)
        {
            _title = title;
            _path = path;
        }
        /// <summary>
        /// 标题
        /// </summary>
        private string _title;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return _title; }
        }
        /// <summary>
        /// 路径
        /// </summary>
        private string _path;
        /// <summary>
        /// 路径
        /// </summary>
        public string Path
        {
            get { return _path; }
        }

        /// <summary>
        /// 从XML节点读取类
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static HelpDocItem LoadForNode(XmlNode node)
        {
            HelpDocItem info = new HelpDocItem();
            XmlAttribute att = node.Attributes["title"];
            if (att != null)
            {
                info._title = att.InnerText;
            }
            att = node.Attributes["path"];
            if (att != null)
            {
                info._path = att.InnerText;
            }
            
            return info;
        }
    }
}
