using CsvHelper.Configuration.Attributes;

namespace TransactionCalculator.Models.Transaction
{
    public class Transaction
    {
        [Name("SALE_ARRIVAL_COUNTRY")]
        public string SaleArrivalCountry { get; set; }

        [Name("SALE_DEPART_COUNTRY")]
        public string SaleDepartureCountry { get; set; }

        [Name("TRANSACTION_SELLER_VAT_NUMBER_COUNTRY")]
        public string TransactionSellerVATNumberCountry { get; set; }

        [Name("TRANSACTION_CURRENCY_CODE")]
        public string TransactionCurrencyCode { get; set; }

        [Name("TOTAL_ACTIVITY_VALUE_AMT_VAT_INCL")]
        public decimal? TotalActivityVatIncludedAmount { get; set; }
    }
}
