using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TransactionCalculator.Models.Transaction;
using TransactionsCalculator.Core.Helpers;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Services
{
    public class FileReaderService : IFileReaderService
    {
        private readonly IAppConfigurationService appConfigurationService;
        private readonly CsvConfiguration config;

        public FileReaderService(IAppConfigurationService appConfigurationService)
        {
            this.appConfigurationService = appConfigurationService;
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

        private CsvConfiguration CreateFileReadingConfiguration()
        {
            var badData = new List<string>();
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture);
            config.Delimiter = this.appConfigurationService.FileDelimiter;
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
