namespace Kernel.Library.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Specific exception for domain.
    /// </summary>
    [Serializable]
    public class DomainException : BaseException
    {
        public IList<string> ErrorMessages { get; private set; }

        public DomainException(string errorMessage) : base(errorMessage)
        {
            ErrorMessages = new List<string> { errorMessage };
        }

        public DomainException(IList<string> errorMessages)
            : base(string.Join("\n", errorMessages))
        {
            ErrorMessages = new List<string>(errorMessages);
        }

        public DomainException()
        {
        }

        protected DomainException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
