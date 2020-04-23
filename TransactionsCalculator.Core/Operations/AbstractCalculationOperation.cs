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

        private readonly int currencyPrecision = 2;

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
        protected virtual decimal RoundAmount(decimal amount)
        {
            return Math.Round(amount, currencyPrecision);
        }

        public virtual decimal CalculateAmount(IEnumerable<ITransaction> transactions)
        {
            return this.RoundAmount(this.Calculate(transactions));
        }

        protected virtual decimal GetTransactionExchangeRate(ITransaction x)
        {
            return GetExchangeRate(x.TransactionCurrencyCode, x.TransactionCompleteDate);
        }

        protected abstract decimal Calculate(IEnumerable<ITransaction> transactions);
    }
}
