using System;
using System.Collections.Generic;

namespace TransactionsCalculator.Interfaces.Models
{
    public interface IDirectoryProcessingResult
    {
        string WorkingDirectory { get; }

        List<IFileOperationResult> FileOperationResultList { get; }

        IEnumerable<IExchangeRateInfo> ExchangeRateInfoList { get; }

        public DateTime Timestamp { get; }
    }
}
