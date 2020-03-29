using System;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Operations;

namespace TransactionCalculator.Models.Operations
{
    public class CalculationOperationResult : ICalculationOperationResult
    {
        public decimal CalulatedAmount { get; set; }

        public Exception Exception { get; set; }

        public ICalculationOperation CalculationOperation { get; set; }

        public string OperationDescription { get => CalculationOperation.OperationDescription; }

        public CalculationOperationResult(ICalculationOperation calculationOperation)
        {
            this.CalculationOperation = calculationOperation;
        }
    }
}
