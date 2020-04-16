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
        public void CalculateSumsTotalActivityVatIncludedAmountFilteringBySaleArrivalCountry()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(1);
            List<ITransaction> transactionList = new List<ITransaction>()
            {
                new Transaction{SaleArrivalCountry=this.referenceCountryCode, TotalActivityVATIncludedAmount=100 },
                new Transaction{SaleArrivalCountry=this.referenceCountryCode, TotalActivityVATIncludedAmount=100 },
                new Transaction{SaleArrivalCountry="USA", TotalActivityVATIncludedAmount=100 },
                new Transaction{SaleArrivalCountry="ESP", TotalActivityVATIncludedAmount=100 }
            };
            StepOneCalculationOperation target = this.CreateStepOneCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(200, actualResult);
        }

        [Fact]
        public void CalculateIgnoresTotalActivityVatIncludedAmountIfNull()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(1);
            List<ITransaction> transactionList = new List<ITransaction>()
            {
                new Transaction{SaleArrivalCountry=this.referenceCountryCode, TotalActivityVATIncludedAmount=200 },
                new Transaction{SaleArrivalCountry=this.referenceCountryCode, TotalActivityVATIncludedAmount=300 },
                new Transaction{SaleArrivalCountry=this.referenceCountryCode, TotalActivityVATIncludedAmount=null},
                new Transaction{SaleArrivalCountry=this.referenceCountryCode, TotalActivityVATIncludedAmount=null},
                new Transaction{SaleArrivalCountry="USA", TotalActivityVATIncludedAmount=100 },
                new Transaction{SaleArrivalCountry="ESP", TotalActivityVATIncludedAmount=100 }
            };
            StepOneCalculationOperation target = this.CreateStepOneCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(500, actualResult);
        }

        [Fact]
        public void CalculateConsidersExchangeRates()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(this.referenceCurrencyCode, It.IsAny<DateTime?>())).Returns(1);
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate("USD", It.IsAny<DateTime?>())).Returns(2);
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate("BGN", It.IsAny<DateTime?>())).Returns(5);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                new Transaction{SaleArrivalCountry=this.referenceCountryCode, TotalActivityVATIncludedAmount=100, TransactionCurrencyCode=this.referenceCurrencyCode, TransactionCompleteDate=DateTime.Now },
                new Transaction{SaleArrivalCountry=this.referenceCountryCode, TotalActivityVATIncludedAmount=200, TransactionCurrencyCode="USD", TransactionCompleteDate=DateTime.Now },
                new Transaction{SaleArrivalCountry=this.referenceCountryCode, TotalActivityVATIncludedAmount=100, TransactionCurrencyCode="BGN", TransactionCompleteDate=DateTime.Now },
                new Transaction{SaleArrivalCountry="USD", TotalActivityVATIncludedAmount=100 },
                new Transaction{SaleArrivalCountry="BGN", TotalActivityVATIncludedAmount=100 }
            };

            StepOneCalculationOperation target = this.CreateStepOneCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(1000, actualResult);
        }

        private StepOneCalculationOperation CreateStepOneCalculationOperation()
        {
            return new StepOneCalculationOperation(exchangeServiceMock.Object, this.appConfigurationServiceMock.Object);
        }
    }
}
