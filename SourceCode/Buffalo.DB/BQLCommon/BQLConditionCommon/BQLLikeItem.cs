using Buffalo.DB.DataBaseAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// Like项
    /// </summary>
    public class BQLLikeItem : BQLCondition
    {
       
        private BQLValueItem _sourceHandle;
        private BQLValueItem _targetHandle;
        private BQLLikeType _type;
        private BQLCaseType _caseType;
        /// <summary>
        /// like条件函数
        /// </summary>
        /// <param name="sourceHandle">源参数</param>
        /// <param name="targetHandle">目标值</param>
        /// <param name="type">Like方式</param>
        /// <param name="caseType">大小写参数</param>
        public BQLLikeItem(BQLValueItem sourceHandle, BQLValueItem targetHandle, BQLLikeType type, BQLCaseType caseType)
        {
            this._sourceHandle = sourceHandle;
            this._targetHandle = targetHandle;
            _type = type;
            _caseType = caseType;
            this._valueDbType = DbType.Boolean;
        }
       

        internal override void FillInfo(KeyWordInfomation info)
        {
            BQLValueItem.DoFillInfo(_sourceHandle, info);
            BQLValueItem.DoFillInfo(_targetHandle, info);
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            string strSource = _sourceHandle.DisplayValue(info);
            string strTarget=_targetHandle.DisplayValue(info);
            return info.DBInfo.CurrentDbAdapter.DoLike(strSource, strTarget, _type, _caseType, info.DBInfo);
        }
    }
}
