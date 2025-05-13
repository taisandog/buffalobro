using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;

namespace Buffalo.DBTools.HelperKernel
{
    public class EntityParamFieldCollection : List<EntityParamField>
    {
        public void SortItem() 
        {
            this.Sort(new FieldComparer<EntityParamField>());
        }

        /// <summary>
        /// 通过属性名查找字段属性信息
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public EntityParamField FindByPropertyName(string propertyName) 
        {
            foreach (EntityParamField prm in this) 
            {
                if (prm.PropertyName == propertyName) 
                {
                    return prm;
                }
            }
            return null;
        }
    }
    
}
