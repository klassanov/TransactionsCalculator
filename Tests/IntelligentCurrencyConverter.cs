﻿using System.Globalization;

namespace Tests
{
    public static class IntelligentCurrencyConverter
    {
        public static decimal? ConvertCurrency(string value)
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