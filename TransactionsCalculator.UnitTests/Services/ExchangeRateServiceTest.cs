using Moq;
using System;
using System.Linq;
using TransactionsCalculator.Core.Services;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;
using TransactionsCalculator.Interfaces.WebApiClients;
using Xunit;

namespace TransactionsCalculator.UnitTests.Services
{
    public class ExchangeRateServiceTest
    {
        private readonly Mock<IExchangeRatesApiClient> apiClientMock;
        private readonly Mock<IAppConfigurationService> appConfigServiceMock;
        private readonly Mock<IExchangeRateInfo> exchangeRateInfoMock;
        private readonly string referenceCurrencyCode;

        public ExchangeRateServiceTest()
        {
            this.referenceCurrencyCode = "EUR";

            this.exchangeRateInfoMock = new Mock<IExchangeRateInfo>();
            this.exchangeRateInfoMock.Setup(x => x.GetExchangeRate()).Returns(123);
            this.exchangeRateInfoMock.Setup(x => x.GetFromCurrency()).Returns("ZXC");

            this.appConfigServiceMock = new Mock<IAppConfigurationService>();
            this.appConfigServiceMock.SetupGet(x => x.ReferenceCurrencyCode).Returns(this.referenceCurrencyCode);

            this.apiClientMock = new Mock<IExchangeRatesApiClient>();
            this.apiClientMock.Setup(x => x.GetExchangeRateInfo(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(this.exchangeRateInfoMock.Object);
        }

        [Fact]
        public void GetExchangeRateReturnsOneForReferenceCurrencyCode()
        {
            //Arrange
            ExchangeRatesService target = new ExchangeRatesService(this.apiClientMock.Object, this.appConfigServiceMock.Object);

            //Act
            decimal actualResult = target.GetExchangeRate(this.referenceCurrencyCode, DateTime.Now);

            //Assert
            Assert.Equal(1, actualResult);
        }

        [Fact]
        public void GetExchangeRateDoesNotCallApiClientForReferenceCurrencyCode()
        {
            //Arrange
            ExchangeRatesService target = new ExchangeRatesService(this.apiClientMock.Object, this.appConfigServiceMock.Object);

            //Act
            target.GetExchangeRate(this.referenceCurrencyCode, DateTime.Now);

            //Assert
            this.apiClientMock.Verify(x => x.GetExchangeRateInfo(It.IsAny<string>()), Times.Never());
            this.apiClientMock.Verify(x => x.GetExchangeRateInfo(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never());
        }

        [Fact]
        public void GetExchangeRateCallsApiClientOnceForSameCurrencyCodeAndDate()
        {
            //Arrange
            ExchangeRatesService target = new ExchangeRatesService(this.apiClientMock.Object, this.appConfigServiceMock.Object);
            string currencyCode = "ABC";
            DateTime exchangeDate = DateTime.Now;

            //Act
            target.GetExchangeRate(currencyCode, exchangeDate);
            target.GetExchangeRate(currencyCode, exchangeDate);

            //Assert
            this.apiClientMock.Verify(x => x.GetExchangeRateInfo(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [Fact]
        public void GetExchangeRateCallsApiClientForEachDifferentCurrencyCodeAndDate()
        {
            //Arrange
            ExchangeRatesService target = new ExchangeRatesService(this.apiClientMock.Object, this.appConfigServiceMock.Object);

            //Act
            target.GetExchangeRate("YEN", DateTime.Now);
            target.GetExchangeRate("YEN", DateTime.Now);

            target.GetExchangeRate("GBP", DateTime.Now.AddDays(-5));
            target.GetExchangeRate("GBP", DateTime.Now.AddDays(-5));

            //Assert
            var exchangeRates = target.GetAllExchangeRates();
            this.apiClientMock.Verify(x => x.GetExchangeRateInfo(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Exactly(2));
        }

        [Fact]
        public void GetExchangeRateKeepsExchangeRates()
        {
            //Arrange
            string fromCurrency = "XXX";
            exchangeRateInfoMock.Setup(x => x.GetFromCurrency()).Returns(fromCurrency);

            ExchangeRatesService target = new ExchangeRatesService(this.apiClientMock.Object, this.appConfigServiceMock.Object);

            //Act
            target.GetExchangeRate("ADF", DateTime.Now.AddDays(-3));
            target.GetExchangeRate("ADF", DateTime.Now.AddDays(-5));
            target.GetExchangeRate("BCD", DateTime.Now.AddDays(-15));

            //Assert
            var exchangeRates = target.GetAllExchangeRates();
            var actualResult = exchangeRates.Where(x => x.GetFromCurrency().Equals(fromCurrency)).Count();

            Assert.Equal(3, actualResult);
        }
    }
}

