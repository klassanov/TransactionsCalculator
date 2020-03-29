using System;
using System.Collections.Generic;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Operations;

namespace TransactionCalculator.Models.Operations
{
    public class FileOperationResult : IFileOperationResult
    {
        public string FilePath { get; set; }

        public List<ICalculationOperationResult> OperationsResultList { get; set; }

        public Exception Exception { get; set; }

        public FileOperationResult(string filePath, IEnumerable<ICalculationOperation> calculationOperations)
        {
            this.FilePath = filePath;
            this.OperationsResultList = new List<ICalculationOperationResult>();
            foreach (ICalculationOperation calculationOperation in calculationOperations)
            {
                this.OperationsResultList.Add(new CalculationOperationResult(calculationOperation));
            }
        }
    }
}
