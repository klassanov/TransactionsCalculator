using TransactionsCalculator.Interfaces.Models;

namespace TransactionsCalculator.Interfaces.Services
{
    public interface IPresentationService
    {
        void Present(IDirectoryProcessingResult directoryProcessingResult);
    }
}
