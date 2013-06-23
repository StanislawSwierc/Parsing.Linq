namespace System.Parsing.Linq
{
    public class ParserResult<T>
    {
        public static ParserResult<T> Missing = new ParserResult<T>();

        private readonly T _value;
        private readonly string _source;
        private readonly int _position;
        private readonly int _length;
        private readonly bool _isMissing;

        public ParserResult(T value, string source, int position, int length)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (position < 0 || position >= source.Length) throw new ArgumentOutOfRangeException("position");
            if (length < 0 || position + length > source.Length) throw new ArgumentOutOfRangeException("position");

            _value = value;
            _source = source;
            _position = position;
            _length = length;
            _isMissing = false;
        }

        private ParserResult()
        {
            _isMissing = true;
        }

        public T Value { get { return _value; } }

        public string Source { get { return _source; } }

        public int Position { get { return _position; } }

        public int Length { get { return _length; } }

        public bool IsMissing { get { return _isMissing; } }

        public string Text { get { return _source.Substring(_position, _length); } }
    }
}