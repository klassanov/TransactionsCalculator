﻿using CsvHelper;
using CsvHelper.Configuration;
using Flurl;
using Flurl.Http;
using IronPdf;
using Microsoft.Extensions.Configuration;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            //DecimalParsing();
            FormattingTests();
            return;

            Console.WriteLine("Hello, Gimmy!");
            Console.WriteLine();

            //GeneratePDF();

            IConfiguration appSettings = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

            Console.WriteLine($"Reading from the config: { appSettings["referenceCurrencyCode"]}");

            exchangeCurrenciesDict.Add(ReferenceCurrencyCode, 1);

            List<Transaction> transactionList = null;

            CurrencyDataAPIClient();

            //Read from input
            string saleArrivalCountry = "IT";

            //Read from input
            string directoryName = args.Length > 0 ? args[0] : "D:\\SW Development\\Customers\\Gimmy\\TransactionCalculator\\Tests\\";
            string[] filePaths = Directory.GetFiles(directoryName, "*.txt");


            var bad = new List<string>();
            Configuration config = new Configuration();
            config.Delimiter = "\t";
            config.BadDataFound = context =>
            {
                bad.Add(context.RawRecord);
            };


            //string fullFilename = "D:\\SW Development\\Customers\\Gimmy\\TransactionCalculator\\Docs\\IT178-10.txt";
            //string fullFilename = "D:\\SW Development\\Customers\\Gimmy\\TransactionCalculator\\Docs\\IT120-10.txt";

            Console.WriteLine($"Working directory: {directoryName}");
            Console.WriteLine();

            foreach (string filePath in filePaths)
            {
                Console.WriteLine($"Elaboration of { filePath}");

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, config))
                {
                    transactionList = csv.GetRecords<Transaction>().ToList();
                }

                //Query 1
                decimal q1 = Math.Round(transactionList.Where(x => x.SaleArrivalCountry.Equals(saleArrivalCountry) &&
                                                        x.TotalActivityVatIncludedAmount.HasValue)
                                            .Sum(x => x.TotalActivityVatIncludedAmount.Value * GetExchangeRate(x.TransactionCurrencyCode)), 2);

                //Query 2
                HashSet<string> countriesHash = transactionList.Select(x => x.TransactionSellerVATNumberCountry).ToHashSet();

                decimal q2 = Math.Round(transactionList.Where(x => x.SaleDepartureCountry.Equals(saleArrivalCountry) &&
                                                    x.TotalActivityVatIncludedAmount.HasValue &&
                                                    !countriesHash.Contains(x.SaleArrivalCountry))
                                                    .Sum(x => x.TotalActivityVatIncludedAmount.Value * GetExchangeRate(x.TransactionCurrencyCode)), 2);



                Console.WriteLine($"Step 1: {q1}");
                Console.WriteLine($"Step 2: {q2}");
                Console.WriteLine();
            }

            Console.WriteLine();
            PrintExchangeRates();
            Console.WriteLine();

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

        static void PrintExchangeRates()
        {
            Console.WriteLine("Used exchange rates");
            foreach (string key in exchangeCurrenciesDict.Keys)
            {
                Console.WriteLine($"{key}: {exchangeCurrenciesDict[key]}");
            }
        }

        static void GeneratePDF()
        {
            HtmlToPdf renderer = new HtmlToPdf();

            //Renderer.RenderHtmlAsPdf("<h1>Hello Worldeeeee<h1>").SaveAs("html-string.pdf");
            // string template = "Hello @Model.Name, welcome to RazorEngine!";
            //var result = Engine.Razor.RunCompile(template, "templateKey", null, new { Name = "World" });

            string template = File.ReadAllText(@"PDFTemplates/ReportTemplate.cshtml");
            var result = Engine.Razor.RunCompile(template, "templateKey", null, new { Test = "Kolko e 4asa" });
            renderer.RenderHtmlAsPdf(result).SaveAs("GimmyReport.pdf");
        }

        private static void DecimalParsing()
        {
            string directoryName = "D:\\SW Development\\Customers\\Gimmy\\TransactionCalculator\\Tests\\DecimalParsing";
            string[] filePaths = Directory.GetFiles(directoryName, "Salaries-1.txt");

            var bad = new List<string>();
            Configuration config = new Configuration();
            config.Delimiter = "\t";
            config.RegisterClassMap<PersonMap>();
            config.TrimOptions = TrimOptions.Trim;

            config.BadDataFound = context =>
            {
                bad.Add(context.RawRecord);
            };

            foreach (string filePath in filePaths)
            {
                Console.WriteLine($"Elaboration of { filePath}");
                List<Person> personList = null;

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, config))
                {
                    personList = csv.GetRecords<Person>().ToList();
                }

            }
        }

        private static void FormattingTests()
        {
            CultureInfo culture = new CultureInfo(CultureInfo.InvariantCulture.Name);
            //culture.NumberFormat.NumberDecimalSeparator = ".";


            decimal value = 123456;
            Console.WriteLine(value.ToString("0.00", culture));

            value = 1234.56m;
            Console.WriteLine(value.ToString("0.00", culture));

            value = 1234.5m;
            Console.WriteLine(value.ToString("0.00", culture));



            Console.WriteLine(value.ToString("0.00", culture));
        }
    }
}
