using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
            int expectedCountriesNumber = 3;

            Assert.Equal(exclCountries.Count, expectedCountriesNumber);
            Assert.Contains("BUL", exclCountries);
            Assert.Contains("XUI", exclCountries);
            Assert.Contains(this.referenceCountryCode, exclCountries);
            Assert.DoesNotContain(this.euCountryCodeA, exclCountries);
            Assert.DoesNotContain(this.euCountryCodeB, exclCountries);
        }

        [Fact]
        public void CalculateFiltersDataAndSumsAmounts()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(1);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                //Records that should be taken
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 3000, SaleArrivalCountry =  appConfigurationServiceMock.Object.EUCountryCodes.ElementAt(1),  SaleDepartureCountry = this.referenceCountryCode},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 5000, SaleArrivalCountry =  appConfigurationServiceMock.Object.EUCountryCodes.ElementAt(2),  SaleDepartureCountry = this.referenceCountryCode},

                //Records that should be filtered put
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = null, SaleArrivalCountry = this.referenceCountryCode,  SaleDepartureCountry = this.referenceCountryCode},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 1, SaleArrivalCountry = this.referenceCountryCode,  SaleDepartureCountry = this.referenceCountryCode},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 2, SaleArrivalCountry = "BUL",  SaleDepartureCountry = this.referenceCountryCode},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 3, SaleArrivalCountry = "ZZZ",  SaleDepartureCountry = "YYY"},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 4, SaleArrivalCountry = appConfigurationServiceMock.Object.EUCountryCodes.ElementAt(0),  SaleDepartureCountry = this.referenceCountryCode},
                new Transaction { TransactionSellerVATNumberCountry =  appConfigurationServiceMock.Object.EUCountryCodes.ElementAt(0), TotalActivityVATIncludedAmount =5, SaleArrivalCountry = appConfigurationServiceMock.Object.EUCountryCodes.ElementAt(0),  SaleDepartureCountry = this.referenceCountryCode},
                new Transaction { TransactionSellerVATNumberCountry = "XYI", TotalActivityVATIncludedAmount = 7, SaleArrivalCountry ="XYI",  SaleDepartureCountry =this.referenceCountryCode}
            };

            StepTwoCalculationOperation target = this.CreateStepTwoCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(8000, actualResult);
        }

        [Fact]
        public void CalculateConsidersExchangeRatesWhenSummingAmounts()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(this.referenceCurrencyCode, It.IsAny<DateTime?>())).Returns(1);
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate("USD", It.IsAny<DateTime?>())).Returns(2);
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate("BGN", It.IsAny<DateTime?>())).Returns(5);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                //Records that should be taken
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 3000, SaleArrivalCountry =  appConfigurationServiceMock.Object.EUCountryCodes.ElementAt(1),  SaleDepartureCountry = this.referenceCountryCode, TransactionCurrencyCode="USD"},
                new Transaction { TransactionSellerVATNumberCountry = "BUL", TotalActivityVATIncludedAmount = 5000, SaleArrivalCountry =  appConfigurationServiceMock.Object.EUCountryCodes.ElementAt(2),  SaleDepartureCountry = this.referenceCountryCode, TransactionCurrencyCode=this.referenceCurrencyCode},
            };

            StepTwoCalculationOperation target = this.CreateStepTwoCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(11000, actualResult);
        }

        private StepTwoCalculationOperation CreateStepTwoCalculationOperation()
        {
            return new StepTwoCalculationOperation(exchangeServiceMock.Object, this.appConfigurationServiceMock.Object);
        }
    }
}
