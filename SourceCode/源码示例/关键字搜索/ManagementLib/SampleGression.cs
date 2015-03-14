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
    public partial class SampleGression: Buffalo.DB.CommBase.BusinessBases.ThinModelBase
    {
        ///<summary>
        ///自动增长的ID
        ///</summary>
        protected int? _id;

        /// <summary>
        ///自动增长的ID
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
        ///
        ///</summary>
        protected string _sampleCode;

        /// <summary>
        ///
        ///</summary>
        public string SampleCode
        {
            get
            {
                return _sampleCode;
            }
            set
            {
                _sampleCode=value;
                OnPropertyUpdated("SampleCode");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected int? _storageId;

        /// <summary>
        ///
        ///</summary>
        public int? StorageId
        {
            get
            {
                return _storageId;
            }
            set
            {
                _storageId=value;
                OnPropertyUpdated("StorageId");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected int? _goodShelfId;

        /// <summary>
        ///
        ///</summary>
        public int? GoodShelfId
        {
            get
            {
                return _goodShelfId;
            }
            set
            {
                _goodShelfId=value;
                OnPropertyUpdated("GoodShelfId");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected int? _goodLocationId;

        /// <summary>
        ///
        ///</summary>
        public int? GoodLocationId
        {
            get
            {
                return _goodLocationId;
            }
            set
            {
                _goodLocationId=value;
                OnPropertyUpdated("GoodLocationId");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        protected GoodsLocation _belongGoodsLocation_GoodLocationId_Id;

        /// <summary>
        /// 
        /// </summary>
        public GoodsLocation BelongGoodsLocation_GoodLocationId_Id
        {
            get
            {
               if (_belongGoodsLocation_GoodLocationId_Id == null)
               {
                   FillParent("BelongGoodsLocation_GoodLocationId_Id");
               }
               return _belongGoodsLocation_GoodLocationId_Id;
            }
            set
            {
               _belongGoodsLocation_GoodLocationId_Id = value;
               OnPropertyUpdated("BelongGoodsLocation_GoodLocationId_Id");
            }
        }


        private static ModelContext<SampleGression> _____baseContext=new ModelContext<SampleGression>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<SampleGression> GetContext() 
        {
            return _____baseContext;
        }

    }
}
