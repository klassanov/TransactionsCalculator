using IronPdf;
using RazorEngine;
using RazorEngine.Templating;
using System.IO;
using TransactionsCalculator.Interfaces.Models;
using TransactionsCalculator.Interfaces.Presenters;
using TransactionsCalculator.Interfaces.Services;
using TransactionsCalculator.Presenters.ViewModelConverters;
using TransactionsCalculator.Presenters.ViewModels;

namespace TransactionsCalculator.Presenters.Presenters
{
    public class PDFPresenter : IPresenter
    {
        private IAppConfigurationService appConfigurationService;

        public PDFPresenter(IAppConfigurationService appConfigurationService)
        {
            this.appConfigurationService = appConfigurationService;
        }

        public void PresentInfo(IDirectoryProcessingResult directoryProcessingResult)
        {
            PDFReportViewModelConverter converter = new PDFReportViewModelConverter();
            PDFReportViewModel reportViewModel = converter.Convert(directoryProcessingResult);

            HtmlToPdf pdfRenderer = new HtmlToPdf();
            string razoPdfTemplate = File.ReadAllText(@"Resources/PDFReportTemplate.cshtml");

            var pdfHtmlResult = Engine.Razor.RunCompile(
                templateSource: razoPdfTemplate,
                name: "pdfreport",
                modelType: typeof(PDFReportViewModel),
                model: reportViewModel,
                viewBag: null);

            //pdfRenderer.PrintOptions.CustomCssUrl = @"PDFResources/bootstrap.min.css";
            pdfRenderer.PrintOptions.PaperOrientation = PdfPrintOptions.PdfPaperOrientation.Landscape;
            pdfRenderer.RenderHtmlAsPdf(pdfHtmlResult).SaveAs("GimmyReport.pdf");
        }
    }
}
