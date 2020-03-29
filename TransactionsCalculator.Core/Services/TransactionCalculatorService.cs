using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TransactionCalculator.Models.Operations;
using TransactionsCalculator.Interfaces.Factories;
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
        private readonly ICalculationOperationsFactory calculationOperationsFactory;

        public TransactionCalculatorService(
            IAppConfigurationService appConfigurationService,
            IFileReaderService fileReaderService,
            IExchangeRatesService exchangeService,
            ICalculationOperationsFactory calculationOperationFactory)
        {
            this.appConfigurationService = appConfigurationService;
            this.fileReaderService = fileReaderService;
            this.exchangeService = exchangeService;
            this.calculationOperationsFactory = calculationOperationFactory;
        }

        public IDirectoryProcessingResult ProcessDirectory()
        {
            DirectoryProcessingResult directoryOperationResult = new DirectoryProcessingResult(this.appConfigurationService.WorkingDirectory, this.GetFilePaths(), this.GetCalculationOperations());

            foreach (var fileOperationResult in directoryOperationResult.FileOperationResultList)
            {
                logger.Info($"Processing {fileOperationResult.FilePath}");
                try
                {
                    IEnumerable<ITransaction> transactionList = this.fileReaderService.ReadFile(fileOperationResult.FilePath);
                    this.PerformCalculationOperations(fileOperationResult, transactionList);
                    logger.Debug($"{fileOperationResult.FilePath} processed successfully");
                    logger.Info(string.Empty);
                }
                catch (Exception ex)
                {
                    logger.Error($"{fileOperationResult.FilePath} - Error while processing ", ex);
                    fileOperationResult.Exception = ex;
                }
            }

            directoryOperationResult.ExchangeRateInfoList = exchangeService.GetAllExchangeRates();

            return directoryOperationResult;
        }

        private void PerformCalculationOperations(IFileOperationResult fileOperationResult, IEnumerable<ITransaction> transactionList)
        {
            foreach (var calculationOperationResult in fileOperationResult.OperationsResultList)
            {
                logger.Debug($"Performing {calculationOperationResult.OperationDescription}");
                try
                {
                    decimal result = calculationOperationResult.CalculationOperation.Calculate(transactionList);
                    calculationOperationResult.CalulatedAmount = result;
                    logger.Info($"{calculationOperationResult.OperationDescription}: {result}");
                }
                catch (Exception ex)
                {
                    calculationOperationResult.Exception = ex;
                    logger.Error($"Error durning performing {calculationOperationResult.OperationDescription} ", ex);
                }
            }
        }

        private IEnumerable<ICalculationOperation> GetCalculationOperations()
        {
            IEnumerable<ICalculationOperation> calculationOperations = calculationOperationsFactory.CreateCalculationOperations();
            logger.Info($"{calculationOperations.Count()} calulations will be executed on each file: {string.Join(";", calculationOperations.Select(x => x.OperationDescription))}");
            logger.Info(string.Empty);
            return calculationOperations;
        }

        private string[] GetFilePaths()
        {
            string[] filePaths = Directory.GetFiles(this.appConfigurationService.WorkingDirectory, $"*.{appConfigurationService.FileExtension}");
            logger.Info($"Processing directory {this.appConfigurationService.WorkingDirectory} - {filePaths.Length} files found");
            logger.Info(string.Empty);
            return filePaths;
        }
    }
}
