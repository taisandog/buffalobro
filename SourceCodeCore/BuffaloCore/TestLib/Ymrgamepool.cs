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
        ///����
        ///</summary>
        protected int? _level;

        /// <summary>
        ///����
        ///</summary>
        public virtual int? Level
        {
            get{ return _level;}
            set{ _level=value;}
        }
        ///<summary>
        ///ƫ��ķ���,����ƫ����ٷֿ�ʼ��������
        ///</summary>
        protected int? _offsetScore;

        /// <summary>
        ///ƫ��ķ���,����ƫ����ٷֿ�ʼ��������
        ///</summary>
        public virtual int? OffsetScore
        {
            get{ return _offsetScore;}
            set{ _offsetScore=value;}
        }
        ///<summary>
        ///�������ӷ���
        ///</summary>
        protected long? _poolScore3;

        /// <summary>
        ///�������ӷ���
        ///</summary>
        public virtual long? PoolScore3
        {
            get{ return _poolScore3;}
            set{ _poolScore3=value;}
        }
        ///<summary>
        ///�������ӷ���
        ///</summary>
        protected long? _poolScore2;

        /// <summary>
        ///�������ӷ���
        ///</summary>
        public virtual long? PoolScore2
        {
            get{ return _poolScore2;}
            set{ _poolScore2=value;}
        }
        ///<summary>
        ///�������ӱ���,���֮
        ///</summary>
        protected int? _poolScore2Rake;

        /// <summary>
        ///�������ӱ���,���֮
        ///</summary>
        public virtual int? PoolScore2Rake
        {
            get{ return _poolScore2Rake;}
            set{ _poolScore2Rake=value;}
        }
        ///<summary>
        ///�������ӷ�����ʱ
        ///</summary>
        protected long? _poolScore2Temp;

        /// <summary>
        ///�������ӷ�����ʱ
        ///</summary>
        public virtual long? PoolScore2Temp
        {
            get{ return _poolScore2Temp;}
            set{ _poolScore2Temp=value;}
        }
        ///<summary>
        ///��ǰ��¼�ķ���
        ///</summary>
        protected long? _nowScore;

        /// <summary>
        ///��ǰ��¼�ķ���
        ///</summary>
        public virtual long? NowScore
        {
            get{ return _nowScore;}
            set{ _nowScore=value;}
        }
        ///<summary>
        ///Ҫ��ɵķ���
        ///</summary>
        protected int? _needScore;

        /// <summary>
        ///Ҫ��ɵķ���
        ///</summary>
        public virtual int? NeedScore
        {
            get{ return _needScore;}
            set{ _needScore=value;}
        }
        ///<summary>
        ///��ɵ�Ŀ�����
        ///</summary>
        protected long? _max;

        /// <summary>
        ///��ɵ�Ŀ�����
        ///</summary>
        public virtual long? Max
        {
            get{ return _max;}
            set{ _max=value;}
        }
        ///<summary>
        ///Ҫ��ˮ����
        ///</summary>
        protected int? _taget;

        /// <summary>
        ///Ҫ��ˮ����
        ///</summary>
        public virtual int? Taget
        {
            get{ return _taget;}
            set{ _taget=value;}
        }
        ///<summary>
        ///�ܳ�ɷ�
        ///</summary>
        protected long? _realScore;

        /// <summary>
        ///�ܳ�ɷ�
        ///</summary>
        public virtual long? RealScore
        {
            get{ return _realScore;}
            set{ _realScore=value;}
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
        ///�������ʱ��
        ///</summary>
        protected int? _offlineTime;

        /// <summary>
        ///�������ʱ��
        ///</summary>
        public virtual int? OfflineTime
        {
            get{ return _offlineTime;}
            set{ _offlineTime=value;}
        }
        ///<summary>
        ///ȫ��ը������
        ///</summary>
        protected long? _poolScoreFullScreenBomb;

        /// <summary>
        ///ȫ��ը������
        ///</summary>
        public virtual long? PoolScoreFullScreenBomb
        {
            get{ return _poolScoreFullScreenBomb;}
            set{ _poolScoreFullScreenBomb=value;}
        }
        ///<summary>
        ///ȫ��ը��������ǧ��֮
        ///</summary>
        protected int? _poolScoreFullScreenBombRake;

        /// <summary>
        ///ȫ��ը��������ǧ��֮
        ///</summary>
        public virtual int? PoolScoreFullScreenBombRake
        {
            get{ return _poolScoreFullScreenBombRake;}
            set{ _poolScoreFullScreenBombRake=value;}
        }
        ///<summary>
        ///�����Ը���ֵ(��λ���)
        ///</summary>
        protected int? _maxPay;

        /// <summary>
        ///�����Ը���ֵ(��λ���)
        ///</summary>
        public virtual int? MaxPay
        {
            get{ return _maxPay;}
            set{ _maxPay=value;}
        }
        ///<summary>
        ///�������ʼʱ���
        ///</summary>
        protected DateTime _redEnvelopesStartTime;

        /// <summary>
        ///�������ʼʱ���
        ///</summary>
        public virtual DateTime RedEnvelopesStartTime
        {
            get{ return _redEnvelopesStartTime;}
            set{ _redEnvelopesStartTime=value;}
        }
        ///<summary>
        ///���������ʱ���
        ///</summary>
        protected DateTime _redEnvelopesEndTime;

        /// <summary>
        ///���������ʱ���
        ///</summary>
        public virtual DateTime RedEnvelopesEndTime
        {
            get{ return _redEnvelopesEndTime;}
            set{ _redEnvelopesEndTime=value;}
        }
        ///<summary>
        ///��ǰ�����ʱ��
        ///</summary>
        protected DateTime _redEnvelopesNowTime;

        /// <summary>
        ///��ǰ�����ʱ��
        ///</summary>
        public virtual DateTime RedEnvelopesNowTime
        {
            get{ return _redEnvelopesNowTime;}
            set{ _redEnvelopesNowTime=value;}
        }
        ///<summary>
        ///������ռ�ʱ�䣨�룩
        ///</summary>
        protected int _redEnvelopesCollectTime;

        /// <summary>
        ///������ռ�ʱ�䣨�룩
        ///</summary>
        public virtual int RedEnvelopesCollectTime
        {
            get{ return _redEnvelopesCollectTime;}
            set{ _redEnvelopesCollectTime=value;}
        }
        ///<summary>
        ///�����������С������
        ///</summary>
        protected int _redEnvelopesMinBullet;

        /// <summary>
        ///�����������С������
        ///</summary>
        public virtual int RedEnvelopesMinBullet
        {
            get{ return _redEnvelopesMinBullet;}
            set{ _redEnvelopesMinBullet=value;}
        }
        ///<summary>
        ///�������
        ///</summary>
        protected long _redEnvelopesPool;

        /// <summary>
        ///�������
        ///</summary>
        public virtual long RedEnvelopesPool
        {
            get{ return _redEnvelopesPool;}
            set{ _redEnvelopesPool=value;}
        }
        ///<summary>
        ///������س�ˮ������ǧ��֮��
        ///</summary>
        protected int _redEnvelopesPoolRake;

        /// <summary>
        ///������س�ˮ������ǧ��֮��
        ///</summary>
        public virtual int RedEnvelopesPoolRake
        {
            get{ return _redEnvelopesPoolRake;}
            set{ _redEnvelopesPoolRake=value;}
        }
        ///<summary>
        ///���������
        ///</summary>
        protected string _redEnvelopesData;

        /// <summary>
        ///���������
        ///</summary>
        public virtual string RedEnvelopesData
        {
            get{ return _redEnvelopesData;}
            set{ _redEnvelopesData=value;}
        }





    }
}
