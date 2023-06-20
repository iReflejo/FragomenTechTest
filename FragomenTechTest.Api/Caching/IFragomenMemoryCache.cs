using Microsoft.Extensions.Caching.Memory;

namespace FragomenTechTest.Api.Caching
{
    public interface IFragomenMemoryCache : IMemoryCache
    {
        MemoryCacheEntryOptions GetDefaultCacheEntryOptions();
    }
}