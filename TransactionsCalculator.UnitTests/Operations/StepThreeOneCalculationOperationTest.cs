using Moq;
using System;
using System.Collections.Generic;
using TransactionCalculator.Models.Transaction;
using TransactionsCalculator.Core.Operations;
using TransactionsCalculator.Interfaces.Models;
using Xunit;

namespace TransactionsCalculator.UnitTests.Operations
{
    public class StepThreeOneCalculationOperationTest : BaseOperationTest
    {
        public StepThreeOneCalculationOperationTest() : base()
        {
        }

        [Fact]
        public void CalculateFiltersData()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(1);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                //Data that should  be taken
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}1234", BuyerVATNumberCountry="CH", TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 100},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry="CH", TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 200},
               
                //Data that should be filtered out
                new Transaction {TransactionSellerVATNumber=$"BG234", BuyerVATNumberCountry="CH", TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 300},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 400},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry="CH", TotalActivityVATAmount=4, TotalActivityVATIncludedAmount = 500},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry="CH", TotalActivityVATAmount=null, TotalActivityVATIncludedAmount = 600},
            };

            StepThreeOneCalculationOperation target = this.CreateStepThreeOneCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(300, actualResult);
        }

        [Fact]
        public void CalculateConsidersExchangeRates()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(this.referenceCurrencyCode, It.IsAny<DateTime?>())).Returns(1);
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate("USD", It.IsAny<DateTime?>())).Returns(2);
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate("BGN", It.IsAny<DateTime?>())).Returns(5);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                //Data that should  be taken
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}1234", BuyerVATNumberCountry="CH", TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 100, TransactionCurrencyCode="BGN"},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry="CH", TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 200, TransactionCurrencyCode="USD"},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry="CH", TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 300, TransactionCurrencyCode=this.referenceCurrencyCode},
               
                //Data that should be filtered out
                new Transaction {TransactionSellerVATNumber=$"BG234", BuyerVATNumberCountry="CH", TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 300},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 400},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry="CH", TotalActivityVATAmount=4, TotalActivityVATIncludedAmount = 500},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry="CH", TotalActivityVATAmount=null, TotalActivityVATIncludedAmount = 600},
            };

            StepThreeOneCalculationOperation target = this.CreateStepThreeOneCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(1200, actualResult);
        }

        private StepThreeOneCalculationOperation CreateStepThreeOneCalculationOperation()
        {
            return new StepThreeOneCalculationOperation(exchangeServiceMock.Object, this.appConfigurationServiceMock.Object);
        }
    }
}
