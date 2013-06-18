using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace NPQ.Tests
{
    [TestFixture]
    public class FileTests
    {
        List<string> validEndings = new List<string>()
            {
                ".html", ".htm"
            };

        [TestCase("file.html", true)]
        [TestCase("file.htm", true)]
        [TestCase("file.hmt", false)]
        public void CheckIfFileIsValidHtmlFile(string file, bool expected)
        {
            var extension = Path.GetExtension(file);
            Assert.That(validEndings.Contains(extension) == expected);
        }

        [TestCase(@"C:\\Users\\Andreas.Person", false)]
        public void CheckIfPathIsValidDirectoryWithHtmlFIlesIn(string path, bool expected)
        {
            Assert.That(Directory.Exists(path) == expected);
        }
    }
}
