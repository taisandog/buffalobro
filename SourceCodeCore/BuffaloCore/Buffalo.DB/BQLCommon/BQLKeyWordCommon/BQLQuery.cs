using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    /// <summary>
    /// BQL��ѯ
    /// </summary>
    public abstract class BQLQuery
    {
        private BQLQuery previous;
        /// <summary>
        /// �ؼ�����
        /// </summary>
        /// <param name="keyWordName">�ؼ�����</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal BQLQuery( BQLQuery previous) 
        {
            this.previous = previous;
        }
       
        /// <summary>
        /// ��һ���ؼ���
        /// </summary>
        internal BQLQuery Previous
        {
            get
            {
                return previous;
            }
        }
        /// <summary>
        /// �ؼ��ֽ���
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal abstract void Tran(KeyWordInfomation info);
        internal abstract void LoadInfo(KeyWordInfomation info);
        /// <summary>
        /// �������ѯ����һ������
        /// </summary>
        /// <param name="asName">����(�������Ҫ��������Ϊnull��"")</param>
        /// <returns></returns>
        public BQLAliasHandle AS(string asName) 
        {
            BQLAliasHandle item = new BQLAliasHandle(this, asName);
            return item;
        }
    }
}
