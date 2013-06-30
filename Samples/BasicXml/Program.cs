using System.Parsing.Linq;

namespace BasicXml
{
    public class XmlElement
    {
        public string Name { get; set; }
        public XmlElement[] Children { get; set; }
    }

    public class XmlParser
    {
        public static readonly Parser<string> Name = Parser.FromRegex(@"\w[\w\d-_]+");

        public static readonly Parser<string> StartTag =
            from t1 in Parser.FromChar('<')
            from t2 in Name
            from t3 in Parser.FromChar('>')
            select t2;

        public static readonly Parser<string> EndTag =
            from t1 in Parser.FromChar('<')
            from t2 in Parser.FromChar('/')
            from t3 in Name
            from t4 in Parser.FromChar('>')
            select t3;

        public static readonly Parser<string> ShortTag =
            from t1 in Parser.FromChar('<')
            from t2 in Name
            from t3 in Parser.FromChar('/')
            from t4 in Parser.FromChar('>')
            select t2;

        public static readonly Parser<XmlElement> FullElement =
            from t1 in StartTag
            from t2 in Element.ZeroOrMore()
            from t3 in EndTag
            where t1 == t3
            select new XmlElement() { Name = t1, Children = t2 };

        public static readonly Parser<XmlElement> ShortElement =
            from t1 in ShortTag
            select new XmlElement() { Name = t1 };

        public static Parser<XmlElement> Element = FullElement | ShortElement;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var result = XmlParser.Element.Parse("<root><child1></child1><child2/></root>");
        }
    }
}
