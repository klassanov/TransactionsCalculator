using System.Collections.Generic;

namespace TransactionsCalculator.Presenters.ViewModels
{
    public class ReportViewModel
    {
        public ReportViewModel()
        {
            this.Title = string.Empty;
            this.Timestamp = string.Empty;
            this.WorkingDirectory = string.Empty;
            this.TableHeaders = new List<string>();
            this.TableDataRows = new List<TableDataRow>();
        }

        public string Title { get; set; }

        public string Timestamp { get; set; }

        public string WorkingDirectory { get; set; }

        public List<string> TableHeaders { get; set; }

        public List<TableDataRow> TableDataRows { get; set; }
    }
}
