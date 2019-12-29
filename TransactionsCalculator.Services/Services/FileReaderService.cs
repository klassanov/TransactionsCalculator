using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TransactionCalculator.Models.Transaction;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Services
{
    public class FileReaderService : IFileReaderService
    {
        private Configuration config;

        public FileReaderService()
        {
            this.config = this.CreateFileReadingConfiguration();
        }

        public IEnumerable<ITransaction> ReadFile(string filePath)
        {
            List<Transaction> transactionsList = null;
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, this.config))
            {
                transactionsList = csv.GetRecords<Transaction>().ToList();
            }

            return transactionsList;
        }

        private Configuration CreateFileReadingConfiguration()
        {
            var badData = new List<string>();
            Configuration config = new Configuration();
            config.Delimiter = "\t";
            config.RegisterClassMap<TransactionMap>();
            config.TrimOptions = TrimOptions.Trim;
            config.BadDataFound = context =>
            {
                badData.Add(context.RawRecord);
            };

            return config;
        }
    }
}
