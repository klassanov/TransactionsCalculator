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
            IAppConfigurationService appConfigurationService,
            ICalculationParameters calculationParameters)
            : base(exchangeService, appConfigurationService, calculationParameters)
        {
            this.operationDescription = "Step 2";
        }

        public override decimal Calculate(IEnumerable<ITransaction> transactionList)
        {
            HashSet<string> countriesHash = transactionList.Select(x => x.TransactionSellerVATNumberCountry).ToHashSet();

            return RoundAmount(transactionList.Where(x => x.SaleDepartureCountry.Equals(this.calculationParameters.SaleArrivalCountry) &&
                                                x.TotalActivityVatIncludedAmount.HasValue &&
                                                !countriesHash.Contains(x.SaleArrivalCountry))
                                                .Sum(x => x.TotalActivityVatIncludedAmount.Value * GetExchangeRate(x.TransactionCurrencyCode, x.TaxCalculationDate)));
        }
    }
}
