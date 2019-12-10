using CsvHelper;
using CsvHelper.Configuration;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TransactionCalculator.Models.ExchangeRates;
using TransactionCalculator.Models.Transaction;
using TransactionsCalculator.Interfaces.Models;

namespace Tests
{
    class Program
    {
        private static readonly string ReferenceCurrencyCode = "EUR";
        private static Dictionary<string, decimal> exchangeCurrenciesDict = new Dictionary<string, decimal>();

        static void Main(string[] args)
        {
            exchangeCurrenciesDict.Add(ReferenceCurrencyCode, 1);

            CurrencyDataAPIClient();

            //Read from input
            string saleArrivalCountry = "IT";

            Console.WriteLine("Hello World!");
            var bad = new List<string>();
            Configuration config = new Configuration();
            config.Delimiter = "\t";
            config.BadDataFound = context =>
            {
                bad.Add(context.RawRecord);
            };

            List<Transaction> transactionList = null;

            using (var reader = new StreamReader("D:\\SW Development\\Customers\\Gimmy\\TransactionCalculator\\Docs\\IT178-10.txt"))
            using (var csv = new CsvReader(reader, config))
            {
                transactionList = csv.GetRecords<Transaction>().ToList();


                //foreach (Transaction transaction in transactionList)
                //{
                //    if (transaction.TransactionCurrencyCode.Equals(CurrencyCodeEUR))
                //    {

                //    }
                //    else if (!string.IsNullOrEmpty(transaction.TransactionCurrencyCode))
                //    {

                //    }
                //}
            }


            decimal q1 = transactionList.Where(x => x.SaleArrivalCountry.Equals(saleArrivalCountry) && x.TotalActivityVatIncludedAmount.HasValue)
                                        .Sum(x => x.TotalActivityVatIncludedAmount.Value * GetExchangeRate(x.TransactionCurrencyCode));



            Console.WriteLine(q1);

        }

        static decimal GetExchangeRate(string currencyCode)
        {
            decimal exchangeRate;
            if (exchangeCurrenciesDict.ContainsKey(currencyCode))
            {
                exchangeRate = exchangeCurrenciesDict[currencyCode];
            }
            else
            {
                exchangeRate = GetExchangeRateFromAPI(currencyCode);
                exchangeCurrenciesDict.Add(currencyCode, exchangeRate);
            }

            return exchangeRate;
        }

        static decimal GetExchangeRateFromAPI(string currencyCode)
        {
            IExchangeRatesInfo exchangeRateInfo = "https://frankfurter.app/"
               .AppendPathSegment("latest")
               .SetQueryParam("from", currencyCode)
               .SetQueryParam("to", ReferenceCurrencyCode)
               .GetJsonAsync<FrakfurterExchangeRatesInfoEUR>()
               .Result;

            return exchangeRateInfo.GetReferenceExchangeRate();
        }


        static void CurrencyDataAPIClient()
        {
            //TODO: Use backup client/values
            //TODO: Using Flurl With an IoC Container?
            //FrakfurterExchangeRatesInfo result = "https://help.frankfurter.app/"
            //     .AppendPathSegment("latest")
            //     .GetJsonAsync<FrakfurterExchangeRatesInfo>()
            //     .Result;


            string currencyCode = "USD";

            //CLIENT
            //JObject jObject = "https://frankfurter.app/"
            //     .AppendPathSegment("latest")
            //     .SetQueryParam("from", currencyCode)
            //     .SetQueryParam("to", CurrencyCodeEUR)
            //     .GetJsonAsync<JObject>()
            //     .Result;

            IExchangeRatesInfo result = "https://frankfurter.app/"
               .AppendPathSegment("latest")
               .SetQueryParam("from", currencyCode)
               .SetQueryParam("to", ReferenceCurrencyCode)
               .GetJsonAsync<FrakfurterExchangeRatesInfoEUR>()
               .Result;



        }
    }
}
