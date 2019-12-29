using System.Collections.Generic;
using TransactionsCalculator.Interfaces.Models;

namespace TransactionsCalculator.Interfaces.Services
{
    public interface IExchangeRatesService
    {
        decimal GetExchangeRate(string currencyCode);

        IEnumerable<IExchangeRateInfo> GetAllExchangeRates();
    }
}
