using System;
using System.Collections.Generic;

namespace TransactionsCalculator.Presenters.ViewModels
{
    public class ExcelReportViewModel
    {
        public ExcelReportViewModel()
        {
            this.Title = string.Empty;
            this.Timestamp = DateTime.Now;
            this.WorkingDirectory = string.Empty;
            this.TableHeaders = new List<string>();
            this.TableDataRows = new List<ExcelTableDataRow>();
        }

        public string Title { get; set; }

        public DateTime Timestamp { get; set; }

        public string WorkingDirectory { get; set; }

        public List<string> TableHeaders { get; set; }

        public List<ExcelTableDataRow> TableDataRows { get; set; }
    }
}
