using System.Collections.Generic;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;
using TransactionsCalculator.Interfaces.WebApiClients;

namespace TransactionsCalculator.Core.Services
{
    public class ExchangeRatesService : IExchangeRatesService
    {
        private static Dictionary<string, IExchangeRateInfo> exchangeCurrenciesDict = new Dictionary<string, IExchangeRateInfo>();
        private readonly IExchangeRatesApiClient exchangeRatesWebApiClient;

        public ExchangeRatesService(IExchangeRatesApiClient exchangeRatesWebApiClient)
        {
            this.exchangeRatesWebApiClient = exchangeRatesWebApiClient;
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
