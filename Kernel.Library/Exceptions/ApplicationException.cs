namespace Kernel.Library.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Specific exception for application.
    /// </summary>
    [Serializable]
    public class ApplicationException : BaseException
    {
        public IList<string> ErrorMessages { get; private set; }

        public ApplicationException(string errorMessage) : base(errorMessage)
        {
            ErrorMessages = new List<string> { errorMessage };
        }

        public ApplicationException(IList<string> errorMessages)
            : base(string.Join("\n", errorMessages))
        {
            ErrorMessages = new List<string>(errorMessages);
        }

        public ApplicationException()
        {
        }

        protected ApplicationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
