using System.Collections.Generic;
using System.Linq;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Operations
{
    public class StepFourTwoCalculationOperation : AbstractCalculationOperation
    {
        public StepFourTwoCalculationOperation(
            IExchangeRatesService exchangeService,
            IAppConfigurationService appConfigurationService)
            : base(exchangeService, appConfigurationService)
        {
            this.operationDescription = "Step 4.2";
        }

        public override decimal Calculate(IEnumerable<ITransaction> transactions)
        {
            return RoundAmount(transactions.Where(x => string.IsNullOrEmpty(x.TaxableJurisdiction) &&
                                                  x.TotalActivityVATIncludedAmount.HasValue)
                                           .Sum(x => x.TotalActivityVATIncludedAmount.Value * GetExchangeRate(x.TransactionCurrencyCode, x.TransactionCompleteDate)));
        }
    }
}
