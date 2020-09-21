using System.Collections.Generic;

namespace Logipharma.PugPdf.Core
{
    public class PdfFooter
    {
        public string LeftText { get; set; } = string.Empty;
        public string CenterText { get; set; } = string.Empty;
        public string RightText { get; set; } = string.Empty;
        public string HTMLUrl { get; set; } = string.Empty;
        public bool DisplayLine { get; set; } = false;
        public string FontName { get; set; } = "Arial";
        public double FontSize { get; set; } = 12d;
        public int Spacing { get; set; } = 0;
        public string Replace { get; set; } = string.Empty;

        public IEnumerable<string> GetSwitches()
        {
            var switches = new List<string>();

            if (!string.IsNullOrEmpty(LeftText))
                switches.Add($"--footer-left \"{LeftText}\"");

            if (!string.IsNullOrEmpty(CenterText))
                switches.Add($"--footer-center \"{CenterText}\"");

            if (!string.IsNullOrEmpty(RightText))
                switches.Add($"--footer-right \"{RightText}\"");

            if (!string.IsNullOrEmpty(HTMLUrl))
                switches.Add($"--footer-html \"{HTMLUrl}\"");

            if (DisplayLine)
                switches.Add("--footer-line");

            switches.Add($"--footer-font-size {FontSize}");
            switches.Add($"--footer-font-name {FontName}");
            switches.Add($"--footer-spacing {Spacing}");

            if (!string.IsNullOrEmpty(Replace))
                switches.Add($"--replace {Replace}");

            return switches;
        }
    }
}
