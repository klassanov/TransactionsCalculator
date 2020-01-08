using System;

namespace TransactionsCalculator.Interfaces.Models
{
    public interface ITransaction
    {
        string SaleArrivalCountry { get; set; }

        string SaleDepartureCountry { get; set; }

        string TransactionSellerVATNumberCountry { get; set; }

        string TransactionCurrencyCode { get; set; }

        decimal? TotalActivityVatIncludedAmount { get; set; }

        DateTime? TaxCalculationDate { get; set; }
    }
}
