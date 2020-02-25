using System;
using System.Collections.Generic;
using FD.Simple.Utils.Agent;

namespace M.WFDesigner.RefData
{


    #region 缓存前缀

    /// <summary>
    /// 缓存前缀
    /// </summary> 
    public enum ERefCachePre
    {
        /// <summary>
        /// WFService
        /// </summary>
        WFS=0
    }

    #endregion
    public class RefdataRepository<T>
        where T : new()
    {
        ICache _cache;
        public RefdataRepository(ICache cache)
        {
            this._cache = cache;
        }

        protected virtual string CachePre { get; }

        protected virtual List<T> GetDataList(string key, Func<string, List<T>> func)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }
            var cacheKey = $"{CachePre}{key}";
            var list = this._cache.Get<List<T>>(cacheKey);
            if (list == null )
            {
                list = func(key);
                this._cache.Set<List<T>>(cacheKey, list, new TimeSpan(1, 1, 1));
            }
            return list;
        }
        protected virtual T GetData(string key, Func<string, T> func)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return default(T);
            }
            var cacheKey = $"{CachePre}{key}";
            var data = this._cache.Get<T>(cacheKey);
            if (data == null)
            {
                data = func(key);
                this._cache.Set<T>(cacheKey, data, new TimeSpan(1, 1, 1));
            }
            return data;
        }

        /// <summary>
        /// 失效缓存数据
        /// </summary>
        /// <returns></returns>
        protected virtual bool DisableData(string key)
        {
            bool result = true;
            var cacheKey = $"{CachePre}{key}";
            this._cache.Set<T>(cacheKey, default(T), new TimeSpan(0, 0, 0, 1));
            return result;
        }
    }
}
