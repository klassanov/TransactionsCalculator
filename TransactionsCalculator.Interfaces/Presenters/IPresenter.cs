using TransactionsCalculator.Interfaces.Models;

namespace TransactionsCalculator.Interfaces.Presenters
{
    public interface IPresenter
    {
        void PresentInfo(IDirectoryProcessingResult directoryProcessingResult);
    }
}
