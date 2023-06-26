using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Abstract.Exchanges;
using CryptoExchange.Abstract.General;
using CryptoExchange.Models;
using CryptoExchange.Models.Enums;
using CryptoExchange.Models.Exchanges.Binance;
using CryptoExchange.Utility.Helpers;
using System.Globalization;

namespace CryptoExchange.Concrete.Exchanges
{
    public class BinanceExchangeManager : IBinanceExchangeService
    {
        private readonly IExchangeCacheService _exchangeCacheService;

        public BinanceExchangeManager(IExchangeCacheService exchangeCacheService)
        {
            _exchangeCacheService = exchangeCacheService;
        }

        public async Task<SymbolResult?> GetSymbolsAsync(SymbolTypeEnum type, CancellationToken ct = default)
        {
            var cacheKey = $"{CryptoExchangeHelper._exchangeBinancePrefix}_Symbols_{type}";
            if (!_exchangeCacheService.TryGet(cacheKey, out SymbolResult data))
            {
                data = new SymbolResult();
                var client = new BinanceRestClient();

                if (type == SymbolTypeEnum.SPOT || type == SymbolTypeEnum.ALL)
                {
                    var exchangeInfoResult = await client.SpotApi.ExchangeData.GetExchangeInfoAsync(ct);
                    if (exchangeInfoResult.Success && exchangeInfoResult.Data?.Symbols?.Any() == true)
                    {
                        data.Spot = exchangeInfoResult.Data.Symbols;
                    }
                }

                if (type == SymbolTypeEnum.USD_M || type == SymbolTypeEnum.ALL)
                {
                    var exchangeInfoResult = await client.UsdFuturesApi.ExchangeData.GetExchangeInfoAsync(ct);
                    if (exchangeInfoResult.Success && exchangeInfoResult.Data?.Symbols?.Any() == true)
                    {
                        data.UsdFutures = exchangeInfoResult.Data.Symbols;
                    }
                }

                if (type == SymbolTypeEnum.COIN_M || type == SymbolTypeEnum.ALL)
                {
                    var exchangeInfoResult = await client.CoinFuturesApi.ExchangeData.GetExchangeInfoAsync(ct);
                    if (exchangeInfoResult.Success && exchangeInfoResult.Data?.Symbols?.Any() == true)
                    {
                        data.CoinFutures = exchangeInfoResult.Data.Symbols;
                    }
                }

                _exchangeCacheService.Set(cacheKey, data);
            }

            return data;
        }

        public async Task<SymbolResult?> GetSymbolsAsync(SymbolTypeEnum type, SymbolPairEnum pair, CancellationToken ct = default)
        {
            var quoteAsset = pair.ToString();
            var cacheKey = $"{CryptoExchangeHelper._exchangeBinancePrefix}_Symbols_{type}_{quoteAsset}";

            if (!_exchangeCacheService.TryGet(cacheKey, out SymbolResult data))
            {
                data = new SymbolResult();
                var symbols = await GetSymbolsAsync(type, ct);
                if (symbols is not null)
                {
                    data.Spot = symbols.Spot?.Where(p => p.QuoteAsset == quoteAsset).ToList();
                    data.UsdFutures = symbols.UsdFutures?.Where(p => p.QuoteAsset == quoteAsset).ToList();
                    data.CoinFutures = symbols.CoinFutures?.Where(p => p.QuoteAsset == quoteAsset).ToList();
                    _exchangeCacheService.Set(cacheKey, data);
                }
            }

            return data;
        }

        public async Task<IEnumerable<SymbolInfo>?> GetSymbolInfosAsync(SymbolTypeEnum type, SymbolPairEnum pair, CancellationToken ct = default)
        {
            var cacheKey = $"{CryptoExchangeHelper._exchangeBinancePrefix}_SymbolInfos_{type}_{pair}";

            if (!_exchangeCacheService.TryGet(cacheKey, out IEnumerable<SymbolInfo>? data))
            {
                var symbols = await GetSymbolsAsync(type, pair, ct);
                if (symbols is not null)
                {
                    foreach (var item in symbols.Spot)
                    {

                    }

                    var result = symbols.Spot?.Select(symbol => new SymbolInfo
                    {
                        Name = symbol.Name,
                        Symbol = symbol.Name,
                        MinAmount = ((BinanceSymbolNotionalFilter)symbol.Filters.FirstOrDefault(p => p.FilterType == SymbolFilterType.Notional))?.MinNotional ?? 0,
                        //MinQty = symbol.Filters.Find(p => p.FilterType == "LOT_SIZE")?.MinQty ?? 0,
                        PricePrecission = CryptoExchangeHelper.GetPrecission(symbol.PriceFilter?.TickSize.ToString(CultureInfo.InvariantCulture)),
                        QuantityPrecission = CryptoExchangeHelper.GetPrecission(symbol.LotSizeFilter?.StepSize.ToString(CultureInfo.InvariantCulture))
                    });

                    var result2 = symbols.CoinFutures?.Select(symbol => new SymbolInfo
                    {
                        Name = symbol.Name,
                        Symbol = symbol.Pair,
                        //MinAmount = symbol.Filters.Find(p => p.FilterType == "NOTIONAL")?.MinNotional ?? 0,
                        //MinQty = symbol.Filters.Find(p => p.FilterType == "LOT_SIZE")?.MinQty ?? 0,
                        PricePrecission = symbol.PricePrecision,
                        //QuantityPrecission = symbol.EqualQuantityPrecision,
                    });

                    //if (result?.Any() ?? false)
                    //{
                    //    data = result;
                    //    _exchangeCacheService.Set(cacheKey, data);
                    //}
                }
            }
            return data;
        }
    }
}
