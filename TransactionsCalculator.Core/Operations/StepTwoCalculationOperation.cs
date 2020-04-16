using System.Collections.Generic;
using System.Linq;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Operations
{
    public class StepTwoCalculationOperation : AbstractCalculationOperation
    {
        public StepTwoCalculationOperation(
            IExchangeRatesService exchangeService,
            IAppConfigurationService appConfigurationService)
            : base(exchangeService, appConfigurationService)
        {
            this.operationDescription = "Step 2";
        }

        protected override decimal Calculate(IEnumerable<ITransaction> transactionList)
        {
            HashSet<string> countriesHash = transactionList.Select(x => x.TransactionSellerVATNumberCountry).ToHashSet();

            return transactionList.Where(x => this.appConfigurationService.ReferenceCountryCode.Equals(x.SaleDepartureCountry) &&
                                                x.TotalActivityVATIncludedAmount.HasValue &&
                                                !countriesHash.Contains(x.SaleArrivalCountry))
                                                .Sum(x => x.TotalActivityVATIncludedAmount.Value * GetExchangeRate(x.TransactionCurrencyCode, x.TransactionCompleteDate));
        }
    }
}
