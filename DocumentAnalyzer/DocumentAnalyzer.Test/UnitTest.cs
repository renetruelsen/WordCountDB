using DocumentAnalyzer.Test.Classes;

namespace DocumentAnalyzer.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestWordCount()
        {
            var testString = "A horse, a horse, my kingdom for a horse!";
            var correctResult = new[] {
                new { Text = "a", Count = 3 },
                new { Text = "horse", Count = 3 },
                new { Text = "for", Count = 1 },
                new { Text = "kingdom", Count = 1 },
                new { Text = "my", Count = 1 },
            };

            var formFile = StreamFormFile.GenerateFile(testString);

            Assert.IsNotNull(formFile);

            var result = await DocumentAnalyzer.Classes.WordCountFacade.AnalyzeFileAsync(formFile);

            Assert.IsNotNull(result);

            for (var i = 0; i < result.Count(); i++)
            {
                Assert.That(correctResult[i].Text, Is.EqualTo(result[i].Word));
                Assert.That(correctResult[i].Count, Is.EqualTo(result[i].Count));
            }

        }
    }
}