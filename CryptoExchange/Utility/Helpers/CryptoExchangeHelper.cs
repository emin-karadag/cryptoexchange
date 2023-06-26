namespace CryptoExchange.Utility.Helpers
{
    public class CryptoExchangeHelper
    {
        public const string _exchangeBinanceTrPrefix = "BinanceTr";
        public const string _exchangeBybitPrefix = "Bybit";
        public const string _exchangeBinancePrefix = "Binance";
        public static int GetPrecission(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            value = value.Replace(",", ".").TrimEnd('0');
            var index = value.IndexOf(".");
            if (index != -1)
            {
                var precission = value[index..].Length - 1;
                return precission;
            }
            return 0;
        }
    }
}
