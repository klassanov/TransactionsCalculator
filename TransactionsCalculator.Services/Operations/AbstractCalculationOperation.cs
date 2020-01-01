using System;
using System.Collections.Generic;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Operations;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Operations
{
    public abstract class AbstractCalculationOperation : ICalculationOperation
    {
        protected readonly IExchangeRatesService exchangeService;
        protected readonly IAppConfigurationService appConfigurationService;
        protected readonly ICalculationParameters calculationParameters;

        public AbstractCalculationOperation(
            IExchangeRatesService exchangeService,
            IAppConfigurationService appConfigurationService,
            ICalculationParameters calculationParameters)
        {
            this.exchangeService = exchangeService;
            this.appConfigurationService = appConfigurationService;
            this.calculationParameters = calculationParameters;
        }

        protected decimal GetExchangeRate(string currencyCode)
        {
            return this.exchangeService.GetExchangeRate(currencyCode);
        }

        protected decimal RoundAmount(decimal amount)
        {
            return Math.Round(amount);
        }

        public abstract decimal Calculate(IEnumerable<ITransaction> transactions);
    }
}
