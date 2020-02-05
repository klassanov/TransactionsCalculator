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
            IAppConfigurationService appConfigurationService,
            ICalculationParameters calculationParameters)
            : base(exchangeService, appConfigurationService, calculationParameters)
        {
            this.operationDescription = "Step 1";
        }

        public override decimal Calculate(IEnumerable<ITransaction> transactionList)
        {
            return RoundAmount(transactionList.Where(x => this.calculationParameters.ReferenceCountry.Equals(x.SaleArrivalCountry) &&
                                                          x.TotalActivityVatIncludedAmount.HasValue)
                                               .Sum(x => x.TotalActivityVatIncludedAmount.Value * GetExchangeRate(x.TransactionCurrencyCode, x.TaxCalculationDate)));
        }
    }
}
