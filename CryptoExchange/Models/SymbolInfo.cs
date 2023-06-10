namespace CryptoExchange.Models
{
    public class SymbolInfo
    {
        public string Name { get; set; } = "";
        public string Symbol { get; set; } = "";
        public int PricePrecission { get; set; }
        public int QuantityPrecission { get; set; }

        public decimal? MinQty { get => minQty; set => minQty = value / 1.000000000000000000000000000000000m; }
        private decimal? minQty;
    }
}
