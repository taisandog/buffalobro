using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buffalo.Kernel.FastReflection;

namespace Buffalo.Kernel.UpdateModelUnit
{
    /// <summary>
    /// 格式化类型的方法
    /// </summary>
    /// <param name="handle">属性的信息</param>
    /// <param name="entity">所属实体</param>
    /// <param name="value">新值</param>
    /// <returns></returns>
    public delegate object DelUpdateModelFormatValue(PropertyInfoHandle handle, object entity, object value);
    /// <summary>
    /// 更新实体工具
    /// </summary>
    public class UpdateUnit
    {

        /// <summary>
        /// 填充实体
        /// </summary>
        /// <param name="dic">字典</param>
        /// <param name="model">实体</param>
        /// <param name="type">字典中的键类型</param>
        /// <param name="formatHandle">格式化信息</param>
        public static void UpdateModel(IDictionary<string, object> dic,
            object model, PrefixType type, DelUpdateModelFormatValue formatHandle) 
        {
            Type objType = model.GetType();

            foreach (KeyValuePair<string, object> kvp in dic) 
            {
                string key=kvp.Key;
                string pName=UpdateModelInfo.GetKey(key,type);
                PropertyInfoHandle handle=FastValueGetSet.GetPropertyInfoHandle(pName,objType);
                if (handle != null && handle.HasSetHandle) 
                {
                    object value=kvp.Value;
                    if (formatHandle != null)
                    {
                        value = formatHandle(handle, model, value);
                    }
                    else if (value.GetType() != handle.PropertyType) 
                    {
                        value = Convert.ChangeType(value, handle.PropertyType);
                    }
                    handle.SetValue(model, value);
                }
            }
        }
         /// <summary>
        /// 填充实体
        /// </summary>
        /// <param name="dic">字典</param>
        /// <param name="model">实体</param>
        /// <param name="type">字典中的键类型</param>
        public static void UpdateModel(IDictionary<string, object> dic,
            object model, PrefixType type)
        {
            UpdateModel(dic, model, type,null);
        }

    }
}
