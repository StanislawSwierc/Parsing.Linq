using System.Linq;
using System.Text.RegularExpressions;

namespace System.Parsing.Linq
{
    public static partial class Parser
    {
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

            if (matches.Count != count) throw new FormatException(string.Format(
                "Format should have exactly {0} {{number}} tokens", count));

            // Save positions
            var positions = new int[count + 1];
            for (var i = 0; i < count; i++)
            {
                positions[i] = matches[i].Index;
            }

            // Add guard at the end.
            positions[count] = format.Length;

            var parts = new Parser<string>[count + 1];
            var curr = 0;
            for (var i = 0; i <= count; i++)
            {
                var next = positions[i];
                var text = format.Substring(curr, next - curr);
                parts[i] = text != string.Empty
                    ? Parser.FromText(text)
                    : Parser.Empty<string>();
                curr = next + 3;
            }

            return parts;
        }

        public static Parser<Tuple<T1>> FromFormat<T1>(string format, Parser<T1> p1)
        {
            var parts = SplitFormat(format, 1);

            return
                from t1 in parts[0]
                from t2 in p1
                from t3 in parts[1]
                select new Tuple<T1>(t2);
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
                select new Tuple<T1, T2>(t2, t4);
        }

        public static Parser<Tuple<T1, T2, T3>> FromFormat<T1, T2, T3>(string format, Parser<T1> p1, Parser<T2> p2, Parser<T3> p3)
        {
            var parts = SplitFormat(format, 3);

            return
                from t1 in parts[0]
                from t2 in p1
                from t3 in parts[1]
                from t4 in p2
                from t5 in parts[2]
                from t6 in p3
                from t7 in parts[3]
                select new Tuple<T1, T2, T3>(t2, t4, t6);
        }

        public static Parser<Tuple<T1, T2, T3, T4>> FromFormat<T1, T2, T3, T4>(string format, Parser<T1> p1, Parser<T2> p2, Parser<T3> p3, Parser<T4> p4)
        {
            var parts = SplitFormat(format, 4);

            return
                from t1 in parts[0]
                from t2 in p1
                from t3 in parts[1]
                from t4 in p2
                from t5 in parts[2]
                from t6 in p3
                from t7 in parts[3]
                from t8 in p4
                from t9 in parts[4]
                select new Tuple<T1, T2, T3, T4>(t2, t4, t6, t8);
        }

        public static Parser<Tuple<T1, T2, T3, T4, T5>> FromFormat<T1, T2, T3, T4, T5>(string format, Parser<T1> p1, Parser<T2> p2, Parser<T3> p3, Parser<T4> p4, Parser<T5> p5)
        {
            var parts = SplitFormat(format, 5);

            return
                from t1 in parts[0]
                from t2 in p1
                from t3 in parts[1]
                from t4 in p2
                from t5 in parts[2]
                from t6 in p3
                from t7 in parts[3]
                from t8 in p4
                from t9 in parts[4]
                from t10 in p5
                from t11 in parts[5]
                select new Tuple<T1, T2, T3, T4, T5>(t2, t4, t6, t8, t10);
        }

        public static Parser<Tuple<T1, T2, T3, T4, T5, T6>> FromFormat<T1, T2, T3, T4, T5, T6>(string format, Parser<T1> p1, Parser<T2> p2, Parser<T3> p3, Parser<T4> p4, Parser<T5> p5, Parser<T6> p6)
        {
            var parts = SplitFormat(format, 6);

            return
                from t1 in parts[0]
                from t2 in p1
                from t3 in parts[1]
                from t4 in p2
                from t5 in parts[2]
                from t6 in p3
                from t7 in parts[3]
                from t8 in p4
                from t9 in parts[4]
                from t10 in p5
                from t11 in parts[5]
                from t12 in p6
                from t13 in parts[6]
                select new Tuple<T1, T2, T3, T4, T5, T6>(t2, t4, t6, t8, t10, t12);
        }

        public static Parser<Tuple<T1, T2, T3, T4, T5, T6, T7>> FromFormat<T1, T2, T3, T4, T5, T6, T7>(string format, Parser<T1> p1, Parser<T2> p2, Parser<T3> p3, Parser<T4> p4, Parser<T5> p5, Parser<T6> p6, Parser<T7> p7)
        {
            var parts = SplitFormat(format, 7);

            return
                from t1 in parts[0]
                from t2 in p1
                from t3 in parts[1]
                from t4 in p2
                from t5 in parts[2]
                from t6 in p3
                from t7 in parts[3]
                from t8 in p4
                from t9 in parts[4]
                from t10 in p5
                from t11 in parts[5]
                from t12 in p6
                from t13 in parts[6]
                from t14 in p7
                from t15 in parts[7]
                select new Tuple<T1, T2, T3, T4, T5, T6, T7>(t2, t4, t6, t8, t10, t12, t14);
        }

        public static Parser<Tuple<T1, T2, T3, T4, T5, T6, T7, T8>> FromFormat<T1, T2, T3, T4, T5, T6, T7, T8>(string format, Parser<T1> p1, Parser<T2> p2, Parser<T3> p3, Parser<T4> p4, Parser<T5> p5, Parser<T6> p6, Parser<T7> p7, Parser<T8> p8)
        {
            var parts = SplitFormat(format, 8);

            return
                from t1 in parts[0]
                from t2 in p1
                from t3 in parts[1]
                from t4 in p2
                from t5 in parts[2]
                from t6 in p3
                from t7 in parts[3]
                from t8 in p4
                from t9 in parts[4]
                from t10 in p5
                from t11 in parts[5]
                from t12 in p6
                from t13 in parts[6]
                from t14 in p7
                from t15 in parts[7]
                from t16 in p8
                from t17 in parts[8]
                select new Tuple<T1, T2, T3, T4, T5, T6, T7, T8>(t2, t4, t6, t8, t10, t12, t14, t16);
        }
    }
}