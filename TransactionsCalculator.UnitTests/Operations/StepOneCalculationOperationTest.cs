using Moq;
using System;
using System.Collections.Generic;
using TransactionCalculator.Models.Transaction;
using TransactionsCalculator.Core.Operations;
using TransactionsCalculator.Interfaces.Models;
using Xunit;

namespace TransactionsCalculator.UnitTests.Operations
{
    public class StepOneCalculationOperationTest : BaseOperationTest
    {
        public StepOneCalculationOperationTest() : base()
        {
        }

        [Fact]
        public void CalculateFiltersDataAndSumsAmounts()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(1);
            List<ITransaction> transactionList = new List<ITransaction>()
            {
                //Data that will be taken
                new Transaction{SaleArrivalCountry=this.referenceCountryCode, TotalActivityVATIncludedAmount=1000, SaleDepartureCountry = this.euCountryCodeA },
                new Transaction{SaleArrivalCountry=this.referenceCountryCode, TotalActivityVATIncludedAmount=2000, SaleDepartureCountry = this.euCountryCodeB },
                new Transaction{SaleArrivalCountry=this.referenceCountryCode, TotalActivityVATIncludedAmount=3000, SaleDepartureCountry = this.euCountryCodeC },

                //Data that will be filtered
                new Transaction{SaleArrivalCountry="ESP", TotalActivityVATIncludedAmount=1 },
                new Transaction{SaleArrivalCountry="USA", TotalActivityVATIncludedAmount=2, SaleDepartureCountry = this.euCountryCodeA },
                new Transaction{SaleArrivalCountry=this.referenceCountryCode, TotalActivityVATIncludedAmount=null, SaleDepartureCountry = "random-1"},
                new Transaction{SaleArrivalCountry=this.referenceCountryCode, TotalActivityVATIncludedAmount=3, SaleDepartureCountry = "random-2" },
            };
            StepOneCalculationOperation target = this.CreateStepOneCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(6000, actualResult);
        }

        [Fact]
        public void CalculateConsidersExchangeRatesWhenSummingAmounts()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(this.referenceCurrencyCode, It.IsAny<DateTime?>())).Returns(1);
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate("USD", It.IsAny<DateTime?>())).Returns(2);
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate("BGN", It.IsAny<DateTime?>())).Returns(5);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                //Data that will be taken
                new Transaction{SaleArrivalCountry=this.referenceCountryCode, TransactionCurrencyCode=this.referenceCountryCode, TotalActivityVATIncludedAmount=1000, SaleDepartureCountry = this.euCountryCodeA },
                new Transaction{SaleArrivalCountry=this.referenceCountryCode, TransactionCurrencyCode="USD", TotalActivityVATIncludedAmount=2000, SaleDepartureCountry = this.euCountryCodeB },
                new Transaction{SaleArrivalCountry=this.referenceCountryCode, TransactionCurrencyCode="BGN", TotalActivityVATIncludedAmount=3000, SaleDepartureCountry = this.euCountryCodeC },
            };

            StepOneCalculationOperation target = this.CreateStepOneCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(19000, actualResult);
        }

        private StepOneCalculationOperation CreateStepOneCalculationOperation()
        {
            return new StepOneCalculationOperation(exchangeServiceMock.Object, this.appConfigurationServiceMock.Object);
        }
    }
}
