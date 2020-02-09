﻿using System.Collections.Generic;
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
            this.operationDescription = "Step 1";
        }

        public override decimal Calculate(IEnumerable<ITransaction> transactionList)
        {
            return RoundAmount(transactionList.Where(x => this.appConfigurationService.ReferenceCountry.Equals(x.SaleArrivalCountry) &&
                                                          x.TotalActivityVATIncludedAmount.HasValue)
                                               .Sum(x => x.TotalActivityVATIncludedAmount.Value * GetExchangeRate(x.TransactionCurrencyCode, x.TaxCalculationDate)));
        }
    }
}
