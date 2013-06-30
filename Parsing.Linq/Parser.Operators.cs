using System.Collections.Generic;

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

        public static Parser<T[]> ZeroOrMore<T>(
            this Parser<T> parser)
        {
            return Create((text, offset) =>
                {
                    var list = new List<T>();
                    var curr = offset;
                    var length = 0;
                    for (var res = parser.Parse(text, curr);
                        !res.IsMissing;
                        curr += res.Length,
                        res = curr < text.Length ? parser.Parse(text, curr) : ParserResult<T>.Missing)
                    {
                        list.Add(res.Value);
                        length += res.Length;
                    }

                    return list.Count > 0
                        ? new ParserResult<T[]>(list.ToArray(), text, offset, length)
                        : new ParserResult<T[]>(new T[0], text, offset, length);
                });
        }
    }
}
