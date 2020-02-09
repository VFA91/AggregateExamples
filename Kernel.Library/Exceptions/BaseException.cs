namespace Kernel.Library.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception base.
    /// </summary>
    [Serializable]
    public class BaseException : Exception
    {
        public BaseException()
        {
        }

        public BaseException(string message)
            : base(message)
        {
        }

        public BaseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
