namespace System.Parsing.Linq
{
    public class ParserResult<T>
    {
        public static ParserResult<T> Missing = new ParserResult<T>();

        private readonly T _value;
        private readonly string _fullText;
        private readonly int _position;
        private readonly int _length;
        private readonly bool _isMissing;

        public ParserResult(T value, string fullText, int position, int length)
        {
            if (fullText == null) throw new ArgumentNullException("fullText");
            if (position < 0 || position >= fullText.Length) throw new ArgumentOutOfRangeException("position");
            if (length < 0 || position + length > fullText.Length) throw new ArgumentOutOfRangeException("position");

            _value = value;
            _fullText = fullText;
            _position = position;
            _length = length;
            _isMissing = false;
        }

        private ParserResult()
        {
            _isMissing = true;
        }

        public T Value { get { return _value; } }

        public string FullText { get { return _fullText; } }

        public int Position { get { return _position; } }

        public int Length { get { return _length; } }

        public bool IsMissing { get { return _isMissing; } }

        public string Text { get { return _fullText.Substring(_position, _length); } }
    }
}