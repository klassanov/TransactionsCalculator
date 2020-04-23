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
            this.operationDescription = "MAG-IT";
        }

        protected override decimal Calculate(IEnumerable<ITransaction> transactionList)
        {
            HashSet<string> excludedCountriesHash = this.GetExcludedCountriesHashSet(transactionList);

            return transactionList.Where(x => this.appConfigurationService.ReferenceCountryCode.Equals(x.SaleDepartureCountry) &&
                                                x.TotalActivityVATIncludedAmount.HasValue &&
                                                !excludedCountriesHash.Contains(x.SaleArrivalCountry))
                                                .Sum(x => x.TotalActivityVATIncludedAmount.Value * GetExchangeRate(x.TransactionCurrencyCode, x.TransactionCompleteDate));
        }

        public HashSet<string> GetExcludedCountriesHashSet(IEnumerable<ITransaction> transactionList)
        {
            List<string> excludedCountriesList = new List<string>();

            excludedCountriesList.AddRange(transactionList.Select(x => x.TransactionSellerVATNumberCountry));
            excludedCountriesList.AddRange(this.appConfigurationService.EUCountryCodes);
            excludedCountriesList.Add(this.appConfigurationService.ReferenceCountryCode);

            return excludedCountriesList.ToHashSet();
        }
    }
}
