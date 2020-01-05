namespace TransactionsCalculator.Interfaces.Models
{
    public interface ICalculationOperationResult
    {
        string OperationDescription { get; }

        decimal CalulatedAmount { get; }
    }
}
