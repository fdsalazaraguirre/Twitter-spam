using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TweetinCore.Enum;
using TweetinCore.Events;
using Tweetinvi.Utils;
using oAuthConnection;
using TweetinCore.Interfaces;
using TwitterToken.Properties;
using TweetinCore.Interfaces.oAuth;
using System.Text.RegularExpressions;

namespace TwitterToken
{
    /// <summary>
    /// Class allowing to create a Token by requesting it from Twitter
    /// </summary>
    public class TokenCreator : ITokenCreator
    {
        #region Private Fields

        private readonly IOAuthToken _tokenRequester;
        private readonly ITokenCredentials _credentials;

        #endregion

        #region Constructors

        public TokenCreator(string consumerKey, string consumerSecret)
        {
            _credentials = new TokenCredentials(null, null, consumerKey, consumerSecret);
            _tokenRequester = new OAuthToken(_credentials);
        }

        #endregion

        #region Helpers
        /// <summary>
        /// Request a Token authorization url
        /// </summary>
        /// <returns>Authorization url</returns>
        private string RequestToken()
        {
            // TODO : Add x_auth_access_type parameter to the query
            return _tokenRequester.ExecuteQuery(Resources.OAuthRequestToken, HttpMethod.POST, null);
        }

        /// <summary>
        /// Request an access token 
        /// </summary>
        /// <param name="username">Twitter user's name</param>
        /// <param name="password">Twitter user's password</param>
        /// <param name="callBackURL">Twitter callback URL</param>
        private void RequestAccessToken(string username, string password, string callBackURL)
        {
            // TODO : Implement a method to automatically validate the 
            throw new NotImplementedException("Please be patient as we implement new features!");
        }

        /// <summary>
        /// Generate a Token by providing a verifier key
        /// </summary>
        /// <param name="twitterConfirmationCode">Verifier Key</param>
        /// <returns>Requested Token</returns>
        public IToken GenerateToken(string twitterConfirmationCode)
        {
            return GenerateToken(twitterConfirmationCode,
                                 _credentials.AuthorizationKey,
                                 _credentials.AuthorizationSecret,
                                 _credentials.ConsumerKey,
                                 _credentials.ConsumerSecret);
        }
        #endregion

        #region ITokenCreator Members
        public virtual IToken CreateToken(RetrieveCaptchaDelegate captchaDelegate)
        {
            if (captchaDelegate == null)
            {
                return null;
            }

            string tokenRequestUrl = CreateTokenRequestAuthorizationUrl();
            return GenerateToken(captchaDelegate(tokenRequestUrl).ToString());
        }

        public string CreateTokenRequestAuthorizationUrl()
        {
            string result = null;
            string requestTokenResponse = RequestToken();

            if (!string.IsNullOrEmpty(requestTokenResponse) &&
                requestTokenResponse != Resources.OAuthRequestToken)
            {
                Match tokenInformation = Regex.Match(requestTokenResponse, Resources.OAuthTokenRequestRegex);

                _credentials.AuthorizationKey = tokenInformation.Groups["oauth_token"].Value;
                _credentials.AuthorizationSecret = tokenInformation.Groups["oauth_token_secret"].Value;
                // var callbackConfirmed = tokenInformation.Groups["oauth_callback_confirmed"].Value;

                result = String.Format("{0}?oauth_token={1}",
                                        Resources.OAuthRequestAuthorize,
                                       _credentials.AuthorizationKey);
            }

            return result;
        }

        public IToken GenerateToken(
            string twitterConfirmationCode,
            string authorizationKey,
            string authorizationSecret,
            string consumerKey,
            string consumerSecret)
        {
            IOAuthCredentials credentials = new OAuthCredentials(authorizationKey,
                                                                 authorizationSecret,
                                                                 consumerKey,
                                                                 consumerSecret);

            IOAuthToken token = new OAuthToken(credentials);

            List<IOAuthQueryParameter> headers = token.GenerateParameters().ToList();
            headers.Add(new OAuthQueryParameter("oauth_verifier", twitterConfirmationCode, true, true, false));

            string response = "";

            WebExceptionHandlingDelegate wex = delegate(WebException exception)
            {
                if (ExceptionUtils.GetWebExceptionStatusNumber(exception) == 401)
                {
                    response = null;
                }
            };

            response = token.ExecuteQueryWithSpecificParameters(Resources.OAuthRequestAccessToken,
                                                             HttpMethod.POST,
                                                             wex,
                                                             headers);

            if (response == null)
            {
                return null;
            }

            Match responseInformation = Regex.Match(response, Resources.OAuthTokenAccessRegex);

            IToken result = null;
            if (responseInformation.Groups["oauth_token"] != null &&
                responseInformation.Groups["oauth_token_secret"] != null)
            {
                result = new Token(responseInformation.Groups["oauth_token"].Value,
                                   responseInformation.Groups["oauth_token_secret"].Value,
                                   consumerKey,
                                   consumerSecret);
            }

            return result;
        }

        #endregion
    }
}
