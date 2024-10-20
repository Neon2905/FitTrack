using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Core
{   
    /// <summary>
    /// Contains a collection of static rules used throughout the application.
    /// </summary>
    public class Rules
    {
        /// <summary>
        /// The minimum length required for passwords.
        /// </summary>
        public static readonly int PasswordMustHaveLength = 12;

        /// <summary>
        /// The maximum attempts of failed logins.
        /// </summary>
        public static readonly int MaximumLoginAttempts = 8;

        public static readonly int DurationPerCoolDown = 30;
    }
}
