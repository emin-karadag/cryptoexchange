using Bybit.Business.Abstract;
using Bybit.Entity.Dtos.Market;
using Bybit.Entity.Models.Market;
using Bybit.Models.Enums;
using CryptoExchange.Abstract.Exchanges;
using CryptoExchange.Abstract.General;
using CryptoExchange.Models;
using CryptoExchange.Models.Enums;
using CryptoExchange.Models.Exchanges.Bybit;
using CryptoExchange.Utility.Helpers;
using System.Globalization;

namespace CryptoExchange.Concrete.Exchanges
{
    public class BybitExchangeManager : IBybitExchangeService
    {
        private readonly IExchangeCacheService _exchangeCacheService;
        private readonly IBybitService _bybitService;

        public BybitExchangeManager(IExchangeCacheService exchangeCacheService, IBybitService bybitService)
        {
            _exchangeCacheService = exchangeCacheService;
            _bybitService = bybitService;
        }

        public async Task<SymbolResult?> GetSymbolsAsync(SymbolTypeEnum type, CancellationToken ct = default)
        {
            var cacheKey = $"{CryptoExchangeHelper._exchangeBybitPrefix}_Symbols_{type}";
            if (!_exchangeCacheService.TryGet(cacheKey, out SymbolResult data))
            {
                data = new SymbolResult();
                var category = GetCategoryBySymbolType(type);
                var instrumentsResult = await _bybitService.Market.GetInstrumentsInfoAsync(new InstrumentsInfoDto { Category = category }, ct);
                if (instrumentsResult.Success && instrumentsResult.Data?.Count > 0)
                {
                    switch (category)
                    {
                        case CategoryEnum.LINEAR:
                            data.Linear = instrumentsResult.Data;
                            break;
                        case CategoryEnum.INVERSE:
                            data.Inverse = instrumentsResult.Data;
                            break;
                        case CategoryEnum.OPTION:
                            data.Option = instrumentsResult.Data;
                            break;
                        default:
                            data.Spot = instrumentsResult.Data;
                            break;
                    }

                    _exchangeCacheService.Set(cacheKey, data);
                }
            }

            return data;
        }

        public async Task<SymbolResult?> GetSymbolsAsync(SymbolTypeEnum type, SymbolPairEnum pair, CancellationToken ct = default)
        {
            var quoteAsset = pair.ToString();
            var cacheKey = $"{CryptoExchangeHelper._exchangeBybitPrefix}_Symbols_{type}_{quoteAsset}";

            if (!_exchangeCacheService.TryGet(cacheKey, out SymbolResult data))
            {
                data = new SymbolResult();
                var symbols = await GetSymbolsAsync(type, ct);
                if (symbols is not null)
                {
                    data.Spot = symbols.Spot?.Where(p => p.QuoteCoin == quoteAsset).ToList();
                    data.Linear = symbols.Linear?.Where(p => p.QuoteCoin == quoteAsset).ToList();
                    data.Inverse = symbols.Inverse?.Where(p => p.QuoteCoin == quoteAsset).ToList();
                    data.Option = symbols.Option?.Where(p => p.QuoteCoin == quoteAsset).ToList();
                    _exchangeCacheService.Set(cacheKey, data);
                }
            }

            return data;
        }

        public async Task<IEnumerable<SymbolInfo>?> GetSymbolInfosAsync(SymbolTypeEnum type, SymbolPairEnum pair, CancellationToken ct = default)
        {
            var cacheKey = $"{CryptoExchangeHelper._exchangeBybitPrefix}_SymbolInfos_{type}_{pair}";

            if (!_exchangeCacheService.TryGet(cacheKey, out IEnumerable<SymbolInfo>? data))
            {
                var symbols = await GetSymbolsAsync(type, pair, ct);
                if (symbols is not null)
                {
                    var list = new List<SymbolInfo>();
                    if (symbols.Spot is not null)
                        foreach (var symbol in symbols.Spot)
                            FillSymbolInfoList(list, symbol);

                    if (symbols.Linear is not null)
                        foreach (var symbol in symbols.Linear)
                            FillSymbolInfoList(list, symbol, CategoryEnum.LINEAR);

                    if (symbols.Inverse is not null)
                        foreach (var symbol in symbols.Inverse)
                            FillSymbolInfoList(list, symbol);

                    if (symbols.Option is not null)
                        foreach (var symbol in symbols.Option)
                            FillSymbolInfoList(list, symbol);

                    if (list?.Any() ?? false)
                    {
                        data = list;
                        _exchangeCacheService.Set(cacheKey, data);
                    }
                }
            }
            return data;
        }

        private static CategoryEnum GetCategoryBySymbolType(SymbolTypeEnum type)
        {
            switch (type)
            {
                case SymbolTypeEnum.USD_M:
                    return CategoryEnum.LINEAR;
                case SymbolTypeEnum.COIN_M:
                    return CategoryEnum.INVERSE;
                default:
                    return CategoryEnum.SPOT;
            }
        }

        private static void FillSymbolInfoList(List<SymbolInfo> list, InstrumentsInfoDataList symbol, CategoryEnum category = CategoryEnum.SPOT)
        {
            var quantityPrecission = symbol.LotSizeFilter?.BasePrecision;
            if (category == CategoryEnum.LINEAR)
                quantityPrecission = symbol.LotSizeFilter?.MinOrderQty;

            list.Add(new SymbolInfo
            {
                Name = symbol.Symbol ?? "",
                Symbol = symbol.Symbol ?? "",
                MinQty = symbol.LotSizeFilter?.MinOrderQty,
                MinAmount = symbol.LotSizeFilter?.MinOrderAmt ?? 0,
                PricePrecission = CryptoExchangeHelper.GetPrecission(symbol.PriceFilter?.TickSize.ToString(CultureInfo.InvariantCulture)),
                QuantityPrecission = CryptoExchangeHelper.GetPrecission(quantityPrecission?.ToString(CultureInfo.InvariantCulture))
            });
        }
    }
}
