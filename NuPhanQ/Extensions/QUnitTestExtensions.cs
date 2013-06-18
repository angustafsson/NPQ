using NPQ.Models;

using NUnit.Framework;

namespace NPQ.Extensions
{
    public static class QUnitTestExtensions
    {
        private const string QUNIT_PASS = "pass";

        public static void ShouldPass(this NPQTest t)
        {
            Assert.AreEqual(QUNIT_PASS, t.Result, string.Format("'{0}'", t.ErrorMessage));
        }
    }
}
