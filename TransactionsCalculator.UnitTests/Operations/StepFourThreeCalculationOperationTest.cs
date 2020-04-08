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
    public class StepFourThreeCalculationOperationTest
    {
        private readonly Mock<IExchangeRatesService> exchangeServiceMock;
        private readonly Mock<IAppConfigurationService> appConfigurationServiceMock;

        private readonly string referenceTaxableJurisdiction = "ITALIA";
        private readonly string referenceCountry = "ITA";
        private readonly string referenceCurrencyCode = "EUR";

        public StepFourThreeCalculationOperationTest()
        {
            this.exchangeServiceMock = new Mock<IExchangeRatesService>();

            this.appConfigurationServiceMock = new Mock<IAppConfigurationService>();
            this.appConfigurationServiceMock.SetupGet<string>(x => x.ReferenceCountryCode).Returns(this.referenceCountry);
        }

        [Fact]
        public void CalculateFiltersData()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(1);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                //Data that should be taken
                new Transaction{TaxableJurisdiction=null, TotalActivityVATIncludedAmount=1000, SaleArrivalCountry=this.referenceCountry},
                new Transaction{TaxableJurisdiction=string.Empty, TotalActivityVATIncludedAmount=2000, SaleArrivalCountry=this.referenceCountry},
                new Transaction{TaxableJurisdiction=string.Empty, TotalActivityVATIncludedAmount=3000, SaleArrivalCountry=this.referenceCountry},

                //Data that should be filtered out
                new Transaction{TaxableJurisdiction=string.Empty, TotalActivityVATIncludedAmount=3000, SaleArrivalCountry="USA"},
                new Transaction{TaxableJurisdiction=string.Empty, TotalActivityVATIncludedAmount=null},
                new Transaction{TaxableJurisdiction=null, TotalActivityVATIncludedAmount=null},
                new Transaction{TaxableJurisdiction=this.referenceTaxableJurisdiction, TotalActivityVATIncludedAmount=5000}
            };

            StepFourThreeCalculationOperation target = this.CreateStepFourThreeCalculationOperation();
            var actualResult = target.Calculate(transactionList);

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
                //Data that should be taken
                new Transaction{TaxableJurisdiction=null, TotalActivityVATIncludedAmount=1000, SaleArrivalCountry=this.referenceCountry, TransactionCurrencyCode=this.referenceCurrencyCode},
                new Transaction{TaxableJurisdiction=string.Empty, TotalActivityVATIncludedAmount=2000, SaleArrivalCountry=this.referenceCountry, TransactionCurrencyCode="USD"},
                new Transaction{TaxableJurisdiction=string.Empty, TotalActivityVATIncludedAmount=3000, SaleArrivalCountry=this.referenceCountry, TransactionCurrencyCode="BGN"},

                //Data that should be filtered out
                new Transaction{TaxableJurisdiction=string.Empty, TotalActivityVATIncludedAmount=3000, SaleArrivalCountry="USA", TransactionCurrencyCode="USD"},
                new Transaction{TaxableJurisdiction=string.Empty, TotalActivityVATIncludedAmount=null},
                new Transaction{TaxableJurisdiction=null, TotalActivityVATIncludedAmount=null},
                new Transaction{TaxableJurisdiction=this.referenceTaxableJurisdiction, TotalActivityVATIncludedAmount=5000}
            };

            StepFourThreeCalculationOperation target = this.CreateStepFourThreeCalculationOperation();
            var actualResult = target.Calculate(transactionList);

            Assert.Equal(20000, actualResult);
        }

        private StepFourThreeCalculationOperation CreateStepFourThreeCalculationOperation()
        {
            return new StepFourThreeCalculationOperation(exchangeServiceMock.Object, this.appConfigurationServiceMock.Object);
        }
    }
}
