using log4net;
using System;
using System.Linq;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Services
{
    public class DirectoryProcessingResultConsolePrinterService : IDirectoryProcessingResultPrinterService
    {
        private static ILog logger = LogManager.GetLogger(typeof(DirectoryProcessingResultConsolePrinterService));
        private readonly IAppConfigurationService appConfigurationService;
        private readonly string dateFormat = "dd-MMM-yyyy";
        private readonly char spaceChar = ' ';
        private readonly string rateFormat = "n4";

        public DirectoryProcessingResultConsolePrinterService(IAppConfigurationService appConfigurationService)
        {
            this.appConfigurationService = appConfigurationService;
        }

        public void Print(IDirectoryProcessingResult directoryProcessingResult)
        {
            throw new System.NotImplementedException();
        }

        public void PrintExchangeRatesToConsole(IDirectoryProcessingResult directoryProcessingResult)
        {

            logger.Info($"Reference currency code: {appConfigurationService.ReferenceCurrencyCode}");

            string printingInfo = "Used exhcange rates printed as (Currency, Exchange Rate, Transaction Date, Effective Exchange Date, Source)";
            Console.WriteLine(printingInfo);
            Console.WriteLine();

            logger.Debug(printingInfo);
            logger.Debug(string.Empty);

            if (directoryProcessingResult.ExchangeRateInfoList != null)
            {
                foreach (IExchangeRateInfo item in directoryProcessingResult.ExchangeRateInfoList.OrderBy(x => x.TransactionDate))
                {
                    string exchangeRateInfoString = $"{item.GetFromCurrency()}  {item.GetExchangeRate().ToString(rateFormat)}  {this.FormatNullableDate(item.TransactionDate)}  {item.GetEffectiveExchangeDate().ToString(dateFormat)}  {item.Source}";
                    logger.Debug(exchangeRateInfoString);
                    Console.WriteLine(exchangeRateInfoString);
                }
            }
        }

        private string FormatNullableDate(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString(this.dateFormat) : string.Empty.PadLeft(this.dateFormat.Length, this.spaceChar);
        }
    }
}
