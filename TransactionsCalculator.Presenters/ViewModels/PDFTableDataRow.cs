﻿using System.Collections.Generic;

namespace TransactionsCalculator.Presenters.ViewModels
{
    public class PDFTableDataRow
    {
        public PDFTableDataRow()
        {
            this.CellValues = new List<string>();
        }

        public string Filename { get; set; }

        public string OperationExitCode { get; set; }

        public List<string> CellValues { get; set; }
    }
}
