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
        ///��С����������ʱ�䣨�룩
        ///</summary>
        protected int? _minJoinExitDeskTime;

        /// <summary>
        ///��С����������ʱ�䣨�룩
        ///</summary>
        public virtual int? MinJoinExitDeskTime
        {
            get{ return _minJoinExitDeskTime;}
            set{ _minJoinExitDeskTime=value;}
        }
        ///<summary>
        ///������������ʱ�䣨�룩
        ///</summary>
        protected int? _maxJoinExitDeskTime;

        /// <summary>
        ///������������ʱ�䣨�룩
        ///</summary>
        public virtual int? MaxJoinExitDeskTime
        {
            get{ return _maxJoinExitDeskTime;}
            set{ _maxJoinExitDeskTime=value;}
        }
        ///<summary>
        ///����ȼ�
        ///</summary>
        protected int? _roomLevel;

        /// <summary>
        ///����ȼ�
        ///</summary>
        public virtual int? RoomLevel
        {
            get{ return _roomLevel;}
            set{ _roomLevel=value;}
        }
        ///<summary>
        ///ÿ����������������
        ///</summary>
        protected int? _roomMaxRobotCount;

        /// <summary>
        ///ÿ����������������
        ///</summary>
        public virtual int? RoomMaxRobotCount
        {
            get{ return _roomMaxRobotCount;}
            set{ _roomMaxRobotCount=value;}
        }
        ///<summary>
        ///������Ա���������������
        ///</summary>
        protected int? _deskMaxRobotCount;

        /// <summary>
        ///������Ա���������������
        ///</summary>
        public virtual int? DeskMaxRobotCount
        {
            get{ return _deskMaxRobotCount;}
            set{ _deskMaxRobotCount=value;}
        }
        ///<summary>
        ///���뷿����С��Ԫ��
        ///</summary>
        protected int? _joinRoomMinMoney;

        /// <summary>
        ///���뷿����С��Ԫ��
        ///</summary>
        public virtual int? JoinRoomMinMoney
        {
            get{ return _joinRoomMinMoney;}
            set{ _joinRoomMinMoney=value;}
        }
        ///<summary>
        ///���뷿�������Ԫ��
        ///</summary>
        protected int? _joinRoomMaxMoney;

        /// <summary>
        ///���뷿�������Ԫ��
        ///</summary>
        public virtual int? JoinRoomMaxMoney
        {
            get{ return _joinRoomMaxMoney;}
            set{ _joinRoomMaxMoney=value;}
        }
        ///<summary>
        ///�����С���ʣ���ұ���Ϊ����ڽ����֣�
        ///</summary>
        protected int? _storageMinMultiple;

        /// <summary>
        ///�����С���ʣ���ұ���Ϊ����ڽ����֣�
        ///</summary>
        public virtual int? StorageMinMultiple
        {
            get{ return _storageMinMultiple;}
            set{ _storageMinMultiple=value;}
        }
        ///<summary>
        ///�������ʣ���ұ���Ϊ����ڽ����֣�
        ///</summary>
        protected int? _storageMaxMultiple;

        /// <summary>
        ///�������ʣ���ұ���Ϊ����ڽ����֣�
        ///</summary>
        public virtual int? StorageMaxMultiple
        {
            get{ return _storageMaxMultiple;}
            set{ _storageMaxMultiple=value;}
        }
        ///<summary>
        ///ǿ���˳�Ӯ����С����
        ///</summary>
        protected int? _compelExitMinWinMultiple;

        /// <summary>
        ///ǿ���˳�Ӯ����С����
        ///</summary>
        public virtual int? CompelExitMinWinMultiple
        {
            get{ return _compelExitMinWinMultiple;}
            set{ _compelExitMinWinMultiple=value;}
        }
        ///<summary>
        ///ǿ���˳�Ӯ�������
        ///</summary>
        protected int? _compelExitMaxWinMultiple;

        /// <summary>
        ///ǿ���˳�Ӯ�������
        ///</summary>
        public virtual int? CompelExitMaxWinMultiple
        {
            get{ return _compelExitMaxWinMultiple;}
            set{ _compelExitMaxWinMultiple=value;}
        }
        ///<summary>
        ///���������С����
        ///</summary>
        protected int? _withdrawalsMinCount;

        /// <summary>
        ///���������С����
        ///</summary>
        public virtual int? WithdrawalsMinCount
        {
            get{ return _withdrawalsMinCount;}
            set{ _withdrawalsMinCount=value;}
        }
        ///<summary>
        ///�������������
        ///</summary>
        protected int? _withdrawalsMaxCount;

        /// <summary>
        ///�������������
        ///</summary>
        public virtual int? WithdrawalsMaxCount
        {
            get{ return _withdrawalsMaxCount;}
            set{ _withdrawalsMaxCount=value;}
        }
        ///<summary>
        ///��ѯ���뷿����Сʱ�䣨���ӣ�
        ///</summary>
        protected int? _pollingJoinRoomMinTime;

        /// <summary>
        ///��ѯ���뷿����Сʱ�䣨���ӣ�
        ///</summary>
        public virtual int? PollingJoinRoomMinTime
        {
            get{ return _pollingJoinRoomMinTime;}
            set{ _pollingJoinRoomMinTime=value;}
        }
        ///<summary>
        ///��ѯ���뷿�����ʱ�䣨���ӣ�
        ///</summary>
        protected int? _pollingJoinRoomMaxTime;

        /// <summary>
        ///��ѯ���뷿�����ʱ�䣨���ӣ�
        ///</summary>
        public virtual int? PollingJoinRoomMaxTime
        {
            get{ return _pollingJoinRoomMaxTime;}
            set{ _pollingJoinRoomMaxTime=value;}
        }
        ///<summary>
        ///ͨ����Сͣ��ʱ�䣨�룬��Ҫ���ڴ�ȡ�Һ�ͣ��÷��ڣ��˳�����ǰͣ����ã�
        ///</summary>
        protected int? _commonMinStayTime;

        /// <summary>
        ///ͨ����Сͣ��ʱ�䣨�룬��Ҫ���ڴ�ȡ�Һ�ͣ��÷��ڣ��˳�����ǰͣ����ã�
        ///</summary>
        public virtual int? CommonMinStayTime
        {
            get{ return _commonMinStayTime;}
            set{ _commonMinStayTime=value;}
        }
        ///<summary>
        ///ͨ�����ͣ��ʱ�䣨�룬��Ҫ���ڴ�ȡ�Һ�ͣ��÷��ڣ��˳�����ǰͣ����ã�
        ///</summary>
        protected int? _commonMaxStayTime;

        /// <summary>
        ///ͨ�����ͣ��ʱ�䣨�룬��Ҫ���ڴ�ȡ�Һ�ͣ��÷��ڣ��˳�����ǰͣ����ã�
        ///</summary>
        public virtual int? CommonMaxStayTime
        {
            get{ return _commonMaxStayTime;}
            set{ _commonMaxStayTime=value;}
        }





    }
}
