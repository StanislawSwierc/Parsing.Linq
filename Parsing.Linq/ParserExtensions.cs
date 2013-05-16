using System.Text.RegularExpressions;

namespace System.Parsing.Linq
{
    public static class ParserExtensions
    {
        public static T ParseComplete<T>(this Parser<T> parser, string input)
        {
            var result = parser.Parse(input);
            return result != null && string.IsNullOrEmpty(result.Rest)
                ? result.Value
                : default(T);
        }

        public static Parser<T> Create<T>(Func<string, ParserResult<T>> func)
        {
            return new AnonymousParser<T>(func);
        }

        public static Parser<string> FromString(string value)
        {
            return Create(input =>
                {
                    return input.StartsWith(value)
                        ? new ParserResult<string>(value, input.Substring(value.Length))
                        : null;
                });
        }

        public static Parser<string> FromRegex(string pattern)
        {
            return FromRegex(pattern, match => match.Value);
        }

        public static Parser<T> FromRegex<T>(string pattern, Func<string, T> select)
        {
            return FromRegex(pattern, match => select(match.Value));
        }

        public static Parser<T> FromRegex<T>(string pattern, Func<Match, T> select)
        {
            // Make sure the regex starts matching from the beginning.
            if (!pattern.StartsWith("^"))
            {
                pattern = string.Concat("^", pattern);
            }

            var regex = new Regex(pattern);

            return Create(input =>
                {
                    var match = regex.Match(input);
                    return match.Success
                        ? new ParserResult<T>(select(match), input.Substring(match.Length))
                        : null;
                });
        }


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