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
        ///����ʱ��
        ///</summary>
        protected DateTime? _createDate;

        /// <summary>
        ///����ʱ��
        ///</summary>
        public virtual DateTime? CreateDate
        {
            get{ return _createDate;}
            set{ _createDate=value;}
        }
        ///<summary>
        ///������ʱ��
        ///</summary>
        protected DateTime? _lastDate;

        /// <summary>
        ///������ʱ��
        ///</summary>
        public virtual DateTime? LastDate
        {
            get{ return _lastDate;}
            set{ _lastDate=value;}
        }
        ///<summary>
        ///���ݸı��json��
        ///</summary>
        protected string _afterChange;

        /// <summary>
        ///���ݸı��json��
        ///</summary>
        public virtual string AfterChange
        {
            get{ return _afterChange;}
            set{ _afterChange=value;}
        }
        ///<summary>
        ///���ݸı�ǰ��json��
        ///</summary>
        protected string _beforeChange;

        /// <summary>
        ///���ݸı�ǰ��json��
        ///</summary>
        public virtual string BeforeChange
        {
            get{ return _beforeChange;}
            set{ _beforeChange=value;}
        }





    }
}
