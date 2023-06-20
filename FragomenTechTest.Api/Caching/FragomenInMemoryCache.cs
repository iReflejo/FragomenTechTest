using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace FragomenTechTest.Api.Caching
{
    public class FragomenInMemoryCache : MemoryCache, IFragomenMemoryCache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<FragomenInMemoryCache> _logger;
        private readonly CachingOptions _cachingOptions;
        private bool _disposed;

        public FragomenInMemoryCache(IMemoryCache memoryCache, IOptions<CachingOptions> cachingOptions,
            ILogger<FragomenInMemoryCache> logger) : base(new MemoryCacheOptions())
        {
            this._memoryCache = memoryCache;
            this._logger = logger;
            this._cachingOptions = cachingOptions.Value;
        }
        
        public virtual ICacheEntry CreateEntry(object key)
        {
            var result = _memoryCache.CreateEntry(key);
            if (result != null)
            {
                result.RegisterPostEvictionCallback(callback: EvictionCallback);
                var options = GetDefaultCacheEntryOptions();
                result.SetOptions(options);
            }
            return result;
        }

        private bool CacheEnabled => _cachingOptions.CacheEnabled;
        private TimeSpan? AbsoluteExpiration => _cachingOptions.CacheAbsoluteExpiration;
        private TimeSpan? SlidingExpiration => _cachingOptions.CacheSlidingExpiration;
        
        public MemoryCacheEntryOptions GetDefaultCacheEntryOptions()
        {
            var result = new MemoryCacheEntryOptions();

            if (!CacheEnabled)
            {
                result.AbsoluteExpirationRelativeToNow = TimeSpan.FromTicks(1);
            }
            else
            {
                if (AbsoluteExpiration != null)
                {
                    result.AbsoluteExpirationRelativeToNow = AbsoluteExpiration;
                }
                else if (SlidingExpiration != null)
                {
                    result.SlidingExpiration = SlidingExpiration;
                }
            }

            return result;
        }

        ~FragomenInMemoryCache()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _memoryCache.Dispose();
                }
                _disposed = true;
            }
        }

        
        protected virtual void EvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            _logger.LogInformation($"EvictionCallback: Cache with key {key} has expired.");
        }        
    }
}