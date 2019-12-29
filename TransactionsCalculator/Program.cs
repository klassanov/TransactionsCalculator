using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            LoadAppConfiguration();
            RegisterServices();
            Tests();

            //The olny call I need and then it starts
            //ITransactionCalculatorService transactionCalculatorService = serviceProvider.GetService<ITransactionCalculatorService>();
            //transactionCalculatorService.ProcessDirectory();

            Console.WriteLine("All done!");
        }

        private static void RegisterServices()
        {
            serviceProvider = new ServiceCollection()
                .AddSingleton<IAppConfigurationService>(appConfigurationService)
                .AddTransient<IExchangeRatesApiClient, FrankfurterWebApiClient>()
                .AddTransient<IExchangeRatesService, ExchangeRatesService>()
                .AddTransient<IFileReaderService, FileReaderService>()
                //Add other service dependencies
                .AddTransient<ITransactionCalculatorService, TransactionCalculatorService>() // Biggest service
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

        private static void Tests()
        {
            string directoryName = "D:\\SW Development\\Customers\\Gimmy\\TransactionCalculator\\Tests\\OriginalFiles";
            string[] filePaths = Directory.GetFiles(directoryName, "*.txt");

            IFileReaderService fileReaderService = serviceProvider.GetService<IFileReaderService>();
            IEnumerable<ITransaction> transactions = fileReaderService.ReadFile(filePaths[0]);

            var orderedTransactions = transactions.OrderByDescending(t => t.TotalActivityVatIncludedAmount);

            var s = serviceProvider.GetService<ITransactionCalculatorService>();
        }
    }
}
