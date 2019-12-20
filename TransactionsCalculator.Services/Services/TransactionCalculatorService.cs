using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Services
{
    public class TransactionCalculatorService : ITransactionCalculatorService
    {
        private string processingDirectory;
        private IFileReaderService fileReaderService;
        private IExchangeService exchangeService;
        private IReportGeneratorService reportGeneratorService;
        //private IConfigurationService configurationService -> how to inject or wrap the built - in one?


        public TransactionCalculatorService()
        {

        }

        public void ProcessDirectory()
        {

        }
    }
}
