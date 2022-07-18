using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Caching
{
    public class Cache : ICache
    {
        private readonly MemoryCache _cache = new(new MemoryCacheOptions());
        private static readonly SemaphoreSlim _semaphore = new(1, 1);

        public async Task<T> GetOrCreateAsync<T>(object key, Func<Task<T>> getFromDb)
        {
            if (!_cache.TryGetValue(key, out T cacheEntry))
            {
                await _semaphore.WaitAsync();

                try
                {
                    if (!_cache.TryGetValue(key, out cacheEntry))
                    {
                        cacheEntry = await getFromDb();
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSize(1)
                        .SetPriority(CacheItemPriority.High)
                        .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                        .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                        _cache.Set(key, cacheEntry, cacheEntryOptions);
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            return cacheEntry;
        }
    }
}
