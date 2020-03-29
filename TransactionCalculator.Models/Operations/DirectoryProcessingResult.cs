using System.Collections.Generic;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Operations;

namespace TransactionCalculator.Models.Operations
{
    public class DirectoryProcessingResult : IDirectoryProcessingResult
    {
        public string WorkingDirectory { get; set; }

        public string[] FilePaths { get; set; }

        public List<IFileOperationResult> FileOperationResultList { get; set; }

        public IEnumerable<IExchangeRateInfo> ExchangeRateInfoList { get; set; }

        public IEnumerable<ICalculationOperation> CalculationOperations { get; set; }

        public DirectoryProcessingResult()
        {
        }

        public DirectoryProcessingResult(string workingDirectory, string[] filePaths, IEnumerable<ICalculationOperation> calculationOperations)
        {
            this.WorkingDirectory = workingDirectory;
            this.FilePaths = filePaths;
            this.CalculationOperations = calculationOperations;
            this.ExchangeRateInfoList = new List<IExchangeRateInfo>();
            this.CreateFileOperationResultList();
        }

        private void CreateFileOperationResultList()
        {
            this.FileOperationResultList = new List<IFileOperationResult>();
            foreach (string filePath in FilePaths)
            {
                this.FileOperationResultList.Add(new FileOperationResult(filePath, CalculationOperations));
            }
        }
    }
}
