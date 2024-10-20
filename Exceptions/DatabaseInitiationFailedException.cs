using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a database initialization operation fails.
    /// </summary>
    class DatabaseInitiationFailedException : Exception
    {
        /// <summary>
        /// Gets the SQL query that caused the initialization failure.
        /// </summary>
        public string FailedQuery { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseInitiationFailedException"/> class.
        /// </summary>
        public DatabaseInitiationFailedException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseInitiationFailedException"/> class with a specified error message,
        /// an inner exception, and the SQL query that caused the failure.
        /// </summary>
        /// <param name="Message">The error message that explains the reason for the exception.</param>
        /// <param name="InnerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
        /// <param name="FailedQuery">The SQL query that caused the initialization failure.</param>
        public DatabaseInitiationFailedException(string Message, Exception InnerException = null, string FailedQuery = null) : base(Message,InnerException)
        {
            this.FailedQuery = FailedQuery;
        }
    }
}
