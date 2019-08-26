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
    public partial class Ymrrobotconfig: TestLib.YMRBase
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
        ///是否可用
        ///</summary>
        protected bool? _enable;

        /// <summary>
        ///是否可用
        ///</summary>
        public virtual bool? Enable
        {
            get{ return _enable;}
            set{ _enable=value;}
        }
        ///<summary>
        ///是否可以进第一个场
        ///</summary>
        protected bool? _joinRoomZero;

        /// <summary>
        ///是否可以进第一个场
        ///</summary>
        public virtual bool? JoinRoomZero
        {
            get{ return _joinRoomZero;}
            set{ _joinRoomZero=value;}
        }
        ///<summary>
        ///是否可以进第二个场
        ///</summary>
        protected bool? _joinRoomOne;

        /// <summary>
        ///是否可以进第二个场
        ///</summary>
        public virtual bool? JoinRoomOne
        {
            get{ return _joinRoomOne;}
            set{ _joinRoomOne=value;}
        }
        ///<summary>
        ///是否可以进第三个场
        ///</summary>
        protected bool? _joinRoomTwo;

        /// <summary>
        ///是否可以进第三个场
        ///</summary>
        public virtual bool? JoinRoomTwo
        {
            get{ return _joinRoomTwo;}
            set{ _joinRoomTwo=value;}
        }
        ///<summary>
        ///是否可以进第四个场
        ///</summary>
        protected bool? _joinRoomThree;

        /// <summary>
        ///是否可以进第四个场
        ///</summary>
        public virtual bool? JoinRoomThree
        {
            get{ return _joinRoomThree;}
            set{ _joinRoomThree=value;}
        }
        ///<summary>
        ///最小游戏时间（分钟）
        ///</summary>
        protected int? _minPlayGameTime;

        /// <summary>
        ///最小游戏时间（分钟）
        ///</summary>
        public virtual int? MinPlayGameTime
        {
            get{ return _minPlayGameTime;}
            set{ _minPlayGameTime=value;}
        }
        ///<summary>
        ///最大游戏时间（分钟）
        ///</summary>
        protected int? _maxPlayGameTime;

        /// <summary>
        ///最大游戏时间（分钟）
        ///</summary>
        public virtual int? MaxPlayGameTime
        {
            get{ return _maxPlayGameTime;}
            set{ _maxPlayGameTime=value;}
        }
        ///<summary>
        ///随机锁定最小时间（秒）
        ///</summary>
        protected int? _randomLockMinTime;

        /// <summary>
        ///随机锁定最小时间（秒）
        ///</summary>
        public virtual int? RandomLockMinTime
        {
            get{ return _randomLockMinTime;}
            set{ _randomLockMinTime=value;}
        }
        ///<summary>
        ///随机锁定最大时间（秒）
        ///</summary>
        protected int? _randomLockMaxTime;

        /// <summary>
        ///随机锁定最大时间（秒）
        ///</summary>
        public virtual int? RandomLockMaxTime
        {
            get{ return _randomLockMaxTime;}
            set{ _randomLockMaxTime=value;}
        }
        ///<summary>
        ///随机模式最小时间（秒）
        ///</summary>
        protected int? _randomPatternMinTime;

        /// <summary>
        ///随机模式最小时间（秒）
        ///</summary>
        public virtual int? RandomPatternMinTime
        {
            get{ return _randomPatternMinTime;}
            set{ _randomPatternMinTime=value;}
        }
        ///<summary>
        ///随机模式最大时间（秒）
        ///</summary>
        protected int? _randomPatternMaxTime;

        /// <summary>
        ///随机模式最大时间（秒）
        ///</summary>
        public virtual int? RandomPatternMaxTime
        {
            get{ return _randomPatternMaxTime;}
            set{ _randomPatternMaxTime=value;}
        }
        ///<summary>
        ///是否打圈圈鱼
        ///</summary>
        protected bool? _canShootCircleFish;

        /// <summary>
        ///是否打圈圈鱼
        ///</summary>
        public virtual bool? CanShootCircleFish
        {
            get{ return _canShootCircleFish;}
            set{ _canShootCircleFish=value;}
        }
        ///<summary>
        ///停止发炮最小间隔（秒）
        ///</summary>
        protected int? _stopShootMinInterval;

        /// <summary>
        ///停止发炮最小间隔（秒）
        ///</summary>
        public virtual int? StopShootMinInterval
        {
            get{ return _stopShootMinInterval;}
            set{ _stopShootMinInterval=value;}
        }
        ///<summary>
        ///停止发炮最大间隔（秒）
        ///</summary>
        protected int? _stopShootMaxInterval;

        /// <summary>
        ///停止发炮最大间隔（秒）
        ///</summary>
        public virtual int? StopShootMaxInterval
        {
            get{ return _stopShootMaxInterval;}
            set{ _stopShootMaxInterval=value;}
        }
        ///<summary>
        ///停止发炮最小时间（秒）
        ///</summary>
        protected int? _stopShootMinTime;

        /// <summary>
        ///停止发炮最小时间（秒）
        ///</summary>
        public virtual int? StopShootMinTime
        {
            get{ return _stopShootMinTime;}
            set{ _stopShootMinTime=value;}
        }
        ///<summary>
        ///停止发炮最大时间（秒）
        ///</summary>
        protected int? _stopShootMaxTime;

        /// <summary>
        ///停止发炮最大时间（秒）
        ///</summary>
        public virtual int? StopShootMaxTime
        {
            get{ return _stopShootMaxTime;}
            set{ _stopShootMaxTime=value;}
        }
        ///<summary>
        ///最大有效射击范围（像素）
        ///</summary>
        protected int? _straightLineMaxValidShootScope;

        /// <summary>
        ///最大有效射击范围（像素）
        ///</summary>
        public virtual int? StraightLineMaxValidShootScope
        {
            get{ return _straightLineMaxValidShootScope;}
            set{ _straightLineMaxValidShootScope=value;}
        }
        ///<summary>
        ///随机发炮间隔最小时间（秒）
        ///</summary>
        protected float? _randomShootMinTime;

        /// <summary>
        ///随机发炮间隔最小时间（秒）
        ///</summary>
        public virtual float? RandomShootMinTime
        {
            get{ return _randomShootMinTime;}
            set{ _randomShootMinTime=value;}
        }
        ///<summary>
        ///随机发炮间隔最大时间（秒）
        ///</summary>
        protected float? _randomShootMaxTime;

        /// <summary>
        ///随机发炮间隔最大时间（秒）
        ///</summary>
        public virtual float? RandomShootMaxTime
        {
            get{ return _randomShootMaxTime;}
            set{ _randomShootMaxTime=value;}
        }
        ///<summary>
        ///选中该配置的概率
        ///</summary>
        protected int? _probability;

        /// <summary>
        ///选中该配置的概率
        ///</summary>
        public virtual int? Probability
        {
            get{ return _probability;}
            set{ _probability=value;}
        }
        ///<summary>
        ///打圈圈鱼最小发炮间隔（秒）
        ///</summary>
        protected float? _shootCircleMinInterval;

        /// <summary>
        ///打圈圈鱼最小发炮间隔（秒）
        ///</summary>
        public virtual float? ShootCircleMinInterval
        {
            get{ return _shootCircleMinInterval;}
            set{ _shootCircleMinInterval=value;}
        }
        ///<summary>
        ///打圈圈鱼最大发炮间隔（秒）
        ///</summary>
        protected float? _shootCircleMaxInterval;

        /// <summary>
        ///打圈圈鱼最大发炮间隔（秒）
        ///</summary>
        public virtual float? ShootCircleMaxInterval
        {
            get{ return _shootCircleMaxInterval;}
            set{ _shootCircleMaxInterval=value;}
        }
        ///<summary>
        ///圈圈鱼优先级
        ///</summary>
        protected int? _shootCirclePriority;

        /// <summary>
        ///圈圈鱼优先级
        ///</summary>
        public virtual int? ShootCirclePriority
        {
            get{ return _shootCirclePriority;}
            set{ _shootCirclePriority=value;}
        }
        ///<summary>
        ///打圈圈鱼的时候是否找更高优先级的鱼
        ///</summary>
        protected bool? _circleIsFindHigherLevel;

        /// <summary>
        ///打圈圈鱼的时候是否找更高优先级的鱼
        ///</summary>
        public virtual bool? CircleIsFindHigherLevel
        {
            get{ return _circleIsFindHigherLevel;}
            set{ _circleIsFindHigherLevel=value;}
        }
        ///<summary>
        ///打圈圈鱼的时候是否模仿人
        ///</summary>
        protected bool? _circleIsCanImitatePeople;

        /// <summary>
        ///打圈圈鱼的时候是否模仿人
        ///</summary>
        public virtual bool? CircleIsCanImitatePeople
        {
            get{ return _circleIsCanImitatePeople;}
            set{ _circleIsCanImitatePeople=value;}
        }
        ///<summary>
        ///锁定模式
        ///</summary>
        protected int? _lockState;

        /// <summary>
        ///锁定模式
        ///</summary>
        public virtual int? LockState
        {
            get{ return _lockState;}
            set{ _lockState=value;}
        }
        ///<summary>
        ///射击模式
        ///</summary>
        protected int? _shootState;

        /// <summary>
        ///射击模式
        ///</summary>
        public virtual int? ShootState
        {
            get{ return _shootState;}
            set{ _shootState=value;}
        }





    }
}
