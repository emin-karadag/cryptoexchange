using BinanceTR.Business.Abstract;
using CryptoExchange.Abstract.Exchanges;
using CryptoExchange.Abstract.General;
using CryptoExchange.Models;
using CryptoExchange.Models.Enums;
using CryptoExchange.Models.Exchanges.BinanceTr;
using CryptoExchange.Utility.Helpers;
using System.Globalization;

namespace CryptoExchange.Concrete.Exchanges
{
    public class BinanceTrExchangeManager : IBinanceTrExchangeService
    {
        private readonly IExchangeCacheService _exchangeCacheService;
        private readonly IBinanceTrService _binanceTrService;

        public BinanceTrExchangeManager(IExchangeCacheService exchangeCacheService, IBinanceTrService binanceTrService)
        {
            _exchangeCacheService = exchangeCacheService;
            _binanceTrService = binanceTrService;
        }

        public async Task<SymbolResult?> GetSymbolsAsync(SymbolTypeEnum type, CancellationToken ct = default)
        {
            var cacheKey = $"{CryptoExchangeHelper._exchangeBinanceTrPrefix}_Symbols_{type}";
            if (!_exchangeCacheService.TryGet(cacheKey, out SymbolResult data))
            {
                var result = await _binanceTrService.Common.GetSymbolsAsync(ct);
                if (result.Success && result.Data?.Count > 0)
                {
                    data = new SymbolResult { Spot = result.Data };
                    _exchangeCacheService.Set(cacheKey, data);
                }
            }

            return data;
        }

        public async Task<SymbolResult?> GetSymbolsAsync(SymbolTypeEnum type, SymbolPairEnum pair, CancellationToken ct = default)
        {
            var quoteAsset = pair.ToString();
            var cacheKey = $"{CryptoExchangeHelper._exchangeBinanceTrPrefix}_Symbols_{type}_{quoteAsset}";

            if (!_exchangeCacheService.TryGet(cacheKey, out SymbolResult data))
            {
                data = new SymbolResult();
                var symbols = await GetSymbolsAsync(type, ct);
                if (symbols is not null)
                {
                    data.Spot = symbols.Spot?.Where(p => p.QuoteAsset == quoteAsset).ToList();
                    _exchangeCacheService.Set(cacheKey, data);
                }
            }

            return data;
        }

        public async Task<IEnumerable<SymbolInfo>?> GetSymbolInfosAsync(SymbolTypeEnum type, SymbolPairEnum pair, CancellationToken ct = default)
        {
            var cacheKey = $"{CryptoExchangeHelper._exchangeBinanceTrPrefix}_SymbolInfos_{type}_{pair}";

            if (!_exchangeCacheService.TryGet(cacheKey, out IEnumerable<SymbolInfo>? data))
            {
                var symbols = await GetSymbolsAsync(type, pair, ct);
                if (symbols is not null)
                {
                    var result = symbols.Spot?.Select(symbol => new SymbolInfo
                    {
                        Name = symbol.Symbol.Replace("_", ""),
                        Symbol = symbol.Symbol,
                        MinAmount = symbol.Filters.Find(p => p.FilterType == "NOTIONAL")?.MinNotional ?? 0,
                        MinQty = symbol.Filters.Find(p => p.FilterType == "LOT_SIZE")?.MinQty ?? 0,
                        PricePrecission = CryptoExchangeHelper.GetPrecission(symbol.Filters.Find(p => p.FilterType == "PRICE_FILTER")?.TickSize.ToString(CultureInfo.InvariantCulture)),
                        QuantityPrecission = CryptoExchangeHelper.GetPrecission(symbol.Filters.Find(p => p.FilterType == "LOT_SIZE")?.StepSize.ToString(CultureInfo.InvariantCulture))
                    });

                    if (result?.Any() ?? false)
                    {
                        data = result;
                        _exchangeCacheService.Set(cacheKey, data);
                    }
                }
            }
            return data;
        }
    }
}
