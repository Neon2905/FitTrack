using System;
using FitTrack.Model;

namespace FitTrack.Exceptions
{
    /// <summary>
    /// Represents an exception thrown when login credentials for <see cref="Account"/> are invalid.
    /// </summary>
    class InvalidLoginCreditentialException : Exception
    {
        /// <summary>
        /// Gets the username associated with the invalid login attempt.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Gets the password associated with the invalid login attempt.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidLoginCreditentialException"/> class.
        /// </summary>
        public InvalidLoginCreditentialException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidLoginCreditentialException"/> class
        /// with a specified error message, inner exception, username, and password.
        /// </summary>
        /// <param name="Message">The message that describes the error.</param>
        /// <param name="InnerException">The exception that is the cause of the current exception.</param>
        /// <param name="Username">The username associated with the invalid login attempt.</param>
        /// <param name="Password">The password associated with the invalid login attempt.</param>
        public InvalidLoginCreditentialException(string Message, Exception InnerException = null, string Username = null, string Password = null) : base(Message, InnerException)
        {
            this.Username = Username;
            this.Password = Password;
        }
    }
}
