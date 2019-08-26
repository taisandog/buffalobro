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
        ///�Ƿ����
        ///</summary>
        protected bool? _enable;

        /// <summary>
        ///�Ƿ����
        ///</summary>
        public virtual bool? Enable
        {
            get{ return _enable;}
            set{ _enable=value;}
        }
        ///<summary>
        ///�Ƿ���Խ���һ����
        ///</summary>
        protected bool? _joinRoomZero;

        /// <summary>
        ///�Ƿ���Խ���һ����
        ///</summary>
        public virtual bool? JoinRoomZero
        {
            get{ return _joinRoomZero;}
            set{ _joinRoomZero=value;}
        }
        ///<summary>
        ///�Ƿ���Խ��ڶ�����
        ///</summary>
        protected bool? _joinRoomOne;

        /// <summary>
        ///�Ƿ���Խ��ڶ�����
        ///</summary>
        public virtual bool? JoinRoomOne
        {
            get{ return _joinRoomOne;}
            set{ _joinRoomOne=value;}
        }
        ///<summary>
        ///�Ƿ���Խ���������
        ///</summary>
        protected bool? _joinRoomTwo;

        /// <summary>
        ///�Ƿ���Խ���������
        ///</summary>
        public virtual bool? JoinRoomTwo
        {
            get{ return _joinRoomTwo;}
            set{ _joinRoomTwo=value;}
        }
        ///<summary>
        ///�Ƿ���Խ����ĸ���
        ///</summary>
        protected bool? _joinRoomThree;

        /// <summary>
        ///�Ƿ���Խ����ĸ���
        ///</summary>
        public virtual bool? JoinRoomThree
        {
            get{ return _joinRoomThree;}
            set{ _joinRoomThree=value;}
        }
        ///<summary>
        ///��С��Ϸʱ�䣨���ӣ�
        ///</summary>
        protected int? _minPlayGameTime;

        /// <summary>
        ///��С��Ϸʱ�䣨���ӣ�
        ///</summary>
        public virtual int? MinPlayGameTime
        {
            get{ return _minPlayGameTime;}
            set{ _minPlayGameTime=value;}
        }
        ///<summary>
        ///�����Ϸʱ�䣨���ӣ�
        ///</summary>
        protected int? _maxPlayGameTime;

        /// <summary>
        ///�����Ϸʱ�䣨���ӣ�
        ///</summary>
        public virtual int? MaxPlayGameTime
        {
            get{ return _maxPlayGameTime;}
            set{ _maxPlayGameTime=value;}
        }
        ///<summary>
        ///���������Сʱ�䣨�룩
        ///</summary>
        protected int? _randomLockMinTime;

        /// <summary>
        ///���������Сʱ�䣨�룩
        ///</summary>
        public virtual int? RandomLockMinTime
        {
            get{ return _randomLockMinTime;}
            set{ _randomLockMinTime=value;}
        }
        ///<summary>
        ///����������ʱ�䣨�룩
        ///</summary>
        protected int? _randomLockMaxTime;

        /// <summary>
        ///����������ʱ�䣨�룩
        ///</summary>
        public virtual int? RandomLockMaxTime
        {
            get{ return _randomLockMaxTime;}
            set{ _randomLockMaxTime=value;}
        }
        ///<summary>
        ///���ģʽ��Сʱ�䣨�룩
        ///</summary>
        protected int? _randomPatternMinTime;

        /// <summary>
        ///���ģʽ��Сʱ�䣨�룩
        ///</summary>
        public virtual int? RandomPatternMinTime
        {
            get{ return _randomPatternMinTime;}
            set{ _randomPatternMinTime=value;}
        }
        ///<summary>
        ///���ģʽ���ʱ�䣨�룩
        ///</summary>
        protected int? _randomPatternMaxTime;

        /// <summary>
        ///���ģʽ���ʱ�䣨�룩
        ///</summary>
        public virtual int? RandomPatternMaxTime
        {
            get{ return _randomPatternMaxTime;}
            set{ _randomPatternMaxTime=value;}
        }
        ///<summary>
        ///�Ƿ��ȦȦ��
        ///</summary>
        protected bool? _canShootCircleFish;

        /// <summary>
        ///�Ƿ��ȦȦ��
        ///</summary>
        public virtual bool? CanShootCircleFish
        {
            get{ return _canShootCircleFish;}
            set{ _canShootCircleFish=value;}
        }
        ///<summary>
        ///ֹͣ������С������룩
        ///</summary>
        protected int? _stopShootMinInterval;

        /// <summary>
        ///ֹͣ������С������룩
        ///</summary>
        public virtual int? StopShootMinInterval
        {
            get{ return _stopShootMinInterval;}
            set{ _stopShootMinInterval=value;}
        }
        ///<summary>
        ///ֹͣ������������룩
        ///</summary>
        protected int? _stopShootMaxInterval;

        /// <summary>
        ///ֹͣ������������룩
        ///</summary>
        public virtual int? StopShootMaxInterval
        {
            get{ return _stopShootMaxInterval;}
            set{ _stopShootMaxInterval=value;}
        }
        ///<summary>
        ///ֹͣ������Сʱ�䣨�룩
        ///</summary>
        protected int? _stopShootMinTime;

        /// <summary>
        ///ֹͣ������Сʱ�䣨�룩
        ///</summary>
        public virtual int? StopShootMinTime
        {
            get{ return _stopShootMinTime;}
            set{ _stopShootMinTime=value;}
        }
        ///<summary>
        ///ֹͣ�������ʱ�䣨�룩
        ///</summary>
        protected int? _stopShootMaxTime;

        /// <summary>
        ///ֹͣ�������ʱ�䣨�룩
        ///</summary>
        public virtual int? StopShootMaxTime
        {
            get{ return _stopShootMaxTime;}
            set{ _stopShootMaxTime=value;}
        }
        ///<summary>
        ///�����Ч�����Χ�����أ�
        ///</summary>
        protected int? _straightLineMaxValidShootScope;

        /// <summary>
        ///�����Ч�����Χ�����أ�
        ///</summary>
        public virtual int? StraightLineMaxValidShootScope
        {
            get{ return _straightLineMaxValidShootScope;}
            set{ _straightLineMaxValidShootScope=value;}
        }
        ///<summary>
        ///������ڼ����Сʱ�䣨�룩
        ///</summary>
        protected float? _randomShootMinTime;

        /// <summary>
        ///������ڼ����Сʱ�䣨�룩
        ///</summary>
        public virtual float? RandomShootMinTime
        {
            get{ return _randomShootMinTime;}
            set{ _randomShootMinTime=value;}
        }
        ///<summary>
        ///������ڼ�����ʱ�䣨�룩
        ///</summary>
        protected float? _randomShootMaxTime;

        /// <summary>
        ///������ڼ�����ʱ�䣨�룩
        ///</summary>
        public virtual float? RandomShootMaxTime
        {
            get{ return _randomShootMaxTime;}
            set{ _randomShootMaxTime=value;}
        }
        ///<summary>
        ///ѡ�и����õĸ���
        ///</summary>
        protected int? _probability;

        /// <summary>
        ///ѡ�и����õĸ���
        ///</summary>
        public virtual int? Probability
        {
            get{ return _probability;}
            set{ _probability=value;}
        }
        ///<summary>
        ///��ȦȦ����С���ڼ�����룩
        ///</summary>
        protected float? _shootCircleMinInterval;

        /// <summary>
        ///��ȦȦ����С���ڼ�����룩
        ///</summary>
        public virtual float? ShootCircleMinInterval
        {
            get{ return _shootCircleMinInterval;}
            set{ _shootCircleMinInterval=value;}
        }
        ///<summary>
        ///��ȦȦ������ڼ�����룩
        ///</summary>
        protected float? _shootCircleMaxInterval;

        /// <summary>
        ///��ȦȦ������ڼ�����룩
        ///</summary>
        public virtual float? ShootCircleMaxInterval
        {
            get{ return _shootCircleMaxInterval;}
            set{ _shootCircleMaxInterval=value;}
        }
        ///<summary>
        ///ȦȦ�����ȼ�
        ///</summary>
        protected int? _shootCirclePriority;

        /// <summary>
        ///ȦȦ�����ȼ�
        ///</summary>
        public virtual int? ShootCirclePriority
        {
            get{ return _shootCirclePriority;}
            set{ _shootCirclePriority=value;}
        }
        ///<summary>
        ///��ȦȦ���ʱ���Ƿ��Ҹ������ȼ�����
        ///</summary>
        protected bool? _circleIsFindHigherLevel;

        /// <summary>
        ///��ȦȦ���ʱ���Ƿ��Ҹ������ȼ�����
        ///</summary>
        public virtual bool? CircleIsFindHigherLevel
        {
            get{ return _circleIsFindHigherLevel;}
            set{ _circleIsFindHigherLevel=value;}
        }
        ///<summary>
        ///��ȦȦ���ʱ���Ƿ�ģ����
        ///</summary>
        protected bool? _circleIsCanImitatePeople;

        /// <summary>
        ///��ȦȦ���ʱ���Ƿ�ģ����
        ///</summary>
        public virtual bool? CircleIsCanImitatePeople
        {
            get{ return _circleIsCanImitatePeople;}
            set{ _circleIsCanImitatePeople=value;}
        }
        ///<summary>
        ///����ģʽ
        ///</summary>
        protected int? _lockState;

        /// <summary>
        ///����ģʽ
        ///</summary>
        public virtual int? LockState
        {
            get{ return _lockState;}
            set{ _lockState=value;}
        }
        ///<summary>
        ///���ģʽ
        ///</summary>
        protected int? _shootState;

        /// <summary>
        ///���ģʽ
        ///</summary>
        public virtual int? ShootState
        {
            get{ return _shootState;}
            set{ _shootState=value;}
        }





    }
}
