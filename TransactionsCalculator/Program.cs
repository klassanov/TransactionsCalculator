using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using TransactionCalculator.Models;
using TransactionsCalculator.Core.Services;
using TransactionsCalculator.Core.WebApiClients;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;
using TransactionsCalculator.Interfaces.WebApiClients;

namespace TransactionsCalculator
{
    class Program
    {
        private static IServiceProvider serviceProvider;
        private static AppConfigurationService appConfigurationService;
        private static string[] inputArgs;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            inputArgs = args;
            LoadAppConfiguration();
            RegisterServices();
            ITransactionCalculatorService transactionCalculatorService = serviceProvider.GetService<ITransactionCalculatorService>();
            transactionCalculatorService.ProcessDirectory();
            Console.WriteLine("All done!");
        }

        private static void RegisterServices()
        {
            serviceProvider = new ServiceCollection()
                .AddSingleton<IServiceArgs>(new ServiceArgs() { WorkingDirectory = inputArgs[0] })
                .AddSingleton<IAppConfigurationService>(appConfigurationService)
                .AddTransient<IExchangeRatesApiClient, FrankfurterWebApiClient>()
                .AddTransient<IExchangeRatesService, ExchangeRatesService>()
                .AddTransient<IFileReaderService, FileReaderService>()
                .AddTransient<ITransactionCalculatorService, TransactionCalculatorService>()
                .BuildServiceProvider();
        }

        private static void LoadAppConfiguration()
        {
            IConfiguration appSettings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            appConfigurationService = new AppConfigurationService()
            {
                ReferenceCurrencyCode = appSettings["referenceCurrencyCode"],
                FileDelimiter = appSettings["fileDelimiter"],
                FileExtension = appSettings["fileExtension"]
            };
        }
    }
}
