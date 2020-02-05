using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Services
{
    public class AppConfigurationService : IAppConfigurationService
    {
        public string FileExtension { get; set; }
        public string FileDelimiter { get; set; }
        public string ReferenceCurrencyCode { get; set; }
    }
}
