using Moq;
using System;
using System.Collections.Generic;
using TransactionCalculator.Models.Transaction;
using TransactionsCalculator.Core.Operations;
using TransactionsCalculator.Interfaces.Models;
using Xunit;

namespace TransactionsCalculator.UnitTests.Operations
{
    public class StepThreeFourCalculationOperationTest : BaseOperationTest
    {
        public StepThreeFourCalculationOperationTest() : base()
        {
        }

        [Fact]
        public void CalculateFiltersDataAndSums()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(1);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                //Records that should be taken
                new Transaction{ TransactionSellerVATNumber="xyz", BuyerVATNumber="xyz", TotalActivityVATAmount=1000, TotalActivityVATIncludedAmount=1000 },
                new Transaction{ TransactionSellerVATNumber="abc", BuyerVATNumber="def", TotalActivityVATAmount=1000, TotalActivityVATIncludedAmount=2000 },
                new Transaction{ TransactionSellerVATNumber="ghh", BuyerVATNumber="gss", TotalActivityVATAmount=1000, TotalActivityVATIncludedAmount=3000 },

                //Records that shpuld be filtered out
                new Transaction{ TransactionSellerVATNumber=null, BuyerVATNumber="xyz", TotalActivityVATAmount=100, TotalActivityVATIncludedAmount=1},
                new Transaction{ TransactionSellerVATNumber=string.Empty, BuyerVATNumber="xyz", TotalActivityVATAmount=100, TotalActivityVATIncludedAmount=2},
                new Transaction{ TransactionSellerVATNumber="xyz", BuyerVATNumber=null, TotalActivityVATAmount=200, TotalActivityVATIncludedAmount=3},
                new Transaction{ TransactionSellerVATNumber="xyz", BuyerVATNumber=string.Empty, TotalActivityVATAmount=200, TotalActivityVATIncludedAmount=4},
                new Transaction{ TransactionSellerVATNumber="xyz", BuyerVATNumber="xyz", TotalActivityVATAmount=null, TotalActivityVATIncludedAmount=5},
                new Transaction{ TransactionSellerVATNumber="xyz", BuyerVATNumber="xyz", TotalActivityVATAmount=0, TotalActivityVATIncludedAmount=6},
                new Transaction{ TransactionSellerVATNumber="xyz", BuyerVATNumber="xyz", TotalActivityVATAmount=0, TotalActivityVATIncludedAmount=null}
            };

            StepThreeFourCalculationOperation target = this.CreateStepThreeFourCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(6000, actualResult);
        }

        [Fact]
        public void CalculateConsidersExchangeRatesAndSums()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(this.referenceCurrencyCode, It.IsAny<DateTime?>())).Returns(1);
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate("USD", It.IsAny<DateTime?>())).Returns(2);
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate("BGN", It.IsAny<DateTime?>())).Returns(5);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                //Records that should be taken
                new Transaction{ TransactionSellerVATNumber="xyz", BuyerVATNumber="xyz", TotalActivityVATAmount=1000, TotalActivityVATIncludedAmount=1000, TransactionCurrencyCode=this.referenceCurrencyCode },
                new Transaction{ TransactionSellerVATNumber="abc", BuyerVATNumber="def", TotalActivityVATAmount=1000, TotalActivityVATIncludedAmount=2000, TransactionCurrencyCode="USD" },
                new Transaction{ TransactionSellerVATNumber="ghh", BuyerVATNumber="gss", TotalActivityVATAmount=1000, TotalActivityVATIncludedAmount=3000, TransactionCurrencyCode="BGN" },
            };

            StepThreeFourCalculationOperation target = this.CreateStepThreeFourCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(20000, actualResult);
        }

        private StepThreeFourCalculationOperation CreateStepThreeFourCalculationOperation()
        {
            return new StepThreeFourCalculationOperation(this.exchangeServiceMock.Object, this.appConfigurationServiceMock.Object);
        }
    }
}
