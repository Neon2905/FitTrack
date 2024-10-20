using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Database
{
    /// <summary>
    /// Represents the different types of session activities in the system.
    /// </summary>
    public enum SessionKind
    {
        /// <summary>
        /// Indicates a sign-in session.
        /// </summary>
        SignIn,

        /// <summary>
        /// Indicates a sign-out session.
        /// </summary>
        SignOut,

        /// <summary>
        /// Indicates a failed sign-in attempt.
        /// </summary>
        FailedSignInAttempt,

        /// <summary>
        /// Indicates a password change session.
        /// </summary>
        PasswordChange,

        /// <summary>
        /// Indicates a username change session.
        /// </summary>
        UsernameChange,

        /// <summary>
        /// Indicates a new account registration session.
        /// </summary>
        RegisterAccount,

        /// <summary>
        /// Indicates an unknown session type.
        /// </summary>
        Unknown
    }
}