using CryptoExchange.Models;
using CryptoExchange.Models.Enums;
using CryptoExchange.Models.Exchanges.Bybit;

namespace CryptoExchange.Abstract.Exchanges
{
    public interface IBybitExchangeService
    {
        Task<SymbolResult?> GetSymbolsAsync(SymbolTypeEnum type, CancellationToken ct = default);
        Task<SymbolResult?> GetSymbolsAsync(SymbolTypeEnum type, SymbolPairEnum pair, CancellationToken ct = default);
        Task<IEnumerable<SymbolInfo>?> GetSymbolInfosAsync(SymbolTypeEnum type, SymbolPairEnum pair, CancellationToken ct = default);
    }
}
