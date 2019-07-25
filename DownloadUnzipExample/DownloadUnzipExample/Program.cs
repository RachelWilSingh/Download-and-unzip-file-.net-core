using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

/*
 * REFERENCES:
 * https://stackoverflow.com/questions/43281554/create-zip-in-net-core-from-urls-without-downloading-on-server
 * https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpcontent?view=netframework-4.8
 * https://docs.microsoft.com/en-us/dotnet/api/system.io.compression.ziparchive?view=netframework-4.8
 * https://stackoverflow.com/questions/20520238/how-to-read-file-from-zip-archive-to-memory-without-extracting-it-to-file-first
 * https://stackoverflow.com/questions/5282999/reading-csv-file-and-storing-values-into-an-array
 * */

namespace DownloadUnzipExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DownloadAndOpenFile().Wait();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception " + ex.Message);
            }
        }

        static async Task DownloadAndOpenFile()
        {
            Console.WriteLine("Download file...");
            Console.WriteLine("----------------");
            string url = "https://github.com/RachelWilSingh/Download-and-unzip-file-.net-core/blob/master/example-text-file.zip?raw=true";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);

            Console.WriteLine("Result: " + response.StatusCode);
            Console.WriteLine("Content: " + response.Content.Headers);

            Console.WriteLine("");
            Console.WriteLine("Convert stream to zip file...");
            Console.WriteLine("-----------------------------");

            Stream stream = (response.Content.ReadAsStreamAsync()).Result;
            ZipArchive zip = new ZipArchive(stream);
            Console.WriteLine(zip.ToString());

            Console.WriteLine("");
            Console.WriteLine("Read files in zip file...");
            Console.WriteLine("-------------------------");
            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                Console.WriteLine("");
                Console.WriteLine(entry.FullName);

                Stream fileStream = entry.Open();
                StreamReader fileReader = new StreamReader(fileStream);

                while (!fileReader.EndOfStream)
                {
                    Console.WriteLine(fileReader.ReadLine());
                }
            }
        }
    }
}
