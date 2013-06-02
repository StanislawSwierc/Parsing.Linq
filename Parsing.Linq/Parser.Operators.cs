namespace System.Parsing.Linq
{
    public static partial class Parser
    {
        public static Parser<T2> Select<T1, T2>(
            this Parser<T1> parser,
            Func<T1, T2> selector)
        {
            return Create(input =>
                {
                    var res = parser.Parse(input);
                    if (res == null) return null;
                    return new ParserResult<T2>(selector(res.Value), res.Rest);
                });
        }

        public static Parser<TValue2> SelectMany<TValue, TIntermediate, TValue2>(
            this Parser<TValue> parser,
            Func<TValue, Parser<TIntermediate>> selector,
            Func<TValue, TIntermediate, TValue2> projector)
        {
            return Create(input =>
                {
                    var res = parser.Parse(input);
                    if (res == null) return null;
                    var val = res.Value;
                    var res2 = selector(val).Parse(res.Rest);
                    if (res2 == null) return null;
                    return new ParserResult<TValue2>(projector(val, res2.Value), res2.Rest);
                });
        }

        public static Parser<T> Where<T>(
            this Parser<T> parser,
            Func<T, bool> predicate)
        {
            return Create(input =>
                {
                    var res = parser.Parse(input);
                    if (res == null || !predicate(res.Value)) return null;
                    return res;
                });
        }
    }
}
