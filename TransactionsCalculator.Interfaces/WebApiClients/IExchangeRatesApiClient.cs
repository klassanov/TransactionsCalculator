using System;
using TransactionsCalculator.Interfaces.Models;

namespace TransactionsCalculator.Interfaces.WebApiClients
{
    public interface IExchangeRatesApiClient
    {
        IExchangeRateInfo GetExchangeRateInfo(string currencyCode);

        IExchangeRateInfo GetExchangeRateInfo(string currencyCode, DateTime exchangeDate);
    }
}
