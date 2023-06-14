using BinanceTR.Models.Common;
using Bybit.Entity.Models.Market;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
