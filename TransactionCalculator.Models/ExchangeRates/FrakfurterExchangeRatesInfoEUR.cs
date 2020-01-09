using System;
using TransactionsCalculator.Interfaces.Models;

namespace TransactionCalculator.Models.ExchangeRates
{
    public class FrakfurterExchangeRatesInfoEUR : IExchangeRateInfo
    {
        public DateTime Date { get; set; }

        public string Base { get; set; }

        public decimal Amount { get; set; }

        public EuroRate Rates { get; set; }

        public string Source { get; set; }

        public DateTime? TransactionDate { get; set; }

        public string GetFromCurrency()
        {
            return this.Base;
        }

        public DateTime GetEffectiveExchangeDate()
        {
            return this.Date;
        }

        public decimal GetExchangeRate()
        {
            return this.Rates.EUR;
        }
    }
}
