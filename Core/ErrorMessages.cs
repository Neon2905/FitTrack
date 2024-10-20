namespace FitTrack.Core
{
    /// <summary>
    /// Contains a collection of static error messages used throughout the application.
    /// </summary>
    public class ErrorMessages
    {
        /// <summary>
        /// The message displayed when the login data is incorrect.
        /// </summary>
        public static readonly string InvalidLoginDataMessage = "Username or password is incorrect";

        /// <summary>
        /// The message displayed when the provided password does not meet the required length.
        /// </summary>
        public static readonly string RequiredPasswordLengthNotMetMessage = $"Password must be of length {Rules.PasswordMustHaveLength} characters.";

        /// <summary>
        /// The message displayed when the passwords do not match.
        /// </summary>
        public static readonly string PasswordsNotMatchMessage = "Passwords do not match.";

        /// <summary>
        /// The message displayed when the password does not meet the required criteria (upper and lower-case letters).
        /// </summary>
        public static readonly string PasswordRequirementsNotMetMessage = "Must include upper and lower-case letters.";

        /// <summary>
        /// The message displayed when the username is already taken.
        /// </summary>
        public static readonly string UsernameAlreadyTakenMessage = "Username is already taken.";

        /// <summary>
        /// The message displayed when the username contains special characters or spaces.
        /// </summary>
        public static readonly string UsernameRequirementsNotMetMessage = "Username is not valid.";
    }
}
