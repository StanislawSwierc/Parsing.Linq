namespace System.Parsing.Linq
{
    /// <summary>
    /// Parser which does not consule any input and always returns default
    /// value for a given type.
    /// </summary>
    /// <typeparam name="T">Type of the parser.</typeparam>
    internal class EmptyParser<T> : Parser<T>
    {
        public override ParserResult<T> Parse(string input, int offset = 0)
        {
            return new ParserResult<T>(default(T), input, offset, 0);
        }
    }
}
