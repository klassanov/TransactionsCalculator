using System.Collections.Generic;
using System.Linq;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Operations
{
    public class StepThreeFourCalculationOperation : AbstractCalculationOperation
    {
        public StepThreeFourCalculationOperation(
            IExchangeRatesService exchangeService,
            IAppConfigurationService appConfigurationService)
            : base(exchangeService, appConfigurationService)
        {
            this.operationDescription = "ERRORI INTRA";
        }

        protected override decimal Calculate(IEnumerable<ITransaction> transactions)
        {
            return transactions.Where(x => !string.IsNullOrEmpty(x.TransactionSellerVATNumber) &&
                                           !string.IsNullOrEmpty(x.BuyerVATNumber) &&
                                           x.TotalActivityVATAmount.HasValue &&
                                           x.TotalActivityVATAmount.Value != 0 &&
                                           x.TotalActivityVATIncludedAmount.HasValue)
                               .Sum(x => x.TotalActivityVATIncludedAmount.Value * GetTransactionExchangeRate(x));
        }
    }
}
