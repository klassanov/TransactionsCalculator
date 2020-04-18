using System.Collections.Generic;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Presenters;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Services
{
    public class PresentationService : IPresentationService
    {
        private readonly IAppConfigurationService appConfigurationService;
        private readonly IEnumerable<IPresenter> presenters;

        public PresentationService(IAppConfigurationService appConfigurationService, IEnumerable<IPresenter> presenters)
        {
            this.appConfigurationService = appConfigurationService;
            this.presenters = presenters;
        }

        public void Present(IDirectoryProcessingResult directoryProcessingResult)
        {
            foreach (IPresenter presenter in presenters)
            {
                presenter.PresentInfo(directoryProcessingResult);
            }
        }
    }
}
