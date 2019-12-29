using Microsoft.Extensions.DependencyInjection;
using System;
using TransactionsCalculator.Core.Services;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator
{
    class Program
    {
        private static IServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //IConfiguration appSettings = new ConfigurationBuilder()
            //     .SetBasePath(Directory.GetCurrentDirectory())
            //     .AddJsonFile("appsettings.json")
            //     .Build();

            RegisterServices();
            ITransactionCalculatorService transactionCalculatorService = serviceProvider.GetService<ITransactionCalculatorService>();
            transactionCalculatorService.ProcessDirectory();



            Console.WriteLine("All done!");

        }

        private static void RegisterServices()
        {
            serviceProvider = new ServiceCollection()
                .AddTransient<IFileReaderService, FileReaderService>()
                //Add other dependencies
                .AddTransient<ITransactionCalculatorService, TransactionCalculatorService>() // Biggest service
                .BuildServiceProvider();
        }
    }
}
