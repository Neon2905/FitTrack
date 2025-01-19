using FitTrack.Model;

namespace FitTrack.Database
{
    /// <summary>
    /// Specifies the available methods for <see cref="Account"/> sigin authentication.
    /// </summary>
    public enum AuthenticationMethod
    {
        /// <summary>
        /// Authentication using a password.
        /// </summary>
        ByPassword,

        /// <summary>
        /// Authentication using a login token.
        /// </summary>
        ByLoginToken
    }
}
