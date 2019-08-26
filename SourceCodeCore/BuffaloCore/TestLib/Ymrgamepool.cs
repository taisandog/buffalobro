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
    public partial class Ymrgamepool: TestLib.YMRBase
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
        ///场次
        ///</summary>
        protected int? _level;

        /// <summary>
        ///场次
        ///</summary>
        public virtual int? Level
        {
            get{ return _level;}
            set{ _level=value;}
        }
        ///<summary>
        ///偏差的分数,奖池偏差多少分开始调整概率
        ///</summary>
        protected int? _offsetScore;

        /// <summary>
        ///偏差的分数,奖池偏差多少分开始调整概率
        ///</summary>
        public virtual int? OffsetScore
        {
            get{ return _offsetScore;}
            set{ _offsetScore=value;}
        }
        ///<summary>
        ///三级池子分数
        ///</summary>
        protected long? _poolScore3;

        /// <summary>
        ///三级池子分数
        ///</summary>
        public virtual long? PoolScore3
        {
            get{ return _poolScore3;}
            set{ _poolScore3=value;}
        }
        ///<summary>
        ///二级池子分数
        ///</summary>
        protected long? _poolScore2;

        /// <summary>
        ///二级池子分数
        ///</summary>
        public virtual long? PoolScore2
        {
            get{ return _poolScore2;}
            set{ _poolScore2=value;}
        }
        ///<summary>
        ///二级池子比例,万分之
        ///</summary>
        protected int? _poolScore2Rake;

        /// <summary>
        ///二级池子比例,万分之
        ///</summary>
        public virtual int? PoolScore2Rake
        {
            get{ return _poolScore2Rake;}
            set{ _poolScore2Rake=value;}
        }
        ///<summary>
        ///二级池子分数临时
        ///</summary>
        protected long? _poolScore2Temp;

        /// <summary>
        ///二级池子分数临时
        ///</summary>
        public virtual long? PoolScore2Temp
        {
            get{ return _poolScore2Temp;}
            set{ _poolScore2Temp=value;}
        }
        ///<summary>
        ///当前记录的分数
        ///</summary>
        protected long? _nowScore;

        /// <summary>
        ///当前记录的分数
        ///</summary>
        public virtual long? NowScore
        {
            get{ return _nowScore;}
            set{ _nowScore=value;}
        }
        ///<summary>
        ///要抽成的分数
        ///</summary>
        protected int? _needScore;

        /// <summary>
        ///要抽成的分数
        ///</summary>
        public virtual int? NeedScore
        {
            get{ return _needScore;}
            set{ _needScore=value;}
        }
        ///<summary>
        ///抽成的目标分数
        ///</summary>
        protected long? _max;

        /// <summary>
        ///抽成的目标分数
        ///</summary>
        public virtual long? Max
        {
            get{ return _max;}
            set{ _max=value;}
        }
        ///<summary>
        ///要抽水比例
        ///</summary>
        protected int? _taget;

        /// <summary>
        ///要抽水比例
        ///</summary>
        public virtual int? Taget
        {
            get{ return _taget;}
            set{ _taget=value;}
        }
        ///<summary>
        ///总抽成分
        ///</summary>
        protected long? _realScore;

        /// <summary>
        ///总抽成分
        ///</summary>
        public virtual long? RealScore
        {
            get{ return _realScore;}
            set{ _realScore=value;}
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
        ///玩家离线时间
        ///</summary>
        protected int? _offlineTime;

        /// <summary>
        ///玩家离线时间
        ///</summary>
        public virtual int? OfflineTime
        {
            get{ return _offlineTime;}
            set{ _offlineTime=value;}
        }
        ///<summary>
        ///全屏炸弹分数
        ///</summary>
        protected long? _poolScoreFullScreenBomb;

        /// <summary>
        ///全屏炸弹分数
        ///</summary>
        public virtual long? PoolScoreFullScreenBomb
        {
            get{ return _poolScoreFullScreenBomb;}
            set{ _poolScoreFullScreenBomb=value;}
        }
        ///<summary>
        ///全屏炸弹比例，千分之
        ///</summary>
        protected int? _poolScoreFullScreenBombRake;

        /// <summary>
        ///全屏炸弹比例，千分之
        ///</summary>
        public virtual int? PoolScoreFullScreenBombRake
        {
            get{ return _poolScoreFullScreenBombRake;}
            set{ _poolScoreFullScreenBombRake=value;}
        }
        ///<summary>
        ///最多可以负的值(单位金币)
        ///</summary>
        protected int? _maxPay;

        /// <summary>
        ///最多可以负的值(单位金币)
        ///</summary>
        public virtual int? MaxPay
        {
            get{ return _maxPay;}
            set{ _maxPay=value;}
        }
        ///<summary>
        ///抢红包开始时间段
        ///</summary>
        protected DateTime _redEnvelopesStartTime;

        /// <summary>
        ///抢红包开始时间段
        ///</summary>
        public virtual DateTime RedEnvelopesStartTime
        {
            get{ return _redEnvelopesStartTime;}
            set{ _redEnvelopesStartTime=value;}
        }
        ///<summary>
        ///抢红包结束时间段
        ///</summary>
        protected DateTime _redEnvelopesEndTime;

        /// <summary>
        ///抢红包结束时间段
        ///</summary>
        public virtual DateTime RedEnvelopesEndTime
        {
            get{ return _redEnvelopesEndTime;}
            set{ _redEnvelopesEndTime=value;}
        }
        ///<summary>
        ///当前抢红包时间
        ///</summary>
        protected DateTime _redEnvelopesNowTime;

        /// <summary>
        ///当前抢红包时间
        ///</summary>
        public virtual DateTime RedEnvelopesNowTime
        {
            get{ return _redEnvelopesNowTime;}
            set{ _redEnvelopesNowTime=value;}
        }
        ///<summary>
        ///抢红包收集时间（秒）
        ///</summary>
        protected int _redEnvelopesCollectTime;

        /// <summary>
        ///抢红包收集时间（秒）
        ///</summary>
        public virtual int RedEnvelopesCollectTime
        {
            get{ return _redEnvelopesCollectTime;}
            set{ _redEnvelopesCollectTime=value;}
        }
        ///<summary>
        ///参与抢红包最小发炮量
        ///</summary>
        protected int _redEnvelopesMinBullet;

        /// <summary>
        ///参与抢红包最小发炮量
        ///</summary>
        public virtual int RedEnvelopesMinBullet
        {
            get{ return _redEnvelopesMinBullet;}
            set{ _redEnvelopesMinBullet=value;}
        }
        ///<summary>
        ///红包奖池
        ///</summary>
        protected long _redEnvelopesPool;

        /// <summary>
        ///红包奖池
        ///</summary>
        public virtual long RedEnvelopesPool
        {
            get{ return _redEnvelopesPool;}
            set{ _redEnvelopesPool=value;}
        }
        ///<summary>
        ///红包奖池抽水比例（千分之）
        ///</summary>
        protected int _redEnvelopesPoolRake;

        /// <summary>
        ///红包奖池抽水比例（千分之）
        ///</summary>
        public virtual int RedEnvelopesPoolRake
        {
            get{ return _redEnvelopesPoolRake;}
            set{ _redEnvelopesPoolRake=value;}
        }
        ///<summary>
        ///抢红包数据
        ///</summary>
        protected string _redEnvelopesData;

        /// <summary>
        ///抢红包数据
        ///</summary>
        public virtual string RedEnvelopesData
        {
            get{ return _redEnvelopesData;}
            set{ _redEnvelopesData=value;}
        }





    }
}
