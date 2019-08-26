using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.CommBase.BusinessBases;
namespace TestLib
{
	/// <summary>
    /// 
    /// </summary>
    public partial class Ymr_insidemodifyrecord: TestLib.YMRBase
    {
        ///<summary>
        ///ID
        ///</summary>
        protected int _id;

        /// <summary>
        ///ID
        ///</summary>
        public virtual int Id
        {
            get{ return _id;}
            set{ _id=value;}
        }
        ///<summary>
        ///创建时间
        ///</summary>
        protected DateTime? _createDate;

        /// <summary>
        ///创建时间
        ///</summary>
        public virtual DateTime? CreateDate
        {
            get{ return _createDate;}
            set{ _createDate=value;}
        }
        ///<summary>
        ///最后更新时间
        ///</summary>
        protected DateTime? _lastDate;

        /// <summary>
        ///最后更新时间
        ///</summary>
        public virtual DateTime? LastDate
        {
            get{ return _lastDate;}
            set{ _lastDate=value;}
        }
        ///<summary>
        ///数据改变后（json）
        ///</summary>
        protected string _afterChange;

        /// <summary>
        ///数据改变后（json）
        ///</summary>
        public virtual string AfterChange
        {
            get{ return _afterChange;}
            set{ _afterChange=value;}
        }
        ///<summary>
        ///数据改变前（json）
        ///</summary>
        protected string _beforeChange;

        /// <summary>
        ///数据改变前（json）
        ///</summary>
        public virtual string BeforeChange
        {
            get{ return _beforeChange;}
            set{ _beforeChange=value;}
        }





    }
}
