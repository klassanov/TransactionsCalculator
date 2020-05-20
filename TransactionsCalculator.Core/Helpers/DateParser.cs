using System;
using System.Globalization;

namespace TransactionsCalculator.Core.Helpers
{
    public static class DateParser
    {
        private static readonly string[] allowedDateFormats = { "dd-MM-yyyy", "dd/MM/yyyy" };

        public static DateTime? ParseDate(string value)
        {
            return string.IsNullOrEmpty(value.Trim()) ? (DateTime?)null : DateTime.ParseExact(value, allowedDateFormats, CultureInfo.InvariantCulture);
        }
    }
}
