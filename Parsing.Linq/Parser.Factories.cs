using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace System.Parsing.Linq
{
    public static partial class Parser
    {
        public static Parser<T> Create<T>(Func<string, int, ParserResult<T>> func)
        {
            return new AnonymousParser<T>(func);
        }

        public static Parser<T> Empty<T>()
        {
            return new EmptyParser<T>();
        }

        public static Parser<char> FromChar(char c)
        {
            return Create((text, offset) =>
                {
                    return offset < text.Length && text[offset] == c
                        ? new ParserResult<char>(c, text, offset, 1)
                        : ParserResult<char>.Missing;
                });
        }

        public static Parser<char> FromChar(Func<char, bool> predicate)
        {
            return Create((text, offset) =>
                {
                    var result = default(ParserResult<char>);
                    if(offset < text.Length)
                    {
                        var c = text[offset];
                        if(predicate(c))
                        {
                            result = new ParserResult<char>(c, text, offset, 1);
                        }
                    }
                    return result ?? ParserResult<char>.Missing;
                });
        }

        public static Parser<char> FromChar(IEnumerable<char> source)
        {
            var set = new HashSet<char>(source);
            return FromChar(set.Contains);
        }

        public static Parser<string> FromText(string value)
        {
            return Create((text, offset) =>
                {
                    return (offset + value.Length <= text.Length) && (text.Substring(offset, value.Length) == value)
                        ? new ParserResult<string>(value, text, offset, value.Length)
                        : ParserResult<string>.Missing;
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
            if (pattern == null) throw new ArgumentNullException("pattern");
            if (select == null) throw new ArgumentNullException("select");

            // Add an anchor to make sure tha the match starts precisely at offset
            pattern = @"\G" + pattern;

            // Create the regex object to validate the pattern
            var regex = new Regex(pattern);

            return Create((text, offset) =>
                {
                    var match = regex.Match(text, offset);
                    return match.Success
                        ? new ParserResult<T>(select(match), text, offset, match.Length)
                        : ParserResult<T>.Missing;
                });
        }


    }
}