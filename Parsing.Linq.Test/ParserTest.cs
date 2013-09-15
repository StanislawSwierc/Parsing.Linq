using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable InconsistentNaming
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
        public void SelectManyTest()
        {
            var p =
                from t1 in Parser.FromChar('a')
                from t2 in Parser.FromChar('b')
                from t3 in Parser.FromChar('c')
                select Tuple.Create(t1, t2, t3);

            var r = p.ParseAll("abc");

            Assert.AreEqual('a', r.Item1);
            Assert.AreEqual('b', r.Item2);
            Assert.AreEqual('c', r.Item3);
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
        public void AndTest()
        {
            var p1 = Parser.FromRegex(@"\d");
            var p2 = Parser.FromRegex(@"\w");
            var p = p1 & p2;

            var result = p.ParseAll("1a");

            Assert.AreEqual(result.Item1, "1");
            Assert.AreEqual(result.Item2, "a");
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
        public void FromTextTest()
        {
            var p = Parser.FromText("Text");

            Assert.IsTrue(CanParse(p, "Text"));
            Assert.IsFalse(CanParse(p, "NotText"));
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

            var result = parser.ParseAll("There are two characters: 'a' and 'b'");

            Assert.AreEqual('a', result.Item1);
            Assert.AreEqual('b', result.Item2);
        }

        [TestMethod]
        public void ZeroOrMore_EmptyParser_Terminates()
        {
            var parser = Parser.Empty<char>().ZeroOrMore();

            var result = parser.Parse("test");

            Assert.AreEqual(1, result.Value.Length);
        }

        [TestMethod]
        public void ZeroOrMore_ParseAll_EmptyString()
        {
            var parser = Parser.FromChar('c').ZeroOrMore();

            var result = parser.ParseAll(string.Empty);

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void ZeroOrMore_ParseAll_One()
        {
            var parser = Parser.FromChar('a').ZeroOrMore();

            var result = parser.ParseAll("a");

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual('a', result[0]);
        }

        [TestMethod]
        public void ZeroOrMore_ParseAll_Many()
        {
            var parser = Parser.FromChar('a').ZeroOrMore();

            var result = parser.ParseAll("aaaaa");

            Assert.AreEqual(5, result.Length);
        }

        [TestMethod]
        public void ZeroOrMore_Parse_Zero()
        {
            var parser = Parser.FromChar('a').ZeroOrMore();

            var result = parser.Parse("b");

            Assert.IsTrue(!result.IsMissing);
            Assert.AreEqual(0, result.Value.Length);
        }

        [TestMethod]
        public void ZeroOrMore_Parse_One()
        {
            var parser = Parser.FromChar('a').ZeroOrMore();

            var result = parser.Parse("ab");

            Assert.IsTrue(!result.IsMissing);
            Assert.AreEqual(1, result.Value.Length);
        }

        [TestMethod]
        public void ZeroOrMore_Parse_Many()
        {
            var parser = Parser.FromChar('a').ZeroOrMore();

            var result = parser.Parse("aaaaab");

            Assert.IsTrue(!result.IsMissing);
            Assert.AreEqual(5, result.Value.Length);
        }

        [TestMethod]
        public void OneOrMore_ParseAll_One()
        {
            var parser = Parser.FromChar('a').OneOrMore();

            var result = parser.ParseAll("a");

            Assert.AreEqual(1, result.Length);
        }

        [TestMethod]
        public void OneOrMore_ParseAll_Many()
        {
            var parser = Parser.FromChar('a').OneOrMore();

            var result = parser.ParseAll("aaaaa");

            Assert.AreEqual(5, result.Length);
        }

        [TestMethod]
        public void OneOrMore_Parse_EmptyString()
        {
            var parser = Parser.FromChar('a').OneOrMore();

            var result = parser.Parse(string.Empty);

            Assert.IsTrue(result.IsMissing);
        }

        [TestMethod]
        public void OneOrMore_Parse_Zero()
        {
            var parser = Parser.FromChar('a').OneOrMore();

            var result = parser.Parse("b");

            Assert.IsTrue(result.IsMissing);
        }

        [TestMethod]
        public void OneOrMore_Parse_One()
        {
            var parser = Parser.FromChar('a').OneOrMore();

            var result = parser.Parse("ab");

            Assert.IsTrue(!result.IsMissing);
            Assert.AreEqual(1, result.Value.Length);
        }

        [TestMethod]
        public void OneOrMore_Parse_Many()
        {
            var parser = Parser.FromChar('a').OneOrMore();

            var result = parser.Parse("aaaaab");

            Assert.IsTrue(!result.IsMissing);
            Assert.AreEqual(5, result.Value.Length);
        }

        [TestMethod]
        public void JoinTest()
        {
            var elements = Enumerable.Range(0, 10).Select(i => i.ToString());
            var separator = "\t";
            var parser = Parser.Join(Parser.FromText(separator), Parser.FromRegex("\\d+"));
            var text = string.Join(separator, elements);

            var result = parser.ParseAll(text);

            Assert.IsTrue(Enumerable.SequenceEqual(elements, result));
        }

        [TestMethod]
        public void FromRegexTest()
        {
            var parser = Parser.FromRegex("[aoeiu]");
            var result = parser.Parse("cat");

            Assert.IsFalse(result.IsMissing);
            Assert.AreEqual("a", result.Value);
            Assert.AreEqual(1, result.Position);
            Assert.AreEqual(1, result.Length);
        }

        [TestMethod]
        public void OffsetTest()
        {
            var parser = Parser.FromRegex("word");
            var line = @"word abcdefgh";

            var result = parser.Parse(line, 5);
            Assert.IsTrue(result.IsMissing);
        }

        [TestMethod]
        public void SequencingTest()
        {
            var parser =
                from hello in Parser.FromText("Hello")
                from space in CharParsers.WhiteSpace
                from world in Parser.FromText("world")
                from period in Parser.FromChar('.')
                select hello + world;

            Assert.IsTrue(CanParse(parser, "Hello world."));
        }

        [TestMethod]
        public void SequencingRegexTest()
        {
            var parser =
                from hello in Parser.FromRegex("Hello")
                from space in CharParsers.WhiteSpace
                from world in Parser.FromText("world")
                from period in Parser.FromChar('.')
                select hello + world;

            Assert.IsTrue(CanParse(parser, "preffix Hello world."));
        }

        [TestMethod]
        public void SequencingCorrectOffsetIsSetTest()
        {
            var parser =
                from a in Parser.FromRegex("aaa")
                from b in Parser.FromText("bbb")
                select a + b;

            Assert.IsTrue(CanParse(parser, "preffix...aaabbb...suffix"));
            Assert.IsFalse(CanParse(parser, "123bbbaaa"));
        }
    }
}
