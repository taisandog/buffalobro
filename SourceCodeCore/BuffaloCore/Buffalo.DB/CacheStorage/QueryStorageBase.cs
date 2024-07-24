using Buffalo.DB.CacheManager;
using Buffalo.Kernel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.DB.CacheStorage
{
    /// <summary>
    /// 存储基类
    /// </summary>
    public abstract class QueryStorageBase<T>
    {
        /// <summary>
        /// 组版本
        /// </summary>
        protected QueryCache _verCache = null;
        /// <summary>
        /// 组数据
        /// </summary>
        protected QueryCache _dataCache = null;

        /// <summary>
        /// 缓存Key
        /// </summary>
        protected abstract string CacheKey
        {
            get;
        }

        /// <summary>
        /// 初始化组版本缓存
        /// </summary>
        /// <returns></returns>
        protected virtual void InitCache(string verCacheConnstring, string cacheType)
        {
            _dataCache=CacheUnit.CreateCache(BuffaloCacheTypes.System, "");
            _verCache= CacheUnit.CreateCache(cacheType, verCacheConnstring);
        }

        public QueryStorageBase() 
        {

        }

        /// <summary>
        /// 获取组版本
        /// </summary>
        /// <param name="accid"></param>
        /// <returns></returns>
        protected virtual int GetVersion(string id)
        {
            string key = GetKey(id);
            int ver = 0;
            try
            {
                ver = ValueConvertExtend.ConvertValue<int>(_verCache.GetValue(key));
            }
            catch { }
            return ver;
        }
        /// <summary>
        /// 获取组版本自增
        /// </summary>
        /// <param name="accid"></param>
        /// <returns></returns>
        public virtual long IncVersion(string id)
        {
            string key = GetKey(id);
            return _verCache.DoIncrement(key, 1);
        }
        /// <summary>
        /// 获取组版本自增
        /// </summary>
        /// <param name="accid"></param>
        /// <returns></returns>
        public virtual Task<long> IncVersionAsync(string id)
        {
            string key = GetKey(id);
            return _verCache.DoIncrementAsync(key, 1);
        }
        /// <summary>
        /// 获取存储值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string GetKey(string id) 
        {
            StringBuilder sbKey = new StringBuilder(64);
            sbKey.Append(CacheKey);
            sbKey.Append(".");
            sbKey.Append(id);
            return sbKey.ToString();
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual T GetValue(string id) 
        {
            string key = GetKey(id);
            CacheStorageItem<T> dicItem = _dataCache.GetValue<CacheStorageItem<T>>(key);
            int ver = GetVersion(id);
            if (dicItem == null || dicItem.Version != ver)
            {
                T value = SearchValue(id);
                dicItem = new CacheStorageItem<T>();
                dicItem.Key = key;
                dicItem.Version = ver;
                dicItem.Value = value;
                _dataCache.SetValue<CacheStorageItem<T>>(key, dicItem);
            }
            T ret = dicItem.Value;
            
            return ret;
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual async Task<T> GetValueAsync(string id)
        {
            string key = GetKey(id);
            CacheStorageItem<T> dicItem = await _dataCache.GetValueAsync<CacheStorageItem<T>>(key);
            int ver = GetVersion(id);
            if (dicItem == null || dicItem.Version != ver)
            {
                T value = await SearchValueAsync(id);
                dicItem = new CacheStorageItem<T>();
                dicItem.Key = key;
                dicItem.Version = ver;
                dicItem.Value = value;
                await _dataCache.SetValueAsync<CacheStorageItem<T>>(key, dicItem);
            }
            T ret = dicItem.Value;

            return ret;
        }
        /// <summary>
        /// 对比版本(如果跟数据版本不同则返回false)
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public bool EqualVersion(string id) 
        {
            string key = GetKey(id);
            CacheStorageItem<T> dicItem = _dataCache.GetValue<CacheStorageItem<T>>(key);
            int ver = GetVersion(id);
            if (dicItem == null || dicItem.Version != ver)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 对比版本(如果跟数据版本不同则返回false)
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public async Task<bool> EqualVersionAsync(string id)
        {
            string key = GetKey(id);
            CacheStorageItem<T> dicItem = await _dataCache.GetValueAsync<CacheStorageItem<T>>(key);
            int ver = GetVersion(id);
            if (dicItem == null || dicItem.Version != ver)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        protected virtual bool SetValue(string id, T value) 
        {
            string key = GetKey(id);
            IncVersion(id);
            int ver = GetVersion(id);
            CacheStorageItem<T> dicItem = new CacheStorageItem<T>();
            dicItem.Key = key;
            dicItem.Version = ver;
            dicItem.Value = value;
            return _dataCache.SetValue<CacheStorageItem<T>>(key, dicItem);
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        protected virtual async Task<bool> SetValueAsync(string id, T value)
        {
            string key = GetKey(id);
            IncVersion(id);
            int ver = GetVersion(id);
            CacheStorageItem<T> dicItem = new CacheStorageItem<T>();
            dicItem.Key = key;
            dicItem.Version = ver;
            dicItem.Value = value;
            return await _dataCache.SetValueAsync<CacheStorageItem<T>>(key, dicItem);
        }
        /// <summary>
        /// 查询内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual T SearchValue(string id) { return default(T); }
        /// <summary>
        /// 查询内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual async Task<T> SearchValueAsync(string id) { return default(T); }
    }
}
/*
public class UpdateInfoCache : QueryStorageBase<Dictionary<string, GBVersionItem>>
{
    public static UpdateInfoCache Default = GetCache();

    private static UpdateInfoCache GetCache()
    {
        string connStr = System.Configuration.ConfigurationManager.AppSettings["GBUpdate.Cache"];
        UpdateInfoCache ret = new UpdateInfoCache();
        ret.InitCache(connStr, BuffaloCacheTypes.Memcached);
        return ret;
    }
    protected override string CacheKey
    {
        get { return "GB.Update"; }
    }
    /// <summary>
    /// 关联的ID
    /// </summary>
    private static string DataKey = "NONE";
    protected override Dictionary<string, GBVersionItem> SearchValue(string id)
    {
        GBUpdateItemBusiness bo = new GBUpdateItemBusiness();
        ScopeList lstScope = new ScopeList();
        lstScope.ShowEntity.Add(GBDB.GBUpdateItem.UpdateInfo);
        List<GBUpdateItem> lst = bo.SelectList(lstScope);
        Dictionary<string, GBVersionItem> retDic = new Dictionary<string, GBVersionItem>();
        GBVersionItem curitem = null;
        foreach (GBUpdateItem item in lst)
        {
            string key = GetKey(item.UpdateInfo.GameCode, item.UpdateInfo.Platform);
            if (!retDic.TryGetValue(key, out curitem))
            {
                curitem = new GBVersionItem();
                retDic[key] = curitem;
            }
            curitem.AddItem(item.UpdateInfo.Version, item);
        }
        return retDic;
    }
    /// <summary>
    /// 获取键
    /// </summary>
    /// <param name="game">游戏</param>
    /// <param name="platform">平台</param>
    /// <returns></returns>
    private string GetKey(string game, string platform)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(game.ToLower());
        sb.Append(".");
        sb.Append(platform.ToLower());
        return sb.ToString();
    }

    public void ClearData()
    {
        this.IncVersion(DataKey);
    }

    /// <summary>
    /// 获取升级配置
    /// </summary>
    /// <param name="game"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    public List<GBUpdateItem> GetUpdate(string game, string platform, string version)
    {
        Dictionary<string, GBVersionItem> map = this.GetValue(DataKey);
        string key = GetKey(game, platform);
        GBVersionItem item = map.GetMapValue<GBVersionItem>(key);
        if (item == null)
        {
            return null;
        }
        string ver = GetVersion(version);
        List<GBUpdateItem> ret = item.GetMapValue<List<GBUpdateItem>>(ver);
        if (ret == null)
        {
            ret = item.GetMapValue<List<GBUpdateItem>>(item.DefaultVersion);
        }
        foreach (GBUpdateItem citem in ret)
        {
            citem.Path = citem.Path.Trim(' ', '\r', '\n', '\t', '/', '\\');
        }
        return ret;
    }
    /// <summary>
    /// 获取版本号
    /// </summary>
    /// <param name="version"></param>
    /// <returns></returns>
    private string GetVersion(string version)
    {
        string[] verPart = version.Split('.');
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < verPart.Length; i++)
        {
            if (i > 2)
            {
                break;
            }
            string part = verPart[i];
            if (string.IsNullOrWhiteSpace(part))
            {
                continue;
            }

            sb.Append(part);
            sb.Append('.');
        }
        if (sb.Length > 0)
        {
            sb.Remove(sb.Length - 1, 1);
        }
        return sb.ToString();
    }
}*/