using BinanceTR.Business.Abstract;
using BinanceTR.Business.Concrete;
using CryptoExchange.Abstract;
using CryptoExchange.Abstract.General;
using CryptoExchange.Concrete;
using CryptoExchange.Concrete.General.Caching;
using CryptoExchange.Test;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMemoryCache();
        services.AddSingleton<IBinanceTrService, BinanceTrManager>();
        services.AddSingleton<IExchangeCacheService, ExchangeCacheManager>();
        services.AddSingleton<IExchangeService, ExchangeManager>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
