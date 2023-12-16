using System.Runtime.Serialization;

namespace DocumentAnalyzer.Models
{
    public class WordCountUploadResponse : List<WordCountUploadResponseItem>
    {
        public WordCountUploadResponse() { }

        public WordCountUploadResponse(List<WordCountItem> result)
        {
            foreach(var item in result) {
                Add(new WordCountUploadResponseItem(item.Word, item.Count));
            }
        }
    }

    public class WordCountUploadResponseItem
    {
        public string Word { get; set; }
        public int Count { get; set; }

        public WordCountUploadResponseItem() { }

        public WordCountUploadResponseItem(string word, int count)
        {
            Word = word;
            Count = count;
        }
    }

    public class WordCountUploadRequest
    {
        [DataMember(IsRequired = true)]
        public required IFormFile UploadFile { get; set; }
    }

    public class WordCountItem
    {
        public required string Word { get; set; }
        public int Count { get; set; }
    }

}
