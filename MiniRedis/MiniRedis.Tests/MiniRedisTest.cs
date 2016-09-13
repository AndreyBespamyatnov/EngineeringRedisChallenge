using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniRedis.Tests
{
    [TestClass]
    public class MiniRedisTest
    {
        [TestMethod]
        public void MatchRegEx()
        {
            var r = new Regex("^[a-zA-Z0-9-_ ]*$");
            bool isMatch = r.IsMatch("asdasdASDASD00012 30012378237645287634_-");
            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void NotMatchRegEx()
        {
            var r = new Regex("^[a-zA-Z0-9-_ ]*$");
            bool isMatch = r.IsMatch("asdasdASDA+ + SD00012 30012378237645287634_-");
            Assert.IsFalse(isMatch);
        }
    }
}
