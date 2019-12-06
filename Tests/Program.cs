using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TransactionCalculator.Models.Transaction;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
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
                List<TransactionRow> result = new List<TransactionRow>();
                IEnumerable<TransactionRow> records = csv.GetRecords<TransactionRow>().ToList();
                foreach (var r in records)
                {
                    if (r.SaleArrivalCountry == "IT")
                    {
                        result.Add(r);
                    }
                    Console.WriteLine($"Sale arrival country: {r.SaleArrivalCountry }");
                }

                var x = records.Where(x => x.SaleArrivalCountry.Trim() == "IT");
            }

        }
    }
}
