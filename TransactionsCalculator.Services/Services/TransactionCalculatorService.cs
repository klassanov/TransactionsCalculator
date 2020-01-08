using log4net;
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
        private static ILog logger = LogManager.GetLogger(typeof(TransactionCalculatorService));
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

        public IDirectoryProcessingResult ProcessDirectory()
        {
            DirectoryProcessingResult directoryOperationResult = new DirectoryProcessingResult(this.serviceArgs.WorkingDirectory);
            List<ICalculationOperation> calculationOperations = CreateCalculationOperations();
            string[] filePaths = this.GetFilePathsInWorkingDirectory();
            logger.Info($"Processing directory {this.serviceArgs.WorkingDirectory} - {filePaths.Length} files found");
            logger.Info(string.Empty);

            foreach (string filePath in filePaths)
            {
                logger.Debug($"{filePath} - processing");
                FileOperationResult fileOperationResult = new FileOperationResult(filePath);
                directoryOperationResult.FileOperationResultList.Add(fileOperationResult);

                try
                {
                    IEnumerable<ITransaction> transactionList = this.fileReaderService.ReadFile(filePath);
                    List<ICalculationOperationResult> calculationOperationsResultList = new List<ICalculationOperationResult>();

                    foreach (ICalculationOperation operation in calculationOperations)
                    {
                        logger.Debug($"{filePath} - performing {operation.OperationDescription}");
                        decimal result = operation.Calculate(transactionList);
                        calculationOperationsResultList.Add(new CalculationOperationResult(result, operation.OperationDescription));
                        logger.Info($"{filePath} - {operation.OperationDescription}: {result}");
                    }

                    fileOperationResult.OperationsResultList = calculationOperationsResultList;
                    logger.Debug($"{filePath} processed successfully");
                    logger.Info(string.Empty);
                }
                catch (Exception ex)
                {
                    logger.Error($"{filePath} - Error while processing ", ex);
                    fileOperationResult.Exception = ex;
                }
            }

            directoryOperationResult.exchangeRateInfoList = exchangeService.GetAllExchangeRates();

            return directoryOperationResult;
        }

        private string[] GetFilePathsInWorkingDirectory()
        {
            return Directory.GetFiles(this.serviceArgs.WorkingDirectory, $"*.{appConfigurationService.FileExtension}");
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
