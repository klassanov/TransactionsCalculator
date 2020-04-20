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
            this.operationDescription = "INTRA-IT";
        }

        protected override decimal Calculate(IEnumerable<ITransaction> transactions)
        {
            return transactions.Where(x => x.TransactionSellerVATNumber.StartsWith(this.appConfigurationService.ReferenceCountryCode) &&
                                                       !string.IsNullOrEmpty(x.BuyerVATNumberCountry) &&
                                                       !x.BuyerVATNumberCountry.Equals(this.appConfigurationService.ReferenceCountryCode) &&
                                                        x.TotalActivityVATAmount.HasValue && x.TotalActivityVATAmount.Value == 0)
                                            .Sum(x => x.TotalActivityVATIncludedAmount.Value * GetExchangeRate(x.TransactionCurrencyCode, x.TransactionCompleteDate));
        }
    }
}
