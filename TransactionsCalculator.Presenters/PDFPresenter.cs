using IronPdf;
using RazorEngine;
using RazorEngine.Templating;
using System.IO;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Presenters;
using TransactionsCalculator.Presenters.ViewModelConverters;
using TransactionsCalculator.Presenters.ViewModels;

namespace TransactionsCalculator.Presenters
{
    public class PDFPresenter : IPresenter
    {
        public void PresentInfo(IDirectoryProcessingResult directoryProcessingResult)
        {
            ReportViewModelConverter converter = new ReportViewModelConverter();
            ReportViewModel reportViewModel = converter.Convert(directoryProcessingResult);

            HtmlToPdf pdfRenderer = new HtmlToPdf();
            string razoPdfTemplate = File.ReadAllText(@"PDFResources/PDFReportTemplate2.cshtml");

            var pdfHtmlResult = Engine.Razor.RunCompile(
                templateSource: razoPdfTemplate,
                name: "pdfreport",
                modelType: typeof(ReportViewModel),
                model: reportViewModel,
                viewBag: null);

            //pdfRenderer.PrintOptions.CustomCssUrl = @"PDFResources/bootstrap.min.css";
            pdfRenderer.PrintOptions.PaperOrientation = PdfPrintOptions.PdfPaperOrientation.Portrait;
            pdfRenderer.RenderHtmlAsPdf(pdfHtmlResult).SaveAs("GimmyReport.pdf");
        }
    }
}
