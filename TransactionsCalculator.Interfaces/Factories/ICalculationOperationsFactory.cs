using System.Collections.Generic;
using TransactionsCalculator.Interfaces.Operations;

namespace TransactionsCalculator.Interfaces.Factories
{
    public interface ICalculationOperationsFactory
    {
        IEnumerable<ICalculationOperation> CreateCalculationOperations();
    }
}
