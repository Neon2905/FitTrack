using System;

namespace FitTrack.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a database operation to drop an entity fails.
    /// </summary>
    class DatabaseFailedToDropEntityException : Exception
    {
        public string FailedQuery { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseFailedToDropEntityException"/> class.
        /// </summary>
        public DatabaseFailedToDropEntityException(): base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseFailedToDropEntityException"/> class with a specified error message,
        /// an inner exception, and the SQL query that failed.
        /// </summary>
        /// <param name="Message">The error message that explains the reason for the exception.</param>
        /// <param name="InnerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
        /// <param name="FailedQuery">The SQL query that failed during the operation.</param>
        public DatabaseFailedToDropEntityException(string Message, Exception InnerException = null, string FailedQuery = null): base(Message,InnerException)
        {
            this.FailedQuery = FailedQuery;
        }
    }
}
