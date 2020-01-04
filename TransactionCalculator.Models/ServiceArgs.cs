using TransactionsCalculator.Interfaces.Models;

namespace TransactionCalculator.Models
{
    public class ServiceArgs : IServiceArgs
    {
        public string WorkingDirectory { get; set; }
    }
}
