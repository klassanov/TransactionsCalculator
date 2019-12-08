using CsvHelper;
using CsvHelper.Configuration;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.IO;
using TransactionCalculator.Models.ExchangeRates;
using TransactionCalculator.Models.Transaction;
using TransactionsCalculator.Interfaces.Models;

namespace Tests
{


    class Program
    {

        //TODO: ClassMap<Foo> create a class map for trimming the read values
        //TODO: make it as a configuration parameter
        //TODO: rewrite everything depending entirely on interfaces
        private static readonly string CurrencyCodeEUR = "EUR";

        static void Main(string[] args)
        {
            CurrencyDataAPIClientAsync();

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

            using (var reader = new StreamReader("D:\\SW Development\\Customers\\Gimmy\\TransactionCalculator\\Docs\\IT178-10.txt"))
            using (var csv = new CsvReader(reader, config))
            {
                List<Transaction> result = new List<Transaction>();
                IEnumerable<Transaction> transactionList = csv.GetRecords<Transaction>();
                foreach (Transaction transaction in transactionList)
                {
                    if (transaction.TransactionCurrencyCode.Equals(CurrencyCodeEUR))
                    {

                    }
                    else if (!string.IsNullOrEmpty(transaction.TransactionCurrencyCode))
                    {

                    }
                }

            }

        }


        static void CurrencyDataAPIClientAsync()
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
               .SetQueryParam("to", CurrencyCodeEUR)
               .GetJsonAsync<FrakfurterExchangeRatesInfo>()
               .Result;



        }
    }
}
