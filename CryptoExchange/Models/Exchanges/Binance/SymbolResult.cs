using Binance.Net.Objects.Models.Futures;
using Binance.Net.Objects.Models.Spot;

namespace CryptoExchange.Models.Exchanges.Binance
{
    public class SymbolResult
    {
        public IEnumerable<BinanceSymbol>? Spot { get; set; }
        public IEnumerable<BinanceFuturesUsdtSymbol>? UsdFutures { get; set; }
        public IEnumerable<BinanceFuturesCoinSymbol>? CoinFutures { get; set; }
    }
}
