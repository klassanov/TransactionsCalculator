using IronPdf;
using RazorEngine;
using RazorEngine.Templating;
using System.IO;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Presenters;

namespace TransactionsCalculator.Presenters
{
    public class PDFPresenter : IPresenter
    {
        public void PresentInfo(IDirectoryProcessingResult directoryProcessingResult)
        {
            HtmlToPdf pdfRenderer = new HtmlToPdf();
            string razoPdfTemplate = File.ReadAllText(@"PDFResources/PDFReportTemplate.cshtml");

            var pdfHtmlResult = Engine.Razor.RunCompile(
                templateSource: razoPdfTemplate,
                name: "pdfreport",
                modelType: typeof(IDirectoryProcessingResult),
                model: directoryProcessingResult,
                viewBag: null);

            pdfRenderer.PrintOptions.CustomCssUrl = @"PDFResources/bootstrap.min.css";
            pdfRenderer.PrintOptions.PaperOrientation = PdfPrintOptions.PdfPaperOrientation.Portrait;

            pdfRenderer.RenderHtmlAsPdf(pdfHtmlResult).SaveAs("GimmyReport.pdf");
        }
    }
}
