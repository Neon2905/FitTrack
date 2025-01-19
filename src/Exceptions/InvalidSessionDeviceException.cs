using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when there is an invalid session device.
    /// </summary>
    /// <remarks>
    /// This exception is used to signal issues related to invalid session devices, such as incorrect or unrecognized device identifiers.
    /// </remarks>
    class InvalidSessionDeviceException : Exception
    {
        /// <summary>
        /// Gets the identifier of the device that caused the exception.
        /// </summary>
        public object SessionDevice { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSessionDeviceException"/> class.
        /// </summary>
        public InvalidSessionDeviceException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSessionDeviceException"/> class with a specified error message,
        /// an inner exception, and the device identifier that caused the exception.
        /// </summary>
        /// <param name="Message">The message that describes the error.</param>
        /// <param name="InnerException">The exception that is the cause of the current exception, or <c>null</c> if no inner exception is specified.</param>
        /// <param name="SessionDevice">The identifier of the device that caused the exception.</param>
        public InvalidSessionDeviceException(string Message, Exception InnerException = null, object SessionDevice = null) : base(Message,InnerException) 
        {
            this.SessionDevice = SessionDevice;
        }
    }
}
