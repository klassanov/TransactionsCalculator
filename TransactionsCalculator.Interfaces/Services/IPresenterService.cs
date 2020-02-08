using TransactionsCalculator.Interfaces.Models;

namespace TransactionsCalculator.Interfaces.Services
{
    public interface IPresenterService
    {
        void PresentInfo(IDirectoryProcessingResult directoryProcessingResult);
    }
}
