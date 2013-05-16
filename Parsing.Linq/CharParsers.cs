using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Parsing.Linq
{
    public static class CharParsers
    {
        public static readonly Parser<char> IsControl = ParserExtensions.FromChar(char.IsControl);
        public static readonly Parser<char> IsDigit = ParserExtensions.FromChar(char.IsDigit);
        public static readonly Parser<char> IsHighSurrogate = ParserExtensions.FromChar(char.IsHighSurrogate);
        public static readonly Parser<char> IsLetter = ParserExtensions.FromChar(char.IsLetter);
        public static readonly Parser<char> IsLetterOrDigit = ParserExtensions.FromChar(char.IsLetterOrDigit);
        public static readonly Parser<char> IsLowSurrogate = ParserExtensions.FromChar(char.IsLowSurrogate);
        public static readonly Parser<char> IsLower = ParserExtensions.FromChar(char.IsLower);
        public static readonly Parser<char> IsNumber = ParserExtensions.FromChar(char.IsNumber);
        public static readonly Parser<char> IsPunctuation = ParserExtensions.FromChar(char.IsPunctuation);
        public static readonly Parser<char> IsSeparator = ParserExtensions.FromChar(char.IsSeparator);
        public static readonly Parser<char> IsSurrogate = ParserExtensions.FromChar(char.IsSurrogate);
        public static readonly Parser<char> IsSymbol = ParserExtensions.FromChar(char.IsSymbol);
        public static readonly Parser<char> IsUpper = ParserExtensions.FromChar(char.IsUpper);
        public static readonly Parser<char> IsWhiteSpace = ParserExtensions.FromChar(char.IsWhiteSpace);

        public static readonly Parser<char> IsLeftParen = ParserExtensions.FromChar('(');
        public static readonly Parser<char> IsRightParen = ParserExtensions.FromChar(')');

    }
}
