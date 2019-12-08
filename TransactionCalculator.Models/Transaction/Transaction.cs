using CsvHelper.Configuration.Attributes;

namespace TransactionCalculator.Models.Transaction
{
    public class Transaction
    {
        [Name("SALE_ARRIVAL_COUNTRY")]
        public string SaleArrivalCountry { get; set; }

        [Name("TRANSACTION_CURRENCY_CODE")]
        public string TransactionCurrencyCode { get; set; }

        [Name("TOTAL_ACTIVITY_VALUE_AMT_VAT_INCL")]
        public decimal? TotalActivityVatIncludedAmount { get; set; }
    }
}
