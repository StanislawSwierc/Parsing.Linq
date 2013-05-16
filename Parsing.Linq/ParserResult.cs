namespace System.Parsing.Linq
{
    public class ParserResult<T>
    {
        public readonly T Value;
        public readonly string Rest;

        public ParserResult(T value, string rest)
        {
            Value = value;
            Rest = rest;
        }
    }
}