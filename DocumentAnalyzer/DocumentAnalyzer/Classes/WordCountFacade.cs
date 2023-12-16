using DocumentAnalyzer.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DocumentAnalyzer.Classes
{
    public class WordCountFacade
    {
        /// <summary>
        /// Takes a file and counts the number of words.
        /// The method ignores non-alphanumerical characters
        /// </summary>
        /// <param name="file"></param>
        /// <returns>
        /// A list of {Word,Count}, sorted by Count DESC, Word ASC
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="FileLoadException"></exception>
        public static async Task<List<WordCountItem>?> AnalyzeFileAsync(IFormFile file)
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

        /// <summary>
        /// Reads a txt File. This can be extended to support other file formats
        /// </summary>
        /// <param name="file"></param>
        /// <returns>
        /// The content of the file as a string
        /// </returns>
        private async static Task<string?> ReadFileAsync(IFormFile file)
        {
            if (file == null) return null;

            var reader = new StreamReader(file.OpenReadStream(), Encoding.Default);

            var content = await reader.ReadToEndAsync();

            return content;

        }

        /// <summary>
        /// Performs a word count on a text string, ignoring anything except alphanumerical characters
        /// </summary>
        /// <param name="data"></param>
        /// <returns>
        /// A list of {Word,Count}, sorted by Count DESC, Word ASC
        /// </returns>
        private static List<WordCountItem>? WordCountString(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;

            /* Reduce to alphanumerical text (remove commas, etc...) */
            data = Regex.Replace(data, "[^a-zA-Z0-9 -]", "");

            /* Split into tokens */
            var tokens = Regex.Split(data, $"\\s+");

            var tokenCount = tokens.GroupBy(x => x.ToLower()).Select(x => new WordCountItem() { Word = x.Key, Count = x.Count() }).ToList();

            return tokenCount.OrderByDescending(x => x.Count).ThenBy(x => x.Word).ToList();
        }
    }
}
