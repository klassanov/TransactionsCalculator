using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using TransactionsCalculator.Core.Factories;
using TransactionsCalculator.Core.Services;
using TransactionsCalculator.Core.WebApiClients;
using TransactionsCalculator.Interfaces.Factories;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Presenters;
using TransactionsCalculator.Interfaces.Services;
using TransactionsCalculator.Interfaces.WebApiClients;
using TransactionsCalculator.Presenters.Presenters;

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
            IConfiguration appSettings = LoadAppConfiguration();
            CreateAppConfigurationService(appSettings);
            RegisterServices();

            ITransactionCalculatorService transactionCalculatorService = serviceProvider.GetService<ITransactionCalculatorService>();
            IDirectoryProcessingResult processResult = transactionCalculatorService.ProcessDirectory();

            IPresentationService presentationService = serviceProvider.GetService<IPresentationService>();
            presentationService.Present(processResult);

            logger.Info(string.Empty);
            logger.Info("Done!");
        }

        private static void RegisterServices()
        {
            serviceProvider = new ServiceCollection()
                .AddSingleton<IAppConfigurationService>(appConfigurationService)
                .AddTransient<IExchangeRatesService, ExchangeRatesService>()
                .AddTransient<IExchangeRatesApiClient, FrankfurterWebApiClient>()
                .AddTransient<ICalculationOperationsFactory, CalculationOperationsFactory>()
                .AddTransient<IFileReaderService, FileReaderService>()
                .AddTransient<ITransactionCalculatorService, TransactionCalculatorService>()
                .AddTransient<IPresentationService, PresentationService>()
                .AddTransient<IPresenter, PDFPresenter>()
                .AddTransient<IPresenter, ExcelPresenter>()
                .BuildServiceProvider();

            logger.Debug("Services registrated");
        }

        private static IConfiguration LoadAppConfiguration()
        {
            IConfiguration appSettings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            logger.Debug("AppConfiguration Loaded");

            return appSettings;
        }

        private static void CreateAppConfigurationService(IConfiguration appSettings)
        {
            appConfigurationService = new AppConfigurationService()
            {
                ReferenceCurrencyCode = appSettings["referenceCurrencyCode"],
                ReferenceCountryCode = appSettings["referenceCountryCode"],
                FileDelimiter = appSettings["fileDelimiter"],
                FileExtension = appSettings["fileExtension"],
                ReferenceTaxableJurisdiction = appSettings["referenceTaxableJurisdiction"],
                ProduceExcel = bool.Parse(appSettings["produceExcel"]),
                ProducePdf = bool.Parse(appSettings["producePdf"]),
                WorkingDirectory = inputArgs[0]
            };
        }

        private static void LoadLoggingConfiguration()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            logger.Debug("Logging Configuration Loaded");
        }
    }
}
