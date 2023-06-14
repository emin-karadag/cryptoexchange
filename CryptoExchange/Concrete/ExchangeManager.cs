using BinanceTR.Business.Abstract;
using Bybit.Business.Abstract;
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
        private readonly IBybitService _bybitService;

        public ExchangeManager(IExchangeCacheService exchangeCacheService, IBinanceTrService binanceTrService, IBybitService bybitService)
        {
            _exchangeCacheService = exchangeCacheService;
            _binanceTrService = binanceTrService;
            _bybitService = bybitService;

            BinanceTr = new BinanceTrExchangeManager(_exchangeCacheService, _binanceTrService);
            Bybit = new BybitExchangeManager(_exchangeCacheService, _bybitService);
        }

        public IBinanceTrExchangeService BinanceTr { get; set; }
        public IBybitExchangeService Bybit { get; set; }

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
