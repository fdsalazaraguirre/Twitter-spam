using System.Runtime.Serialization;
using TweetinCore.Interfaces.oAuth;

namespace oAuthConnection
{
    /// <summary>
    /// This class provides host basic information for authorizing a OAuth
    /// consumer to connect to a service. It does not contain any logic
    /// </summary>
    [DataContract]
    public class OAuthCredentials : IOAuthCredentials
    {
        #region Public Properties

        [DataMember]
        public virtual string AccessToken { get; set; }

        [DataMember]
        public virtual string AccessTokenSecret { get; set; }

        [DataMember]
        public virtual string ConsumerKey { get; set; }

        [DataMember]
        public virtual string ConsumerSecret { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Generate an OAuthCredentials with information 
        /// related with the consumer only
        /// </summary>
        /// <param name="consumerKey">Consumer Key</param>
        /// <param name="consumerSecret">Consumer Secret Key</param>
        public OAuthCredentials(string consumerKey, string consumerSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        /// <summary>
        /// Generate an OAuthCredentials with all the information
        /// required to connect to a consumer service
        /// </summary>
        /// <param name="accessToken">Client token key</param>
        /// <param name="accessTokenSecret">Client token secret key</param>
        /// <param name="consumerKey">Consumer Key</param>
        /// <param name="consumerSecret">Consumer Secret Key</param>
        public OAuthCredentials(
            string accessToken,
            string accessTokenSecret,
            string consumerKey,
            string consumerSecret)
        {
            AccessToken = accessToken;
            AccessTokenSecret = accessTokenSecret;
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        } 

        #endregion
    }
}
