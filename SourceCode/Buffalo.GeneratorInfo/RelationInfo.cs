using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.GeneratorInfo
{
    /// <summary>
    /// ���Ե������Ϣ
    /// </summary>
    public class RelationInfo
    {

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="targetName">Ŀ��ʵ���������</param>
        /// <param name="sourceName">��ʵ��Ĺ�������</param>
        /// <param name="isParent">�Ƿ���������</param>
        /// <param name="sourceType">Դ���Ե�����</param>
        /// <param name="sourceTypeFullName">Դ���Ե�����ȫ��</param>
        public RelationInfo(string targetName,
            string sourceName, bool isParent, string sourceType,string sourceTypeFullName) 
        {
            _targetName = targetName;
            _sourceName = sourceName;
            _isParent = IsParent;
            _sourceType = sourceType;
            _sourceTypeFullName = sourceTypeFullName;
        }
        
        private string _targetName;
        /// <summary>
        /// Ŀ��ʵ���������
        /// </summary>
        public string TargetName
        {
            get { return _targetName; }
        }

        
        private string _sourceName;
        /// <summary>
        /// ��ʵ��Ĺ�������
        /// </summary>
        public string SourceName
        {
            get { return _sourceName; }
        }

        private string _sourceType;
        /// <summary>
        /// �������Ե���ֵ����
        /// </summary>
        public string SourceType
        {
            get { return _sourceType; }
        }

        private string _sourceTypeFullName;
        /// <summary>
        /// �������Ե���ֵ����ȫ��
        /// </summary>
        public string SourceTypeFullName
        {
            get { return _sourceTypeFullName; }
        }
        private bool _isParent;
        /// <summary>
        /// �Ƿ���������
        /// </summary>
        public bool IsParent
        {
            get { return _isParent; }
        }


    }
}
