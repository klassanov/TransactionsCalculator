using System;
using System.Globalization;

namespace TransactionsCalculator.Core.Helpers
{
    public static class DateParser
    {
        public static DateTime? ParseDate(string value)
        {
            return string.IsNullOrEmpty(value.Trim()) ? (DateTime?)null : DateTime.ParseExact(value, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        }
    }
}
