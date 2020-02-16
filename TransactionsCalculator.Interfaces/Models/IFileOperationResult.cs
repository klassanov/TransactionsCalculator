using System;
using System.Collections.Generic;

namespace TransactionsCalculator.Interfaces.Models
{
    public interface IFileOperationResult
    {
        string FilePath { get; }

        string FileName { get; }

        List<ICalculationOperationResult> OperationsResultList { get; }

        public Exception Exception { get; }
    }
}
