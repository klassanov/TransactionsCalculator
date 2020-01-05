using TransactionsCalculator.Interfaces.Models;

namespace TransactionCalculator.Models.Operations
{
    public class ServiceArgs : IServiceArgs
    {
        public string WorkingDirectory { get; set; }
    }
}
