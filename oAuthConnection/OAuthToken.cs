using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using TweetinCore.Enum;
using TweetinCore.Events;
using TweetinCore.Interfaces.oAuth;
using oAuthConnection.Utils;

namespace oAuthConnection
{
    /// <summary>
    /// Generate a Token that can be used to perform OAuth queries
    /// </summary>
    public class OAuthToken : IOAuthToken
    {
        #region Attributes

        /// <summary>
        /// Object used to generate the HttpWebRequest based on parameters
        /// </summary>
        private readonly OAuthWebRequestGenerator _queryGenerator;

        #endregion

        #region Properties

        /// <summary>
        /// Credentials used by the Token to create queries
        /// </summary>
        public virtual IOAuthCredentials Credentials { get; set; }
        
        /// <summary>
        /// Headers of the latest WebResponse
        /// </summary>
        protected WebHeaderCollection _lastHeadersResult { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Generates a Token with credentials of both user and consumer
        /// </summary>
        /// <param name="accessToken">Client token key</param>
        /// <param name="accessTokenSecret">Client token secret key</param>
        /// <param name="consumerKey">Consumer Key</param>
        /// <param name="consumerSecret">Consumer Secret Key</param>
        public OAuthToken(
            string accessToken,
            string accessTokenSecret,
            string consumerKey,
            string consumerSecret)
            : this(new OAuthCredentials(accessToken, accessTokenSecret, consumerKey, consumerSecret))
        {
        }

        /// <summary>
        /// Generates a Token with a specific OAuthCredentials
        /// </summary>
        /// <param name="credentials">Credentials object</param>
        public OAuthToken(IOAuthCredentials credentials)
        {
            if (credentials == null)
            {
                throw new ArgumentNullException("credentials");
            }

            Credentials = credentials;
            _queryGenerator = new OAuthWebRequestGenerator();
        }

        #endregion

        #region IOAuthToken Members

        public virtual IEnumerable<IOAuthQueryParameter> GenerateParameters()
        {
            List<IOAuthQueryParameter> headers = new List<IOAuthQueryParameter>();
            // Add Header for every connection to a Twitter Application
            if (!String.IsNullOrEmpty(Credentials.ConsumerKey) && !String.IsNullOrEmpty(Credentials.ConsumerSecret))
            {
                headers.Add(new OAuthQueryParameter("oauth_consumer_key", StringFormater.UrlEncode(Credentials.ConsumerKey), true, true, false));
                headers.Add(new OAuthQueryParameter("oauth_consumer_secret", StringFormater.UrlEncode(Credentials.ConsumerSecret), false, false, true));
            }

            // Add Header for authenticated connection to a Twitter Application
            if (!String.IsNullOrEmpty(Credentials.AccessToken) && !String.IsNullOrEmpty(Credentials.AccessTokenSecret))
            {
                headers.Add(new OAuthQueryParameter("oauth_token", StringFormater.UrlEncode(Credentials.AccessToken), true, true, false));
                headers.Add(new OAuthQueryParameter("oauth_token_secret", StringFormater.UrlEncode(Credentials.AccessTokenSecret), false, false, true));
            }
            else
            {
                headers.Add(new OAuthQueryParameter("oauth_token", "", false, false, true));
            }

            return headers;
        }

        // Retrieve a HttpWebRequest for a specific query
        public virtual HttpWebRequest GetQueryWebRequest(
            string url,
            HttpMethod httpMethod,
            IEnumerable<IOAuthQueryParameter> headers = null)
        {
            if (headers == null)
            {
                headers = GenerateParameters();
            }

            return _queryGenerator.GenerateWebRequest(url, httpMethod, headers);
        }

        public virtual string ExecuteQueryWithSpecificParameters(
            string url,
            HttpMethod httpMethod,
            WebExceptionHandlingDelegate exceptionHandler,
            IEnumerable<IOAuthQueryParameter> headers)
        {
            string result = null;

            HttpWebRequest httpWebRequest = null;
            WebResponse response = null;

            try
            {
                httpWebRequest = GetQueryWebRequest(url, httpMethod, headers);
                httpWebRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);

                // Opening the connection
                response = httpWebRequest.GetResponse();
                Stream stream = response.GetResponseStream();

                _lastHeadersResult = response.Headers;

                if (stream != null)
                {
                    // Getting the result
                    StreamReader responseReader = new StreamReader(stream);
                    result = responseReader.ReadLine();
                }

                // Closing the connection
                response.Close();
                httpWebRequest.Abort();
            }
            catch (WebException wex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(wex);
                }
                else
                {
                    throw;
                }

                if (response != null)
                {
                    response.Close();
                }

                if (httpWebRequest != null)
                {
                    httpWebRequest.Abort();
                }
            }

            return result;
        }

        // Execute a generic simple query
        public virtual string ExecuteQuery(
            string url,
            HttpMethod httpMethod,
            WebExceptionHandlingDelegate exceptionHandler)
        {
            return ExecuteQueryWithSpecificParameters(url, httpMethod, exceptionHandler, GenerateParameters());
        }

        #endregion
    }
}
