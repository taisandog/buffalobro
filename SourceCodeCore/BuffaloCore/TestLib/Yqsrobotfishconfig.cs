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
    public partial class Yqsrobotfishconfig: TestLib.YMRBase
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
        ///鱼ID
        ///</summary>
        protected int? _fishId;

        /// <summary>
        ///鱼ID
        ///</summary>
        public virtual int? FishId
        {
            get{ return _fishId;}
            set{ _fishId=value;}
        }
        ///<summary>
        ///最小发炮间隔（秒）
        ///</summary>
        protected float? _minShootInterval;

        /// <summary>
        ///最小发炮间隔（秒）
        ///</summary>
        public virtual float? MinShootInterval
        {
            get{ return _minShootInterval;}
            set{ _minShootInterval=value;}
        }
        ///<summary>
        ///最大发炮间隔（秒）
        ///</summary>
        protected float? _maxShootInterval;

        /// <summary>
        ///最大发炮间隔（秒）
        ///</summary>
        public virtual float? MaxShootInterval
        {
            get{ return _maxShootInterval;}
            set{ _maxShootInterval=value;}
        }
        ///<summary>
        ///是否可以打这条鱼
        ///</summary>
        protected bool? _canShoot;

        /// <summary>
        ///是否可以打这条鱼
        ///</summary>
        public virtual bool? CanShoot
        {
            get{ return _canShoot;}
            set{ _canShoot=value;}
        }
        ///<summary>
        ///是否找优先级更高的鱼
        ///</summary>
        protected bool? _isFindHigherLevel;

        /// <summary>
        ///是否找优先级更高的鱼
        ///</summary>
        public virtual bool? IsFindHigherLevel
        {
            get{ return _isFindHigherLevel;}
            set{ _isFindHigherLevel=value;}
        }
        ///<summary>
        ///优先级（值越大优先级越高）
        ///</summary>
        protected int? _priority;

        /// <summary>
        ///优先级（值越大优先级越高）
        ///</summary>
        public virtual int? Priority
        {
            get{ return _priority;}
            set{ _priority=value;}
        }
        ///<summary>
        ///是否模仿人
        ///</summary>
        protected bool? _canImitatePeople;

        /// <summary>
        ///是否模仿人
        ///</summary>
        public virtual bool? CanImitatePeople
        {
            get{ return _canImitatePeople;}
            set{ _canImitatePeople=value;}
        }
        ///<summary>
        ///对应的机器人配置ID
        ///</summary>
        protected int? _robotConfigId;

        /// <summary>
        ///对应的机器人配置ID
        ///</summary>
        public virtual int? RobotConfigId
        {
            get{ return _robotConfigId;}
            set{ _robotConfigId=value;}
        }





    }
}
