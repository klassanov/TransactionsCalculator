using TransactionsCalculator.Interfaces.Models;

namespace TransactionsCalculator.Interfaces.Services
{
    public interface ITransactionCalculatorService
    {
        IDirectoryProcessingResult ProcessDirectory();
    }
}
