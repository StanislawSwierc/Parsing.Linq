using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Parsing.Linq.Test
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void Parse_Digit_ReturnsInt()
        {
            var p = ParserExtensions.FromRegex(@"\d", int.Parse);
            var result = p.Parse("1a");
            Assert.AreEqual(1, result.Value);
            Assert.AreEqual("a", result.Rest);
        }

        [TestMethod]
        public void OrTest()
        {
            var p1 = ParserExtensions.FromRegex(@"\d");
            var p2 = ParserExtensions.FromRegex(@"\w");
            var p = p1 | p2;
            Assert.IsNotNull(p.Parse("1"));
            Assert.IsNotNull(p.Parse("a"));
        }
    }
}
