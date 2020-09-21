using System.Collections.Generic;

namespace Logipharma.PugPdf.Core
{
    public class PdfPrintOptions
    {
        public double MarginLeft { get; set; } = 10d;
        public double MarginRight { get; set; } = 10d;
        public double MarginTop { get; set; } = 10d;
        public double MarginBottom { get; set; } = 10d;
        public PdfPageSize PageSize { get; set; } = PdfPageSize.A4;
        public PdfOrientation Orientation { get; set; } = PdfOrientation.Portrait;
        public string Title { get; set; }
        public bool LowQuality { get; set; } = false;
        public bool UsePrintMediaType { get; set; } = false;
        public bool Grayscale { get; set; } = false;
        public int ImageDPI { get; set; } = 600;
        public int ImageQuality { get; set; } = 94;

        public PdfHeader Header { get; set; } = new PdfHeader();
        public PdfFooter Footer { get; set; } = new PdfFooter();

        public IEnumerable<string> GetSwitches()
        {
            var switches = new List<string>
            {
                $"--margin-bottom {MarginBottom} ",
                $"--margin-top {MarginTop}",
                $"--margin-left {MarginLeft}",
                $"--margin-right {MarginRight}",

                $"--page-size {PageSize}",

                $"--orientation {Orientation}"
            };

            if (!string.IsNullOrEmpty(Title))
                switches.Add($"--title \"{Title}\"");

            if (LowQuality)
                switches.Add("--lowquality");

            if (UsePrintMediaType)
                switches.Add("--print-media-type");

            if (Grayscale)
                switches.Add("--grayscale");

            switches.Add($"--image-dpi {ImageDPI}");
            switches.Add($"--image-quality {ImageQuality}");
            switches.Add("--disable-smart-shrinking");
            switches.Add("--enable-local-file-access");

            if (Header != null)
                switches.AddRange(Header.GetSwitches());
            if (Footer != null)
                switches.AddRange(Footer.GetSwitches());

            return switches;
        }
    }
}