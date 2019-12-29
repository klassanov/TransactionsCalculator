using Flurl;
using Flurl.Http;
using TransactionCalculator.Models.ExchangeRates;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;
using TransactionsCalculator.Interfaces.WebApiClients;

namespace TransactionsCalculator.Core.WebApiClients
{
    public class FrankfurterWebApiClient : IExchangeRatesApiClient
    {
        private readonly IAppConfigurationService appConfigurationService;

        public FrankfurterWebApiClient(IAppConfigurationService appConfigurationService)
        {
            this.appConfigurationService = appConfigurationService;
        }

        public IExchangeRateInfo GetExchangeRateInfo(string currencyCode)
        {
            IExchangeRateInfo exchangeRateInfo = "https://api.frankfurter.app/"
               .AppendPathSegment("latest")
               .SetQueryParam("from", currencyCode)
               .SetQueryParam("to", appConfigurationService.ReferenceCurrencyCode)
               .GetJsonAsync<FrakfurterExchangeRatesInfoEUR>()
               .Result;

            return exchangeRateInfo;
        }
    }
}
