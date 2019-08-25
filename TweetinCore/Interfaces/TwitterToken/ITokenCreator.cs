using TweetinCore.Events;

namespace TweetinCore.Interfaces
{
    /// <summary>
    /// Class used to get Token from Twitter
    /// </summary>
    public interface ITokenCreator
    {
        /// <summary>
        /// Create a token for a user
        /// </summary>
        /// <param name="captchaDelegate">
        /// This delegate ask for a user to provide the verifier information
        /// provided by twitter when the user authorize an application
        /// </param>
        /// <returns>Token for a specific user and specific consumer</returns>
        IToken CreateToken(RetrieveCaptchaDelegate captchaDelegate);

        /// <summary>
        /// Get the authorization URL to access a consumer and request an access Token
        /// </summary>
        /// <returns></returns>
        string CreateTokenRequestAuthorizationUrl();

        IToken GenerateToken(string twitterConfirmationCode);

        /// <summary>
        /// Generate a Token from all the information required
        /// </summary>
        /// <param name="twitterConfirmationCode">Verifier code on Twitter validation page</param>
        /// <param name="authorizationKey">Key authorizing the consumer to ask for a Token</param>
        /// <param name="authorizationSecret">Secret Key authorizing the consumer to ask for a Token</param>
        /// <param name="consumerKey">Key linked with the consumer</param>
        /// <param name="consumerSecret">Secret Key linked with the consumer</param>
        /// <returns>Token for a specific user and specific consumer</returns>
        IToken GenerateToken(
            string twitterConfirmationCode,
            string authorizationKey,
            string authorizationSecret,
            string consumerKey,
            string consumerSecret);
    }
}
