using Bybit.Entity.Models.Market;

namespace CryptoExchange.Models.Exchanges.Bybit
{
    public class SymbolResult
    {
        public List<InstrumentsInfoDataList>? Spot { get; set; }
        public List<InstrumentsInfoDataList>? Linear { get; set; }
        public List<InstrumentsInfoDataList>? Inverse { get; set; }
        public List<InstrumentsInfoDataList>? Option { get; set; }
    }
}
