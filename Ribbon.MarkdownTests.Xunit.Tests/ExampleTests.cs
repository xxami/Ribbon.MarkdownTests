using Xunit;

namespace Ribbon.MarkdownTests.Xunit.Tests
{
    public class ExampleTests
    {
        [Theory]
        [MarkdownFileData("Examples/example_1.md")]
        public void Example1_IsValid(string fileData)
        {
            Assert.True(fileData == "example 1");
        }

        [Theory]
        [MarkdownFileData("Examples/example_2.md")]
        public void Example2_IsValid(string fileData)
        {
            var number = int.Parse(fileData);
            Assert.True(number > 0 && number < 10);
        }

        [Theory]
        [MarkdownFileData("Examples/example_3.md", "Test case 2")]
        public void Example3_IsValid(string fileData)
        {
            var number = int.Parse(fileData);
            Assert.True(number > 0 && number < 10);
        }
    }
}