using System.IO;
using OpenQA.Selenium;

namespace NPQ.Models
{
    public class NPQTest
    {
        private string errorMessage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webElement">Qunit-test web element</param>
        /// <param name="htmlFile">HtmlFile that was runned (path)</param>
        public NPQTest(IWebElement webElement, string htmlFile)
        {
            var className = webElement.GetAttribute(QUnitTestConstants.QunitClassAttribute);

            Result = className;
            Name = webElement.FindElement(By.ClassName(QUnitTestConstants.QunitTestName)).Text;
            HtmlFileName = htmlFile;
            Description = ""; // webElement.FindElement(By.ClassName("test-message")).Text;

            //Message fail
            if (className.Equals(QUnitTestConstants.QunitFail))
            {
                //Check for errors
                if (webElement.FindElements(By.ClassName(QUnitTestConstants.QunitErrorTestMessage)).Count > 0)
                {
                    var testErrorMessage = webElement.FindElement(By.ClassName(QUnitTestConstants.QunitErrorTestMessage));
                    var table = webElement.FindElement(By.TagName(QUnitTestConstants.QunitTableTag));

                    var trExpected = table.FindElement(By.ClassName(QUnitTestConstants.QunitErrorExpected));
                    var trActual = table.FindElement(By.ClassName(QUnitTestConstants.QunitErrorActual));
                    var trDiff = table.FindElement(By.ClassName(QUnitTestConstants.QunitErrorDiff));
                    var trSource = table.FindElement(By.ClassName(QUnitTestConstants.QunitErrorTestSource));

                    var jsFileName = trSource.Text.Substring(trSource.Text.LastIndexOf('/') + 1);
                    jsFileName = jsFileName.Substring(0, jsFileName.LastIndexOf(':'));
                    var rowNumber = trSource.Text.Substring(trSource.Text.LastIndexOf(':') + 1);

                    var htmlFileName = Path.GetFileName(htmlFile);
                    var fileNames = string.Format("Error in: {0}, loaded in: {1}, at row number: {2}", jsFileName, htmlFileName, rowNumber);

                    errorMessage = string.Format(
                        "Errormessage: {0}, '{1}', '{2}', '{3}', '{4}",
                        testErrorMessage.Text, trExpected.Text, trActual.Text, trDiff.Text, fileNames);
                }
                else
                {
                    errorMessage = webElement.Text;
                }
            }

            ErrorMessage = errorMessage;

        }

        /// <summary>
        /// Name from the runned Test function
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description from the runned Test function
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Result from the runned Test
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// HtmlFileName, file name from loaded HtmlFile
        /// </summary>
        public string HtmlFileName { get; set; }
        /// <summary>
        /// ErrorMessage from runned Test, empty if the Test passed
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
