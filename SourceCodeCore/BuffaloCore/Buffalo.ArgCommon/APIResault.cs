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

        /// <summary>
        /// 数据
        /// </summary>
        public object Data
        {
            get { return _data; }
            set { _data = value; }
        }

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
                _message = value;
            }
        }
        private bool _dataAsString;
        /// <summary>
        /// 统一转换成字符串
        /// </summary>
        [JsonIgnore]
        public bool DataAsString
        {
            get
            {
                return _dataAsString;
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
        public JsonSerializerSettings Setting
        {
            get
            {
                return _settings;
            }

        }
        private Formatting _formatting = Formatting.None;
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
        /// <summary>
        /// 实体类型
        /// </summary>
        private Type _type;
        /// <summary>
        /// 实体类型
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
        /// 显示或屏蔽属性的方式
        /// </summary>
        private ExcludePropertiesContractResolver _excludeProperties;
        /// <summary>
        /// 显示或屏蔽属性的方式
        /// </summary>
        [JsonIgnore]
        public ExcludePropertiesContractResolver ExcludeProperties
        {
            get
            {
                return _excludeProperties;
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
            object res = null;
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
        public APIResault FromSettings(JsonSerializerSettings setting)
        {
            _settings = setting;
            return this;
        }
        /// <summary>
        /// 设置属性显示或屏蔽方式，并返回本实例
        /// </summary>
        /// <param name="excludeProperty">显示或屏蔽开关</param>
        /// <returns></returns>
        public APIResault FromExcludeProperties(ExcludePropertiesContractResolver excludeProperty)
        {
            _excludeProperties = excludeProperty;
            return this;
        }
        /// <summary>
        /// 设置显示的属性并返回本实例
        /// </summary>
        /// <param name="showProperty">显示的属性</param>
        /// <returns></returns>
        public APIResault FromShowProperty(IEnumerable<string> showProperty)
        {
            return FromExcludeProperties(new ExcludePropertiesContractResolver(showProperty, JsonParseType.Show));
        }

        /// <summary>
        /// 设置忽略的属性并返回本实例
        /// </summary>
        /// <param name="ignoreProperty">忽略的属性</param>
        /// <returns></returns>
        public APIResault FromIgnoreProperty(IEnumerable<string> ignoreProperty)
        {
            return FromExcludeProperties(new ExcludePropertiesContractResolver(ignoreProperty, JsonParseType.Ignore));
        }
        /// <summary>
        /// 转换时候把非字符串的data值打成json字符串
        /// </summary>
        /// <returns></returns>
        public APIResault FromDataToString()
        {

            return FromDataToString(true);
        }
        /// <summary>
        /// 转换时候把非字符串的data值打成json字符串
        /// </summary>
        /// <param name="toString">是否转换成字符串</param>
        /// <returns></returns>
        public APIResault FromDataToString(bool toString)
        {
            _dataAsString = toString;
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


            if (_excludeProperties != null)//忽略或显示属性开关
            {
                if (_settings == null)
                {
                    _settings = new JsonSerializerSettings();
                }
                _settings.ContractResolver = _excludeProperties;
                type = null;
            }

            if (_dataAsString && !(data is string))
            {
                data = JsonConvert.SerializeObject(data, _type, _formatting, _settings);
            }


            ret["data"] = data;

            return JsonConvert.SerializeObject(ret, type, _formatting, _settings);
        }

        /// <summary>
        /// 只显示Data某些属性序列化成Json
        /// </summary>
        /// <param name="showProperty">只显示属性集合</param>
        /// <returns></returns>
        public string ToJsonShow(IEnumerable<string> showProperty)
        {
            return FromShowProperty(showProperty).ToJson();
        }
        /// <summary>
        /// 忽略Data某些属性序列化成Json
        /// </summary>
        /// <param name="ignoreProperty">忽略属性集合</param>
        /// <returns></returns>
        public string ToJsonIgnore(IEnumerable<string> ignoreProperty)
        {
            return FromIgnoreProperty(ignoreProperty).ToJson();
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
        /// 状态设置为成功
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="data">返回值内容</param>
        public void SetSuccess(string message, object data)
        {
            _resault = ResaultType.Success;
            _message = message;
            _data = data;
        }
        /// <summary>
        /// 状态设置为成功
        /// </summary>
        public void SetSuccess()
        {
            SetSuccess(null, null);
        }

        /// <summary>
        /// 状态设置为一般错误
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">内容</param>
        public void SetFault(string message, object data)
        {
            _resault = ResaultType.Fault;
            _message = message;
            _data = data;
        }
        /// <summary>
        /// 状态设置为一般错误
        /// </summary>
        /// <param name="message">消息</param>
        public void SetFault(string message)
        {
            SetFault(message, null);
        }
        /// <summary>
        /// 状态设置为一般错误
        /// </summary>
        public void SetFault()
        {
            SetFault(null, null);
        }
        /// <summary>
        /// 设置超时错误
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">内容</param>
        public void SetTimeout(string message, object data)
        {
            _resault = ResaultType.Timeout;
            _message = message;
            _data = data;
        }
        /// <summary>
        /// 设置超时错误
        /// </summary>
        public void SetTimeout()
        {
            SetTimeout(null, null);
        }


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
