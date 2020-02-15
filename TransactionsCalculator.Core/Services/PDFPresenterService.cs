using IronPdf;
using RazorEngine;
using RazorEngine.Templating;
using System.IO;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Services;

namespace TransactionsCalculator.Core.Services
{
    public class PDFPresenterService : IPresenterService
    {
        public void PresentInfo(IDirectoryProcessingResult directoryProcessingResult)
        {
            HtmlToPdf pdfRenderer = new HtmlToPdf();
            string razoPdfTemplate = File.ReadAllText(@"ReportTemplates/PDFReport.cshtml");

            var pdfHtmlResult = Engine.Razor.RunCompile(
                templateSource: razoPdfTemplate,
                name: "pdfreport",
                modelType: typeof(IDirectoryProcessingResult),
                model: directoryProcessingResult,
                viewBag: null);

            pdfRenderer.PrintOptions.CustomCssUrl = @"ReportTemplates/bootstrap.min.css";
            pdfRenderer.PrintOptions.PaperOrientation = PdfPrintOptions.PdfPaperOrientation.Landscape;

            pdfRenderer.RenderHtmlAsPdf(pdfHtmlResult).SaveAs("GimmyReport.pdf");
        }
    }
}
