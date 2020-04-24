using Moq;
using System;
using System.Collections.Generic;
using TransactionCalculator.Models.Transaction;
using TransactionsCalculator.Core.Operations;
using TransactionsCalculator.Interfaces.Models;
using Xunit;

namespace TransactionsCalculator.UnitTests.Operations
{
    public class StepFourOneCalculationOperationTest : BaseOperationTest
    {
        public StepFourOneCalculationOperationTest() : base()
        {
        }

        [Fact]
        public void CalculateFiltersDataAndSumsAmounts()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(1);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                 //Data that should be taken
                new Transaction{TaxableJurisdiction=this.referenceTaxableJurisdiction, TotalActivityVATAmount=1500},
                new Transaction{TaxableJurisdiction=this.referenceTaxableJurisdiction, TotalActivityVATAmount=2000},

                //Data that should be filtered out
                new Transaction{TaxableJurisdiction=string.Empty, TotalActivityVATAmount=1 },
                new Transaction{TaxableJurisdiction=null, TotalActivityVATAmount=2},
                new Transaction{TaxableJurisdiction="BLABLA", TotalActivityVATAmount=3},
                new Transaction{TaxableJurisdiction=this.referenceTaxableJurisdiction, TotalActivityVATAmount=null},
            };

            StepFourOneCalculationOperation target = this.CreateStepFourOneCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(3500, actualResult);
        }

        [Fact]
        public void CalculateConsidersExchangeRatesWhenSummingAmounts()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(this.referenceCurrencyCode, It.IsAny<DateTime?>())).Returns(1);
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate("USD", It.IsAny<DateTime?>())).Returns(2);
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate("BGN", It.IsAny<DateTime?>())).Returns(5);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                 //Data that should be taken
                new Transaction{TaxableJurisdiction=this.referenceTaxableJurisdiction, TotalActivityVATAmount=1500, TransactionCurrencyCode=this.referenceCurrencyCode},
                new Transaction{TaxableJurisdiction=this.referenceTaxableJurisdiction, TotalActivityVATAmount=2000, TransactionCurrencyCode="USD"},
                new Transaction{TaxableJurisdiction=this.referenceTaxableJurisdiction, TotalActivityVATAmount=3000, TransactionCurrencyCode="BGN"}
            };

            StepFourOneCalculationOperation target = this.CreateStepFourOneCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(20500, actualResult);
        }

        private StepFourOneCalculationOperation CreateStepFourOneCalculationOperation()
        {
            return new StepFourOneCalculationOperation(exchangeServiceMock.Object, this.appConfigurationServiceMock.Object);
        }
    }
}
