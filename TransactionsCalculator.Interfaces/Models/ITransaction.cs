﻿using System;

namespace TransactionsCalculator.Interfaces.Models
{
    public interface ITransaction
    {
        string SaleArrivalCountry { get; set; }

        string SaleDepartureCountry { get; set; }

        string TransactionSellerVATNumberCountry { get; set; }

        string TransactionCurrencyCode { get; set; }

        string BuyerVATNumberCountry { get; set; }

        decimal? TotalActivityVATIncludedAmount { get; set; }

        decimal? TotalActivityVATAmount { get; set; }

        public string TaxableJurisdiction { get; set; }

        public DateTime? TransactionCompleteDate { get; set; }

        public string TransactionSellerVATNumber { get; set; }
    }
}
