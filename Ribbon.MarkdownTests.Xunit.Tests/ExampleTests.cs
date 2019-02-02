using Xunit;

namespace Ribbon.MarkdownTests.Xunit.Tests
{
    public class MdParsingTest
    {
        [Theory]
        [MarkdownFileData("Examples/example_1.md")]
        public void Example1_IsValid(string fileData)
        {
            Assert.True(fileData == "example 1");
        }
    }
}