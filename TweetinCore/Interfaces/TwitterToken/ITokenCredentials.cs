using TweetinCore.Interfaces.oAuth;

namespace TweetinCore.Interfaces
{
    /// <summary>
    /// Extends IOAuthCredentials and provide information that allows
    /// a user to connect to a twitter api consumer
    /// </summary>
    public interface ITokenCredentials : IOAuthCredentials
    {
        /// <summary>
        /// Temporary AccessTokenKey when trying to validate an application
        /// </summary>
        string AuthorizationKey { get; set; }

        /// <summary>
        /// Temporary AccessTokenSecret when trying to validate an application
        /// </summary>
        string AuthorizationSecret { get; set; }

        /// <summary>
        /// Key provided on twitter when trying to validate an application
        /// </summary>
        string VerifierKey { get; set; }
    }
}
