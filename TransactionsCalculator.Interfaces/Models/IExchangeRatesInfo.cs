using System;

namespace TransactionsCalculator.Interfaces.Models
{
    public interface IExchangeRatesInfo
    {
        decimal GetExchangeRateToEUR();

        DateTime GetExchangeDate();
    }
}
