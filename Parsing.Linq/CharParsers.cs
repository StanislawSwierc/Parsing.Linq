namespace System.Parsing.Linq
{
    public static class CharParsers
    {
        public static readonly Parser<char> IsControl = Parser.FromChar(char.IsControl);
        public static readonly Parser<char> IsDigit = Parser.FromChar(char.IsDigit);
        public static readonly Parser<char> IsHighSurrogate = Parser.FromChar(char.IsHighSurrogate);
        public static readonly Parser<char> IsLetter = Parser.FromChar(char.IsLetter);
        public static readonly Parser<char> IsLetterOrDigit = Parser.FromChar(char.IsLetterOrDigit);
        public static readonly Parser<char> IsLowSurrogate = Parser.FromChar(char.IsLowSurrogate);
        public static readonly Parser<char> IsLower = Parser.FromChar(char.IsLower);
        public static readonly Parser<char> IsNumber = Parser.FromChar(char.IsNumber);
        public static readonly Parser<char> IsPunctuation = Parser.FromChar(char.IsPunctuation);
        public static readonly Parser<char> IsSeparator = Parser.FromChar(char.IsSeparator);
        public static readonly Parser<char> IsSurrogate = Parser.FromChar(char.IsSurrogate);
        public static readonly Parser<char> IsSymbol = Parser.FromChar(char.IsSymbol);
        public static readonly Parser<char> IsUpper = Parser.FromChar(char.IsUpper);
        public static readonly Parser<char> IsWhiteSpace = Parser.FromChar(char.IsWhiteSpace);

        public static readonly Parser<char> IsLeftParen = Parser.FromChar('(');
        public static readonly Parser<char> IsRightParen = Parser.FromChar(')');

    }
}
