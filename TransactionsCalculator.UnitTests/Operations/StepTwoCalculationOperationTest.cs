using Moq;
using System;
using System.Collections.Generic;
using TransactionCalculator.Models.Transaction;
using TransactionsCalculator.Core.Operations;
using TransactionsCalculator.Interfaces.Models;
using Xunit;

namespace TransactionsCalculator.UnitTests.Operations
{
    public class StepTwoCalculationOperationTest : BaseOperationTest
    {
        public StepTwoCalculationOperationTest() : base()
        {
        }

        [Fact]
        public void GetExcludedCountriesHashSetCreatesHashSetCorrectly()
        {
            List<ITransaction> transactionList = new List<ITransaction>()
            {
                new Transaction { TransactionSellerVATNumberCountry = "BUL" },
                new Transaction { TransactionSellerVATNumberCountry = "BUL" },
                new Transaction { TransactionSellerVATNumberCountry = "BUL" },
                new Transaction { TransactionSellerVATNumberCountry = "XUI" },
                new Transaction { TransactionSellerVATNumberCountry = "XUI" },
            };

            StepTwoCalculationOperation target = this.CreateStepTwoCalculationOperation();
            HashSet<string> exclCountries = target.GetExcludedCountriesHashSet(transactionList);

            //Asserts
            //BUL, XUI, reference country code, setup eu countries
            int expectedCountriesNumber = 2 + 1 + this.appConfigurationServiceMock.Object.EUCountryCodes.Length;

            Assert.Equal(exclCountries.Count, expectedCountriesNumber);
            Assert.Contains("BUL", exclCountries);
            Assert.Contains("XUI", exclCountries);
            Assert.Contains(this.referenceCountryCode, exclCountries);
            Assert.Contains(this.euCountryCodeA, exclCountries);
            Assert.Contains(this.euCountryCodeB, exclCountries);
        }

        [Fact]
        public void CalculateFiltersData()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(1);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                //Records that should be taken
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 1000, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = this.referenceCountryCode},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 2000, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = this.referenceCountryCode},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 3000, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = this.referenceCountryCode},

                //Records that should be filtered put
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = null, SaleArrivalCountry = this.referenceCountryCode,  SaleDepartureCountry = this.referenceCountryCode},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 100, SaleArrivalCountry = this.referenceCountryCode,  SaleDepartureCountry = this.referenceCountryCode},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 200, SaleArrivalCountry = "BUL",  SaleDepartureCountry = this.referenceCountryCode},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 300, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = "YYY"},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 400, SaleArrivalCountry = this.euCountryCodeA,  SaleDepartureCountry = this.referenceCountryCode},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 500, SaleArrivalCountry = this.euCountryCodeB,  SaleDepartureCountry =this.referenceCountryCode},
                new Transaction { TransactionSellerVATNumberCountry = "XYI", TotalActivityVATIncludedAmount = 600, SaleArrivalCountry ="XYI",  SaleDepartureCountry =this.referenceCountryCode}
            };

            StepTwoCalculationOperation target = this.CreateStepTwoCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(6000, actualResult);
        }

        [Fact]
        public void CalculateConsidersExchangeRates()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(this.referenceCurrencyCode, It.IsAny<DateTime?>())).Returns(1);
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate("USD", It.IsAny<DateTime?>())).Returns(2);
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate("BGN", It.IsAny<DateTime?>())).Returns(5);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                //Records that should be taken
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 1000, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = this.referenceCountryCode, TransactionCurrencyCode=this.referenceCurrencyCode },
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 2000, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = this.referenceCountryCode, TransactionCurrencyCode="USD" },
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 3000, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = this.referenceCountryCode, TransactionCurrencyCode="BGN" },
            };

            StepTwoCalculationOperation target = this.CreateStepTwoCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(20000, actualResult);
        }

        private StepTwoCalculationOperation CreateStepTwoCalculationOperation()
        {
            return new StepTwoCalculationOperation(exchangeServiceMock.Object, this.appConfigurationServiceMock.Object);
        }
    }
}
