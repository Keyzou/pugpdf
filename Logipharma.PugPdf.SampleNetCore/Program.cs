using System;
using System.IO;
using System.Threading.Tasks;
using Logipharma.PugPdf.Core;

namespace Logipharma.PugPdf.SampleNetCore
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            await PrintWithHeaderAndFooter();
        }

        static async Task PrintWithImageFromUrl()
        {
            var renderer = new HtmlToPdf();

            renderer.PrintOptions.Title = "My title";

            var pdf = await renderer.RenderHtmlAsPdfAsync("<img src=\"https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_272x92dp.png\"><h1>Hello world</h1>");

            await pdf.SaveAsAsync("c:\\my.pdf");

            var filePath = await pdf.SaveInTempFolderAsync();

            Console.WriteLine(filePath);

            Console.ReadLine();
        }

        static async Task PrintWithImageFromFile()
        {
            var renderer = new HtmlToPdf();

            renderer.PrintOptions.Title = "My title";

            var path = Path.Combine(Directory.GetCurrentDirectory(), "files", "GitHub_Logo.png");

            var pdf = await renderer.RenderHtmlAsPdfAsync($"<img src=\"{path}\"><h1>Hello world</h1>");

            await pdf.SaveAsAsync("c:\\my.pdf");

            var filePath = await pdf.SaveInTempFolderAsync();

            Console.WriteLine(filePath);

            Console.ReadLine();
        }

        private static async Task PrintWithHeaderAndFooter()
        {
            var renderer = new HtmlToPdf
            {

                PrintOptions = new PdfPrintOptions
                {
                    PageSize = PdfPageSize.A4,
                    Title = "My title",
                    Footer = new PdfFooter
                    {
                        CenterText = "[page]/[topage]",
                        DisplayLine = true,
                    },
                }
            };


            var pdf = await renderer.RenderHtmlAsPdfAsync("<h1>Hello world</h1>");

            await pdf.SaveAsAsync("my.pdf");

        }
    }
}