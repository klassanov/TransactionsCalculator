using System.Collections.Generic;
using TransactionCalculator.Models.ExchangeRates;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;
using TransactionsCalculator.Interfaces.WebApiClients;

namespace TransactionsCalculator.Core.Services
{
    public class ExchangeRatesService : IExchangeRatesService
    {
        private Dictionary<string, IExchangeRateInfo> exchangeCurrenciesDict = new Dictionary<string, IExchangeRateInfo>();
        private readonly IExchangeRatesApiClient exchangeRatesWebApiClient;
        private readonly IAppConfigurationService appConfiguratuinService;

        public ExchangeRatesService(
            IExchangeRatesApiClient exchangeRatesWebApiClient,
            IAppConfigurationService appConfiguratuinService)
        {
            this.exchangeRatesWebApiClient = exchangeRatesWebApiClient;
            this.appConfiguratuinService = appConfiguratuinService;
            exchangeCurrenciesDict.Add(this.appConfiguratuinService.ReferenceCurrencyCode, new ReferenceExchangeRatesInfo(this.appConfiguratuinService.ReferenceCurrencyCode));
        }

        public decimal GetExchangeRate(string currencyCode)
        {
            IExchangeRateInfo ratesInfo = null;

            if (exchangeCurrenciesDict.ContainsKey(currencyCode))
            {
                ratesInfo = exchangeCurrenciesDict[currencyCode];
            }
            else
            {
                IExchangeRateInfo exchangeRatesInfo = exchangeRatesWebApiClient.GetExchangeRateInfo(currencyCode);
                exchangeCurrenciesDict.Add(currencyCode, exchangeRatesInfo);
                ratesInfo = exchangeRatesInfo;
            }

            return ratesInfo.GetExchangeRate();
        }

        public IEnumerable<IExchangeRateInfo> GetAllExchangeRates()
        {
            return exchangeCurrenciesDict.Values;
        }
    }
}
