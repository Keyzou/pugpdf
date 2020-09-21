using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Logipharma.PugPdf.Core
{
    public static class WkHtmlToPdfDriver
    {
        private static string _executablePath;

        private static string GetPath()
        {
            if (!string.IsNullOrEmpty(_executablePath))
                return _executablePath;

            var path = GetExecutablePath();

            if (!File.Exists(path))
                throw new FileNotFoundException("Executable not found.", path);

            _executablePath = path;

            return path;
        }

        private static string GetExecutablePath()
        {
            var assembly = Assembly.GetAssembly(typeof(WkHtmlToPdfDriver));

            var assemblyFolderPath = Path.GetDirectoryName(assembly.Location);

            var path = Path.Combine(assemblyFolderPath, "wkhtmltopdf");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                switch (RuntimeInformation.ProcessArchitecture)
                {
                    case Architecture.X86:
                        return Path.Combine(path, "windows", "x86", "wkhtmltopdf.exe");
                    case Architecture.X64:
                        return Path.Combine(path, "windows", "x64", "wkhtmltopdf.exe");
                    default:
                        throw new NotSupportedException("Process architecture not supported.");
                }
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                switch (RuntimeInformation.ProcessArchitecture)
                {
                    case Architecture.X64:
                        return Path.Combine(path, "linux", "x64", "wkhtmltopdf");
                    default:
                        throw new NotSupportedException("Process architecture not supported.");
                }
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                switch (RuntimeInformation.ProcessArchitecture)
                {
                    case Architecture.X64:
                        return Path.Combine(path, "mac", "x64", "wkhtmltopdf");
                    default:
                        throw new NotSupportedException("Process architecture not supported.");
                }
            }

            throw new NotSupportedException("OS not supported.");
        }

        private static string SpecialCharsEncode(string text)
        {
            var charArray = text.ToCharArray();
            var stringBuilder = new StringBuilder();

            foreach (var ch in charArray)
            {
                var charInt = Convert.ToInt32(ch);

                if (charInt > sbyte.MaxValue)
                    stringBuilder.AppendFormat("&#{0};", charInt);
                else
                    stringBuilder.Append(ch);
            }

            return stringBuilder.ToString();
        }

        public static async Task<byte[]> ConvertAsync(string html, IEnumerable<string> switches)
        {
            var args = string.Join(" ", switches) + " -";
            if (!string.IsNullOrEmpty(html))
            {
                args += " -";
                html = SpecialCharsEncode(html);
            }


            using var process = new Process();
            try
            {

                process.StartInfo = new ProcessStartInfo
                {
                    FileName = GetPath(),
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true
                };

                process.Start();
            }
            catch (Exception)
            {
                throw;
            }

            if (!string.IsNullOrEmpty(html))
            {
                using var stdin = process.StandardInput;
                stdin.WriteLine(html);
            }

            using var memoryStream = new MemoryStream();
            using var baseStream = process.StandardOutput.BaseStream;
            var buffer = new byte[4096];
            int read;

            while ((read = baseStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                memoryStream.Write(buffer, 0, read);
            }

            string error = process.StandardError.ReadToEnd();
            if (memoryStream.Length == 0)
            {
                throw new Exception(error);
            }

            process.WaitForExit();
            return memoryStream.ToArray();
        }
    }
}