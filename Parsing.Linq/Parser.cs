namespace System.Parsing.Linq
{
    public abstract class Parser<T>
    {
        public abstract ParserResult<T> Parse(string input);
    }
}
