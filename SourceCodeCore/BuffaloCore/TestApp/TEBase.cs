using Buffalo.DB.CommBase;
using System;
using System.Collections.Generic;
using System.Text;

using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using System.Data;
using Buffalo.DB.CommBase.BusinessBases;
namespace TestApp
{
    public partial class TEBase:EntityBase
    {
        /// <summary>
        /// ID
        /// </summary>
        protected int _id;
        /// <summary>
        /// 创建日期
        /// </summary>
        protected DateTime _createDate;
        /// <summary>
        /// ID
        /// </summary>
        public virtual int Id
        {
            get{ return _id; }
            set{ _id=value; }
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreateDate
        {
            get{ return _createDate; }
            set{ _createDate=value; }
        }
    }
}
