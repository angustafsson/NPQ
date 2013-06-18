using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NPQ.Extensions;
using NPQ.Models;
using NUnit.Framework;
using OpenQA.Selenium.PhantomJS;

namespace NPQ.Implementation
{
    public static class TestRunner 
    {
        static readonly List<string> _validEndings = new List<string>
            {
                ".html", ".htm"
            };

        /// <summary>
        /// Path is assuming that Phantomjs executable is in project folder (root) of tests.
        /// Path is from the root to the file or folder.
        /// </summary>
        /// <param name="path">Path to file (htm, html) or folder to files (htm, html)</param>
        /// <returns></returns> 
        public static IEnumerable<TestCaseData> GetTestResults(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new NullReferenceException("parameter file cant be null or empty!");

            bool isPathDirectory;

            var combinedPath = Path.Combine(Environment.CurrentDirectory, path);

            if (!PathIsValid(combinedPath, out isPathDirectory))
            {
                if (!isPathDirectory)
                    throw new Exception("File not exepted, only, htm or html files are ok");

                throw new Exception("Directory contains no valid files");
            }

            using (var phantomJs = new PhantomJSDriver(Environment.CurrentDirectory))
                {
                    phantomJs.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 0, 10));

                    if (isPathDirectory)
                    {
                        var qUnitTests = new List<NPQTest>();

                        foreach (var file in Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, path), "*.htm").Select(file => file.Replace(Environment.CurrentDirectory + "\\", "")).ToList())
                        {
                            var tests = phantomJs.GetTests(file);
                            qUnitTests.AddRange(tests);
                        }

                        foreach (var qUnitTest in qUnitTests)
                        {
                            yield return MapTestCaseData(qUnitTest);
                        }

                    }
                    else
                    {
                        foreach (var qTest in phantomJs.GetTests(path))
                        {
                            yield return MapTestCaseData(qTest);
                        }
                    }
                }

        }

        private static void CheckPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new NullReferenceException("parameter file cant be null or empty!");
        }

        private static void CheckPantomJsExe(string phantomJsExecutable)
        {
            CheckPath(phantomJsExecutable);

            if(!phantomJsExecutable.EndsWith(".exe"))
                throw new Exception("Parameter: 'phantomJsExecutable' has to be an .exe file");
        }

        private static bool PathIsValid(string path, out bool isDirectory)
        {
            return PathIsDirectory(path, out isDirectory) ? isDirectory : FileIsValid(path);
        }

        private static bool PathIsDirectory(string path, out bool isDirectory)
        {
            var folderExists = Directory.Exists(path);
            var pathIsOk = false;

            if (folderExists)
            {
                var htmFiles = Directory.GetFiles(path, "*.htm" ).ToList();

                pathIsOk = htmFiles.Any();
            }

            isDirectory = pathIsOk;
            return isDirectory;
        }

        private static bool FileIsValid(string path)
        {
            return _validEndings.Contains(Path.GetExtension(path));
        }

        private static TestCaseData MapTestCaseData(NPQTest qUnitTest)
        {
            var testCase = new TestCaseData(qUnitTest);
            testCase.SetName(qUnitTest.Name);
            testCase.SetDescription(qUnitTest.Description);
            return testCase;
        }
    }
}
