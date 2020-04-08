using Moq;
using System;
using System.Collections.Generic;
using TransactionCalculator.Models.Transaction;
using TransactionsCalculator.Core.Operations;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;
using Xunit;

namespace TransactionsCalculator.UnitTests.Operations
{
    public class StepTwoCalculationOperationTest
    {
        private readonly Mock<IExchangeRatesService> exchangeServiceMock;
        private readonly Mock<IAppConfigurationService> appConfigurationServiceMock;
        private readonly string referenceCountry = "ITA";
        private readonly string referenceCurrencyCode = "EUR";

        public StepTwoCalculationOperationTest()
        {
            this.exchangeServiceMock = new Mock<IExchangeRatesService>();

            this.appConfigurationServiceMock = new Mock<IAppConfigurationService>();
            this.appConfigurationServiceMock.SetupGet<string>(x => x.ReferenceCountryCode).Returns(this.referenceCountry);
        }

        [Fact]
        public void CalculateConsidersSaleArrivalCountriesNotInTransactionSellerVATNumberCountries()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(1);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                new Transaction { TransactionSellerVATNumberCountry = this.referenceCountry, TotalActivityVATIncludedAmount = 100, SaleArrivalCountry = "BUL", SaleDepartureCountry = this.referenceCountry},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 200, SaleArrivalCountry = this.referenceCountry,  SaleDepartureCountry = this.referenceCountry},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 300, SaleArrivalCountry = "CAN",  SaleDepartureCountry = this.referenceCountry},
                new Transaction { TransactionSellerVATNumberCountry = "XXX", TotalActivityVATIncludedAmount = 400, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = this.referenceCountry},
            };

            StepTwoCalculationOperation target = this.CreateStepTwoCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(700, actualResult);
        }

        [Fact]
        public void CalculateFiltersBySaleDepartCountry()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(1);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 100, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = this.referenceCountry},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 200, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = this.referenceCountry},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 300, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = this.referenceCountry},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 400, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = "YYY"},
            };

            StepTwoCalculationOperation target = this.CreateStepTwoCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(600, actualResult);
        }

        [Fact]
        public void CalculateConsidersExchangeRates()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(this.referenceCurrencyCode, It.IsAny<DateTime?>())).Returns(1);
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate("USD", It.IsAny<DateTime?>())).Returns(2);
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate("BGN", It.IsAny<DateTime?>())).Returns(5);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 100, TransactionCurrencyCode ="USD", SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = this.referenceCountry},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 200, TransactionCurrencyCode ="BGN", SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = this.referenceCountry},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 300, TransactionCurrencyCode = this.referenceCurrencyCode, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = this.referenceCountry},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 400, TransactionCurrencyCode = this.referenceCurrencyCode, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = "YYY"},
            };

            StepTwoCalculationOperation target = this.CreateStepTwoCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(1500, actualResult);
        }

        [Fact]
        public void CalculateIgnoresTotalActivityVatIncludedAmountIfNull()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(1);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 100, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = this.referenceCountry},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 200, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = this.referenceCountry},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 300, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = this.referenceCountry},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = null, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = this.referenceCountry},
            };

            StepTwoCalculationOperation target = this.CreateStepTwoCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(600, actualResult);
        }

        private StepTwoCalculationOperation CreateStepTwoCalculationOperation()
        {
            return new StepTwoCalculationOperation(exchangeServiceMock.Object, this.appConfigurationServiceMock.Object);
        }
    }
}
