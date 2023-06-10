using CryptoExchange.Abstract;
using CryptoExchange.Models.Enums;

namespace CryptoExchange.Test
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IExchangeService _exchangeService;

        public Worker(ILogger<Worker> logger, IExchangeService exchangeService)
        {
            _logger = logger;
            _exchangeService = exchangeService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var result = await _exchangeService.BinanceTr.GetSymbolInfosAsync(SymbolTypeEnum.ALL, SymbolPairEnum.TRY, stoppingToken);
            }
            catch (Exception ex)
            {

            }
        }
    }
}