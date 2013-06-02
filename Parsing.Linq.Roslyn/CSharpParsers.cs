using Roslyn.Compilers.CSharp;
using Roslyn.Compilers.Common;

namespace System.Parsing.Linq
{
    public static class CSharpParsers
    {
        public static Parser<T> FromSyntaxParse<T>(Func<string, int, T> func) where T : CommonSyntaxNode
        {
            return Parser.Create(input =>
                {
                    var syntax = func(input, 0);
                    return syntax.IsMissing ? null :
                        new ParserResult<T>(syntax, input.Substring(syntax.FullSpan.Length));
                });
        }

        public static Parser<T> FromSyntaxParse<T>(Func<string, int, ParseOptions, T> func, ParseOptions options = null) where T : CommonSyntaxNode
        {
            return Parser.Create(input =>
                {
                    var syntax = func(input, 0, options);
                    return syntax.IsMissing ? null :
                        new ParserResult<T>(syntax, input.Substring(syntax.FullSpan.Length));
                });
        }

        public static readonly Parser<NameSyntax> Identifier = FromSyntaxParse(Syntax.ParseName);
        public static readonly Parser<ExpressionSyntax> Expression = FromSyntaxParse(Syntax.ParseExpression);
        public static readonly Parser<TypeSyntax> TypeName = FromSyntaxParse(Syntax.ParseTypeName);
        public static readonly Parser<ParameterListSyntax> ParameterList = FromSyntaxParse(Syntax.ParseParameterList);
    }
}
