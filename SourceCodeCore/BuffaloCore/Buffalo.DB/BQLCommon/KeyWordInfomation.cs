using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.IdentityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.DataAccessBases.AliasTableMappingManagers;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public class KeyWordInfomation : ICloneable
    {
        public KeyWordInfomation()
        {

        }


        //private QueryParamCollection queryParams = new QueryParamCollection();
        //private AliasCollection alias = new AliasCollection();
        private List<IdentityInfo> _identityInfos = new List<IdentityInfo>();


        private bool _hasGroup = false;

        /// <summary>
        /// �Ƿ��оۺ����
        /// </summary>
        public bool HasGroup
        {
            get
            {
                return _hasGroup;
            }
            set
            {
                _hasGroup = value;
            }
        }

        private bool _showTableName=true;

        /// <summary>
        /// ��ǰ����Ƿ���ʾ����
        /// </summary>
        public bool ShowTableName
        {
            get { return _showTableName; }
            set { _showTableName = value; }
        }

        private int _primaryKeys;


        /// <summary>
        /// ������
        /// </summary>
        public int PrimaryKeys
        {
            get { return _primaryKeys; }
            set { _primaryKeys = value; }
        }
        private bool _isWhere = false;

        /// <summary>
        /// �Ƿ������������
        /// </summary>
        public bool IsWhere
        {
            get { return _isWhere; }
            set { _isWhere = value; }
        }
        private AbsCondition _condition = null;
        /// <summary>
        /// �����������Ϣ 
        /// </summary>
        public AbsCondition Condition
        {
            set { _condition = value; }
            get { return _condition; }
        }

        private BQLInfos _infos = null;
        /// <summary>
        /// �������
        /// </summary>
        public BQLInfos Infos
        {
            get { return _infos; }
            set { _infos = value; }
        }

        private DBInfo _dbInfo;
        /// <summary>
        /// ���ݿ���Ϣ
        /// </summary>
        public DBInfo DBInfo
        {
            get { return _dbInfo; }
            set { _dbInfo = value; }
        }
        private BQLEntityTableHandle _fromTable;
        /// <summary>
        /// From�ĵ�һ����
        /// </summary>
        public BQLEntityTableHandle FromTable
        {
            get { return _fromTable; }
            set { _fromTable = value; }
        }
        ///// <summary>
        ///// ��ѯ��ʾ�����Ժ��ֶεĶ�Ӧ��
        ///// </summary>
        //internal QueryParamCollection QueryParams 
        //{
        //    get 
        //    {
        //        return queryParams;
        //    }
        //}

        ///// <summary>
        ///// �������������
        ///// </summary>
        //internal AliasCollection Alias
        //{
        //    get
        //    {
        //        return alias;
        //    }
        //}

        private Dictionary<string, bool> _containTables=new Dictionary<string,bool>();
        /// <summary>
        /// �����ı�
        /// </summary>
        public Dictionary<string, bool> ContainTables
        {
            get { return _containTables; }
        }

        protected ParamList _paramList = null;
        public ParamList ParamList
        {
            get
            {
                return _paramList;
            }
            set
            {
                _paramList = value;
            }
        }

        /// <summary>
        /// �Զ������ļ���
        /// </summary>
        internal List<IdentityInfo> IdentityInfos
        {
            get
            {
                return _identityInfos;
            }
        }

        private TableAliasNameManager _aliasManager;

        /// <summary>
        /// ��ӳ�������
        /// </summary>
        internal TableAliasNameManager AliasManager
        {
            get
            {
                return _aliasManager;
            }
            set
            {
                _aliasManager = value;
            }
        }


        private bool _outPutModle = false;
        /// <summary>
        /// �Ƿ����SQL���ģʽ
        /// </summary>
        public bool OutPutModle
        {
            get { return _outPutModle; }
            set { _outPutModle = value; }
        }

        #region ICloneable ��Ա

        public object Clone()
        {
            KeyWordInfomation info = new KeyWordInfomation();
            info._infos = this._infos;
            //info._isPutPropertyName = this._isPutPropertyName;
            //info._isPage = this._isShowTableName;
            info._paramList = _paramList;
            info._dbInfo = this._dbInfo;
            info.OutPutModle = this.OutPutModle;
            return info;
        }

        #endregion
    }
}
