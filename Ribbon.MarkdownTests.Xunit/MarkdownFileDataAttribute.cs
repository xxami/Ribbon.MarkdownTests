using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Ribbon.MarkdownTests.Xunit.Parser;
using Xunit.Sdk;

namespace Ribbon.MarkdownTests.Xunit
{
    public class MarkdownFileDataAttribute : DataAttribute
    {
        private readonly string _filePath;
        private readonly string _sectionHeading;

        public MarkdownFileDataAttribute(string filePath, string sectionHeading = null)
        {
            _filePath = filePath;
            _sectionHeading = sectionHeading;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null)
            {
                throw new ArgumentNullException(nameof(testMethod));
            }

            var path = Path.IsPathRooted(_filePath)
                ? _filePath
                : Path.GetRelativePath(Directory.GetCurrentDirectory(), _filePath);

            if (!File.Exists(path))
            {
                throw new ArgumentException($"Could not find file: {path}");
            }

            var fileData = File.ReadAllText(_filePath);
            var parser = new MdTestCaseParser();
            var testCases = parser.Parse(fileData);

            var result = new List<object[]>();
            foreach (var kvp in testCases)
            {
                if (_sectionHeading != null && _sectionHeading != kvp.Key)
                    continue;
                
                foreach (var testCase in kvp.Value)
                {
                    result.Add(new object [] {testCase.Contents});
                }
            }
            
            return result;
        }
    }
}