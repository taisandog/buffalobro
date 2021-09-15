using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// UI������
    /// </summary>
    public class UIProjectItem
    {
        private string _modelPath;
        /// <summary>
        /// ģ��·��
        /// </summary>
        public string ModelPath
        {
            get { return _modelPath; }
            set { _modelPath = value; }
        }

        private BuildAction _genType;
        /// <summary>
        /// ��������
        /// </summary>
        public BuildAction GenType
        {
            get { return _genType; }
            set { _genType = value; }
        }

        private string _targetPath;
        /// <summary>
        /// ����·��
        /// </summary>
        public string TargetPath
        {
            get { return _targetPath; }
            set { _targetPath = value; }
        }

        private Encoding _encoding = Encoding.GetEncoding("gb2312");
        /// <summary>
        /// ����ļ��ı���
        /// </summary>
        public Encoding Encoding
        {
            get { return _encoding; }
            set { _encoding = value; }
        }

        /// <summary>
        /// ���ɵĴ��뻺��
        /// </summary>
        private Type _genCodeCache;

        

        private List<UIProjectItem> _childItems=new List<UIProjectItem>();

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public List<UIProjectItem> ChildItems
        {
            get { return _childItems; }
        }
    }
}
