namespace CryptoExchange.Abstract.General
{
    public interface IExchangeCacheService
    {
        bool TryGet<T>(string cacheKey, out T value);
        T Set<T>(string cacheKey, T value, int expirationInMinutes = 60);
        void Remove(string cacheKey);
        bool Contains(string cacheKey);
    }
}
