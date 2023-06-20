namespace FragomenTechTest.Api.Caching
{
    public static class CacheKey
    {
        public static string With(params string[] keys)
        {
            return string.Join("-", keys);
        }

        public static string With<T>(params object[] keys)
        {
            return With($"{typeof(T).GetCacheKey()}:{string.Join("-", keys)}");
        }
    }
}