using Moq;
using System;
using System.Collections.Generic;
using TransactionCalculator.Models.Transaction;
using TransactionsCalculator.Core.Operations;
using TransactionsCalculator.Interfaces.Models;
using Xunit;

namespace TransactionsCalculator.UnitTests.Operations
{
    public class StepThreeThreeCalculationOperationTest : BaseOperationTest
    {
        public StepThreeThreeCalculationOperationTest() : base()
        {
        }

        [Fact]
        public void CalculateFiltersData()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(1);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                //Data that should  be taken
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}1234", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 2100},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 2200},
               
                //Data that should be filtered out
                new Transaction {TransactionSellerVATNumber=$"BG234", BuyerVATNumberCountry="CH", TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 300},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=5, TotalActivityVATIncludedAmount = 400},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry="CH", TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 500},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry="CH", TotalActivityVATAmount=null, TotalActivityVATIncludedAmount = 600},
            };

            StepThreeThreeCalculationOperation target = this.CreateStepThreeThreeCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(4300, actualResult);
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
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}1234", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 2000, TransactionCurrencyCode = this.referenceCurrencyCode},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 3000, TransactionCurrencyCode= "BGN"},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 4000, TransactionCurrencyCode= "USD"},
               
                //Data that should be filtered out
                new Transaction {TransactionSellerVATNumber=$"BG234", BuyerVATNumberCountry="CH", TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 300},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=5, TotalActivityVATIncludedAmount = 400},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry="CH", TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 500},
                new Transaction {TransactionSellerVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry="CH", TotalActivityVATAmount=null, TotalActivityVATIncludedAmount = 600},
            };

            StepThreeThreeCalculationOperation target = this.CreateStepThreeThreeCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(25000, actualResult);
        }

        private StepThreeThreeCalculationOperation CreateStepThreeThreeCalculationOperation()
        {
            return new StepThreeThreeCalculationOperation(exchangeServiceMock.Object, this.appConfigurationServiceMock.Object);
        }
    }
}
