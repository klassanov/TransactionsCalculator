using System.Collections.Generic;
using System.Linq;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Operations
{
    public class StepFourOneCalculationOperation : AbstractCalculationOperation
    {
        public StepFourOneCalculationOperation(
            IExchangeRatesService exchangeService,
            IAppConfigurationService appConfigurationService)
            : base(exchangeService, appConfigurationService)
        {
            this.operationDescription = "Step 4.1";
        }

        protected override decimal Calculate(IEnumerable<ITransaction> transactions)
        {
            return transactions.Where(x => !string.IsNullOrEmpty(x.TaxableJurisdiction)
                                                   && x.TaxableJurisdiction.Equals(appConfigurationService.ReferenceTaxableJurisdiction)
                                                   && x.TotalActivityVATAmount.HasValue)
                                           .Sum(x => x.TotalActivityVATAmount.Value * GetExchangeRate(x.TransactionCurrencyCode, x.TransactionCompleteDate));
        }
    }
}
