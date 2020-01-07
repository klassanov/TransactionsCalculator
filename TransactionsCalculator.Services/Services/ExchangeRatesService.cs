using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TransactionCalculator.Models.ExchangeRates;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;
using TransactionsCalculator.Interfaces.WebApiClients;

namespace TransactionsCalculator.Core.Services
{
    public class ExchangeRatesService : IExchangeRatesService
    {
        private static ConcurrentDictionary<string, IExchangeRateInfo> exchangeCurrenciesDict = new ConcurrentDictionary<string, IExchangeRateInfo>();
        private readonly IExchangeRatesApiClient exchangeRatesWebApiClient;
        private readonly IAppConfigurationService appConfiguratuinService;

        public ExchangeRatesService(
            IExchangeRatesApiClient exchangeRatesWebApiClient,
            IAppConfigurationService appConfiguratuinService)
        {
            this.exchangeRatesWebApiClient = exchangeRatesWebApiClient;
            this.appConfiguratuinService = appConfiguratuinService;
            this.AddReferenceExchangeRatesInfo();
        }

        public decimal GetExchangeRate(string currencyCode, DateTime exchangeDate)
        {
            IExchangeRateInfo ratesInfo = null;
            string exchangeRateInfoUniqueKey = ExchangeRateInfoHelper.GetExchangeRateInfoUniqueKey(currencyCode, exchangeDate);

            if (exchangeCurrenciesDict.ContainsKey(exchangeRateInfoUniqueKey))
            {
                ratesInfo = exchangeCurrenciesDict[exchangeRateInfoUniqueKey];
            }
            else
            {
                IExchangeRateInfo exchangeRatesInfo = exchangeRatesWebApiClient.GetExchangeRateInfo(currencyCode, exchangeDate);
                exchangeCurrenciesDict.TryAdd(exchangeRateInfoUniqueKey, exchangeRatesInfo);
                ratesInfo = exchangeRatesInfo;
            }

            return ratesInfo.GetExchangeRate();
        }

        public IEnumerable<IExchangeRateInfo> GetAllExchangeRates()
        {
            return exchangeCurrenciesDict.Values;
        }

        private void AddReferenceExchangeRatesInfo()
        {
            ReferenceExchangeRatesInfo referenceExchangeRatesInfo = new ReferenceExchangeRatesInfo(this.appConfiguratuinService.ReferenceCurrencyCode);
            string exchangeRateInfoUniqueKey = ExchangeRateInfoHelper.GetExchangeRateInfoUniqueKey(referenceExchangeRatesInfo.GetFromCurrency(), referenceExchangeRatesInfo.GetExchangeDate());
            exchangeCurrenciesDict.TryAdd(exchangeRateInfoUniqueKey, referenceExchangeRatesInfo);
        }


    }
}
