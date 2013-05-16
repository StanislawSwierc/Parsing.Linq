namespace System.Parsing.Linq
{
    public abstract class Parser<T>
    {
        public abstract ParserResult<T> Parse(string input);

        public static Parser<T> operator |(Parser<T> p1, Parser<T> p2)
        {
            return ParserExtensions.Create(input => p1.Parse(input) ?? p2.Parse(input));
        }

        public Parser<T2> Cast<T2>()
        {
            return this.Select(_ => (T2)(object)_);
        }

    }
}
