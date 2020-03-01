using System.Collections.Generic;
using TransactionsCalculator.Interfaces.Models;

namespace TransactionCalculator.Models.Operations
{
    public class DirectoryProcessingResult : IDirectoryProcessingResult
    {
        public string WorkingDirectory { get; set; }

        public List<IFileOperationResult> FileOperationResultList { get; set; }

        public IEnumerable<IExchangeRateInfo> ExchangeRateInfoList { get; set; }

        public DirectoryProcessingResult(string workingDirectory)
        {
            this.WorkingDirectory = workingDirectory;
            this.FileOperationResultList = new List<IFileOperationResult>();
        }
    }
}
