using System;
using FitTrack.Database;

namespace FitTrack.Exceptions
{
    /// <summary>
    /// Represents an exception thrown when an object is not found in the database.
    /// </summary>
    class ObjectNotFoundInDatabaseException : Exception
    {
        /// <summary>
        /// Gets the identifier of the object that was not found.
        /// </summary>
        public object Id { get; }

        /// <summary>
        /// Gets the table in which the object was expected to be found.
        /// </summary>
        public Entities.Table Table { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectNotFoundInDatabaseException"/> class.
        /// </summary>
        public ObjectNotFoundInDatabaseException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectNotFoundInDatabaseException"/> class
        /// with a specified error message, inner exception, object identifier, and table.
        /// </summary>
        /// <param name="Message">The message that describes the error.</param>
        /// <param name="InnerException">The exception that is the cause of the current exception.</param>
        /// <param name="Id">The identifier of the object that was not found.</param>
        /// <param name="Table">The table in which the object was expected to be found.</param>
        public ObjectNotFoundInDatabaseException(string Message, Exception InnerException = null, object Id = null, Entities.Table Table = new Entities.Table()) : base(Message,InnerException)
        {
            this.Id = Id;
            this.Table = Table;
        }
    }
}
