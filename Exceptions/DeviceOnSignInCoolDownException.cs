using System;
using FitTrack.Model;

namespace FitTrack.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a <see cref="SessionDevice"/> is on a sign-in cooldown.
    /// </summary>
    class DeviceOnSignInCoolDownException : Exception
    {
        /// <summary>
        /// Sign-in cooldown timestamp when a <see cref="SessionDevice"/> is prevented from signing in.
        /// </summary>
        public DateTime? SignInCoolDown { get; }

        /// <summary>
        /// Gets the <see cref="SessionDevice"/> that is on a sign-in cooldown.
        /// </summary>
        public SessionDevice Device { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceOnSignInCoolDownException"/> class.
        /// </summary>
        public DeviceOnSignInCoolDownException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceOnSignInCoolDownException"/> class with a specified error message,
        /// an inner exception, the device on cooldown, and the sign-in cooldown timestamp.
        /// </summary>
        /// <param name="Message">The error message that explains the reason for the exception.</param>
        /// <param name="InnerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
        /// <param name="Device">The device that is on cooldown, preventing further sign-ins.</param>
        /// <param name="SignInCoolDown">The timestamp when the device is on cooldown, preventing further sign-ins.</param>
        public DeviceOnSignInCoolDownException(string Message, Exception InnerException = null, SessionDevice Device = null, DateTime? SignInCoolDown = null) : base(Message, InnerException)
        {
            this.SignInCoolDown = SignInCoolDown;
            this.Device = Device;
        }
    }
}
