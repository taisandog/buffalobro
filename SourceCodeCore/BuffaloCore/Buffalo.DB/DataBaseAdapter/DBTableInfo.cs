using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.PropertyAttributes;
using Buffalo.Kernel;

namespace Buffalo.DB.DataBaseAdapter
{
    /// <summary>
    /// ���ݿ�ı���Ϣ
    /// </summary>
    public class DBTableInfo
    {
        private List<EntityParam> _primaryParam;

        /// <summary>
        /// ����
        /// </summary>
        public List<EntityParam> PrimaryParam
        {
            get
            {
                if (_primaryParam == null)
                {
                    _primaryParam = new List<EntityParam>();
                    int pkValue = (int)EntityPropertyType.PrimaryKey;
                    foreach (EntityParam prm in _tparams)
                    {
                        if (EnumUnit.ContainerValue((int)prm.PropertyType, pkValue))
                        {
                            _primaryParam.Add(prm);
                            
                        }
                    }
                }
                return _primaryParam;
            }

        }


        private string _name;

        /// <summary>
        /// ������
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private bool _isView;

        /// <summary>
        /// �Ƿ���ͼ
        /// </summary>
        public bool IsView
        {
            get { return _isView; }
            set { _isView = value; }
        }
        private string _description;

        /// <summary>
        /// ��ע��
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        private List<TableRelationAttribute> _relationItems;

        /// <summary>
        /// ��ϵ����
        /// </summary>
        public List<TableRelationAttribute> RelationItems
        {
            get { return _relationItems; }
            set { _relationItems = value; }
        }
        protected List<EntityParam> _tparams;
        /// <summary>
        /// �ֶ�
        /// </summary>
        public List<EntityParam> Params
        {
            get { return _tparams; }
            set { _tparams = value; }
        }
        private bool _isGenerate;
        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public bool IsGenerate
        {
            get { return _isGenerate; }
            set { _isGenerate = value; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string ObjectTypeName 
        {
            get 
            {
                return _isView ? "��ͼ" : "��";
            }
        }
    }
}
