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
        private string processingDirectory;
        private readonly IAppConfigurationService appConfigurationService;
        private readonly IFileReaderService fileReaderService;
        private readonly IExchangeRatesService exchangeService;

        public TransactionCalculatorService(
            IAppConfigurationService appConfigurationService,
            IFileReaderService fileReaderService,
            IExchangeRatesService exchangeService)
        {
            this.appConfigurationService = appConfigurationService;
            this.fileReaderService = fileReaderService;
            this.exchangeService = exchangeService;
        }

        public void ProcessDirectory()
        {
            List<ICalculationOperation> calculationOperations = CreateCalculationOperations();
            string[] filePaths = Directory.GetFiles(this.processingDirectory, appConfigurationService.FileExtension);
            foreach (string filePath in filePaths)
            {
                try
                {
                    IEnumerable<ITransaction> transactionList = this.fileReaderService.ReadFile(filePath);
                    foreach (ICalculationOperation calculation in calculationOperations)
                    {
                        decimal result = calculation.Calculate(transactionList);
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
