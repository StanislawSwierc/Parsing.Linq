namespace System.Parsing.Linq
{
    public static class CharParsers
    {
        public static readonly Parser<char> Control = Parser.FromChar(char.IsControl);
        public static readonly Parser<char> Digit = Parser.FromChar(char.IsDigit);
        public static readonly Parser<char> HighSurrogate = Parser.FromChar(char.IsHighSurrogate);
        public static readonly Parser<char> Letter = Parser.FromChar(char.IsLetter);
        public static readonly Parser<char> LetterOrDigit = Parser.FromChar(char.IsLetterOrDigit);
        public static readonly Parser<char> LowSurrogate = Parser.FromChar(char.IsLowSurrogate);
        public static readonly Parser<char> Lower = Parser.FromChar(char.IsLower);
        public static readonly Parser<char> Number = Parser.FromChar(char.IsNumber);
        public static readonly Parser<char> Punctuation = Parser.FromChar(char.IsPunctuation);
        public static readonly Parser<char> Separator = Parser.FromChar(char.IsSeparator);
        public static readonly Parser<char> Surrogate = Parser.FromChar(char.IsSurrogate);
        public static readonly Parser<char> Symbol = Parser.FromChar(char.IsSymbol);
        public static readonly Parser<char> Upper = Parser.FromChar(char.IsUpper);
        public static readonly Parser<char> WhiteSpace = Parser.FromChar(char.IsWhiteSpace);

        public static readonly Parser<char> LeftParen = Parser.FromChar('(');
        public static readonly Parser<char> RightParen = Parser.FromChar(')');

        public static readonly Parser<char> CarriageReturn = Parser.FromChar('\r');
        public static readonly Parser<char> LineFeed = Parser.FromChar('\n');

        /// <summary>
        /// New line parser.
        /// </summary>
        /// <remarks>
        /// One may argue if this parser should appear in CharParsers class. It is a string parser
        /// because depending on the environment the new line may consists of one or two
        /// characters. Nevertheless, conceptually this is a character and CharParsers would be
        /// the first place I would expect it to find.
        /// </remarks>
        public static readonly Parser<string> NewLine = Parser.FromText(Environment.NewLine);

    }
}
