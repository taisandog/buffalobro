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
    public partial class Storage: Buffalo.DB.CommBase.BusinessBases.ThinModelBase
    {
        ///<summary>
        ///主键ID，自动增长
        ///</summary>
        protected int? _id;

        /// <summary>
        ///主键ID，自动增长
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
        ///仓库名称
        ///</summary>
        protected string _storageName;

        /// <summary>
        ///仓库名称
        ///</summary>
        public string StorageName
        {
            get
            {
                return _storageName;
            }
            set
            {
                _storageName=value;
                OnPropertyUpdated("StorageName");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        protected List<SampleGressionRecode> _lstSampleGressionRecodeId_StorageId;

        /// <summary>
        /// 
        /// </summary>
        public List<SampleGressionRecode> LstSampleGressionRecodeId_StorageId
        {
            get
            {
               if (_lstSampleGressionRecodeId_StorageId == null)
               {
                   FillChild("LstSampleGressionRecodeId_StorageId");
               }
               return _lstSampleGressionRecodeId_StorageId;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected List<OutRecode> _lstOutRecodeId_StorageId;

        /// <summary>
        /// 
        /// </summary>
        public List<OutRecode> LstOutRecodeId_StorageId
        {
            get
            {
               if (_lstOutRecodeId_StorageId == null)
               {
                   FillChild("LstOutRecodeId_StorageId");
               }
               return _lstOutRecodeId_StorageId;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected List<GoodsLocation> _lstGoodsLocationId_StorageId;

        /// <summary>
        /// 
        /// </summary>
        public List<GoodsLocation> LstGoodsLocationId_StorageId
        {
            get
            {
               if (_lstGoodsLocationId_StorageId == null)
               {
                   FillChild("LstGoodsLocationId_StorageId");
               }
               return _lstGoodsLocationId_StorageId;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected List<GoodsShelf> _lstGoodsShelfId_StorageId;

        /// <summary>
        /// 
        /// </summary>
        public List<GoodsShelf> LstGoodsShelfId_StorageId
        {
            get
            {
               if (_lstGoodsShelfId_StorageId == null)
               {
                   FillChild("LstGoodsShelfId_StorageId");
               }
               return _lstGoodsShelfId_StorageId;
            }
        }


        private static ModelContext<Storage> _____baseContext=new ModelContext<Storage>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<Storage> GetContext() 
        {
            return _____baseContext;
        }

    }
}
