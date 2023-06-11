using BinanceTR.Business.Abstract;
using CryptoExchange.Abstract;
using CryptoExchange.Abstract.Exchanges;
using CryptoExchange.Abstract.General;
using CryptoExchange.Concrete.Exchanges;

namespace CryptoExchange.Concrete
{
    public class ExchangeManager : IExchangeService
    {
        private readonly IExchangeCacheService _exchangeCacheService;
        private readonly IBinanceTrService _binanceTrService;
        public ExchangeManager(IExchangeCacheService exchangeCacheService, IBinanceTrService binanceTrService)
        {
            _exchangeCacheService = exchangeCacheService;
            _binanceTrService = binanceTrService;

            BinanceTr = new BinanceTrExchangeManager(_exchangeCacheService, _binanceTrService);
        }

        public IBinanceTrExchangeService BinanceTr { get; set; }

        public string CreateClientId(string prefix, int maxLength = 36)
        {
            if (prefix.Length > maxLength)
                return "";

            var guid = Guid.NewGuid().ToString()[..maxLength].Replace("+", "").Replace("-", "").Replace("/", "");
            var id = $"{prefix}{guid}";

            return id.Length <= maxLength ? id : id[..maxLength];
        }
    }
}
