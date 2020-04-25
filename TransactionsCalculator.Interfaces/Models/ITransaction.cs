using System;

namespace TransactionsCalculator.Interfaces.Models
{
    public interface ITransaction
    {
        /// <summary>
        /// BP: SALE_ARRIVAL_COUNTRY
        /// </summary>
        string SaleArrivalCountry { get; set; }

        /// <summary>
        /// BO: SALE_DEPART_COUNTRY
        /// </summary>
        string SaleDepartureCountry { get; set; }

        /// <summary>
        /// BW: TRANSACTION_SELLER_VAT_NUMBER_COUNTRY
        /// </summary>
        string TransactionSellerVATNumberCountry { get; set; }

        /// <summary>
        /// BA: TRANSACTION_CURRENCY_CODE
        /// </summary>
        string TransactionCurrencyCode { get; set; }

        /// <summary>
        /// BY: BUYER_VAT_NUMBER_COUNTRY
        /// </summary>
        string BuyerVATNumberCountry { get; set; }

        /// <summary>
        /// AZ: TOTAL_ACTIVITY_VALUE_AMT_VAT_INCL
        /// </summary>
        decimal? TotalActivityVATIncludedAmount { get; set; }

        /// <summary>
        /// AP: TOTAL_ACTIVITY_VALUE_VAT_AMT
        /// </summary>
        decimal? TotalActivityVATAmount { get; set; }

        /// <summary>
        /// CB: TAXABLE_JURISDICTION
        /// </summary>
        public string TaxableJurisdiction { get; set; }

        /// <summary>
        /// K: TRANSACTION_COMPLETE_DATE
        /// </summary>
        public DateTime? TransactionCompleteDate { get; set; }

        /// <summary>
        /// BX: TRANSACTION_SELLER_VAT_NUMBER
        /// </summary>
        public string TransactionSellerVATNumber { get; set; }

        /// <summary>
        /// BZ: BUYER_VAT_NUMBER
        /// </summary>
        public string BuyerVATNumber { get; set; }

        /// <summary>
        /// CD: VAT_INV_NUMBER
        /// </summary>
        public string VATInvNumber { get; set; }
    }
}
