using CsvHelper.Configuration.Attributes;

namespace TransactionCalculator.Models.Transaction
{
    public class TransactionRow
    {
        [Name("SALE_ARRIVAL_COUNTRY")]
        public string SaleArrivalCountry { get; set; }
    }
}
