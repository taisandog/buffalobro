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
    public partial class Ymr_redenvelopesrecord: TestLib.YMRBase
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
        ///已经抢包玩家id列表
        ///</summary>
        protected string _alreadyRobbedUserIDs;

        /// <summary>
        ///已经抢包玩家id列表
        ///</summary>
        public virtual string AlreadyRobbedUserIDs
        {
            get{ return _alreadyRobbedUserIDs;}
            set{ _alreadyRobbedUserIDs=value;}
        }
        ///<summary>
        ///未抢包玩家id列表
        ///</summary>
        protected string _noRobberyUserIDs;

        /// <summary>
        ///未抢包玩家id列表
        ///</summary>
        public virtual string NoRobberyUserIDs
        {
            get{ return _noRobberyUserIDs;}
            set{ _noRobberyUserIDs=value;}
        }
        ///<summary>
        ///所有包金额
        ///</summary>
        protected string _redEnvelopesMoney;

        /// <summary>
        ///所有包金额
        ///</summary>
        public virtual string RedEnvelopesMoney
        {
            get{ return _redEnvelopesMoney;}
            set{ _redEnvelopesMoney=value;}
        }
        ///<summary>
        ///红包总金额
        ///</summary>
        protected long _redEnvelopesSumGold;

        /// <summary>
        ///红包总金额
        ///</summary>
        public virtual long RedEnvelopesSumGold
        {
            get{ return _redEnvelopesSumGold;}
            set{ _redEnvelopesSumGold=value;}
        }
        ///<summary>
        ///房间等级
        ///</summary>
        protected int? _roomLevel;

        /// <summary>
        ///房间等级
        ///</summary>
        public virtual int? RoomLevel
        {
            get{ return _roomLevel;}
            set{ _roomLevel=value;}
        }





    }
}
