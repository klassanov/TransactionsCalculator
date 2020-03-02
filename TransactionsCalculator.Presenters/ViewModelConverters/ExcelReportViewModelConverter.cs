using System;
using System.IO;
using System.Linq;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Presenters.ViewModels;

namespace TransactionsCalculator.Presenters.ViewModelConverters
{
    public class ExcelReportViewModelConverter : BaseViewModelConverter<IDirectoryProcessingResult, ExcelReportViewModel>
    {
        public override ExcelReportViewModel Convert(IDirectoryProcessingResult source)
        {
            ExcelReportViewModel viewModel = new ExcelReportViewModel();
            if (source != null)
            {
                viewModel.Title = "Elaboration Report";
                viewModel.Timestamp = DateTime.Now;
                viewModel.WorkingDirectory = source.WorkingDirectory;
                this.AssignTableHeaderValues(source, viewModel);
                this.AssignTableDataRows(source, viewModel);
            }
            return viewModel;
        }

        private void AssignTableHeaderValues(IDirectoryProcessingResult source, ExcelReportViewModel viewModel)
        {
            viewModel.TableHeaders.Add("File");
            if (source.FileOperationResultList != null && source.FileOperationResultList.Count > 0)
            {
                viewModel.TableHeaders.AddRange(source.FileOperationResultList[0].OperationsResultList.Select(x => x.OperationDescription));
            }
            viewModel.TableHeaders.Add("Outcome");
        }

        private void AssignTableDataRows(IDirectoryProcessingResult source, ExcelReportViewModel viewModel)
        {
            foreach (var fileOperationResult in source.FileOperationResultList)
            {
                ExcelTableDataRow row = new ExcelTableDataRow();
                row.Filename = Path.GetFileName(fileOperationResult.FilePath);
                row.OperationExitCode = fileOperationResult.Exception is null ? "OK" : "ERROR";

                if (fileOperationResult.OperationsResultList != null && fileOperationResult.OperationsResultList.Count > 0)
                {
                    row.CellValues.AddRange(fileOperationResult.OperationsResultList.Select(x => x.CalulatedAmount));
                }

                viewModel.TableDataRows.Add(row);
            }
        }
    }
}
