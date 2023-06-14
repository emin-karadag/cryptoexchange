using CryptoExchange.Models.Enums;
using CryptoExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
