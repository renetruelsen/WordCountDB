using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System.Text;

namespace DocumentAnalyzer.Test.Classes
{
    public class StreamFormFile : IFormFile
    {
        private readonly byte[] _fileBytes;

        public StreamFormFile(Stream stream, long length, string fileName, string contentType)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            _fileBytes = new byte[length];
            stream.Read(_fileBytes, 0, (int)length);

            Length = length;
            Name = fileName;
            FileName = fileName;
            ContentType = contentType;
        }

        public Stream OpenReadStream() => new MemoryStream(_fileBytes);

        public void CopyTo(Stream target)
        {
            throw new NotImplementedException();
        }

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public long Length { get; }

        public string ContentDisposition => $"form-data; name={Name}; filename={FileName}";

        public string ContentType { get; }

        public string FileName { get; }

        public string Name { get; }

        public IHeaderDictionary Headers => throw new NotImplementedException();

        public static StreamFormFile GenerateFile(string content, string fileName = "testfile.txt")
        {
            var contentType = new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var _contentType)
                ? _contentType : "application/octet-stream";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            return new StreamFormFile(stream, stream.Length, fileName, contentType);

        }
    }
}
