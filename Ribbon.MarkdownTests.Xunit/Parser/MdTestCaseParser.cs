using System.Collections.Generic;
using System.Linq;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Ribbon.MarkdownTests.Xunit.Parser
{
  public class MdTestCaseParser
  {
    public Dictionary<string, List<MdTestCase>> Parse(string contents)
    {
      var testCases = new Dictionary<string, List<MdTestCase>>();
      var cachedTestName = string.Empty;
      var md = Markdown.Parse(contents);
      foreach (var node in md)
      {
        if (node is HeadingBlock header)
        {
          if (header.HeaderChar != '#' && cachedTestName == string.Empty)
            continue;
          
          var inline = (LiteralInline) header.Inline.FirstOrDefault();
          var inlineText = inline?.Content.ToString();
          cachedTestName = inlineText;
          
          continue;
        }

        if (cachedTestName == string.Empty)
          continue;

        if (node is FencedCodeBlock fencedCode)
        {
          if (fencedCode.FencedChar != '`' || fencedCode.FencedCharCount != 3)
            continue;

          var tests = testCases.ContainsKey(cachedTestName)
            ? testCases[cachedTestName]
            : null;
          var testIndex = tests != null ? $"#{tests.Count}" : string.Empty;
          var testName = $"{cachedTestName}{testIndex}";
          var code = fencedCode.Lines.ToString();
          var testCase = new MdTestCase {Description = testName, Contents = code};

          if (tests == null)
            testCases.Add(cachedTestName, new List<MdTestCase>());
          testCases[cachedTestName].Add(testCase);
        }
      }

      return testCases;
    }
  }
}