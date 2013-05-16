namespace System.Parsing.Linq
{
    public class AnonymousParser<T> : Parser<T>
    {
        private readonly Func<string, ParserResult<T>> _func;

        public AnonymousParser(Func<string, ParserResult<T>> func)
        {
            _func = func;
        }

        public override ParserResult<T> Parse(string input)
        {
            return _func(input);
        }
    }
}