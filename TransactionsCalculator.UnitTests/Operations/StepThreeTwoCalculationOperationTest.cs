using Moq;
using System;
using System.Collections.Generic;
using TransactionCalculator.Models.Transaction;
using TransactionsCalculator.Core.Operations;
using TransactionsCalculator.Interfaces.Models;
using Xunit;

namespace TransactionsCalculator.UnitTests.Operations
{
    public class StepThreeTwoCalculationOperationTest : BaseOperationTest
    {
        public StepThreeTwoCalculationOperationTest() : base()
        {
        }

        [Fact]
        public void CalculateFiltersDataAndSumsAmounts()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(1);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                //Data that should be taken
                new Transaction {TransactionSellerVATNumber=$"BG1234", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 100},
                new Transaction {TransactionSellerVATNumber=$"US5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 300},
               
                //Data that should be filtered out
                new Transaction {TransactionSellerVATNumber=string.Empty, BuyerVATNumberCountry="CH", TotalActivityVATAmount=null, TotalActivityVATIncludedAmount = 1600},
                new Transaction {TransactionSellerVATNumber=null, BuyerVATNumberCountry="CH", TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 1300},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 1400},
                new Transaction {TransactionSellerVATNumber=$"BG5678", BuyerVATNumberCountry="CH", TotalActivityVATAmount=4, TotalActivityVATIncludedAmount =1500},
                new Transaction {TransactionSellerVATNumber=$"US5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=10, TotalActivityVATIncludedAmount = 1200},
                new Transaction {TransactionSellerVATNumber=$"US5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=null, TotalActivityVATIncludedAmount = 1200},
            };

            StepThreeTwoCalculationOperation target = this.CreateStepThreeTwoCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(400, actualResult);
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
                new Transaction {TransactionSellerVATNumber=$"BG1234", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 100, TransactionCurrencyCode = "USD"},
                new Transaction {TransactionSellerVATNumber=$"US5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 300, TransactionCurrencyCode = "BGN"},
                new Transaction {TransactionSellerVATNumber=$"US5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 300, TransactionCurrencyCode = this.referenceCurrencyCode}
            };

            StepThreeTwoCalculationOperation target = this.CreateStepThreeTwoCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(2000, actualResult);
        }

        private StepThreeTwoCalculationOperation CreateStepThreeTwoCalculationOperation()
        {
            return new StepThreeTwoCalculationOperation(exchangeServiceMock.Object, this.appConfigurationServiceMock.Object);
        }
    }
}
