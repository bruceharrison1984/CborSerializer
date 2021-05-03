using System;

namespace CborSerialization
{
    public class CborException : Exception
    {
        public CborException(string message, Exception exception) : base(message, exception) { }
        public CborException(string message) : base(message) { }
    }
}
