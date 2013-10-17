using Roslyn.Compilers.CSharp;
using Roslyn.Compilers.Common;

namespace System.Parsing.Linq
{
    public static class CSharpParsers
    {
        public static Parser<T> FromSyntaxParse<T>(Func<string, int, T> func) where T : CommonSyntaxNode
        {
            return Parser.Create((text, offset) =>
                {
                    var syntax = func(text, offset);
                    return syntax.IsMissing
                        ? ParserResult<T>.Missing
                        : new ParserResult<T>(syntax, text, offset, syntax.FullSpan.Length);
                });
        }

        public static Parser<T> FromSyntaxParse<T>(Func<string, int, ParseOptions, T> func, ParseOptions options = null) where T : CommonSyntaxNode
        {
            return Parser.Create((text, offset) =>
                {
                    var syntax = func(text, offset, options);
                    return syntax.IsMissing
                        ? ParserResult<T>.Missing
                        : new ParserResult<T>(syntax, text, offset, syntax.FullSpan.Length);
                });
        }

        public static readonly Parser<ArgumentListSyntax> ParseArgumentList = FromSyntaxParse(Syntax.ParseArgumentList);
        public static readonly Parser<AttributeArgumentListSyntax> AttributeArgumentList = FromSyntaxParse(Syntax.ParseAttributeArgumentList);
        public static readonly Parser<BracketedArgumentListSyntax> BracketedArgumentList = FromSyntaxParse(Syntax.ParseBracketedArgumentList);
        public static readonly Parser<BracketedParameterListSyntax> BracketedParameterList = FromSyntaxParse(Syntax.ParseBracketedParameterList);
        public static readonly Parser<CompilationUnitSyntax> CompilationUnit = FromSyntaxParse(Syntax.ParseCompilationUnit);
        public static readonly Parser<ExpressionSyntax> Expression = FromSyntaxParse(Syntax.ParseExpression);
        public static readonly Parser<NameSyntax> Name = FromSyntaxParse(Syntax.ParseName);
        public static readonly Parser<ParameterListSyntax> ParameterList = FromSyntaxParse(Syntax.ParseParameterList);
        public static readonly Parser<StatementSyntax> Statement = FromSyntaxParse(Syntax.ParseStatement);
        public static readonly Parser<TypeSyntax> TypeName = FromSyntaxParse(Syntax.ParseTypeName);

        public readonly static Parser<IdentifierNameSyntax> IdentifierName = FromSyntaxParse(Syntax.ParseName)
            .Where(s => s.Kind == SyntaxKind.IdentifierName)
            .Select(s => (IdentifierNameSyntax)s);

        public readonly static Parser<GenericNameSyntax> GenericName = FromSyntaxParse(Syntax.ParseName)
            .Where(s => s.Kind == SyntaxKind.GenericName)
            .Select(s => (GenericNameSyntax)s);

        public readonly static Parser<QualifiedNameSyntax> QualifiedName = FromSyntaxParse(Syntax.ParseName)
            .Where(s => s.Kind == SyntaxKind.QualifiedName)
            .Select(s => (QualifiedNameSyntax)s);
    }
}
