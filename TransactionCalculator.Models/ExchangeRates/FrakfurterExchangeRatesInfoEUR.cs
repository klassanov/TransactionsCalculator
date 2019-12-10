using System;
using TransactionsCalculator.Interfaces.Models;

namespace TransactionCalculator.Models.ExchangeRates
{
    public class FrakfurterExchangeRatesInfoEUR : IExchangeRatesInfo
    {
        public DateTime Date { get; set; }

        public string Base { get; set; }

        public decimal Amount { get; set; }

        public EuroRate Rates { get; set; }

        public DateTime GetExchangeDate()
        {
            return this.Date;
        }

        public decimal GetReferenceExchangeRate()
        {
            return this.Rates.EUR;
        }
    }
}
