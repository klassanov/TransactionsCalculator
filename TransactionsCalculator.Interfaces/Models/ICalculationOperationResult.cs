using System;
using TransactionsCalculator.Interfaces.Operations;

namespace TransactionsCalculator.Interfaces.Models
{
    public interface ICalculationOperationResult
    {
        string OperationDescription { get; }

        decimal CalulatedAmount { get; set; }

        public Exception Exception { get; set; }

        public ICalculationOperation CalculationOperation { get; }
    }
}
