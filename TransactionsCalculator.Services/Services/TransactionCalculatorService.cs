using System;
using System.Collections.Generic;
using System.IO;
using TransactionCalculator.Models.Operations;
using TransactionsCalculator.Core.Operations;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Operations;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Services
{
    public class TransactionCalculatorService : ITransactionCalculatorService
    {
        private readonly IAppConfigurationService appConfigurationService;
        private readonly IFileReaderService fileReaderService;
        private readonly IExchangeRatesService exchangeService;
        private readonly IServiceArgs serviceArgs;

        public TransactionCalculatorService(
            IAppConfigurationService appConfigurationService,
            IFileReaderService fileReaderService,
            IExchangeRatesService exchangeService,
            IServiceArgs serviceArgs)
        {
            this.appConfigurationService = appConfigurationService;
            this.fileReaderService = fileReaderService;
            this.exchangeService = exchangeService;
            this.serviceArgs = serviceArgs;
        }

        public void ProcessDirectory()
        {
            List<ICalculationOperation> calculationOperations = CreateCalculationOperations();
            string[] filePaths = Directory.GetFiles(this.serviceArgs.WorkingDirectory, appConfigurationService.FileExtension);
            foreach (string filePath in filePaths)
            {
                try
                {
                    IEnumerable<ITransaction> transactionList = this.fileReaderService.ReadFile(filePath);
                    foreach (ICalculationOperation operation in calculationOperations)
                    {
                        decimal result = operation.Calculate(transactionList);
                    }
                }
                catch (Exception ex)
                {
                    //Log Exception while processing filePath
                }
            }
        }

        private List<ICalculationOperation> CreateCalculationOperations()
        {
            ICalculationParameters calculationParameters = new CalculationParameters()
            {
                SaleArrivalCountry = "IT"
            };

            return new List<ICalculationOperation>()
            {
               new StepOneCalculationOperation(exchangeService, appConfigurationService, calculationParameters),
               new StepTwoCalculationOperation(exchangeService, appConfigurationService, calculationParameters)
            };
        }
    }
}
