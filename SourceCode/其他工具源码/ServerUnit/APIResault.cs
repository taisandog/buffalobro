using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerUnit
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
            if (_data == null) 
            {
                return default(T);
            }

            if (_data is T) 
            {
                return (T)_data;
            }
            if (_data is string) 
            {
                _data = JsonConvert.DeserializeObject<T>(_data as string);
                return (T)_data;
            }
            if (_data is JObject) 
            {

                _data = ((JObject)_data).ToObject<T>();
                return (T)_data;
            }
            if (_data is JArray)
            {

                _data = ((JArray)_data).ToObject<T>();
                return (T)_data;
            }
            _data = Convert.ChangeType(_data,typeof(T));
            return (T)_data;
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
        /// 序列化成Json
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret["state"] = (int)_resault;
            ret["message"] = _message;
            ret["data"] = _data;
            return JsonConvert.SerializeObject(ret);
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
            
            string json=JsonConvert.SerializeObject(_data, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new ExcludePropertiesContractResolver(ignore,false)
            });
            ret["data"] = json;
            return JsonConvert.SerializeObject(ret);
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

            string json = JsonConvert.SerializeObject(_data, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new ExcludePropertiesContractResolver(ignore,true)
            });
            ret["data"] = json;
            return JsonConvert.SerializeObject(ret);
        }
        /// <summary>
        /// 设置成功值
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="data">返回值内容</param>
        public void SetSuccess(string message = null, object data = null)
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
        public void SetFault(string message = null, object data = null)
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
}
