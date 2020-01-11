using System;
using TransactionsCalculator.Interfaces.Models;

namespace TransactionCalculator.Models.Transaction
{
    public class Transaction : ITransaction
    {
        //BP: SALE_ARRIVAL_COUNTRY
        public string SaleArrivalCountry { get; set; }

        //SALE_DEPART_COUNTRY
        public string SaleDepartureCountry { get; set; }

        //TRANSACTION_SELLER_VAT_NUMBER_COUNTRY
        public string TransactionSellerVATNumberCountry { get; set; }

        //TRANSACTION_CURRENCY_CODE
        public string TransactionCurrencyCode { get; set; }

        //AZ: TOTAL_ACTIVITY_VALUE_AMT_VAT_INCL
        public decimal? TotalActivityVatIncludedAmount { get; set; }

        //H: TAX_CALCULATION_DATE
        public DateTime? TaxCalculationDate { get; set; }
    }
}
