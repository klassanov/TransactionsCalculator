using Moq;
using System;
using System.Collections.Generic;
using TransactionCalculator.Models.Transaction;
using TransactionsCalculator.Core.Operations;
using TransactionsCalculator.Interfaces.Models;
using Xunit;

namespace TransactionsCalculator.UnitTests.Operations
{
    public class StepFourFourCalculationOperationTest : BaseOperationTest
    {
        public StepFourFourCalculationOperationTest() : base()
        {
        }

        [Fact]
        public void CalculateFiltersDataAndSumsAmounts()
        {
            this.exchangeServiceMock.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(1);

            List<ITransaction> transactionList = new List<ITransaction>()
            {
                //Data that should be taken
                new Transaction{TaxableJurisdiction = "abc", SaleArrivalCountry=this.referenceCountryCode, VATInvNumber=string.Empty, TotalActivityVATIncludedAmount=1000 },
                new Transaction{TaxableJurisdiction = "abc", SaleArrivalCountry=this.referenceCountryCode, VATInvNumber=null, TotalActivityVATIncludedAmount=1000 },
                new Transaction{TaxableJurisdiction = "def", SaleArrivalCountry=this.referenceCountryCode, VATInvNumber=null, TotalActivityVATIncludedAmount=1000 },

                //Data that should be filtered out
                new Transaction{TaxableJurisdiction = null, SaleArrivalCountry=this.referenceCountryCode, VATInvNumber="abc", TotalActivityVATIncludedAmount=1  },
                new Transaction{TaxableJurisdiction = string.Empty, SaleArrivalCountry=this.referenceCountryCode, VATInvNumber="abc", TotalActivityVATIncludedAmount=2  },
                new Transaction{TaxableJurisdiction = this.referenceTaxableJurisdiction, SaleArrivalCountry=this.referenceCountryCode, VATInvNumber="abc", TotalActivityVATIncludedAmount=3  },
                new Transaction{TaxableJurisdiction = "abc", SaleArrivalCountry=null, VATInvNumber="abc", TotalActivityVATIncludedAmount=4  },
                new Transaction{TaxableJurisdiction = "abc", SaleArrivalCountry=string.Empty, VATInvNumber="abc", TotalActivityVATIncludedAmount=5  },
                new Transaction{TaxableJurisdiction = "abc", SaleArrivalCountry=this.referenceCountryCode, VATInvNumber="abc", TotalActivityVATIncludedAmount=6  },
                new Transaction{TaxableJurisdiction = "abc", SaleArrivalCountry=this.referenceCountryCode, VATInvNumber=null, TotalActivityVATIncludedAmount=null  },
                new Transaction{TaxableJurisdiction = "abc", SaleArrivalCountry=this.referenceCountryCode, VATInvNumber=string.Empty, TotalActivityVATIncludedAmount=null  },
            };

            StepFourFourCalculationOperation target = this.CreateStepFourFourCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(3000, actualResult);
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
                new Transaction{TaxableJurisdiction = "abc", SaleArrivalCountry=this.referenceCountryCode, VATInvNumber=string.Empty, TotalActivityVATIncludedAmount=1000, TransactionCurrencyCode=this.referenceCurrencyCode },
                new Transaction{TaxableJurisdiction = "abc", SaleArrivalCountry=this.referenceCountryCode, VATInvNumber=null, TotalActivityVATIncludedAmount=1000, TransactionCurrencyCode="USD" },
                new Transaction{TaxableJurisdiction = "def", SaleArrivalCountry=this.referenceCountryCode, VATInvNumber=null, TotalActivityVATIncludedAmount=1000, TransactionCurrencyCode="BGN" },
            };

            StepFourFourCalculationOperation target = this.CreateStepFourFourCalculationOperation();
            var actualResult = target.CalculateAmount(transactionList);

            Assert.Equal(8000, actualResult);
        }

        private StepFourFourCalculationOperation CreateStepFourFourCalculationOperation()
        {
            return new StepFourFourCalculationOperation(exchangeServiceMock.Object, this.appConfigurationServiceMock.Object);
        }
    }
}
