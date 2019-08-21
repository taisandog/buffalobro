using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.QueryConditions;

namespace Buffalo.DB.ListExtends
{
    /// <summary>
    /// List���������е��ݴ���
    /// </summary>
    class CompareItem
    {
        private bool curCompare;
        private ConnectType connectType;

        /// <summary>
        /// �ȽϽ�������
        /// </summary>
        /// <param name="curCompare">�ȽϽ��</param>
        /// <param name="connectType">��������</param>
        internal CompareItem(bool curCompare, ConnectType connectType) 
        {
            this.curCompare = curCompare;
            this.connectType = connectType;
        }


        /// <summary>
        /// �ȽϽ��
        /// </summary>
        public bool CurCompare 
        {
            get 
            {
                return curCompare;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public ConnectType ConnectType 
        {
            get 
            {
                return connectType;
            }
        }
    }
}
