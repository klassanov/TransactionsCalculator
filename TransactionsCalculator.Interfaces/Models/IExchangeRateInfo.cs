using System;

namespace TransactionsCalculator.Interfaces.Models
{
    public interface IExchangeRateInfo
    {
        decimal GetExchangeRate();

        DateTime GetExchangeDate();

        string GetFromCurrency();

        public string Source { get; set; }
    }
}
