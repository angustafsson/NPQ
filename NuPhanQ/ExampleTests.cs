﻿using System.Collections.Generic;
using NUnit.Framework;
using NPQ.Extensions;
using NPQ.Implementation;
using NPQ.Models;

namespace NPQ
{
    [TestFixture]
    public class ExampleTests
    {
        /// <summary>
        /// Run alla QunitTest From Directory
        /// </summary>
        /// <param name="test"></param>
        [Test, TestCaseSource("GetTestResultsFromDirectory")]
        public void RunTestFromFolder(NPQTest test)
        {
            test.ShouldPass();
        }

        /// <summary>
        /// Run all QunitTest from specific .html file
        /// </summary>
        /// <param name="test"></param>
        [Test, TestCaseSource("GetTestResultsFromFile")]
        public void RunTestsFromSpecificFile(NPQTest test)
        { 
            test.ShouldPass();
        }

        /// <summary>
        /// Get Test From Directory
        /// </summary>
        public IEnumerable<TestCaseData> GetTestResultsFromDirectory
        {
            get { return TestRunner.GetTestResults("JavaScriptTests"); }
        }

        /// <summary>
        /// Get Test From File
        /// </summary>
        public IEnumerable<TestCaseData> GetTestResultsFromFile
        {
            get { return TestRunner.GetTestResults("JavaScriptTests/bar.html"); }
        }
    }
}
