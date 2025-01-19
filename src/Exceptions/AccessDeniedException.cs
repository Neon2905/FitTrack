using System;
using FitTrack.Model;

namespace FitTrack.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when access to <see cref="Account"/> is denied for a specific <see cref="SessionDevice"/>.
    /// </summary>
    class AccessDeniedException : Exception
    {
        /// <summary>
        /// Gets the username associated with the access denial.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessDeniedException"/> class.
        /// </summary>
        public AccessDeniedException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessDeniedException"/> class with a specified error message,
        /// an inner exception, and a username.
        /// </summary>
        /// <param name="Message">The error message that explains the reason for the exception.</param>
        /// <param name="InnerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
        /// <param name="Username">The username of account where access denial occurs</param>
        public AccessDeniedException(string Message, Exception InnerException = null, string Username = null) : base(Message, InnerException)
        {
            this.Username = Username;
        }
    }
}
