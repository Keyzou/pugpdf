using System.Threading.Tasks;

namespace Logipharma.PugPdf.Core
{
    public class HtmlToPdf
    {
        public PdfPrintOptions PrintOptions { get; set; } = new PdfPrintOptions();

        public async Task<PdfDocument> RenderHtmlAsPdfAsync(string html)
        {
            var data = await WkHtmlToPdfDriver.ConvertAsync(html, PrintOptions?.GetSwitches());

            return new PdfDocument(data);
        }
    }
}