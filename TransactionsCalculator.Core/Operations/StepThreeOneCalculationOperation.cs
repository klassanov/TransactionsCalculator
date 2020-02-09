using System.Collections.Generic;
using System.Linq;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Operations
{
    public class StepThreeOneCalculationOperation : AbstractCalculationOperation
    {
        public StepThreeOneCalculationOperation(
            IExchangeRatesService exchangeService,
            IAppConfigurationService appConfigurationService)
            : base(exchangeService, appConfigurationService)
        {
            this.operationDescription = "Step 3.1";
        }

        public override decimal Calculate(IEnumerable<ITransaction> transactions)
        {
            return RoundAmount(transactions.Where(x => x.SellerDepartCountryVATNumber.StartsWith(this.appConfigurationService.ReferenceCountry) &&
                                                       !string.IsNullOrEmpty(x.BuyerVATNumberCountry) &&
                                                       !x.BuyerVATNumberCountry.Equals(this.appConfigurationService.ReferenceCountry) &&
                                                        x.TotalActivityVATAmount.HasValue && x.TotalActivityVATAmount.Value == 0)
                                            .Sum(x => x.TotalActivityVATIncludedAmount.Value * GetExchangeRate(x.TransactionCurrencyCode, x.TaxCalculationDate)));
        }
    }
}
