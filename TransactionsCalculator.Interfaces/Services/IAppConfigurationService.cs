namespace TransactionsCalculator.Interfaces.Services
{
    public interface IAppConfigurationService
    {
        string FileExtension { get; set; }

        string FileDelimiter { get; set; }

        string ReferenceCurrencyCode { get; set; }

        string ReferenceCountryCode { get; set; }

        string WorkingDirectory { get; set; }

        public string ReferenceTaxableJurisdiction { get; set; }
    }
}
