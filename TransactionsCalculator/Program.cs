﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TransactionsCalculator.Core.Services;
using TransactionsCalculator.Interfaces.Models;
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
            Tests();

            //The olny call I need and then it starts
            //ITransactionCalculatorService transactionCalculatorService = serviceProvider.GetService<ITransactionCalculatorService>();
            //transactionCalculatorService.ProcessDirectory();




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

        private static void Tests()
        {
            string directoryName = "D:\\SW Development\\Customers\\Gimmy\\TransactionCalculator\\Tests\\OriginalFiles";
            string[] filePaths = Directory.GetFiles(directoryName, "*.txt");

            IFileReaderService fileReaderService = serviceProvider.GetService<IFileReaderService>();
            IEnumerable<ITransaction> transactions = fileReaderService.ReadFile(filePaths[0]);

            var orderedTransactions = transactions.OrderByDescending(t => t.TotalActivityVatIncludedAmount);
            int a = 5;
        }
    }
}
