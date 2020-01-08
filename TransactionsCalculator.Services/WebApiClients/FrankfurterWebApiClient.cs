using Flurl;
using Flurl.Http;
using System;
using TransactionCalculator.Models.ExchangeRates;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;
using TransactionsCalculator.Interfaces.WebApiClients;

namespace TransactionsCalculator.Core.WebApiClients
{
    public class FrankfurterWebApiClient : IExchangeRatesApiClient
    {
        private readonly IAppConfigurationService appConfigurationService;
        private const string webAPIUrl = "https://api.frankfurter.app/";

        public FrankfurterWebApiClient(IAppConfigurationService appConfigurationService)
        {
            this.appConfigurationService = appConfigurationService;
        }

        public IExchangeRateInfo GetExchangeRateInfo(string currencyCode)
        {
            return GetExchangeRateInfoFromWebAPI(currencyCode);
        }

        public IExchangeRateInfo GetExchangeRateInfo(string currencyCode, DateTime exchangeDate)
        {
            return GetExchangeRateInfoFromWebAPI(currencyCode, exchangeDate);
        }

        private IExchangeRateInfo GetExchangeRateInfoFromWebAPI(string currencyCode, DateTime? exchangeDate = null)
        {
            IExchangeRateInfo exchangeRateInfo = webAPIUrl
               .AppendPathSegment(exchangeDate.HasValue ? exchangeDate.Value.Date.ToString("yyyy-MM-dd") : "latest")
               .SetQueryParam("from", currencyCode)
               .SetQueryParam("to", appConfigurationService.ReferenceCurrencyCode)
               .GetJsonAsync<FrakfurterExchangeRatesInfoEUR>()
               .Result;

            exchangeRateInfo.Source = webAPIUrl;

            return exchangeRateInfo;
        }
    }
}
