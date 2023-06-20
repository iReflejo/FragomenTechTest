using System.Collections.Concurrent;

namespace FragomenTechTest.Api.Caching
{
    public static class AsyncLock
    {
        private static readonly ConcurrentDictionary<string, Nito.AsyncEx.AsyncLock> LockMap = new();

        public static Nito.AsyncEx.AsyncLock GetLockByKey(string key)
        {
            return LockMap.GetOrAdd(key, (x) => new Nito.AsyncEx.AsyncLock());
        }
    }
}