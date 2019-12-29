namespace TransactionsCalculator.Interfaces.Services
{
    public interface IAppConfigurationService
    {
        public string FileExtension { get; set; }

        public string FileDelimiter { get; set; }

        public string ReferenceCurrencyCode { get; set; }
    }
}
