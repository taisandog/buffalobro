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
        /// 通过属性名查找映射属性信息
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
