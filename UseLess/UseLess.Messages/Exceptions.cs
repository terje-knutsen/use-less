namespace UseLess.Messages
{
    public static class Exceptions
    {
        public sealed class InvalidStateException : Exception 
        {
            private InvalidStateException(string message):base(message)
            { }
            public static InvalidStateException WithMessage(string message) => new(message);
        }
    }
}
