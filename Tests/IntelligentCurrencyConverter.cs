using System.Globalization;

namespace Tests
{
    public static class IntelligentCurrencyConverter
    {
        public static decimal ConvertCurrency(string value)
        {
            value = value.Replace(',', '.');
            return decimal.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}