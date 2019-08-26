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
    public partial class Ymrfishconfig: TestLib.YMRBase
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
        ///鱼编号
        ///</summary>
        protected int _fishID;

        /// <summary>
        ///鱼编号
        ///</summary>
        public virtual int FishID
        {
            get{ return _fishID;}
            set{ _fishID=value;}
        }
        ///<summary>
        ///鱼名称
        ///</summary>
        protected string _fishName;

        /// <summary>
        ///鱼名称
        ///</summary>
        public virtual string FishName
        {
            get{ return _fishName;}
            set{ _fishName=value;}
        }
        ///<summary>
        ///最小分(倍)
        ///</summary>
        protected int _minScore;

        /// <summary>
        ///最小分(倍)
        ///</summary>
        public virtual int MinScore
        {
            get{ return _minScore;}
            set{ _minScore=value;}
        }
        ///<summary>
        ///最大分(倍)
        ///</summary>
        protected int _maxScore;

        /// <summary>
        ///最大分(倍)
        ///</summary>
        public virtual int MaxScore
        {
            get{ return _maxScore;}
            set{ _maxScore=value;}
        }
        ///<summary>
        ///最小数量
        ///</summary>
        protected int _minNumber;

        /// <summary>
        ///最小数量
        ///</summary>
        public virtual int MinNumber
        {
            get{ return _minNumber;}
            set{ _minNumber=value;}
        }
        ///<summary>
        ///最大数量
        ///</summary>
        protected int _maxNumber;

        /// <summary>
        ///最大数量
        ///</summary>
        public virtual int MaxNumber
        {
            get{ return _maxNumber;}
            set{ _maxNumber=value;}
        }
        ///<summary>
        ///出现几率
        ///</summary>
        protected int _createOdds;

        /// <summary>
        ///出现几率
        ///</summary>
        public virtual int CreateOdds
        {
            get{ return _createOdds;}
            set{ _createOdds=value;}
        }
        ///<summary>
        ///实际几率
        ///</summary>
        protected double _realOdds;

        /// <summary>
        ///实际几率
        ///</summary>
        public virtual double RealOdds
        {
            get{ return _realOdds;}
            set{ _realOdds=value;}
        }
        ///<summary>
        ///道具鱼
        ///</summary>
        protected int _props;

        /// <summary>
        ///道具鱼
        ///</summary>
        public virtual int Props
        {
            get{ return _props;}
            set{ _props=value;}
        }
        ///<summary>
        ///是否彩虹
        ///</summary>
        protected bool _isRainbow;

        /// <summary>
        ///是否彩虹
        ///</summary>
        public virtual bool IsRainbow
        {
            get{ return _isRainbow;}
            set{ _isRainbow=value;}
        }
        ///<summary>
        ///是否启用
        ///</summary>
        protected bool _isEnable;

        /// <summary>
        ///是否启用
        ///</summary>
        public virtual bool IsEnable
        {
            get{ return _isEnable;}
            set{ _isEnable=value;}
        }
        ///<summary>
        ///是否上传到排行榜
        ///</summary>
        protected bool _isUploadRankingList;

        /// <summary>
        ///是否上传到排行榜
        ///</summary>
        public virtual bool IsUploadRankingList
        {
            get{ return _isUploadRankingList;}
            set{ _isUploadRankingList=value;}
        }





    }
}
