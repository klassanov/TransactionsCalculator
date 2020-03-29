using IronXL;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Presenters;
using TransactionsCalculator.Presenters.ViewModelConverters;
using TransactionsCalculator.Presenters.ViewModels;

namespace TransactionsCalculator.Presenters.Presenters
{
    public class ExcelPresenter : IPresenter
    {
        private WorkBook xlsWorkbook;
        private WorkSheet xlsSheet;
        private int tableHeaderRowIndex = 8;
        private char currentChar;

        public ExcelPresenter()
        {
            ResetCurrentChar();
        }

        public void PresentInfo(IDirectoryProcessingResult directoryProcessingResult)
        {
            ExcelReportViewModelConverter converter = new ExcelReportViewModelConverter();
            ExcelReportViewModel reportViewModel = converter.Convert(directoryProcessingResult);

            this.CreateWorkbookAndSheet();
            this.WriteTitle(reportViewModel);
            this.WriteTimestamp(reportViewModel);
            this.WriteWorkingDirectory(reportViewModel);
            this.WriteTableHeaders(reportViewModel);
            this.WriteTableDataRows(reportViewModel);

            xlsWorkbook.SaveAs("GimmyReport.xlsx");
        }

        private void WriteTableDataRows(ExcelReportViewModel reportViewModel)
        {
            int dataRowIndex = this.tableHeaderRowIndex + 1;
            foreach (var dataRow in reportViewModel.TableDataRows)
            {
                this.ResetCurrentChar();
                this.xlsSheet[$"{this.currentChar}{dataRowIndex}"].StringValue = dataRow.Filename;
                this.currentChar = GetNextAlphabetChar(this.currentChar);

                foreach (var cellValueModel in dataRow.CellValueModels)
                {
                    AssignCellValue(this.xlsSheet[$"{this.currentChar}{dataRowIndex}"], cellValueModel);
                    this.currentChar = GetNextAlphabetChar(this.currentChar);
                }

                this.xlsSheet[$"{this.currentChar}{dataRowIndex}"].StringValue = dataRow.OperationExitCode;
                this.xlsSheet[$"{this.currentChar}{dataRowIndex}"].Style.HorizontalAlignment = IronXL.Styles.HorizontalAlignment.Center;

                dataRowIndex++;
            }
        }

        private void AssignCellValue(Range cellRange, CellValueModel excelCellValueModel)
        {
            if (excelCellValueModel.IsError)
            {
                cellRange.StringValue = PresentationConstants.ResultKO;
            }
            else
            {
                cellRange.DecimalValue = excelCellValueModel.DecimalValue;
            }
        }

        private void WriteTableHeaders(ExcelReportViewModel reportViewModel)
        {
            foreach (var header in reportViewModel.TableHeaders)
            {
                this.xlsSheet[$"{this.currentChar}{tableHeaderRowIndex}"].StringValue = header;
                this.xlsSheet[$"{this.currentChar}{tableHeaderRowIndex}"].Style.Font.Bold = true;
                this.xlsSheet[$"{this.currentChar}{tableHeaderRowIndex}"].Style.HorizontalAlignment = IronXL.Styles.HorizontalAlignment.Center;
                this.currentChar = GetNextAlphabetChar(this.currentChar);
            }
        }

        private void WriteWorkingDirectory(ExcelReportViewModel reportViewModel)
        {
            this.xlsSheet["B6"].StringValue = "Working Directory";
            this.xlsSheet["B6"].Style.Font.Bold = true;
            this.xlsSheet["C6"].StringValue = reportViewModel.WorkingDirectory;
        }

        private void WriteTimestamp(ExcelReportViewModel reportViewModel)
        {
            this.xlsSheet["B5"].StringValue = "Timestamp";
            this.xlsSheet["B5"].Style.Font.Bold = true;
            this.xlsSheet["C5"].DateTimeValue = reportViewModel.Timestamp;
        }

        private void WriteTitle(ExcelReportViewModel reportViewModel)
        {
            this.xlsSheet["C2"].StringValue = reportViewModel.Title;
            this.xlsSheet["C2"].Style.Font.Bold = true;
        }

        private void CreateWorkbookAndSheet()
        {
            this.xlsWorkbook = WorkBook.Create(ExcelFileFormat.XLS);
            this.xlsWorkbook.Metadata.Author = "TransactionsCalculator";
            this.xlsSheet = this.xlsWorkbook.CreateWorkSheet("elaboration_report");
        }

        private void ResetCurrentChar()
        {
            this.currentChar = 'B';
        }

        private char GetNextAlphabetChar(char c)
        {
            char nextChar;

            if (c == 'z')
            {
                nextChar = 'a';
            }
            else if (c == 'Z')
            {
                nextChar = 'A';
            }
            else
            {
                nextChar = (char)(((int)c) + 1);
            }

            return nextChar;
        }
    }
}


