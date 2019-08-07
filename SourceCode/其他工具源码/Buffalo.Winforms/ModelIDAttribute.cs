using System;
using System.Collections.Generic;

using System.Text;


namespace Buffalo.Winforms
{
    /// <summary>
    /// 模块ID标签
    /// </summary>
    public class ModelIDAttribute:Attribute
    {
        private string _modelId;

        /// <summary>
        /// 模块ID
        /// </summary>
        public string ModelId
        {
            get { return _modelId; }
        }

        private string _modelName;

        /// <summary>
        /// 模块名
        /// </summary>
        public string ModelName
        {
            get { return _modelName; }
        }

        private bool _mainModel;

        /// <summary>
        /// 是否主模块
        /// </summary>
        public bool MainModel
        {
            get { return _mainModel; }
        }
        public ModelIDAttribute(string modelId, string modelName,bool mainModel=true) 
        {
            _modelId = modelId;
            _modelName = modelName;
            _mainModel = mainModel;
        }
    }
}
