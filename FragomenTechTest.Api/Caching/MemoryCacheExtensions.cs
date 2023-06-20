using System.Collections.Concurrent;
using LanguageExt.Common;
using Microsoft.Extensions.Caching.Memory;

namespace FragomenTechTest.Api.Caching
{
    public static class MemoryCacheExtensions
    {
        public static async Task<Result<TItem>> GetOrCreateExclusiveAsync<TItem>(this IMemoryCache cache,
            string key, Func<MemoryCacheEntryOptions, Task<Result<TItem>>> factory,
            bool cacheFailValue = true)
        {
            if (!cache.TryGetValue(key, out var result))
            {
                using (await AsyncLock.GetLockByKey(key).LockAsync())
                {
                    if (!cache.TryGetValue(key, out result))
                    {
                        var options = cache is IFragomenMemoryCache memoryCache
                            ? memoryCache.GetDefaultCacheEntryOptions()
                            : new MemoryCacheEntryOptions();
                        result = await factory(options);
                        if (cacheFailValue || ((Result<TItem>)result).IsSuccess)
                        {
                            cache.Set(key, result, options);
                        }
                    }
                }
            }

            return (Result<TItem>) result;
        }
    }
}