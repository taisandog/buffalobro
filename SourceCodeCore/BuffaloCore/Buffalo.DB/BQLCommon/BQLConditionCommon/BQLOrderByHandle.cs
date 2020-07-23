using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public class BQLOrderByHandle : BQLParamHandle
    {
        private SortType _sortType;
        private BQLParamHandle _arg;
        private BQLCaseType _caseType=BQLCaseType.CaseByDB;

        internal BQLOrderByHandle(BQLParamHandle arg, SortType sortType)
        {
            
            this._sortType = sortType;
            this._arg = arg;
            //this.valueType = BQLValueType.OrderBy;
        }
        /// <summary>
        /// �Ƿ����ִ�Сд����
        /// </summary>
        public BQLCaseType CaseType
        {
            get
            {
                return _caseType;
            }
            internal set
            {
                _caseType = value;
            }
        }
        /// <summary>
        /// ���ݿ��Դ�����
        /// </summary>
        public BQLOrderByHandle CaseByDB
        {
            get
            {
                _caseType = BQLCaseType.CaseByDB;
                return this;
            }
        }
        /// <summary>
        /// �����ִ�Сд
        /// </summary>
        public BQLOrderByHandle CaseIgnore
        {
            get
            {
                _caseType = BQLCaseType.CaseIgnore;
                return this;
            }
        }
        /// <summary>
        /// ���ִ�Сд
        /// </summary>
        public BQLOrderByHandle CaseMatch
        {
            get
            {
                _caseType = BQLCaseType.CaseMatch;
                return this;
            }
        }
        /// <summary>
        /// �����������
        /// </summary>
        internal SortType SortType
        {
            get
            {
                return _sortType;
            }
        }

        /// <summary>
        /// �������
        /// </summary>
        internal BQLParamHandle Param
        {
            get
            {
                return _arg;
            }
        }
       
        internal override string DisplayValue(KeyWordInfomation info)
        {
            info.IsWhere = true;
            //string orderby = _arg.DisplayValue(info);
            //if (_sortType == SortType.DESC)
            //{
            //    orderby += " desc";
            //}
            string orderby = info.DBInfo.CurrentDbAdapter.DoOrderBy(_arg.DisplayValue(info), _sortType, _caseType, info.DBInfo);
            info.IsWhere = false;
            return orderby;
        }


        internal override void FillInfo(KeyWordInfomation info)
        {
            BQLValueItem.DoFillInfo(_arg, info);
        }
    }
}