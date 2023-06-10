using CryptoExchange.Abstract.General;
using Microsoft.Extensions.Caching.Memory;

namespace CryptoExchange.Concrete.General.Caching
{
    public class ExchangeCacheManager : IExchangeCacheService
    {
        private IMemoryCache _memoryCache;

        public ExchangeCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool TryGet<T>(string cacheKey, out T value)
        {
            return _memoryCache.TryGetValue(cacheKey, out value);
        }

        public T Set<T>(string cacheKey, T value, int expirationInMinutes)
        {
            return _memoryCache.Set(cacheKey, value, TimeSpan.FromMinutes(expirationInMinutes));
        }

        public void Remove(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }

        public bool Contains(string cacheKey)
        {
            return _memoryCache.Get(cacheKey) is not null;
        }

        public void ResetAllCache()
        {
            var existingCache = _memoryCache;
            _memoryCache = new MemoryCache(new MemoryCacheOptions());

            // Dispose existing cache (we override) in 10 minutes
            if (existingCache != null)
            {
                Task.Delay(TimeSpan.FromMinutes(10))
                    .ContinueWith(t =>
                    {
                        existingCache.Dispose();
                    });
            }
        }
    }
}
