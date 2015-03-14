using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.PropertyAttributes;
namespace ManagementLib.BQLEntity
{

    public partial class Management
    {
        private static Management_SampleInfo _SampleInfo = new Management_SampleInfo();
    
        public static Management_SampleInfo SampleInfo
        {
            get
            {
                return _SampleInfo;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class Management_SampleInfo : BQLEntityTableHandle
    {
        private BQLEntityParamHandle _id = null;
        /// <summary>
        /// 主键ID，自动增长
        /// </summary>
        public BQLEntityParamHandle Id
        {
            get
            {
                return _id;
            }
         }
        private BQLEntityParamHandle _projectName = null;
        /// <summary>
        /// 项目名称
        /// </summary>
        public BQLEntityParamHandle ProjectName
        {
            get
            {
                return _projectName;
            }
         }
        private BQLEntityParamHandle _projectId = null;
        /// <summary>
        /// 项目编号
        /// </summary>
        public BQLEntityParamHandle ProjectId
        {
            get
            {
                return _projectId;
            }
         }
        private BQLEntityParamHandle _crudeSample = null;
        /// <summary>
        /// 原油样品
        /// </summary>
        public BQLEntityParamHandle CrudeSample
        {
            get
            {
                return _crudeSample;
            }
         }
        private BQLEntityParamHandle _yiYouSample = null;
        /// <summary>
        /// 溢油样品
        /// </summary>
        public BQLEntityParamHandle YiYouSample
        {
            get
            {
                return _yiYouSample;
            }
         }
        private BQLEntityParamHandle _replaceNumber = null;
        /// <summary>
        /// 替代编号
        /// </summary>
        public BQLEntityParamHandle ReplaceNumber
        {
            get
            {
                return _replaceNumber;
            }
         }
        private BQLEntityParamHandle _sampleNumber = null;
        /// <summary>
        /// 样品编号
        /// </summary>
        public BQLEntityParamHandle SampleNumber
        {
            get
            {
                return _sampleNumber;
            }
         }
        private BQLEntityParamHandle _analysis = null;
        /// <summary>
        /// 分析情况
        /// </summary>
        public BQLEntityParamHandle Analysis
        {
            get
            {
                return _analysis;
            }
         }
        private BQLEntityParamHandle _samplingTerrace = null;
        /// <summary>
        /// 采样平台
        /// </summary>
        public BQLEntityParamHandle SamplingTerrace
        {
            get
            {
                return _samplingTerrace;
            }
         }
        private BQLEntityParamHandle _samplingUser = null;
        /// <summary>
        /// 采样人员
        /// </summary>
        public BQLEntityParamHandle SamplingUser
        {
            get
            {
                return _samplingUser;
            }
         }
        private BQLEntityParamHandle _duplicateSample = null;
        /// <summary>
        /// 平行样
        /// </summary>
        public BQLEntityParamHandle DuplicateSample
        {
            get
            {
                return _duplicateSample;
            }
         }
        private BQLEntityParamHandle _samplingTime = null;
        /// <summary>
        /// 采样时间
        /// </summary>
        public BQLEntityParamHandle SamplingTime
        {
            get
            {
                return _samplingTime;
            }
         }
        private BQLEntityParamHandle _sampleName = null;
        /// <summary>
        /// 样品名称
        /// </summary>
        public BQLEntityParamHandle SampleName
        {
            get
            {
                return _sampleName;
            }
         }
        private BQLEntityParamHandle _sampleLocation = null;
        /// <summary>
        /// 采样位置
        /// </summary>
        public BQLEntityParamHandle SampleLocation
        {
            get
            {
                return _sampleLocation;
            }
         }
        private BQLEntityParamHandle _sampledescribe = null;
        /// <summary>
        /// 样品描述
        /// </summary>
        public BQLEntityParamHandle Sampledescribe
        {
            get
            {
                return _sampledescribe;
            }
         }
        private BQLEntityParamHandle _boreholeTime = null;
        /// <summary>
        /// 钻井时间
        /// </summary>
        public BQLEntityParamHandle BoreholeTime
        {
            get
            {
                return _boreholeTime;
            }
         }
        private BQLEntityParamHandle _depth = null;
        /// <summary>
        /// 采油深度
        /// </summary>
        public BQLEntityParamHandle Depth
        {
            get
            {
                return _depth;
            }
         }
        private BQLEntityParamHandle _dailyoutput = null;
        /// <summary>
        /// 日产量（桶）
        /// </summary>
        public BQLEntityParamHandle Dailyoutput
        {
            get
            {
                return _dailyoutput;
            }
         }
        private BQLEntityParamHandle _lithology = null;
        /// <summary>
        /// 岩性
        /// </summary>
        public BQLEntityParamHandle Lithology
        {
            get
            {
                return _lithology;
            }
         }
        private BQLEntityParamHandle _gradation_all = null;
        /// <summary>
        /// 地质层次-统
        /// </summary>
        public BQLEntityParamHandle Gradation_all
        {
            get
            {
                return _gradation_all;
            }
         }
        private BQLEntityParamHandle _gradation_group = null;
        /// <summary>
        /// 地质层次-组
        /// </summary>
        public BQLEntityParamHandle Gradation_group
        {
            get
            {
                return _gradation_group;
            }
         }
        private BQLEntityParamHandle _gradation_part = null;
        /// <summary>
        /// 地质层次-段
        /// </summary>
        public BQLEntityParamHandle Gradation_part
        {
            get
            {
                return _gradation_part;
            }
         }
        private BQLEntityParamHandle _containingWater = null;
        /// <summary>
        /// 含水率（％）
        /// </summary>
        public BQLEntityParamHandle ContainingWater
        {
            get
            {
                return _containingWater;
            }
         }
        private BQLEntityParamHandle _density = null;
        /// <summary>
        /// 密度（g/cm3）
        /// </summary>
        public BQLEntityParamHandle Density
        {
            get
            {
                return _density;
            }
         }
        private BQLEntityParamHandle _viscosity = null;
        /// <summary>
        /// 粘度
        /// </summary>
        public BQLEntityParamHandle Viscosity
        {
            get
            {
                return _viscosity;
            }
         }
        private BQLEntityParamHandle _sulfur = null;
        /// <summary>
        /// 含硫量（%）
        /// </summary>
        public BQLEntityParamHandle Sulfur
        {
            get
            {
                return _sulfur;
            }
         }
        private BQLEntityParamHandle _ripeDepe = null;
        /// <summary>
        /// 成熟度（Ro等）
        /// </summary>
        public BQLEntityParamHandle RipeDepe
        {
            get
            {
                return _ripeDepe;
            }
         }
        private BQLEntityParamHandle _addDose = null;
        /// <summary>
        /// 添加剂
        /// </summary>
        public BQLEntityParamHandle AddDose
        {
            get
            {
                return _addDose;
            }
         }
        private BQLEntityParamHandle _pipeInfo = null;
        /// <summary>
        /// 管汇信息
        /// </summary>
        public BQLEntityParamHandle PipeInfo
        {
            get
            {
                return _pipeInfo;
            }
         }
        private BQLEntityParamHandle _sampleCode = null;
        /// <summary>
        /// 样品条码
        /// </summary>
        public BQLEntityParamHandle SampleCode
        {
            get
            {
                return _sampleCode;
            }
         }
        private BQLEntityParamHandle _printState = null;
        /// <summary>
        /// 是否已打印
        /// </summary>
        public BQLEntityParamHandle PrintState
        {
            get
            {
                return _printState;
            }
         }
        private BQLEntityParamHandle _transmitTime = null;
        /// <summary>
        /// 导入时间
        /// </summary>
        public BQLEntityParamHandle TransmitTime
        {
            get
            {
                return _transmitTime;
            }
         }
        private BQLEntityParamHandle _blInStorage = null;
        /// <summary>
        /// 是否有做入库操作(0为无做入库操作，1为已做入库操作)
        /// </summary>
        public BQLEntityParamHandle BlInStorage
        {
            get
            {
                return _blInStorage;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_SampleInfo(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(ManagementLib.SampleInfo),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_SampleInfo(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _projectName=CreateProperty("ProjectName");
            _projectId=CreateProperty("ProjectId");
            _crudeSample=CreateProperty("CrudeSample");
            _yiYouSample=CreateProperty("YiYouSample");
            _replaceNumber=CreateProperty("ReplaceNumber");
            _sampleNumber=CreateProperty("SampleNumber");
            _analysis=CreateProperty("Analysis");
            _samplingTerrace=CreateProperty("SamplingTerrace");
            _samplingUser=CreateProperty("SamplingUser");
            _duplicateSample=CreateProperty("DuplicateSample");
            _samplingTime=CreateProperty("SamplingTime");
            _sampleName=CreateProperty("SampleName");
            _sampleLocation=CreateProperty("SampleLocation");
            _sampledescribe=CreateProperty("Sampledescribe");
            _boreholeTime=CreateProperty("BoreholeTime");
            _depth=CreateProperty("Depth");
            _dailyoutput=CreateProperty("Dailyoutput");
            _lithology=CreateProperty("Lithology");
            _gradation_all=CreateProperty("Gradation_all");
            _gradation_group=CreateProperty("Gradation_group");
            _gradation_part=CreateProperty("Gradation_part");
            _containingWater=CreateProperty("ContainingWater");
            _density=CreateProperty("Density");
            _viscosity=CreateProperty("Viscosity");
            _sulfur=CreateProperty("Sulfur");
            _ripeDepe=CreateProperty("RipeDepe");
            _addDose=CreateProperty("AddDose");
            _pipeInfo=CreateProperty("PipeInfo");
            _sampleCode=CreateProperty("SampleCode");
            _printState=CreateProperty("PrintState");
            _transmitTime=CreateProperty("TransmitTime");
            _blInStorage=CreateProperty("BlInStorage");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public Management_SampleInfo() 
            :this(null,null)
        {
        }
    }
}
