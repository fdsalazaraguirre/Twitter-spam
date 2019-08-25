using TweetinCore.Interfaces;
using oAuthConnection;
using TweetinCore.Interfaces.oAuth;

namespace TwitterToken
{
    /// <summary>
    /// Credentials to connect a twitter account to a twitter consumer
    /// </summary>
    public class TokenCredentials : OAuthCredentials, ITokenCredentials
    {
        #region Public Properties

        /// <summary>
        /// Temporary AccessTokenKey when trying to validate an application
        /// </summary>
        public virtual string AuthorizationKey { get; set; }

        /// <summary>
        /// Temporary AccessTokenSecret when trying to validate an application
        /// </summary>
        public virtual string AuthorizationSecret { get; set; }

        /// <summary>
        /// Key provided on twitter when trying to validate an application
        /// </summary>
        public virtual string VerifierKey { get; set; }
        
        #endregion

        #region Constructors
        
        /// <summary>
        /// Create Twitter credentials based on an OAuthCredentials
        /// </summary>
        /// <param name="credentials">OAuth Credentials</param>
        public TokenCredentials(IOAuthCredentials credentials)
            : this(credentials.AccessToken, credentials.AccessTokenSecret, 
                   credentials.ConsumerKey, credentials.ConsumerSecret)
        {
        }

        /// <summary>
        /// Create Twitter credentials to connect on a consumer requiring
        /// any specific Twitter account
        /// </summary>
        /// <param name="consumerKey">Consumer Key</param>
        /// <param name="consumerSecret">Consumer Secret Key</param>
        public TokenCredentials(string consumerKey, string consumerSecret)
            : base(consumerKey, consumerSecret)
        {
        }

        /// <summary>
        /// Generate Twitter credentials with all the information
        /// required to connect to a consumer service
        /// </summary>
        /// <param name="accessToken">Client token key</param>
        /// <param name="accessTokenSecret">Client token secret key</param>
        /// <param name="consumerKey">Consumer Key</param>
        /// <param name="consumerSecret">Consumer Secret Key</param>
        public TokenCredentials(string accessToken, string accessTokenSecret, string consumerKey, string consumerSecret)
            : base(accessToken, accessTokenSecret, consumerKey, consumerSecret)
        {
        } 

        #endregion
    }
}
