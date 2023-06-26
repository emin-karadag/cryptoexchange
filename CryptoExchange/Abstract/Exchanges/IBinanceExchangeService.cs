using CryptoExchange.Models.Enums;
using CryptoExchange.Models.Exchanges.Binance;

namespace CryptoExchange.Abstract.Exchanges
{
    public interface IBinanceExchangeService
    {
        Task<SymbolResult?> GetSymbolsAsync(SymbolTypeEnum type, CancellationToken ct = default);
        Task<SymbolResult?> GetSymbolsAsync(SymbolTypeEnum type, SymbolPairEnum pair, CancellationToken ct = default);
        //Task<IEnumerable<SymbolInfo>?> GetSymbolInfosAsync(SymbolTypeEnum type, SymbolPairEnum pair, CancellationToken ct = default);
    }
}
