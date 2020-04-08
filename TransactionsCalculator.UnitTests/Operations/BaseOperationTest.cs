using Moq;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.UnitTests.Operations
{
    public class BaseOperationTest
    {
        protected readonly Mock<IExchangeRatesService> exchangeServiceMock;
        protected readonly Mock<IAppConfigurationService> appConfigurationServiceMock;

        protected readonly string referenceCountryCode = "IT";
        protected readonly string referenceCurrencyCode = "EUR";
        protected readonly string referenceTaxableJurisdiction = "ITALIA";

        public BaseOperationTest()
        {
            this.exchangeServiceMock = new Mock<IExchangeRatesService>();

            this.appConfigurationServiceMock = new Mock<IAppConfigurationService>();
            this.appConfigurationServiceMock.SetupGet<string>(x => x.ReferenceTaxableJurisdiction).Returns(this.referenceTaxableJurisdiction);
            this.appConfigurationServiceMock.SetupGet<string>(x => x.ReferenceCountryCode).Returns(this.referenceCountryCode);
        }
    }
}
