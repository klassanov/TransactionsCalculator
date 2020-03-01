using System;
using System.Collections.Generic;
using TransactionsCalculator.Interfaces.Models;

namespace TransactionCalculator.Models.Operations
{
    public class FileOperationResult : IFileOperationResult
    {
        public string FilePath { get; set; }

        public List<ICalculationOperationResult> OperationsResultList { get; set; }

        public Exception Exception { get; set; }

        public FileOperationResult(string filePath)
        {
            this.FilePath = filePath;
            this.OperationsResultList = new List<ICalculationOperationResult>();
        }
    }
}
