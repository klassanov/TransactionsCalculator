using System.Collections.Generic;
using TransactionsCalculator.Interfaces.Models;

namespace TransactionsCalculator.Interfaces.Operations
{
    public interface ICalculationOperation
    {
        string OperationDescription { get; }

        decimal CalculateAmount(IEnumerable<ITransaction> transactionList);
    }
}
