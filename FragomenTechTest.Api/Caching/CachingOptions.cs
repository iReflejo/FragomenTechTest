namespace FragomenTechTest.Api.Caching
{
    public class CachingOptions
    {
        public bool CacheEnabled { get; set; }
        public TimeSpan? CacheAbsoluteExpiration { get; set; }
        public TimeSpan? CacheSlidingExpiration { get; set; }
    }
}