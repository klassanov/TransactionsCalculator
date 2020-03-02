using System.Collections.Generic;

namespace TransactionsCalculator.Presenters.ViewModels
{
    public class ExcelTableDataRow
    {
        public ExcelTableDataRow()
        {
            this.CellValues = new List<decimal>();
        }

        public string Filename { get; set; }

        public string OperationExitCode { get; set; }

        public List<decimal> CellValues { get; set; }
    }
}
