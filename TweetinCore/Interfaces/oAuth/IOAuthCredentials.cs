namespace TweetinCore.Interfaces.oAuth
{
    /// <summary>
    /// Defines a contract of 4 information to connect to an OAuth service
    /// </summary>
    public interface IOAuthCredentials
    {
        /// <summary>
        /// Key provided to the consumer to provide an authentication of the client
        /// </summary>
        string AccessToken { get; set; }

        /// <summary>
        /// Secret Key provided to the consumer to provide an authentication of the client
        /// </summary>
        string AccessTokenSecret { get; set; }

        /// <summary>
        /// Key identifying a specific consumer service
        /// </summary>
        string ConsumerKey { get; set; }

        /// <summary>
        /// Secret Key identifying a specific consumer service
        /// </summary>
        string ConsumerSecret { get; set; }
    }
}
