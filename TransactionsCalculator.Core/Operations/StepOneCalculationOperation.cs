using System;
using System.Collections.Generic;
using System.Linq;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Operations
{
    public class StepOneCalculationOperation : AbstractCalculationOperation
    {
        public StepOneCalculationOperation(
            IExchangeRatesService exchangeService,
            IAppConfigurationService appConfigurationService)
            : base(exchangeService, appConfigurationService)
        {
            this.operationDescription = "SALES-IT";
        }

        protected override decimal Calculate(IEnumerable<ITransaction> transactionList)
        {
            return transactionList.Where(x => this.appConfigurationService.ReferenceCountryCode.Equals(x.SaleArrivalCountry) &&
                                              this.appConfigurationService.EUCountryCodes.Contains(x.SaleDepartureCountry, StringComparer.OrdinalIgnoreCase) &&
                                                         x.TotalActivityVATIncludedAmount.HasValue)
                                               .Sum(x => x.TotalActivityVATIncludedAmount.Value * GetTransactionExchangeRate(x));
        }
    }
}
