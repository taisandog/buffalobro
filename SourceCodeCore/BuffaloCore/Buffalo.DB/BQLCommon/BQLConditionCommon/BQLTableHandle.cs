using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using System.Data;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public abstract class BQLTableHandle : BQLValueItem
    {
        
        /// <summary>
        /// ��ȡ��Ӧ��ʵ������
        /// </summary>
        /// <returns></returns>
        internal abstract List<ParamInfo> GetParamInfoHandle();
        /// <summary>
        /// ���������һ������
        /// </summary>
        /// <param name="asName">����</param>
        /// <returns></returns>
        public BQLAliasHandle AS(string asName)
        {
            BQLAliasHandle item = new BQLAliasHandle(this, asName);
            return item;
        }

        /// <summary>
        /// ��ȡ�ֶ�
        /// </summary>
        /// <param name="paramName">�ֶ���</param>
        /// <returns></returns>
        public virtual BQLParamHandle this[string paramName]
        {
            get
            {
                return new BQLOtherParamHandle(this, paramName);
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        public virtual List<string> GetPrimaryParam()
        {

            return null;

        }

        /// <summary>
        /// ��ȡ�ֶ�
        /// </summary>
        /// <param name="paramName">�ֶ���</param>
        /// <returns></returns>
        public virtual BQLParamHandle this[string paramName,DbType dbType]
        {
            get
            {
                BQLOtherParamHandle prm = new BQLOtherParamHandle(this, paramName);
                prm.ValueDbType = dbType;
                return prm;
            }
        }
        protected BQLParamHandle __ = null;
        /// <summary>
        /// ��ȡ�����ֶ�
        /// </summary>
        /// <returns></returns>
        public virtual BQLParamHandle _
        {
            get
            {
                if (CommonMethods.IsNull(__))
                {
                    __ = new BQLOtherParamHandle(this, "*");
                }
                return __;
            }
        }
    }
}
