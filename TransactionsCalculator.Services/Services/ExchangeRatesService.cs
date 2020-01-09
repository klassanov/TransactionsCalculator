using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;
using TransactionsCalculator.Interfaces.WebApiClients;

namespace TransactionsCalculator.Core.Services
{
    public class ExchangeRatesService : IExchangeRatesService
    {
        private static ConcurrentDictionary<string, IExchangeRateInfo> exchangeCurrenciesDict = new ConcurrentDictionary<string, IExchangeRateInfo>();
        private readonly IExchangeRatesApiClient exchangeRatesWebApiClient;
        private readonly IAppConfigurationService appConfigurationService;

        public ExchangeRatesService(
            IExchangeRatesApiClient exchangeRatesWebApiClient,
            IAppConfigurationService appConfiguratuinService)
        {
            this.exchangeRatesWebApiClient = exchangeRatesWebApiClient;
            this.appConfigurationService = appConfiguratuinService;
        }

        public decimal GetExchangeRate(string currencyCode, DateTime? tansactionDate)
        {
            if (currencyCode == this.appConfigurationService.ReferenceCurrencyCode) { return 1; }

            IExchangeRateInfo ratesInfo = null;
            string exchangeRateInfoUniqueKey = this.GetExchangeRateInfoKey(currencyCode, tansactionDate.Value);

            if (exchangeCurrenciesDict.ContainsKey(exchangeRateInfoUniqueKey))
            {
                ratesInfo = exchangeCurrenciesDict[exchangeRateInfoUniqueKey];
            }
            else
            {
                IExchangeRateInfo exchangeRatesInfo = exchangeRatesWebApiClient.GetExchangeRateInfo(currencyCode, tansactionDate.Value);
                exchangeCurrenciesDict.TryAdd(exchangeRateInfoUniqueKey, exchangeRatesInfo);
                ratesInfo = exchangeRatesInfo;
            }

            return ratesInfo.GetExchangeRate();
        }

        public IEnumerable<IExchangeRateInfo> GetAllExchangeRates()
        {
            return exchangeCurrenciesDict.Values;
        }

        private string GetExchangeRateInfoKey(string currencyCode, DateTime exchangeDate)
        {
            return $"{exchangeDate.ToString("yyyy-MM-dd")}|{currencyCode}";
        }
    }
}
