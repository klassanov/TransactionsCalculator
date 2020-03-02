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

        public void PresentInfo(IDirectoryProcessingResult directoryProcessingResult)
        {
            ExcelReportViewModelConverter converter = new ExcelReportViewModelConverter();
            ExcelReportViewModel reportViewModel = converter.Convert(directoryProcessingResult);

            this.CreateWorkbookAndSheet();

            //Title
            this.xlsSheet["C2"].StringValue = reportViewModel.Title;
            this.xlsSheet["C2"].Style.Font.Bold = true;

            //Timestamp
            this.xlsSheet["B5"].Value = reportViewModel.Timestamp;

            //Working directory
            this.xlsSheet["B6"].StringValue = reportViewModel.WorkingDirectory;

            //Headers
            char currentChar = 'B';
            int headerRowIndex = 8;
            foreach (var header in reportViewModel.TableHeaders)
            {
                this.xlsSheet[$"{currentChar}{headerRowIndex}"].StringValue = header;
                this.xlsSheet[$"{currentChar}{headerRowIndex}"].Style.Font.Bold = true;
                currentChar = GetNextAlphabetChar(currentChar);
            }

            //Data rows
            int dataRowIndex = 9;
            foreach (var dataRow in reportViewModel.TableDataRows)
            {
                currentChar = 'B';
                this.xlsSheet[$"{currentChar}{dataRowIndex}"].Value = dataRow.Filename;
                currentChar = GetNextAlphabetChar(currentChar);

                foreach (var cellValue in dataRow.CellValues)
                {
                    this.xlsSheet[$"{currentChar}{dataRowIndex}"].Value = cellValue;
                    currentChar = GetNextAlphabetChar(currentChar);
                }

                this.xlsSheet[$"{currentChar}{dataRowIndex}"].Value = dataRow.OperationExitCode;

                dataRowIndex++;
            }

            //Save the excel file
            xlsWorkbook.SaveAs("GimmyReport.xlsx");
        }
        private void CreateWorkbookAndSheet()
        {
            this.xlsWorkbook = WorkBook.Create(ExcelFileFormat.XLS);
            this.xlsWorkbook.Metadata.Author = "Gimmy Schettini";
            this.xlsSheet = this.xlsWorkbook.CreateWorkSheet("report");
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


