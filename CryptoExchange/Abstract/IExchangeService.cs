﻿using CryptoExchange.Abstract.Exchanges;

namespace CryptoExchange.Abstract
{
    public interface IExchangeService
    {
        public IBinanceTrExchangeService BinanceTr { get; set; }
        public IBybitExchangeService Bybit { get; set; }
        public IBinanceExchangeService Binance { get; set; }

        string CreateClientId(string prefix, int maxLength = 36);
    }
}
