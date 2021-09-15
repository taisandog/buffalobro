using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;

namespace Buffalo.DBTools.HelperKernel
{
    public class EntityRelationCollection : List<EntityRelationItem>
    {
        public void SortItem()
        {
            this.Sort(new FieldComparer<EntityRelationItem>());
        }

        /// <summary>
        /// ͨ������������ӳ��������Ϣ
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public EntityRelationItem FindByPropertyName(string propertyName)
        {
            foreach (EntityRelationItem rel in this)
            {
                if (rel.PropertyName == propertyName)
                {
                    return rel;
                }
            }
            return null;
        }

    }

    
}
