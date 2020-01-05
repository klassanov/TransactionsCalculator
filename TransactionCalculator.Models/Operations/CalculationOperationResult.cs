using TransactionsCalculator.Interfaces.Models;

namespace TransactionCalculator.Models.Operations
{
    public class CalculationOperationResult : ICalculationOperationResult
    {
        public CalculationOperationResult(decimal calculatedAmount, string operationDescription)
        {
            this.CalulatedAmount = calculatedAmount;
            this.OperationDescription = operationDescription;
        }

        public string OperationDescription { get; set; }
        public decimal CalulatedAmount { get; set; }

    }
}
