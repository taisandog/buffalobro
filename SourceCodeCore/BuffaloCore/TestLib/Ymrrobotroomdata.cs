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
    public partial class Ymrrobotroomdata: TestLib.YMRBase
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
        ///最小进、出桌子时间（秒）
        ///</summary>
        protected int? _minJoinExitDeskTime;

        /// <summary>
        ///最小进、出桌子时间（秒）
        ///</summary>
        public virtual int? MinJoinExitDeskTime
        {
            get{ return _minJoinExitDeskTime;}
            set{ _minJoinExitDeskTime=value;}
        }
        ///<summary>
        ///最大进、出桌子时间（秒）
        ///</summary>
        protected int? _maxJoinExitDeskTime;

        /// <summary>
        ///最大进、出桌子时间（秒）
        ///</summary>
        public virtual int? MaxJoinExitDeskTime
        {
            get{ return _maxJoinExitDeskTime;}
            set{ _maxJoinExitDeskTime=value;}
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
        ///<summary>
        ///每个场最大机器人数量
        ///</summary>
        protected int? _roomMaxRobotCount;

        /// <summary>
        ///每个场最大机器人数量
        ///</summary>
        public virtual int? RoomMaxRobotCount
        {
            get{ return _roomMaxRobotCount;}
            set{ _roomMaxRobotCount=value;}
        }
        ///<summary>
        ///桌子满员最大保留机器人数量
        ///</summary>
        protected int? _deskMaxRobotCount;

        /// <summary>
        ///桌子满员最大保留机器人数量
        ///</summary>
        public virtual int? DeskMaxRobotCount
        {
            get{ return _deskMaxRobotCount;}
            set{ _deskMaxRobotCount=value;}
        }
        ///<summary>
        ///进入房间最小余额（元）
        ///</summary>
        protected int? _joinRoomMinMoney;

        /// <summary>
        ///进入房间最小余额（元）
        ///</summary>
        public virtual int? JoinRoomMinMoney
        {
            get{ return _joinRoomMinMoney;}
            set{ _joinRoomMinMoney=value;}
        }
        ///<summary>
        ///进入房间最大余额（元）
        ///</summary>
        protected int? _joinRoomMaxMoney;

        /// <summary>
        ///进入房间最大余额（元）
        ///</summary>
        public virtual int? JoinRoomMaxMoney
        {
            get{ return _joinRoomMaxMoney;}
            set{ _joinRoomMaxMoney=value;}
        }
        ///<summary>
        ///存币最小倍率（存币倍率为相对于进场分）
        ///</summary>
        protected int? _storageMinMultiple;

        /// <summary>
        ///存币最小倍率（存币倍率为相对于进场分）
        ///</summary>
        public virtual int? StorageMinMultiple
        {
            get{ return _storageMinMultiple;}
            set{ _storageMinMultiple=value;}
        }
        ///<summary>
        ///存币最大倍率（存币倍率为相对于进场分）
        ///</summary>
        protected int? _storageMaxMultiple;

        /// <summary>
        ///存币最大倍率（存币倍率为相对于进场分）
        ///</summary>
        public virtual int? StorageMaxMultiple
        {
            get{ return _storageMaxMultiple;}
            set{ _storageMaxMultiple=value;}
        }
        ///<summary>
        ///强制退出赢的最小倍率
        ///</summary>
        protected int? _compelExitMinWinMultiple;

        /// <summary>
        ///强制退出赢的最小倍率
        ///</summary>
        public virtual int? CompelExitMinWinMultiple
        {
            get{ return _compelExitMinWinMultiple;}
            set{ _compelExitMinWinMultiple=value;}
        }
        ///<summary>
        ///强制退出赢的最大倍率
        ///</summary>
        protected int? _compelExitMaxWinMultiple;

        /// <summary>
        ///强制退出赢的最大倍率
        ///</summary>
        public virtual int? CompelExitMaxWinMultiple
        {
            get{ return _compelExitMaxWinMultiple;}
            set{ _compelExitMaxWinMultiple=value;}
        }
        ///<summary>
        ///存币上限最小次数
        ///</summary>
        protected int? _withdrawalsMinCount;

        /// <summary>
        ///存币上限最小次数
        ///</summary>
        public virtual int? WithdrawalsMinCount
        {
            get{ return _withdrawalsMinCount;}
            set{ _withdrawalsMinCount=value;}
        }
        ///<summary>
        ///存币上限最大次数
        ///</summary>
        protected int? _withdrawalsMaxCount;

        /// <summary>
        ///存币上限最大次数
        ///</summary>
        public virtual int? WithdrawalsMaxCount
        {
            get{ return _withdrawalsMaxCount;}
            set{ _withdrawalsMaxCount=value;}
        }
        ///<summary>
        ///轮询进入房间最小时间（分钟）
        ///</summary>
        protected int? _pollingJoinRoomMinTime;

        /// <summary>
        ///轮询进入房间最小时间（分钟）
        ///</summary>
        public virtual int? PollingJoinRoomMinTime
        {
            get{ return _pollingJoinRoomMinTime;}
            set{ _pollingJoinRoomMinTime=value;}
        }
        ///<summary>
        ///轮询进入房间最大时间（分钟）
        ///</summary>
        protected int? _pollingJoinRoomMaxTime;

        /// <summary>
        ///轮询进入房间最大时间（分钟）
        ///</summary>
        public virtual int? PollingJoinRoomMaxTime
        {
            get{ return _pollingJoinRoomMaxTime;}
            set{ _pollingJoinRoomMaxTime=value;}
        }
        ///<summary>
        ///通用最小停留时间（秒，主要用于存取币后停多久发炮，退出桌子前停留多久）
        ///</summary>
        protected int? _commonMinStayTime;

        /// <summary>
        ///通用最小停留时间（秒，主要用于存取币后停多久发炮，退出桌子前停留多久）
        ///</summary>
        public virtual int? CommonMinStayTime
        {
            get{ return _commonMinStayTime;}
            set{ _commonMinStayTime=value;}
        }
        ///<summary>
        ///通用最大停留时间（秒，主要用于存取币后停多久发炮，退出桌子前停留多久）
        ///</summary>
        protected int? _commonMaxStayTime;

        /// <summary>
        ///通用最大停留时间（秒，主要用于存取币后停多久发炮，退出桌子前停留多久）
        ///</summary>
        public virtual int? CommonMaxStayTime
        {
            get{ return _commonMaxStayTime;}
            set{ _commonMaxStayTime=value;}
        }





    }
}
