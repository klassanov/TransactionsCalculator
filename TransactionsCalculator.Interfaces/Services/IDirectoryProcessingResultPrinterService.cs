using TransactionsCalculator.Interfaces.Models;

namespace TransactionsCalculator.Interfaces.Services
{
    public interface IDirectoryProcessingResultPrinterService
    {
        void Print(IDirectoryProcessingResult directoryProcessingResult);

        void PrintExchangeRatesToConsole(IDirectoryProcessingResult directoryProcessingResult);

    }
}
