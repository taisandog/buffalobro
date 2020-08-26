using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.ArgCommon
{
    /// <summary>
    /// API返回值
    /// </summary>
    public class APIResault
    {
        

        private object _data;

        ///// <summary>
        ///// 数据
        ///// </summary>
        //public object Data
        //{
        //    get { return _data; }
        //    set { _data=value; }
        //}

        private string _message;
        /// <summary>
        /// 信息
        /// </summary>
        [JsonProperty("message")]
        public string Message 
        { 
            get 
            {
                return _message;
            }
            set 
            {
                _message=value;
            }
        }

        private ResaultType _resault;
        /// <summary>
        /// 信息
        /// </summary>
        [JsonProperty("state")]
        public ResaultType Resault
        {
            get
            {
                return _resault;
            }
            set
            {
                _resault = value;
            }
        }

        private JsonSerializerSettings _settings;
        /// <summary>
        /// 转换模式
        /// </summary>
        [JsonIgnore]
        public JsonSerializerSettings Settiong 
        {
            get
            {
                return _settings;
            }
           
        }
        private Formatting _formatting;
        /// <summary>
        /// 格式
        /// </summary>
        [JsonIgnore]
        public Formatting Formatting
        {
            get
            {
                return _formatting;
            }
            
        }

        private Type _type;
        /// <summary>
        /// 类型
        /// </summary>
        [JsonIgnore]
        public Type Type
        {
            get
            {
                return _type;
            }
            
        }
        /// <summary>
        /// 调用是否成功
        /// </summary>
        [JsonIgnore]
        public bool IsSuccess 
        {
            get 
            {
                return _resault == ResaultType.Success;
            }
        }
        

        private Exception _apiException;
        /// <summary>
        /// 异常
        /// </summary>
        [JsonIgnore]
        public Exception ApiException
        {
            get { return _apiException; }
        }
        /// <summary>
        /// 获取值[Data]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetValue<T>()
        {
            return JsonValueConvertExtend.ConvertJsonValue<T>(_data);
        }

        /// <summary>
        /// 忽略的属性
        /// </summary>
        private IEnumerable<string> _ignoreProperty;
        /// <summary>
        /// 忽略的属性
        /// </summary>
        [JsonIgnore]
        public IEnumerable<string> Ignore
        {
            get
            {
                return _ignoreProperty;
            }

        }
        /// <summary>
        /// 显示的属性
        /// </summary>
        private IEnumerable<string> _showProperty;
        /// <summary>
        /// 显示的属性
        /// </summary>
        [JsonIgnore]
        public IEnumerable<string> ShowProperty
        {
            get
            {
                return _showProperty;
            }

        }
        /// <summary>
        /// 设置内容值
        /// </summary>
        /// <param name="data">内容</param>
        public void SetValue(object data) 
        {
            _data = data;
        }
        public override string ToString()
        {
            return ToJson();
        }
       
        /// <summary>
        /// API返回值
        /// </summary>
        public APIResault()
        {
            
        }
        /// <summary>
        /// 获取值
        /// </summary>
        private void GetResault(Dictionary<string, object> resultJson)
        {
            object res =null;
            if (resultJson.TryGetValue("state", out res)) 
            {
                try
                {
                    _resault = (ResaultType)Convert.ToInt32(res);
                }
                catch { _resault = ResaultType.Fault; }
            }
            if (resultJson.TryGetValue("message", out res))
            {
                _message = res as string;
            }
            if (resultJson.TryGetValue("data", out res))
            {
                _data = res;
            }
        }
        
        /// <summary>
        /// 设置格式并返回本实例
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public APIResault FromFormatting(Formatting format) 
        {
            _formatting = format;
            return this;
        }
        /// <summary>
        /// 设置转换模式并返回本实例
        /// </summary>
        /// <param name="setting">模式</param>
        /// <returns></returns>
        public APIResault FromFormatting(JsonSerializerSettings setting)
        {
            _settings = setting;
            return this;
        }

        /// <summary>
        /// 设置转换模式并返回本实例
        /// </summary>
        /// <param name="setting">模式</param>
        /// <returns></returns>
        public APIResault FromIgnore(JsonSerializerSettings setting)
        {
            _settings = setting;
            return this;
        }
        /// <summary>
        /// 序列化成Json
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret["state"] = (int)_resault;
            ret["message"] = _message;

            object data = _data;
            Type type = _type;
            if (_showProperty != null)
            {
                JsonSerializerSettings setting = new JsonSerializerSettings();
                setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                setting.ContractResolver = new ExcludePropertiesContractResolver(_showProperty, false);

                data = JsonConvert.SerializeObject(_data,_type, _formatting, setting);
                type = null;
            }
            else if (_ignoreProperty!=null) 
            {
                JsonSerializerSettings setting = new JsonSerializerSettings();
                setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                setting.ContractResolver = new ExcludePropertiesContractResolver(_ignoreProperty, true);

                data = JsonConvert.SerializeObject(_data, _type, _formatting, setting);
                type = null;
            }
            
            ret["data"] = _data;

            return JsonConvert.SerializeObject(ret, _type, _formatting, _settings);
        }
        
        
        /// <summary>
        /// 序列化成Json
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> ToObject()
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret["state"] = (int)_resault;
            ret["message"] = _message;
            ret["data"] = _data;
            return ret;
        }
        /// <summary>
        /// 忽略Data某些属性序列化成Json
        /// </summary>
        /// <param name="ignore">忽略属性集合</param>
        /// <returns></returns>
        public string ToJsonIgnore(IEnumerable<string> ignore)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret["state"] = (int)_resault;
            ret["message"] = _message;
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            setting.ContractResolver = new ExcludePropertiesContractResolver(ignore, false);
            string json = JsonConvert.SerializeObject(_data, _formatting, setting);
            ret["data"] = json;
            return JsonConvert.SerializeObject(ret, _type, _formatting, _settings);
        }
        /// <summary>
        /// 只显示Data某些属性序列化成Json
        /// </summary>
        /// <param name="ignore">只显示属性集合</param>
        /// <returns></returns>
        public string ToJsonShow(IEnumerable<string> ignore)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret["state"] = (int)_resault;
            ret["message"] = _message;

            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            setting.ContractResolver = new ExcludePropertiesContractResolver(ignore, true);

            string json = JsonConvert.SerializeObject(_data, _formatting, setting);

            ret["data"] = json;


            return JsonConvert.SerializeObject(ret, _type, _formatting, _settings);
        }
        /// <summary>
        /// 转换成Json
        /// </summary>
        /// <param name="propertys">属性集合</param>
        /// <param name="type">显示或屏蔽这些属性</param>
        /// <returns></returns>
        public string ParseJson(IEnumerable<string> propertys, JsonParseType type)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret["state"] = (int)_resault;
            ret["message"] = _message;

            JsonSerializer js = new JsonSerializer();

            js.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            bool isShow = type == JsonParseType.Show;
            js.ContractResolver = new ExcludePropertiesContractResolver(propertys, isShow);

            if (_data != null)
            {
                if(_data is ValueType || _data is string)
                {
                    ret["data"] = _data;
                }
                else if (_data is IEnumerable)
                {
                    JArray obj = JArray.FromObject(_data, js);
                    ret["data"] = obj;
                }
                else
                {
                    JObject obj = JObject.FromObject(_data, js);
                    ret["data"] = obj;
                }
            }
            return JsonConvert.SerializeObject(ret, _type, _formatting, _settings);
        }
#if !NET_2_0 && !NET_3_5
        /// <summary>
        /// 设置成功值
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="data">返回值内容</param>
        public void SetSuccess(string message=null , object data=null)
        {
            _resault = ResaultType.Success;
            _message = message;
            _data = data;
        }
        /// <summary>
        /// 设置错误
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">内容</param>
        public void SetFault(string message=null, object data=null)
        {
            _resault = ResaultType.Fault;
            _message = message;
            _data = data;
        }
        /// <summary>
        /// 设置错误
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">内容</param>
        public void SetTimeout(string message = null, object data = null)
        {
            _resault = ResaultType.Timeout;
            _message = message;
            _data = data;
        }
#else
        /// <summary>
        /// 设置成功值
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="data">返回值内容</param>
        public void SetSuccess(string message , object data)
        {
            _resault = ResaultType.Success;
            _message = message;
            _data = data;
        }
        /// <summary>
        /// 设置成功值
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="data">返回值内容</param>
        public void SetSuccess()
        {
            SetSuccess(null,null);
        }

        /// <summary>
        /// 设置错误
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">内容</param>
        public void SetFault(string message, object data )
        {
            _resault = ResaultType.Fault;
            _message = message;
            _data = data;
        }
        /// <summary>
        /// 设置错误
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">内容</param>
        public void SetFault()
        {
            SetFault(null,null);
        }
        /// <summary>
        /// 设置错误
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">内容</param>
        public void SetTimeout(string message , object data)
        {
            _resault = ResaultType.Timeout;
            _message = message;
            _data = data;
        }
        /// <summary>
        /// 设置错误
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">内容</param>
        public void SetTimeout()
        {
            SetTimeout(null,null);
        }
#endif
        
        /// <summary>
        /// 设置异常
        /// </summary>
        /// <param name="ex">异常值</param>
        public void SetException(Exception ex)
        {
            _apiException = ex;
            _resault = ResaultType.Exception;
            _message = ex.ToString();
        }

        /// <summary>
        /// 设置json
        /// </summary>
        /// <param name="json">json</param>
        public void SetJson(string json) 
        {
            string ejson = json.Trim();
            if (ejson[0] == '\"') 
            {
                ejson = JsonConvert.DeserializeObject<string>(ejson);
            }
            
            GetResault(JsonConvert.DeserializeObject<Dictionary<string, object>>(ejson));
        }
    }

    /// <summary>
    /// Json转换开关
    /// </summary>
    public enum JsonParseType
    {
        /// <summary>
        /// 显示列表中的属性
        /// </summary>
        Show=1,
        /// <summary>
        /// 跳过列表中的属性
        /// </summary>
        Ignore=2
    }
}
