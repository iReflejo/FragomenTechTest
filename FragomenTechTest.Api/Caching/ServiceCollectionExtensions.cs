namespace FragomenTechTest.Api.Caching
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            services.AddOptions<CachingOptions>().Bind(configuration.GetSection("Caching")).ValidateDataAnnotations();

            //Use MemoryCache decorator to use global platform cache settings
            services.AddSingleton<IFragomenMemoryCache, FragomenInMemoryCache>();

            return services;
        }
        // Example Usage: 

        //var cacheKey =
        //        CacheKey.With<UserManager>(this.apiRequestContextAccessor, "FindByNameAsync", userName);
        //    return await apiCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
        //{
        //    cacheEntry.AddExpirationToken(UserCacheRegion.CreateChangeToken());
        //    var user = await base.FindByNameAsync(userName);
        //    await this.store.GetUserRoles(user);
        //    return user;
        //}, false);
    }
}