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
                    return new ParserResult<T2>(selector(res.Value), res.FullText, res.Position, res.Length);
                });
        }

        public static Parser<TValue2> SelectMany<TValue, TIntermediate, TValue2>(
            this Parser<TValue> parser,
            Func<TValue, Parser<TIntermediate>> selector,
            Func<TValue, TIntermediate, TValue2> projector)
        {
            return Create((text, offset) =>
                {
                    var res = parser.Parse(text, offset);
                    if (res.IsMissing) return ParserResult<TValue2>.Missing;
                    var val = res.Value;
                    var res2 = selector(val).Parse(text, offset + res.Length);
                    if (res2.IsMissing) return ParserResult<TValue2>.Missing;
                    return new ParserResult<TValue2>(projector(val, res2.Value), text, offset, res.Length + res.Length);
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
    }
}
