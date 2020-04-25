using System.Collections.Generic;
using System.Linq;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Operations
{
    public class StepFourFourCalculationOperation : AbstractCalculationOperation
    {
        public StepFourFourCalculationOperation(
            IExchangeRatesService exchangeService,
            IAppConfigurationService appConfigurationService)
            : base(exchangeService, appConfigurationService)
        {
            this.operationDescription = "MAG NO VAT";
        }

        protected override decimal Calculate(IEnumerable<ITransaction> transactions)
        {
            return transactions.Where(x => !string.IsNullOrEmpty(x.TaxableJurisdiction) &&
                                           x.TaxableJurisdiction != appConfigurationService.ReferenceTaxableJurisdiction &&
                                           !string.IsNullOrEmpty(x.SaleArrivalCountry) &&
                                           x.SaleArrivalCountry.Equals(appConfigurationService.ReferenceCountryCode) &&
                                           string.IsNullOrEmpty(x.VATInvNumber) &&
                                           x.TotalActivityVATIncludedAmount.HasValue)
                               .Sum(x => x.TotalActivityVATIncludedAmount.Value * GetTransactionExchangeRate(x));
        }
    }
}
