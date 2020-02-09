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

        public TransactionCalculatorService(
            IAppConfigurationService appConfigurationService,
            IFileReaderService fileReaderService,
            IExchangeRatesService exchangeService)
        {
            this.appConfigurationService = appConfigurationService;
            this.fileReaderService = fileReaderService;
            this.exchangeService = exchangeService;
        }

        public IDirectoryProcessingResult ProcessDirectory()
        {
            DirectoryProcessingResult directoryOperationResult = new DirectoryProcessingResult(this.appConfigurationService.WorkingDirectory);
            List<ICalculationOperation> calculationOperations = CreateCalculationOperations();
            string[] filePaths = this.GetFilePathsInWorkingDirectory();
            logger.Info($"Processing directory {this.appConfigurationService.WorkingDirectory} - {filePaths.Length} files found");
            logger.Info(string.Empty);

            foreach (string filePath in filePaths)
            {
                logger.Info($"Processing {filePath}");
                FileOperationResult fileOperationResult = new FileOperationResult(filePath);
                directoryOperationResult.FileOperationResultList.Add(fileOperationResult);

                try
                {
                    IEnumerable<ITransaction> transactionList = this.fileReaderService.ReadFile(filePath);
                    List<ICalculationOperationResult> calculationOperationsResultList = new List<ICalculationOperationResult>();

                    foreach (ICalculationOperation operation in calculationOperations)
                    {
                        logger.Debug($"Performing {operation.OperationDescription}");
                        decimal result = operation.Calculate(transactionList);
                        calculationOperationsResultList.Add(new CalculationOperationResult(result, operation.OperationDescription));
                        logger.Info($"{operation.OperationDescription}: {result}");
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

            directoryOperationResult.ExchangeRateInfoList = exchangeService.GetAllExchangeRates();

            return directoryOperationResult;
        }

        private string[] GetFilePathsInWorkingDirectory()
        {
            return Directory.GetFiles(this.appConfigurationService.WorkingDirectory, $"*.{appConfigurationService.FileExtension}");
        }

        private List<ICalculationOperation> CreateCalculationOperations()
        {           
            return new List<ICalculationOperation>()
            {
               new StepOneCalculationOperation(exchangeService, appConfigurationService),
               new StepTwoCalculationOperation(exchangeService, appConfigurationService),
               new StepThreeOneCalculationOperation(exchangeService, appConfigurationService)
            };
        }
    }
}
