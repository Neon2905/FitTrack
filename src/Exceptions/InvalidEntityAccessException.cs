using FitTrack.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Exceptions
{
    /// <summary>
    /// Exception thrown when there is an attempt to access an entity with invalid indentity.
    /// </summary>
    class InvalidEntityAccessException : Exception
    {
        /// <summary>
        /// Gets the attribute that was requested and caused the exception.
        /// </summary>
        public Enum RequestedAttribute { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEntityAccessException"/> class.
        /// </summary>
        public InvalidEntityAccessException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEntityAccessException"/> class with a specified error message,
        /// a reference to the inner exception, and the requested attribute that caused the exception.
        /// </summary>
        /// <param name="Message">The message that describes the error.</param>
        /// <param name="InnerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        /// <param name="RequestedAttribute">The attribute that was requested and caused the exception.</param>
        public InvalidEntityAccessException(string Message, Exception InnerException = null, Enum RequestedAttribute = null) : base(Message, InnerException)
        {
            this.RequestedAttribute = RequestedAttribute;
        }
    }
}
