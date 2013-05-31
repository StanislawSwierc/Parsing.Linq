using System.Linq;
using System.Text.RegularExpressions;

namespace System.Parsing.Linq
{
    public static class Parser
    {
        public static Parser<Tuple<T1>> FromFormat<T1>(string format, Parser<T1> p1)
        {
            var parts = SplitFormat(format, 1);

            return
                from t1 in parts[0]
                from t2 in p1
                from t3 in parts[1]
                select Tuple.Create(t2);
        }

        public static Parser<Tuple<T1, T2>> FromFormat<T1, T2>(string format, Parser<T1> p1, Parser<T2> p2)
        {
            var parts = SplitFormat(format, 2);

            return
                from t1 in parts[0]
                from t2 in p1
                from t3 in parts[1]
                from t4 in p2
                from t5 in parts[2]
                select Tuple.Create(t2, t4);
        }

        /// <summary>
        /// Returns an array of string parsers which can match substrings of the
        /// format string that are delimited by markers.
        /// </summary>
        /// <param name="format">Format string.</param>
        /// <param name="count">Number of markers.</param>
        /// <returns>Array of string parsers of size count + 1.</returns>
        private static Parser<string>[] SplitFormat(string format, int count)
        {
            if (format == null) throw new ArgumentNullException("format");

            var matches = Regex.Matches(format, @"{\d}");

            if (matches.Count != 2) throw new FormatException("Format should have exactly 2 {number} tokens");

            // Save positions
            var positions = matches.Cast<Match>()
                .Select(c => c.Index)
                // Add guard at the end.
                .Concat(new[] { format.Length })
                .ToArray();

            var parts = new Parser<string>[3];
            var curr = 0;
            for (var i = 0; i <= 2; i++)
            {
                var next = positions[i];
                var text = format.Substring(curr, next - curr);
                parts[i] = text != string.Empty
                    ? ParserExtensions.FromText(text)
                    : ParserExtensions.Empty<string>();
                curr = next + 3;
            }

            return parts;
        }
    }
}
