using System.Collections.Generic;

namespace TransactionsCalculator.Presenters.ViewModels
{
    public class ExcelTableDataRow
    {
        public ExcelTableDataRow()
        {
            this.CellValueModels = new List<CellValueModel>();
        }

        public string Filename { get; set; }

        public string OperationExitCode { get; set; }

        public List<CellValueModel> CellValueModels { get; set; }
    }
}
