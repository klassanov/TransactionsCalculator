using System.Collections.Generic;
using System.Linq;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Operations
{
    public class StepThreeTwoCalculationOperation : AbstractCalculationOperation
    {
        public StepThreeTwoCalculationOperation(
            IExchangeRatesService exchangeService,
            IAppConfigurationService appConfigurationService)
            : base(exchangeService, appConfigurationService)
        {
            this.operationDescription = "Step 3.2";
        }

        protected override decimal Calculate(IEnumerable<ITransaction> transactions)
        {
            return transactions.Where(x => !string.IsNullOrEmpty(x.SellerDepartCountryVATNumber) &&
                                        !x.SellerDepartCountryVATNumber.StartsWith(this.appConfigurationService.ReferenceCountryCode) &&
                                        x.BuyerVATNumberCountry.Equals(this.appConfigurationService.ReferenceCountryCode) &&
                                        x.TotalActivityVATAmount.HasValue && x.TotalActivityVATAmount.Value == 0)
                               .Sum(x => x.TotalActivityVATIncludedAmount.Value * GetExchangeRate(x.TransactionCurrencyCode, x.TaxCalculationDate));

        }
    }
}
