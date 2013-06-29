namespace System.Parsing.Linq
{
    public abstract class Parser<T>
    {
        /// <summary>
        /// Parse the text starting at the given offset.
        /// </summary>
        /// <param name="text">Text to parse</param>
        /// <param name="offset">Offset to start with.</param>
        /// <returns>Parse result.</returns>
        public abstract ParserResult<T> Parse(string text, int offset = 0);

        /// <summary>
        /// Parse all text or throw an exception if that is impossible.
        /// </summary>
        /// <param name="text">Text to parse.</param>
        /// <returns>Result of the parse.</returns>
        public T ParseAll(string text)
        {
            var parseResult = Parse(text);
            var success = !parseResult.IsMissing && parseResult.Position + parseResult.Length == text.Length;

            if(!success) throw new FormatException("Text could not be parsed.");

            return parseResult.Value;
        }

        /// <summary>
        /// Parse all the text. A return value indicates if the operation was successful.
        /// </summary>
        /// <param name="text">Text to parse.</param>
        /// <param name="result">Result of the parse or default if it failed.</param>
        /// <returns>True if the operation was successful and False otherwise.</returns>
        public bool TryParseAll(string text, out T result)
        {
            var parseResult = Parse(text);
            var success = !parseResult.IsMissing && parseResult.Position + parseResult.Length == text.Length;
            result = success ? parseResult.Value : default(T);
            return success;
        }

        public static Parser<T> operator |(Parser<T> p1, Parser<T> p2)
        {
            return Parser.Create((text, offset) =>
                {
                    var result = p1.Parse(text, offset);
                    return !result.IsMissing
                        ? result
                        : p2.Parse(text, offset);
                });
        }

        /// <remarks>
        /// This method was added directly to the Parse class to get better syntax.
        /// It is conversion from one type to another. In order to pass the information
        /// about types one could you create an extension method but the call would
        /// like Cast[T1, T2]. When this is an istance method one type is taken from
        /// the instance and only the second is passed explicitly. Such code is similar
        /// to Enumerable.Cast.
        /// </remarks>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public Parser<T2> Cast<T2>()
        {
            return this.Select(_ => (T2)(object)_);
        }

    }
}
