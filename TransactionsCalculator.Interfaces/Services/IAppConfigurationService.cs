namespace TransactionsCalculator.Interfaces.Services
{
    public interface IAppConfigurationService
    {
        string FileExtension { get; set; }

        string FileDelimiter { get; set; }

        string ReferenceCurrencyCode { get; set; }

        string ReferenceCountry { get; set; }

        string WorkingDirectory { get; set; }
    }
}
