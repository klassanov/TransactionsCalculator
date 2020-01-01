using System.Collections.Generic;
using TransactionsCalculator.Interfaces.Models;

namespace TransactionsCalculator.Interfaces.Operations
{
    public interface ICalculationOperation
    {
        decimal Calculate(IEnumerable<ITransaction> transactions);
    }
}
