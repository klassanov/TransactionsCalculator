using System.Collections.Generic;
using TransactionsCalculator.Core.Operations;
using TransactionsCalculator.Interfaces.Factories;
using TransactionsCalculator.Interfaces.Operations;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Factories
{
    public class CalculationOperationsFactory : ICalculationOperationsFactory
    {
        private readonly IExchangeRatesService exchangeService;
        private readonly IAppConfigurationService appConfigurationService;

        public CalculationOperationsFactory(IExchangeRatesService exchangeService, IAppConfigurationService appConfigurationService)
        {
            this.exchangeService = exchangeService;
            this.appConfigurationService = appConfigurationService;
        }

        public IEnumerable<ICalculationOperation> CreateCalculationOperations()
        {
            return new List<ICalculationOperation>()
            {
               new StepOneCalculationOperation(exchangeService, appConfigurationService),
               new StepTwoCalculationOperation(exchangeService, appConfigurationService),
               new StepThreeOneCalculationOperation(exchangeService, appConfigurationService),
               new StepThreeTwoCalculationOperation(exchangeService, appConfigurationService),
               new StepThreeThreeCalculationOperation(exchangeService, appConfigurationService),
               new StepFourOneCalculationOperation(exchangeService, appConfigurationService),
               new StepFourTwoCalculationOperation(exchangeService, appConfigurationService)
            };
        }
    }
}
