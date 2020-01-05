using System;
using TransactionsCalculator.Interfaces.Models;

namespace TransactionCalculator.Models.ExchangeRates
{
    public class ReferenceExchangeRatesInfo : IExchangeRateInfo
    {
        private DateTime exchangeDate;
        private string referenceCurrencyCode;

        public ReferenceExchangeRatesInfo(string referenceCurrencyCode)
        {
            this.referenceCurrencyCode = referenceCurrencyCode;
            this.exchangeDate = DateTime.Now;
        }

        public DateTime GetExchangeDate()
        {
            return this.exchangeDate;
        }

        public decimal GetExchangeRate()
        {
            return 1;
        }

        public string GetFromCurrency()
        {
            return referenceCurrencyCode;
        }
    }
}
