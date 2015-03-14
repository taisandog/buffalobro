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
    public partial class View_InSorageRecode: Buffalo.DB.CommBase.BusinessBases.ThinModelBase
    {
        ///<summary>
        ///
        ///</summary>
        protected int? _state;

        /// <summary>
        ///
        ///</summary>
        public int? State
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
        ///
        ///</summary>
        protected string _storageName;

        /// <summary>
        ///
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
        ///<summary>
        ///
        ///</summary>
        protected string _goodShelf;

        /// <summary>
        ///
        ///</summary>
        public string GoodShelf
        {
            get
            {
                return _goodShelf;
            }
            set
            {
                _goodShelf=value;
                OnPropertyUpdated("GoodShelf");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _locationCode;

        /// <summary>
        ///
        ///</summary>
        public string LocationCode
        {
            get
            {
                return _locationCode;
            }
            set
            {
                _locationCode=value;
                OnPropertyUpdated("LocationCode");
            }
        }
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
        ///
        ///</summary>
        protected string _projectName;

        /// <summary>
        ///
        ///</summary>
        public string ProjectName
        {
            get
            {
                return _projectName;
            }
            set
            {
                _projectName=value;
                OnPropertyUpdated("ProjectName");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _projectId;

        /// <summary>
        ///
        ///</summary>
        public string ProjectId
        {
            get
            {
                return _projectId;
            }
            set
            {
                _projectId=value;
                OnPropertyUpdated("ProjectId");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _crudeSample;

        /// <summary>
        ///
        ///</summary>
        public string CrudeSample
        {
            get
            {
                return _crudeSample;
            }
            set
            {
                _crudeSample=value;
                OnPropertyUpdated("CrudeSample");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _yiYouSample;

        /// <summary>
        ///
        ///</summary>
        public string YiYouSample
        {
            get
            {
                return _yiYouSample;
            }
            set
            {
                _yiYouSample=value;
                OnPropertyUpdated("YiYouSample");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _replaceNumber;

        /// <summary>
        ///
        ///</summary>
        public string ReplaceNumber
        {
            get
            {
                return _replaceNumber;
            }
            set
            {
                _replaceNumber=value;
                OnPropertyUpdated("ReplaceNumber");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _sampleNumber;

        /// <summary>
        ///
        ///</summary>
        public string SampleNumber
        {
            get
            {
                return _sampleNumber;
            }
            set
            {
                _sampleNumber=value;
                OnPropertyUpdated("SampleNumber");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _analysis;

        /// <summary>
        ///
        ///</summary>
        public string Analysis
        {
            get
            {
                return _analysis;
            }
            set
            {
                _analysis=value;
                OnPropertyUpdated("Analysis");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _samplingTerrace;

        /// <summary>
        ///
        ///</summary>
        public string SamplingTerrace
        {
            get
            {
                return _samplingTerrace;
            }
            set
            {
                _samplingTerrace=value;
                OnPropertyUpdated("SamplingTerrace");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _samplingUser;

        /// <summary>
        ///
        ///</summary>
        public string SamplingUser
        {
            get
            {
                return _samplingUser;
            }
            set
            {
                _samplingUser=value;
                OnPropertyUpdated("SamplingUser");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _duplicateSample;

        /// <summary>
        ///
        ///</summary>
        public string DuplicateSample
        {
            get
            {
                return _duplicateSample;
            }
            set
            {
                _duplicateSample=value;
                OnPropertyUpdated("DuplicateSample");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected DateTime? _samplingTime;

        /// <summary>
        ///
        ///</summary>
        public DateTime? SamplingTime
        {
            get
            {
                return _samplingTime;
            }
            set
            {
                _samplingTime=value;
                OnPropertyUpdated("SamplingTime");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _sampleName;

        /// <summary>
        ///
        ///</summary>
        public string SampleName
        {
            get
            {
                return _sampleName;
            }
            set
            {
                _sampleName=value;
                OnPropertyUpdated("SampleName");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _sampleLocation;

        /// <summary>
        ///
        ///</summary>
        public string SampleLocation
        {
            get
            {
                return _sampleLocation;
            }
            set
            {
                _sampleLocation=value;
                OnPropertyUpdated("SampleLocation");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _sampledescribe;

        /// <summary>
        ///
        ///</summary>
        public string Sampledescribe
        {
            get
            {
                return _sampledescribe;
            }
            set
            {
                _sampledescribe=value;
                OnPropertyUpdated("Sampledescribe");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _depth;

        /// <summary>
        ///
        ///</summary>
        public string Depth
        {
            get
            {
                return _depth;
            }
            set
            {
                _depth=value;
                OnPropertyUpdated("Depth");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _dailyoutput;

        /// <summary>
        ///
        ///</summary>
        public string Dailyoutput
        {
            get
            {
                return _dailyoutput;
            }
            set
            {
                _dailyoutput=value;
                OnPropertyUpdated("Dailyoutput");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _lithology;

        /// <summary>
        ///
        ///</summary>
        public string Lithology
        {
            get
            {
                return _lithology;
            }
            set
            {
                _lithology=value;
                OnPropertyUpdated("Lithology");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _gradation_all;

        /// <summary>
        ///
        ///</summary>
        public string Gradation_all
        {
            get
            {
                return _gradation_all;
            }
            set
            {
                _gradation_all=value;
                OnPropertyUpdated("Gradation_all");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _gradation_group;

        /// <summary>
        ///
        ///</summary>
        public string Gradation_group
        {
            get
            {
                return _gradation_group;
            }
            set
            {
                _gradation_group=value;
                OnPropertyUpdated("Gradation_group");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _gradation_part;

        /// <summary>
        ///
        ///</summary>
        public string Gradation_part
        {
            get
            {
                return _gradation_part;
            }
            set
            {
                _gradation_part=value;
                OnPropertyUpdated("Gradation_part");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _containingWater;

        /// <summary>
        ///
        ///</summary>
        public string ContainingWater
        {
            get
            {
                return _containingWater;
            }
            set
            {
                _containingWater=value;
                OnPropertyUpdated("ContainingWater");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _density;

        /// <summary>
        ///
        ///</summary>
        public string Density
        {
            get
            {
                return _density;
            }
            set
            {
                _density=value;
                OnPropertyUpdated("Density");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _viscosity;

        /// <summary>
        ///
        ///</summary>
        public string Viscosity
        {
            get
            {
                return _viscosity;
            }
            set
            {
                _viscosity=value;
                OnPropertyUpdated("Viscosity");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _sulfur;

        /// <summary>
        ///
        ///</summary>
        public string Sulfur
        {
            get
            {
                return _sulfur;
            }
            set
            {
                _sulfur=value;
                OnPropertyUpdated("Sulfur");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _ripeDepe;

        /// <summary>
        ///
        ///</summary>
        public string RipeDepe
        {
            get
            {
                return _ripeDepe;
            }
            set
            {
                _ripeDepe=value;
                OnPropertyUpdated("RipeDepe");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _addDose;

        /// <summary>
        ///
        ///</summary>
        public string AddDose
        {
            get
            {
                return _addDose;
            }
            set
            {
                _addDose=value;
                OnPropertyUpdated("AddDose");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _pipeInfo;

        /// <summary>
        ///
        ///</summary>
        public string PipeInfo
        {
            get
            {
                return _pipeInfo;
            }
            set
            {
                _pipeInfo=value;
                OnPropertyUpdated("PipeInfo");
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
        protected bool? _printState;

        /// <summary>
        ///
        ///</summary>
        public bool? PrintState
        {
            get
            {
                return _printState;
            }
            set
            {
                _printState=value;
                OnPropertyUpdated("PrintState");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected DateTime? _transmitTime;

        /// <summary>
        ///
        ///</summary>
        public DateTime? TransmitTime
        {
            get
            {
                return _transmitTime;
            }
            set
            {
                _transmitTime=value;
                OnPropertyUpdated("TransmitTime");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _userName;

        /// <summary>
        ///
        ///</summary>
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName=value;
                OnPropertyUpdated("UserName");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected DateTime? _instorageTime;

        /// <summary>
        ///
        ///</summary>
        public DateTime? InstorageTime
        {
            get
            {
                return _instorageTime;
            }
            set
            {
                _instorageTime=value;
                OnPropertyUpdated("InstorageTime");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected DateTime? _boreholeTime;

        /// <summary>
        ///
        ///</summary>
        public DateTime? BoreholeTime
        {
            get
            {
                return _boreholeTime;
            }
            set
            {
                _boreholeTime=value;
                OnPropertyUpdated("BoreholeTime");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _boxNum;

        /// <summary>
        ///
        ///</summary>
        public string BoxNum
        {
            get
            {
                return _boxNum;
            }
            set
            {
                _boxNum=value;
                OnPropertyUpdated("BoxNum");
            }
        }




        private static ModelContext<View_InSorageRecode> _____baseContext=new ModelContext<View_InSorageRecode>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<View_InSorageRecode> GetContext() 
        {
            return _____baseContext;
        }

    }
}
