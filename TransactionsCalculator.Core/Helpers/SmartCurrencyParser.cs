using System.Globalization;

namespace TransactionsCalculator.Core.Helpers
{
    public static class SmartCurrencyParser
    {
        public static decimal? ParseCurrency(string value)
        {
            decimal? result = null;
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace(',', '.');
                result = decimal.Parse(value, CultureInfo.InvariantCulture);
            }
            return result;
        }
    }
}
