using System;

namespace TransactionsCalculator.Interfaces.Models
{
    public interface IExchangeRatesInfo
    {
        decimal GetReferenceExchangeRate();

        DateTime GetExchangeDate();
    }
}
