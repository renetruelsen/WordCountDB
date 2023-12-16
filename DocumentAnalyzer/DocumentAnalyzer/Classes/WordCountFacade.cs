using DocumentAnalyzer.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DocumentAnalyzer.Classes
{
    public class WordCountFacade
    {
        public static async Task<List<WordCountModel>?> AnalyzeFileAsync(IFormFile file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (file.ContentType != "text/plain")
            {
                throw new FormatException("Wrong format. We only support txt files.");
            }

            var fileContent = await ReadFileAsync(file);

            if (fileContent is null)
            {
                throw new FileLoadException("Error reading file"); ;
            }

            var wordCounts = WordCountString(fileContent);

            return wordCounts;

        }

        private async static Task<string?> ReadFileAsync(IFormFile file)
        {
            if (file == null) return null;

            var reader = new StreamReader(file.OpenReadStream(), Encoding.Default);

            var content = await reader.ReadToEndAsync();

            return content;

        }

        private static List<WordCountModel>? WordCountString(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;

            /* Reduce to alphanumerical text (remove commas, etc...) */
            data = Regex.Replace(data, "[^a-zA-Z0-9 -]", "");

            /* Split into tokens */
            var tokens = Regex.Split(data, $"\\s+");

            var tokenCount = tokens.GroupBy(x => x.ToLower()).Select(x => new WordCountModel() { Word = x.Key, Count = x.Count() }).ToList();

            return tokenCount.OrderByDescending(x => x.Count).ThenBy(x => x.Word).ToList();
        }
    }
}
