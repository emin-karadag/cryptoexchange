﻿using CryptoExchange.Abstract.Exchanges;

namespace CryptoExchange.Abstract
{
    public interface IExchangeService
    {
        public IBinanceTrExchangeService BinanceTr { get; set; }
    }
}