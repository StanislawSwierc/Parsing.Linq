namespace System.Parsing.Linq
{
    public abstract class Parser<T>
    {
        public abstract ParserResult<T> Parse(string input);

        public T ParseComplete(string input)
        {
            var result = Parse(input);
            return result != null && string.IsNullOrEmpty(result.Rest)
                ? result.Value
                : default(T);
        }

        public static Parser<T> operator |(Parser<T> p1, Parser<T> p2)
        {
            return Parser.Create(input => p1.Parse(input) ?? p2.Parse(input));
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
