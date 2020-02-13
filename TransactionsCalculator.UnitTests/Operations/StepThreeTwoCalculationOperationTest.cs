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
    public class StepThreeTwoCalculationOperationTest
    {
        private readonly Mock<IExchangeRatesService> exchangeServiceMock;
        private readonly Mock<IAppConfigurationService> appConfigurationServiceMock;
        private readonly string referenceCountryCode = "IT";
        private readonly string referenceCurrencyCode = "EUR";

        public StepThreeTwoCalculationOperationTest()
        {
            this.exchangeServiceMock = new Mock<IExchangeRatesService>();

            this.appConfigurationServiceMock = new Mock<IAppConfigurationService>();
            this.appConfigurationServiceMock.SetupGet<string>(x => x.ReferenceCountryCode).Returns(this.referenceCountryCode);
        }

        [Fact]
        public void CalculateFiltersData()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(1);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                //Data that should be taken
                new Transaction {SellerDepartCountryVATNumber=$"BG1234", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 100},
                new Transaction {SellerDepartCountryVATNumber=$"US5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 300},
               
                //Data that should be filtered out
                new Transaction {SellerDepartCountryVATNumber=string.Empty, BuyerVATNumberCountry="CH", TotalActivityVATAmount=null, TotalActivityVATIncludedAmount = 1600},
                new Transaction {SellerDepartCountryVATNumber=null, BuyerVATNumberCountry="CH", TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 1300},
                new Transaction {SellerDepartCountryVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 1400},
                new Transaction {SellerDepartCountryVATNumber=$"BG5678", BuyerVATNumberCountry="CH", TotalActivityVATAmount=4, TotalActivityVATIncludedAmount =1500},
                new Transaction {SellerDepartCountryVATNumber=$"US5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=10, TotalActivityVATIncludedAmount = 1200},
                new Transaction {SellerDepartCountryVATNumber=$"US5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=null, TotalActivityVATIncludedAmount = 1200},
            };

            StepThreeTwoCalculationOperation target = this.CreateStepThreeTwoCalculationOperation();
            var actualResult = target.Calculate(transactionList);

            Assert.Equal(400, actualResult);
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
                new Transaction {SellerDepartCountryVATNumber=$"BG1234", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 100, TransactionCurrencyCode = "USD"},
                new Transaction {SellerDepartCountryVATNumber=$"US5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 300, TransactionCurrencyCode = "BGN"},
                new Transaction {SellerDepartCountryVATNumber=$"US5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 300, TransactionCurrencyCode = this.referenceCurrencyCode},
               
                //Data that should be filtered out
                new Transaction {SellerDepartCountryVATNumber=string.Empty, BuyerVATNumberCountry="CH", TotalActivityVATAmount=null, TotalActivityVATIncludedAmount = 1600},
                new Transaction {SellerDepartCountryVATNumber=null, BuyerVATNumberCountry="CH", TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 1300},
                new Transaction {SellerDepartCountryVATNumber=$"{this.referenceCountryCode}5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=0, TotalActivityVATIncludedAmount = 1400},
                new Transaction {SellerDepartCountryVATNumber=$"BG5678", BuyerVATNumberCountry="CH", TotalActivityVATAmount=4, TotalActivityVATIncludedAmount =1500},
                new Transaction {SellerDepartCountryVATNumber=$"US5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=10, TotalActivityVATIncludedAmount = 1200},
                new Transaction {SellerDepartCountryVATNumber=$"US5678", BuyerVATNumberCountry=this.referenceCountryCode, TotalActivityVATAmount=null, TotalActivityVATIncludedAmount = 1200},
            };

            StepThreeTwoCalculationOperation target = this.CreateStepThreeTwoCalculationOperation();
            var actualResult = target.Calculate(transactionList);

            Assert.Equal(2000, actualResult);
        }

        private StepThreeTwoCalculationOperation CreateStepThreeTwoCalculationOperation()
        {
            return new StepThreeTwoCalculationOperation(exchangeServiceMock.Object, this.appConfigurationServiceMock.Object);
        }
    }
}
