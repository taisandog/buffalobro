using Buffalo.Winforms;
using System;
using System.Collections.Generic;
using System.Drawing;

using System.Reflection;
using System.Text;


namespace Buffalo.Winforms.UILoaderUnit
{
    /// <summary>
    /// UI跟ID的对比
    /// </summary>
    public class UIMapID
    {
        private Dictionary<string, Type> _dicModel=new Dictionary<string,Type>();

        
        /// <summary>
        /// 添加模块到集合
        /// </summary>
        /// <param name="type"></param>
        public void AddModel(Type type) 
        {
            object[] objArr = type.GetCustomAttributes(typeof(ModelIDAttribute),false);
            if (objArr.Length <= 0) 
            {
                return;
            }
            ModelIDAttribute att = objArr[0] as ModelIDAttribute;
            _dicModel[att.ModelId] = type;
        }
        public UIMapID() 
        {

        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public Type GetModelType(string id) 
        {
            Type t = null;
            if (_dicModel.TryGetValue(id, out t)) 
            {
                return t;
            }
            return null;
        }

        
    }
}
