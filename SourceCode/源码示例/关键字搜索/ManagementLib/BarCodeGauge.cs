using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.CommBase.BusinessBases;
namespace ManagementLib
{
	/// <summary>
    /// 
    /// </summary>
    public partial class BarCodeGauge: Buffalo.DB.CommBase.BusinessBases.ThinModelBase
    {
        ///<summary>
        ///
        ///</summary>
        protected int? _id;

        /// <summary>
        ///
        ///</summary>
        public int? Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id=value;
                OnPropertyUpdated("Id");
            }
        }
        ///<summary>
        ///用户自定义的条码表头
        ///</summary>
        protected string _customCode;

        /// <summary>
        ///用户自定义的条码表头
        ///</summary>
        public string CustomCode
        {
            get
            {
                return _customCode;
            }
            set
            {
                _customCode=value;
                OnPropertyUpdated("CustomCode");
            }
        }
        ///<summary>
        ///是否有效
        ///</summary>
        protected bool? _state;

        /// <summary>
        ///是否有效
        ///</summary>
        public bool? State
        {
            get
            {
                return _state;
            }
            set
            {
                _state=value;
                OnPropertyUpdated("State");
            }
        }
        ///<summary>
        ///最大流水号
        ///</summary>
        protected int? _maxSerialnumber;

        /// <summary>
        ///最大流水号
        ///</summary>
        public int? MaxSerialnumber
        {
            get
            {
                return _maxSerialnumber;
            }
            set
            {
                _maxSerialnumber=value;
                OnPropertyUpdated("MaxSerialnumber");
            }
        }




        private static ModelContext<BarCodeGauge> _____baseContext=new ModelContext<BarCodeGauge>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<BarCodeGauge> GetContext() 
        {
            return _____baseContext;
        }

    }
}
