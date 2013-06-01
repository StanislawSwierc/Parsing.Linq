using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Parsing.Linq.Test
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void Parse_Digit_Correct()
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

        [TestMethod]
        public void CastTest()
        {
            var p = ParserExtensions.FromRegex(@"\d", int.Parse)
                .Cast<object>()
                .Cast<int>();
            Assert.IsNotNull(p.Parse("1"));
        }

        [TestMethod]
        public void FromChar_Char_Test()
        {
            var p = ParserExtensions.FromChar('c');
            Assert.IsNotNull(p.Parse("c"));
            Assert.IsNull(p.Parse("a"));
        }

        [TestMethod]
        public void FromChar_Predicate_Test()
        {
            var p = ParserExtensions.FromChar(c => c == 'c');
            Assert.IsNotNull(p.Parse("c"));
            Assert.IsNull(p.Parse("a"));

        }
        [TestMethod]
        public void FromChar_Enumerable_Test()
        {
            var p = ParserExtensions.FromChar("cs");
            Assert.IsNotNull(p.Parse("c"));
            Assert.IsNull(p.Parse("a"));
        }

        [TestMethod]
        public void FromFormatTest()
        {
            var parser = Parser.FromFormat(
                "There are two characters: '{0}' and '{1}'",
                ParserExtensions.FromChar('a'),
                ParserExtensions.FromChar('b'));

            var result = parser.ParseComplete("There are two characters: 'a' and 'b'");

            Assert.IsNotNull(result);
            Assert.AreEqual('a', result.Item1);
            Assert.AreEqual('b', result.Item2);
        }
    }
}
