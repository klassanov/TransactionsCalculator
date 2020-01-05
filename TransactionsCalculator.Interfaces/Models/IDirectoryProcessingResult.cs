using System.Collections.Generic;

namespace TransactionsCalculator.Interfaces.Models
{
    public interface IDirectoryProcessingResult
    {
        string WorkingDirectory { get; }

        List<IFileOperationResult> FileOperationResultList { get; }

        IEnumerable<IExchangeRateInfo> exchangeRateInfoList { get; }
    }
}
