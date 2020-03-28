using System;
using TransactionsCalculator.Interfaces.Models;

namespace TransactionCalculator.Models.Transaction
{
    public class Transaction : ITransaction
    {
        //BP: SALE_ARRIVAL_COUNTRY
        public string SaleArrivalCountry { get; set; }

        //BO: SALE_DEPART_COUNTRY
        public string SaleDepartureCountry { get; set; }

        //BW: TRANSACTION_SELLER_VAT_NUMBER_COUNTRY
        public string TransactionSellerVATNumberCountry { get; set; }

        //BA: TRANSACTION_CURRENCY_CODE
        public string TransactionCurrencyCode { get; set; }

        //BT: SELLER_DEPART_COUNTRY_VAT_NUMBER
        public string SellerDepartCountryVATNumber { get; set; }

        //BY: BUYER_VAT_NUMBER_COUNTRY
        public string BuyerVATNumberCountry { get; set; }

        //AZ: TOTAL_ACTIVITY_VALUE_AMT_VAT_INCL
        public decimal? TotalActivityVATIncludedAmount { get; set; }

        //AP: TOTAL_ACTIVITY_VALUE_VAT_AMT
        public decimal? TotalActivityVATAmount { get; set; }

        //H: TAX_CALCULATION_DATE
        public DateTime? TaxCalculationDate { get; set; }

        //CB: TAXABLE_JURISDICTION
        public string TaxableJurisdiction { get; set; }

        //K: TRANSACTION_COMPLETE_DATE
        public DateTime? TransactionCompleteDate { get; set; }
    }
}
