using System.Collections.Generic;
using TransactionsCalculator.Interfaces.Models;

namespace TransactionsCalculator.Interfaces.Operations
{
    public interface ICalculationOperation
    {
        string OperationDescription { get; }

        decimal Calculate(IEnumerable<ITransaction> transactionList);
    }
}
