namespace System.Parsing.Linq
{
    internal class AnonymousParser<T> : Parser<T>
    {
        private readonly Func<string, int, ParserResult<T>> _func;

        public AnonymousParser(Func<string, int, ParserResult<T>> func)
        {
            _func = func;
        }

        public override ParserResult<T> Parse(string input, int offset)
        {
            return _func(input, offset);
        }
    }
}