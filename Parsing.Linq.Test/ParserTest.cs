using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Parsing.Linq.Test
{
    [TestClass]
    public class ParserTest
    {
        public bool CanParse<T>(Parser<T> parser, string text)
        {
            var result = parser.Parse(text);
            return !result.IsMissing;
        }


        [TestMethod]
        public void Parse_Digit_Correct()
        {
            var p = Parser.FromRegex(@"\d", int.Parse);
            var result = p.Parse("1a");
            Assert.AreEqual(1, result.Value);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(0, result.Position);
        }

        [TestMethod]
        public void OrTest()
        {
            var p1 = Parser.FromRegex(@"\d");
            var p2 = Parser.FromRegex(@"\w");
            var p = p1 | p2;
            Assert.IsTrue(CanParse(p, "1"));
            Assert.IsTrue(CanParse(p, "a"));
        }

        [TestMethod]
        public void CastTest()
        {
            var p = Parser.FromRegex(@"\d", int.Parse)
                .Cast<object>()
                .Cast<int>();
            Assert.IsTrue(CanParse(p, "1"));
        }

        [TestMethod]
        public void FromChar_Char_Test()
        {
            var p = Parser.FromChar('c');
            Assert.IsTrue(CanParse(p, "c"));
            Assert.IsFalse(CanParse(p, "a"));
        }

        [TestMethod]
        public void FromChar_Predicate_Test()
        {
            var p = Parser.FromChar(c => c == 'a');

            Assert.IsTrue(CanParse(p, "a"));
            Assert.IsFalse(CanParse(p, "b"));

        }
        [TestMethod]
        public void FromChar_Enumerable_Test()
        {
            var p = Parser.FromChar("ab");

            Assert.IsTrue(CanParse(p, "a"));
            Assert.IsTrue(CanParse(p, "b"));
            Assert.IsFalse(CanParse(p, "c"));
        }

        [TestMethod]
        public void FromFormatTest()
        {
            var parser = Parser.FromFormat(
                "There are two characters: '{0}' and '{1}'",
                Parser.FromChar('a'),
                Parser.FromChar('b'));

            var result = parser.ParseComplete("There are two characters: 'a' and 'b'");

            Assert.IsNotNull(result);
            Assert.AreEqual('a', result.Item1);
            Assert.AreEqual('b', result.Item2);
        }
    }
}
