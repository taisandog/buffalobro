using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Buffalo.DB.PropertyAttributes;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// ӳ����Ϣ
    /// </summary>
    public class EntityRelationItem : EntityFieldBase
    {
        private string _sourceProperty;
        private string _targetProperty;
        private bool _isToDB;
        /// <summary>
        /// �Ƿ���������
        /// </summary>
        public bool IsParent
        {
            get { return GetIsParent(_fInfo); }
        }
        /// <summary>
        /// ���ɵ����ݿ�
        /// </summary>
        public bool IsToDB
        {
            get { return _isToDB; }
            set { _isToDB = value; }
        }
        /// <summary>
        /// Ŀ������
        /// </summary>
        public string TargetProperty
        {
            get { return _targetProperty; }
            set { _targetProperty = value; }
        }
        /// <summary>
        /// Դ����
        /// </summary>
        public string SourceProperty
        {
            get { return _sourceProperty; }
            set { _sourceProperty = value; }
        }


        

        

        private List<string> _targetPropertyList;

        /// <summary>
        /// ��ȡĿ��ʵ�����Ϣ����
        /// </summary>
        public List<string> TargetPropertyList
        {
            get 
            {
                if (_targetPropertyList == null)
                {
                    if (FInfo.MemberType == null) 
                    {
                        _targetPropertyList = new List<string>();
                        return _targetPropertyList;
                    }
                    List<ClrProperty> lstPropertys = EntityConfig.GetAllMember<ClrProperty>(FInfo.MemberType, true);
                    _targetPropertyList = new List<string>(lstPropertys.Count);
                    foreach (ClrProperty pro in lstPropertys)
                    {
                        _targetPropertyList.Add(pro.Name);
                    }
                }
                return _targetPropertyList;
            }
        }

        public TableRelationAttribute GetRelationInfo()
        {
            TableRelationAttribute tr = new TableRelationAttribute();
            tr.Description = Summary;
            tr.FieldName = FieldName;
            tr.FieldTypeName = FInfo.MemberTypeShortName;
            tr.IsParent = IsParent;
            tr.PropertyName = PropertyName;
            tr.SourceName = SourceProperty;
            tr.SourceTable = BelongEntity.TableName;
            tr.TargetName = TargetProperty;
            tr.IsParent = IsParent;
            return tr;
        }

        /// <summary>
        /// ʵ��ӳ��
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="fInfo"></param>
        /// <param name="belongEntity"></param>
        public EntityRelationItem(CodeElementPosition cp, ClrField fInfo, EntityConfig belongEntity) 
        {
            _cp = cp;
            _fInfo = fInfo;
            //_isParent = GetIsParent(fInfo);
            GetInfo(fInfo);
            _belongEntity = belongEntity;
        }

        /// <summary>
        /// ��ȡ���ֶ��Ƿ���������
        /// </summary>
        /// <param name="fInfo"></param>
        /// <returns></returns>
        private static bool GetIsParent(ClrField fInfo)
        {
            if (fInfo.MemberType!=null && fInfo.MemberType.Generic) 
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// ��ȡ�ֶε�������Ϣ
        /// </summary>
        /// <param name="fInfo"></param>
        private void GetInfo(ClrField fInfo)
        {
            _propertyName = ToPascalName(FieldName);
        }

        /// <summary>
        /// ��ӵ�Դ��
        /// </summary>
        /// <param name="source">Դ���б�</param>
        /// <param name="spaces">�ո�</param>
        public void AddSource(List<string> source, string spaces)
        {

            //���ɶ�Ӧ����
            if (!_belongEntity.Properties.ContainsKey(PropertyName))
            {
                if (IsParent)
                {
                    source.Add(spaces + "/// <summary>");
                    source.Add(spaces + "/// " + Summary);
                    source.Add(spaces + "/// </summary>");
                    string propertyText = spaces + "public virtual " + TypeName + " " + PropertyName;
                    source.Add(propertyText);
                    source.Add(spaces + "{");
                    source.Add(spaces + "    get{ return " + FieldName + "; }");
                    source.Add(spaces + "    set{ " + FieldName + " = value; }");
                    source.Add(spaces + "}");

                }
                else 
                {
                    source.Add(spaces + "/// <summary>");
                    source.Add(spaces + "/// " + Summary);
                    source.Add(spaces + "/// </summary>");
                    string propertyText = spaces + "public virtual " + TypeName + " " + PropertyName;
                    source.Add(propertyText);
                    source.Add(spaces + "{");
                    source.Add(spaces + "    get{ return " + FieldName + "; }");
                    source.Add(spaces + "    set{ " + FieldName + " = value; }");
                    source.Add(spaces + "}");
                }
            }
        }


    }
}
