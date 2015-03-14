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
    public partial class SampleInfo: Buffalo.DB.CommBase.BusinessBases.ThinModelBase
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
        ///项目名称
        ///</summary>
        protected string _projectName;

        /// <summary>
        ///项目名称
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
        ///项目编号
        ///</summary>
        protected string _projectId;

        /// <summary>
        ///项目编号
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
        ///原油样品
        ///</summary>
        protected string _crudeSample;

        /// <summary>
        ///原油样品
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
        ///溢油样品
        ///</summary>
        protected string _yiYouSample;

        /// <summary>
        ///溢油样品
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
        ///替代编号
        ///</summary>
        protected string _replaceNumber;

        /// <summary>
        ///替代编号
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
        ///样品编号
        ///</summary>
        protected string _sampleNumber;

        /// <summary>
        ///样品编号
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
        ///分析情况
        ///</summary>
        protected string _analysis;

        /// <summary>
        ///分析情况
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
        ///采样平台
        ///</summary>
        protected string _samplingTerrace;

        /// <summary>
        ///采样平台
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
        ///采样人员
        ///</summary>
        protected string _samplingUser;

        /// <summary>
        ///采样人员
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
        ///平行样
        ///</summary>
        protected string _duplicateSample;

        /// <summary>
        ///平行样
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
        ///采样时间
        ///</summary>
        protected DateTime? _samplingTime;

        /// <summary>
        ///采样时间
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
        ///样品名称
        ///</summary>
        protected string _sampleName;

        /// <summary>
        ///样品名称
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
        ///采样位置
        ///</summary>
        protected string _sampleLocation;

        /// <summary>
        ///采样位置
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
        ///样品描述
        ///</summary>
        protected string _sampledescribe;

        /// <summary>
        ///样品描述
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
        ///钻井时间
        ///</summary>
        protected DateTime? _boreholeTime;

        /// <summary>
        ///钻井时间
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
        ///采油深度
        ///</summary>
        protected string _depth;

        /// <summary>
        ///采油深度
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
        ///日产量（桶）
        ///</summary>
        protected string _dailyoutput;

        /// <summary>
        ///日产量（桶）
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
        ///岩性
        ///</summary>
        protected string _lithology;

        /// <summary>
        ///岩性
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
        ///地质层次-统
        ///</summary>
        protected string _gradation_all;

        /// <summary>
        ///地质层次-统
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
        ///地质层次-组
        ///</summary>
        protected string _gradation_group;

        /// <summary>
        ///地质层次-组
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
        ///地质层次-段
        ///</summary>
        protected string _gradation_part;

        /// <summary>
        ///地质层次-段
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
        ///含水率（％）
        ///</summary>
        protected string _containingWater;

        /// <summary>
        ///含水率（％）
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
        ///密度（g/cm3）
        ///</summary>
        protected string _density;

        /// <summary>
        ///密度（g/cm3）
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
        ///粘度
        ///</summary>
        protected string _viscosity;

        /// <summary>
        ///粘度
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
        ///含硫量（%）
        ///</summary>
        protected string _sulfur;

        /// <summary>
        ///含硫量（%）
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
        ///成熟度（Ro等）
        ///</summary>
        protected string _ripeDepe;

        /// <summary>
        ///成熟度（Ro等）
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
        ///添加剂
        ///</summary>
        protected string _addDose;

        /// <summary>
        ///添加剂
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
        ///管汇信息
        ///</summary>
        protected string _pipeInfo;

        /// <summary>
        ///管汇信息
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
        ///样品条码
        ///</summary>
        protected string _sampleCode;

        /// <summary>
        ///样品条码
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
        ///是否已打印
        ///</summary>
        protected bool? _printState;

        /// <summary>
        ///是否已打印
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
        ///导入时间
        ///</summary>
        protected DateTime? _transmitTime;

        /// <summary>
        ///导入时间
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
        ///是否有做入库操作(0为无做入库操作，1为已做入库操作)
        ///</summary>
        protected bool? _blInStorage;

        /// <summary>
        ///是否有做入库操作(0为无做入库操作，1为已做入库操作)
        ///</summary>
        public bool? BlInStorage
        {
            get
            {
                return _blInStorage;
            }
            set
            {
                _blInStorage=value;
                OnPropertyUpdated("BlInStorage");
            }
        }




        private static ModelContext<SampleInfo> _____baseContext=new ModelContext<SampleInfo>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<SampleInfo> GetContext() 
        {
            return _____baseContext;
        }

    }
}
