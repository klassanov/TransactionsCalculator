using System;

namespace TransactionsCalculator.Interfaces.Models
{
    public interface IExchangeRateInfo
    {
        decimal GetExchangeRate();

        DateTime GetEffectiveExchangeDate();

        string GetFromCurrency();

        public string Source { get; set; }

        public DateTime? TransactionDate { get; set; }
    }
}
