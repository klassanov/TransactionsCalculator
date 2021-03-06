﻿using System.Collections.Generic;

namespace TransactionsCalculator.Presenters.ViewModels
{
    public class PDFReportViewModel
    {
        public PDFReportViewModel()
        {
            this.Title = string.Empty;
            this.Timestamp = string.Empty;
            this.WorkingDirectory = string.Empty;
            this.TableHeaders = new List<string>();
            this.TableDataRows = new List<PDFTableDataRow>();
        }

        public string Title { get; set; }

        public string Timestamp { get; set; }

        public string WorkingDirectory { get; set; }

        public List<string> TableHeaders { get; set; }

        public List<PDFTableDataRow> TableDataRows { get; set; }
    }
}
