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
        ///��ID
        ///</summary>
        protected int? _fishId;

        /// <summary>
        ///��ID
        ///</summary>
        public virtual int? FishId
        {
            get{ return _fishId;}
            set{ _fishId=value;}
        }
        ///<summary>
        ///��С���ڼ�����룩
        ///</summary>
        protected float? _minShootInterval;

        /// <summary>
        ///��С���ڼ�����룩
        ///</summary>
        public virtual float? MinShootInterval
        {
            get{ return _minShootInterval;}
            set{ _minShootInterval=value;}
        }
        ///<summary>
        ///����ڼ�����룩
        ///</summary>
        protected float? _maxShootInterval;

        /// <summary>
        ///����ڼ�����룩
        ///</summary>
        public virtual float? MaxShootInterval
        {
            get{ return _maxShootInterval;}
            set{ _maxShootInterval=value;}
        }
        ///<summary>
        ///�Ƿ���Դ�������
        ///</summary>
        protected bool? _canShoot;

        /// <summary>
        ///�Ƿ���Դ�������
        ///</summary>
        public virtual bool? CanShoot
        {
            get{ return _canShoot;}
            set{ _canShoot=value;}
        }
        ///<summary>
        ///�Ƿ������ȼ����ߵ���
        ///</summary>
        protected bool? _isFindHigherLevel;

        /// <summary>
        ///�Ƿ������ȼ����ߵ���
        ///</summary>
        public virtual bool? IsFindHigherLevel
        {
            get{ return _isFindHigherLevel;}
            set{ _isFindHigherLevel=value;}
        }
        ///<summary>
        ///���ȼ���ֵԽ�����ȼ�Խ�ߣ�
        ///</summary>
        protected int? _priority;

        /// <summary>
        ///���ȼ���ֵԽ�����ȼ�Խ�ߣ�
        ///</summary>
        public virtual int? Priority
        {
            get{ return _priority;}
            set{ _priority=value;}
        }
        ///<summary>
        ///�Ƿ�ģ����
        ///</summary>
        protected bool? _canImitatePeople;

        /// <summary>
        ///�Ƿ�ģ����
        ///</summary>
        public virtual bool? CanImitatePeople
        {
            get{ return _canImitatePeople;}
            set{ _canImitatePeople=value;}
        }
        ///<summary>
        ///��Ӧ�Ļ���������ID
        ///</summary>
        protected int? _robotConfigId;

        /// <summary>
        ///��Ӧ�Ļ���������ID
        ///</summary>
        public virtual int? RobotConfigId
        {
            get{ return _robotConfigId;}
            set{ _robotConfigId=value;}
        }





    }
}
