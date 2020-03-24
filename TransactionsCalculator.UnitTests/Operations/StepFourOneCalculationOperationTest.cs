﻿using Moq;
using System;
using System.Collections.Generic;
using TransactionCalculator.Models.Transaction;
using TransactionsCalculator.Core.Operations;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;
using Xunit;

namespace TransactionsCalculator.UnitTests.Operations
{
    public class StepFourOneCalculationOperationTest
    {
        private readonly Mock<IExchangeRatesService> exchangeServiceMock;
        private readonly Mock<IAppConfigurationService> appConfigurationServiceMock;

        private readonly string referenceTaxableJurisdiction = "ITALIA";
        private readonly string referenceCurrencyCode = "EUR";

        public StepFourOneCalculationOperationTest()
        {
            this.exchangeServiceMock = new Mock<IExchangeRatesService>();

            this.appConfigurationServiceMock = new Mock<IAppConfigurationService>();
            this.appConfigurationServiceMock.SetupGet<string>(x => x.ReferenceTaxableJurisdiction).Returns(this.referenceTaxableJurisdiction);
        }

        [Fact]
        public void CalculateFiltersData()
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
            var actualResult = target.Calculate(transactionList);

            Assert.Equal(3500, actualResult);
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
                new Transaction{TaxableJurisdiction=this.referenceTaxableJurisdiction, TotalActivityVATAmount=1500, TransactionCurrencyCode=this.referenceCurrencyCode},
                new Transaction{TaxableJurisdiction=this.referenceTaxableJurisdiction, TotalActivityVATAmount=2000, TransactionCurrencyCode="USD"},
                new Transaction{TaxableJurisdiction=this.referenceTaxableJurisdiction, TotalActivityVATAmount=3000, TransactionCurrencyCode="BGN"},

                //Data that should be filtered out
                new Transaction{TaxableJurisdiction=string.Empty, TotalActivityVATAmount=1, TransactionCurrencyCode=this.referenceCurrencyCode },
                new Transaction{TaxableJurisdiction=null, TotalActivityVATAmount=2, TransactionCurrencyCode=this.referenceCurrencyCode},
                new Transaction{TaxableJurisdiction="BLABLA", TotalActivityVATAmount=3, TransactionCurrencyCode=this.referenceCurrencyCode},
                new Transaction{TaxableJurisdiction=this.referenceTaxableJurisdiction, TotalActivityVATAmount=null, TransactionCurrencyCode=this.referenceCurrencyCode},
            };

            StepFourOneCalculationOperation target = this.CreateStepFourOneCalculationOperation();
            var actualResult = target.Calculate(transactionList);

            Assert.Equal(20500, actualResult);

        }

        private StepFourOneCalculationOperation CreateStepFourOneCalculationOperation()
        {
            return new StepFourOneCalculationOperation(exchangeServiceMock.Object, this.appConfigurationServiceMock.Object);
        }
    }
}
