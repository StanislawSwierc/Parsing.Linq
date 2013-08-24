using System.Collections.Generic;
using System.Linq;

namespace System.Parsing.Linq
{

    public static partial class Parser
    {
        public static Parser<T2> Select<T1, T2>(
            this Parser<T1> parser,
            Func<T1, T2> selector)
        {
            return Create((text, offset) =>
                {
                    var res = parser.Parse(text, offset);
                    if (res.IsMissing) return ParserResult<T2>.Missing;
                    return new ParserResult<T2>(selector(res.Value), res.Source, res.Position, res.Length);
                });
        }

        public static Parser<TValue2> SelectMany<TValue, TIntermediate, TValue2>(
            this Parser<TValue> parser,
            Func<TValue, Parser<TIntermediate>> selector,
            Func<TValue, TIntermediate, TValue2> projector)
        {
            return Create((text, offset) =>
                {
                    var res1 = parser.Parse(text, offset);
                    if (res1.IsMissing) return ParserResult<TValue2>.Missing;
                    var val1 = res1.Value;
                    var res2 = selector(val1).Parse(text, offset + res1.Length);
                    if (res2.IsMissing) return ParserResult<TValue2>.Missing;
                    return new ParserResult<TValue2>(projector(val1, res2.Value), text, offset, res1.Length + res2.Length);
                });
        }

        public static Parser<T> Where<T>(
            this Parser<T> parser,
            Func<T, bool> predicate)
        {
            return Create((text, offset) =>
                {
                    var res = parser.Parse(text, offset);
                    if (res.IsMissing || !predicate(res.Value)) return ParserResult<T>.Missing;
                    return res;
                });
        }

        /// <summary>
        /// Join operator can easily create parsers which can parse text
        /// consisting of  elements concatenated, using the specified separator
        /// in between.
        /// </summary>
        public static Parser<T2[]> Join<T1, T2>(
            Parser<T1> separator,
            Parser<T2> parser)
        {
            var tailElement =
                from t1 in separator
                from t2 in parser
                select t2;

            return Create((text, offset) =>
                {
                    var head = parser.Parse(text, offset);
                    if (head.IsMissing) return new ParserResult<T2[]>(new T2[0], text, offset, 0);
                    var list = new List<T2>();
                    list.Add(head.Value);
                    var tailLength = XOrMoreCore(tailElement, text, offset + head.Length, list);
                    return new ParserResult<T2[]>(list.ToArray(), text, offset, head.Length + tailLength);
                });
        }

        public static Parser<T[]> ZeroOrMore<T>(
            this Parser<T> parser)
        {
            return XOrMore(parser, true);
        }

        public static Parser<T[]> OneOrMore<T>(
            this Parser<T> parser)
        {
            return XOrMore(parser, false);
        }

        private static Parser<T[]> XOrMore<T>(
            this Parser<T> parser,
            bool allowZero)
        {
            return Create((text, offset) =>
                {
                    var list = new List<T>();
                    var length = XOrMoreCore(parser, text, offset, list);

                    return list.Count > 0
                        ? new ParserResult<T[]>(list.ToArray(), text, offset, length)
                        : allowZero
                        ? new ParserResult<T[]>(new T[0], text, offset, length)
                        : ParserResult<T[]>.Missing;
                });
        }

        /// <remarks>
        /// This method has side effects! It adds elements to the list passed
        /// as a parameter. This allows to get better performance by accessing
        /// the same list in different methods. Join operator is an example of
        /// where it makes sense.
        /// </remarks>>
        private static int XOrMoreCore<T>(Parser<T> parser, string text, int offset, List<T> list)
        {
            var curr = offset;
            var length = 0;
            for (var res = parser.Parse(text, curr);
                !res.IsMissing;
                curr += res.Length,
                res = curr < text.Length
                    ? parser.Parse(text, curr)
                    : ParserResult<T>.Missing)
            {
                list.Add(res.Value);
                length += res.Length;

                // If the parse operation returns result of length zero
                // then in theory the collection returned should be
                // infinitely long. Break the loop as soon as such
                // scenario is detected.
                if (res.Length == 0) break;
            }

            return length;
        }
    }
}
