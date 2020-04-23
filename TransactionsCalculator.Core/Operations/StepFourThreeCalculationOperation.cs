using System.Collections.Generic;
using System.Linq;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Operations
{
    public class StepFourThreeCalculationOperation : AbstractCalculationOperation
    {
        public StepFourThreeCalculationOperation(
            IExchangeRatesService exchangeService,
            IAppConfigurationService appConfigurationService)
            : base(exchangeService, appConfigurationService)
        {
            this.operationDescription = "NO VAT IT";
        }

        protected override decimal Calculate(IEnumerable<ITransaction> transactions)
        {
            return transactions.Where(x => string.IsNullOrEmpty(x.TaxableJurisdiction) &&
                                                       this.appConfigurationService.ReferenceCountryCode.Equals(x.SaleArrivalCountry) &&
                                                       x.TotalActivityVATIncludedAmount.HasValue)
                                            .Sum(x => x.TotalActivityVATIncludedAmount.Value * GetTransactionExchangeRate(x));
        }
    }
}
