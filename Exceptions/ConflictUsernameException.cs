using System;
using FitTrack.Model;

namespace FitTrack.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a new username of <see cref="Account"/> conflicts with an existing <see cref="Account"/>.
    /// </summary>
    class ConflictUsernameException : Exception
    {
        /// <summary>
        /// Gets the username associated with the conflict.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictUsernameException"/> class.
        /// </summary>
        public ConflictUsernameException() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictUsernameException"/> class with a specified error message,
        /// an inner exception, and a username.
        /// </summary>
        /// <param name="Message">The error message that explains the reason for the exception.</param>
        /// <param name="InnerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
        /// <param name="Username">The username associated with the conflict.</param>
        public ConflictUsernameException(string Message, Exception InnerException = null, string Username = null) : base(Message, InnerException)
        {
            this.Username = Username;
        }
    }
}
