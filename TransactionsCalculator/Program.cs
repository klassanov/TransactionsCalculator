using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using TransactionCalculator.Models.Operations;
using TransactionsCalculator.Core.Services;
using TransactionsCalculator.Core.WebApiClients;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;
using TransactionsCalculator.Interfaces.WebApiClients;

namespace TransactionsCalculator
{
    class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));
        private static IServiceProvider serviceProvider;
        private static AppConfigurationService appConfigurationService;
        private static string[] inputArgs;

        static void Main(string[] args)
        {
            inputArgs = args;

            LoadLoggingConfiguration();
            LoadAppConfiguration();
            RegisterServices();

            ITransactionCalculatorService transactionCalculatorService = serviceProvider.GetService<ITransactionCalculatorService>();
            IDirectoryProcessingResult processResult = transactionCalculatorService.ProcessDirectory();

            IDirectoryProcessingResultPrinterService directoryProcessingResultPrinterService = serviceProvider.GetService<IDirectoryProcessingResultPrinterService>();
            directoryProcessingResultPrinterService.PrintExchangeRatesToConsole(processResult);

            logger.Info(string.Empty);
            logger.Info("Done!");
        }

        private static void RegisterServices()
        {
            serviceProvider = new ServiceCollection()
                .AddSingleton<IServiceArgs>(new ServiceArgs() { WorkingDirectory = inputArgs[0] })
                .AddSingleton<IAppConfigurationService>(appConfigurationService)
                .AddTransient<IExchangeRatesService, ExchangeRatesService>()
                .AddTransient<IExchangeRatesApiClient, FrankfurterWebApiClient>()
                .AddTransient<IFileReaderService, FileReaderService>()
                .AddTransient<ITransactionCalculatorService, TransactionCalculatorService>()
                .AddTransient<IDirectoryProcessingResultPrinterService, DirectoryProcessingResultConsolePrinterService>()
                .BuildServiceProvider();

            logger.Debug("Services registrated");
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

            logger.Debug("AppConfiguration Loaded");
        }

        private static void LoadLoggingConfiguration()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            logger.Debug("Logging Configuration Loaded");
        }
    }
}
