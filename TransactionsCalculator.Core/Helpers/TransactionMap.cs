using CsvHelper.Configuration;
using TransactionCalculator.Models.Transaction;

namespace TransactionsCalculator.Core.Helpers
{
    public class TransactionMap : ClassMap<Transaction>
    {
        public TransactionMap()
        {
            Map(t => t.SaleArrivalCountry)
                .Name("SALE_ARRIVAL_COUNTRY");

            Map(t => t.SaleDepartureCountry)
                .Name("SALE_DEPART_COUNTRY");

            Map(t => t.TransactionSellerVATNumberCountry)
                .Name("TRANSACTION_SELLER_VAT_NUMBER_COUNTRY");

            Map(t => t.TransactionCurrencyCode)
                .Name("TRANSACTION_CURRENCY_CODE");

            Map(t => t.TotalActivityVATIncludedAmount)
                .Name("TOTAL_ACTIVITY_VALUE_AMT_VAT_INCL")
                .ConvertUsing(row => SmartCurrencyParser.ParseCurrency(row.GetField("TOTAL_ACTIVITY_VALUE_AMT_VAT_INCL")));

            Map(t => t.TaxCalculationDate)
                .Name("TAX_CALCULATION_DATE")
                .ConvertUsing(row => DateParser.ParseDate(row.GetField("TAX_CALCULATION_DATE")));

            Map(t => t.SellerDepartCountryVATNumber)
                .Name("SELLER_DEPART_COUNTRY_VAT_NUMBER");

            Map(t => t.BuyerVATNumberCountry)
                .Name("BUYER_VAT_NUMBER_COUNTRY");

            Map(t => t.TotalActivityVATAmount)
                .Name("TOTAL_ACTIVITY_VALUE_VAT_AMT")
                .ConvertUsing(row => SmartCurrencyParser.ParseCurrency(row.GetField("TOTAL_ACTIVITY_VALUE_VAT_AMT")));

            Map(t => t.TaxableJurisdiction)
                .Name("TAXABLE_JURISDICTION");

        }
    }
}
