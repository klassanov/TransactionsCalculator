using System;
using System.Globalization;
using System.IO;
using System.Linq;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Presenters.ViewModels;

namespace TransactionsCalculator.Presenters.ViewModelConverters
{
    public class PDFReportViewModelConverter : BaseViewModelConverter<IDirectoryProcessingResult, PDFReportViewModel>
    {
        private IFormatProvider decimalFormatProvider;
        private string timestampFormat;
        private string decimalFormat;

        public PDFReportViewModelConverter()
        {
            this.decimalFormatProvider = CreateDecimalFormatProvider();
            this.timestampFormat = "dd/MM/yyyy  HH:mm:ss";
            this.decimalFormat = "0,0.00";
        }

        public override PDFReportViewModel Convert(IDirectoryProcessingResult source)
        {
            PDFReportViewModel viewModel = new PDFReportViewModel();
            if (source != null)
            {
                viewModel.Title = "Elaboration Report";
                viewModel.Timestamp = DateTime.Now.ToString(timestampFormat, CultureInfo.InvariantCulture);
                viewModel.WorkingDirectory = source.WorkingDirectory;
                this.AssignTableHeaderValues(source, viewModel);
                this.AssignTableDataRows(source, viewModel);
            }
            return viewModel;
        }

        private void AssignTableHeaderValues(IDirectoryProcessingResult source, PDFReportViewModel viewModel)
        {
            viewModel.TableHeaders.Add("File");
            if (source.FileOperationResultList != null && source.FileOperationResultList.Count > 0)
            {
                viewModel.TableHeaders.AddRange(source.FileOperationResultList[0].OperationsResultList.Select(x => x.OperationDescription));
            }
            viewModel.TableHeaders.Add("Outcome");
        }

        private void AssignTableDataRows(IDirectoryProcessingResult source, PDFReportViewModel viewModel)
        {
            foreach (var fileOperationResult in source.FileOperationResultList)
            {
                TableDataRow row = new TableDataRow();
                row.Filename = Path.GetFileName(fileOperationResult.FilePath);
                row.OperationExitCode = fileOperationResult.Exception is null ? "OK" : "ERROR";

                if (fileOperationResult.OperationsResultList != null && fileOperationResult.OperationsResultList.Count > 0)
                {
                    row.CellValues.AddRange(fileOperationResult.OperationsResultList.Select(x => x.CalulatedAmount.ToString(this.decimalFormat, this.decimalFormatProvider)));
                }

                viewModel.TableDataRows.Add(row);
            }
        }

        private IFormatProvider CreateDecimalFormatProvider()
        {
            //return CultureInfo.CurrentCulture;
            return new NumberFormatInfo()
            {
                NumberDecimalSeparator = ",",
                NumberGroupSeparator = "."
            };
        }
    }
}


