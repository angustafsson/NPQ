using System.Collections.Generic;
using System.Linq;

using NPQ.Models;

using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;

namespace NPQ.Extensions
{
    public static class PhantomJSDriverExtensions
    {
        public static List<NPQTest> GetTests(this PhantomJSDriver phantomJs, string filePath)
        {
            var _qunitTests = new List<NPQTest>();
            phantomJs.Navigate().GoToUrl(filePath);
            var qunitTestWrapper = phantomJs.FindElement(By.Id(QUnitTestConstants.QunitTests));
            var qunitTests = qunitTestWrapper.FindElements(By.TagName(QUnitTestConstants.QunitLiTag))
                .Where(w => !string.IsNullOrEmpty(w.GetAttribute(QUnitTestConstants.QunitIdAttribute))).ToList();

            _qunitTests.AddRange(qunitTests.Select(test => new NPQTest(test, filePath)));

            return _qunitTests;
        }
    }
}
