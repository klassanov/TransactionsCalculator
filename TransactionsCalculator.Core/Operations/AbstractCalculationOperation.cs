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
        protected string operationDescription;

        public AbstractCalculationOperation(
            IExchangeRatesService exchangeService,
            IAppConfigurationService appConfigurationService)
        {
            this.exchangeService = exchangeService;
            this.appConfigurationService = appConfigurationService;
        }

        public string OperationDescription => operationDescription;

        protected decimal GetExchangeRate(string currencyCode, DateTime? transactionDate)
        {
            return this.exchangeService.GetExchangeRate(currencyCode, transactionDate);
        }
        private decimal RoundAmount(decimal amount)
        {
            return Math.Round(amount, 2);
        }

        public decimal CalculateAmount(IEnumerable<ITransaction> transactions)
        {
            return this.RoundAmount(this.Calculate(transactions));
        }

        protected abstract decimal Calculate(IEnumerable<ITransaction> transactions);
    }
}
