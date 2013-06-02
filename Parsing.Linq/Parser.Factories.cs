using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace System.Parsing.Linq
{
    public static partial class Parser
    {
        public static Parser<T> Create<T>(Func<string, ParserResult<T>> func)
        {
            return new AnonymousParser<T>(func);
        }

        public static Parser<T> Empty<T>()
        {
            return new EmptyParser<T>();
        }

        public static Parser<char> FromChar(char c)
        {
            return Create(input =>
                {
                    return input.Length > 0 && input[0] == c
                        ? new ParserResult<char>(c, input.Substring(1))
                        : null;
                });
        }

        public static Parser<char> FromChar(Func<char, bool> predicate)
        {
            return Create(input =>
                {
                    var result = default(ParserResult<char>);
                    if(input.Length > 0)
                    {
                        var c = input[0];
                        if(predicate(c))
                        {
                            result = new ParserResult<char>(c, input.Substring(1));
                        }
                    }
                    return result;
                });
        }

        public static Parser<char> FromChar(IEnumerable<char> source)
        {
            var set = new HashSet<char>(source);
            return FromChar(set.Contains);
        }

        public static Parser<string> FromText(string value)
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


    }
}