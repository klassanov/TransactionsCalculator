using System;

namespace TransactionCalculator.Models.ExchangeRates
{
    public static class ExchangeRateInfoHelper
    {
        public static string GetExchangeRateInfoUniqueKey(string currencyCode, DateTime exchangeDate)
        {
            return $"{exchangeDate.ToString("yyyyMMdd")}|{currencyCode}";
        }
    }
}
