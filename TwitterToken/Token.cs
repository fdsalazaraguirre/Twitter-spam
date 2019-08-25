using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using TweetinCore.Enum;
using TweetinCore.Events;
using TweetinCore.Interfaces.oAuth;
using oAuthConnection;
using TweetinCore.Interfaces;
using TwitterToken.Properties;

namespace TwitterToken
{
    /// <summary>
    /// Class allowing a User to execute queries with specific credentials
    /// </summary>
    public class Token : OAuthToken, IToken
    {
        #region Private Attributes

        /// <summary>
        /// Serializer allowing to convert a string into a Dictionary[string, object]
        /// </summary>
        private readonly JavaScriptSerializer _jsSerializer;

        #endregion

        #region Properties

        // Information concerning the current session
        public int XRateLimit { get; set; }
        public int XRateLimitRemaining { get; set; }

        public DateTime XRateLimitResetTime { get; set; }
        public int XRateLimitResetTimeInSeconds { get; set; }

        // Default Exception Handler
        public bool IntegratedExceptionHandler { get; set; }
        public WebExceptionHandlingDelegate ExceptionHandler { get; set; }

        #region IOAuthCredentials Members

        private ITokenCredentials _twitterCredentials;
        public ITokenCredentials TwitterCredentials
        {
            get { return _twitterCredentials; }
            set { _twitterCredentials = value; }
        }

        // Facade hiding the TwitterCredentials
        public override IOAuthCredentials Credentials
        {
            get { return _twitterCredentials; }
            set
            {
                var val = value as ITokenCredentials;
                if (val != null)
                {
                    _twitterCredentials = val;
                }
                else
                {
                    _twitterCredentials = new TokenCredentials(value);
                }
            }
        }

        public string AccessToken
        {
            get { return TwitterCredentials.AccessToken; }
            set { TwitterCredentials.AccessToken = value; }
        }

        public string AccessTokenSecret
        {
            get { return TwitterCredentials.AccessTokenSecret; }
            set { TwitterCredentials.AccessTokenSecret = value; }
        }

        public string ConsumerKey
        {
            get { return TwitterCredentials.ConsumerKey; }
            set { TwitterCredentials.ConsumerKey = value; }
        }

        public string ConsumerSecret
        {
            get { return TwitterCredentials.ConsumerSecret; }
            set { TwitterCredentials.ConsumerSecret = value; }
        }

        public string VerifierKey
        {
            get { return TwitterCredentials.VerifierKey; }
            set { TwitterCredentials.VerifierKey = value; }
        }
        public string AuthorizationKey
        {
            get { return TwitterCredentials.AuthorizationKey; }
            set { TwitterCredentials.AuthorizationKey = value; }
        }
        public string AuthorizationSecret
        {
            get { return TwitterCredentials.AuthorizationSecret; }
            set { TwitterCredentials.AuthorizationSecret = value; }
        }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Generate Twitter token with all the information
        /// required to connect to a consumer service
        /// </summary>
        /// <param name="accessToken">Client token key</param>
        /// <param name="accessTokenSecret">Client token secret key</param>
        /// <param name="consumerKey">Consumer Key</param>
        /// <param name="consumerSecret">Consumer Secret Key</param>
        public Token(string accessToken,
                     string accessTokenSecret,
                     string consumerKey,
                     string consumerSecret)
            : base(accessToken, accessTokenSecret, consumerKey, consumerSecret)
        {
            _jsSerializer = new JavaScriptSerializer();
        }

        #endregion

        #region IToken Members

        #region Execute Simple Query

        public Dictionary<string, object> ExecuteGETQuery(
            string url,
            ObjectResponseDelegate objectHandler = null,
            WebExceptionHandlingDelegate exceptionHandler = null)
        {
            return ExecuteQueryWithSingleResponse(url, HttpMethod.GET, objectHandler, exceptionHandler);
        }

        public Dictionary<string, object> ExecutePOSTQuery(
            string url,
            ObjectResponseDelegate objectHandler = null,
            WebExceptionHandlingDelegate exceptionHandler = null)
        {
            return ExecuteQueryWithSingleResponse(url, HttpMethod.POST, objectHandler, exceptionHandler);
        }

        public Dictionary<string, object>[] ExecuteGETQueryReturningCollectionOfObjects(
            string url,
            ObjectResponseDelegate objectHandler = null,
            WebExceptionHandlingDelegate exceptionHandler = null)
        {
            return ExecuteQuery(url, HttpMethod.GET, objectHandler, exceptionHandler);
        }

        public Dictionary<string, object>[] ExecutePOSTQueryReturningCollectionOfObjects(
           string url,
           ObjectResponseDelegate objectHandler = null,
           WebExceptionHandlingDelegate exceptionHandler = null)
        {
            return ExecuteQuery(url, HttpMethod.POST, objectHandler, exceptionHandler);
        }

        // Execute a query that should return a single object
        private Dictionary<string, object> ExecuteQueryWithSingleResponse(
            string url,
            HttpMethod httpMethod,
            ObjectResponseDelegate objectDeletage,
            WebExceptionHandlingDelegate exceptionHandler)
        {
            Dictionary<string, object>[] results = ExecuteQuery(url, httpMethod, objectDeletage, exceptionHandler);

            if (results != null && results.Count() == 1)
            {
                return results[0];
            }

            return null;
        }

        // Execute a basic query
        private Dictionary<string, object>[] ExecuteQuery(
            string url,
            HttpMethod httpMethod,
            ObjectResponseDelegate objectDeletage,
            WebExceptionHandlingDelegate exceptionHandler)
        {
            dynamic result = null;
            try
            {
                // Exception Handler is being called by the OAuthToken
                string response = ExecuteQuery(url, httpMethod, exceptionHandler);

                if (_lastHeadersResult["X-RateLimit-Remaining"] != null)
                {
                    XRateLimitRemaining = Int32.Parse(_lastHeadersResult["X-RateLimit-Remaining"]);
                }

                // Getting the result back from Twitter

                if (response == null)
                {
                    return null;
                }

                dynamic jsonResponse = _jsSerializer.Deserialize<dynamic>(response);

                if (jsonResponse is object[])
                {
                    object[] temp = (_jsSerializer.Deserialize<dynamic>(response) as object[]);

                    if (temp != null)
                    {
                        result = temp.OfType<Dictionary<string, object>>().ToArray();

                        if (objectDeletage != null)
                        {
                            foreach (Dictionary<string, object> o in result)
                            {
                                objectDeletage(o);
                            }
                        }
                    }
                }
                else
                {
                    result = new Dictionary<string, object>[1];
                    result[0] = jsonResponse;

                    if (objectDeletage != null)
                    {
                        objectDeletage(jsonResponse);
                    }
                }
            }
            catch (WebException wex)
            {
                #region Exception Management
                // This Exception management should only be called when there is no specific exception handler
                // Or when the Exception Handler gets an error it was not expecting

                if (ExceptionHandler != null)
                {
                    // Use the inner ExceptionHandler
                    ExceptionHandler(wex);
                }
                else
                {
                    throw;
                }

                #endregion
            }

            return result;
        }

        #endregion

        #region Execute Cursor Query

        public Dictionary<string, object> ExecuteCursorQuery(
            string url,
            DynamicResponseDelegate cursorDelegate)
        {
            return ExecuteCursorQuery(url, 0, Int32.MaxValue, cursorDelegate);
        }

        public Dictionary<string, object> ExecuteCursorQuery(
            string url,
            long cursor = 0,
            int max = Int32.MaxValue,
            DynamicResponseDelegate responseHandler = null,
            WebExceptionHandlingDelegate exceptionHandler = null)
        {
            return ExecuteCursorQuery(url, ref cursor, max, responseHandler, exceptionHandler);
        }

        public Dictionary<string, object> ExecuteCursorQuery(
            string url,
            ref long cursor,
            int max = Int32.MaxValue,
            DynamicResponseDelegate responseHandler = null,
            WebExceptionHandlingDelegate exceptionHandler = null)
        {
            long previousCursor = cursor;
            long nextCursor = -1;
            int nbOfObjectsProcessed = 0;

            while (previousCursor != nextCursor && nbOfObjectsProcessed < max)
            {
                try
                {
                    string queryDelimiter = "&";
                    if (url.EndsWith(".json") || url.EndsWith(".xml") || url.EndsWith("."))
                    {
                        queryDelimiter = "?";
                    }

                    Dictionary<string, object> responseObject = ExecuteGETQuery(String.Format("{0}" + queryDelimiter + "cursor={1}", url, nextCursor), null, exceptionHandler);

                    // Go to the next object
                    if (responseObject != null)
                    {
                        previousCursor = Int64.Parse(responseObject["previous_cursor"].ToString());
                        nextCursor = Int64.Parse(responseObject["next_cursor"].ToString());
                    }

                    if (responseHandler != null)
                    {
                        // Manual response management
                        nbOfObjectsProcessed += responseHandler(responseObject, previousCursor, nextCursor);
                    }
                }
                catch (WebException wex)
                {
                    if (ExceptionHandler != null)
                    {
                        ExceptionHandler(wex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return null;
        }

        #endregion

        #region Execute Since & Max Query

        /// <summary>
        /// Method allowing to request data from an URL targeting a specific timeframe (using since_id and max_id parameters).
        /// Since_id must be included in the method_url parameter.
        /// Iterate over the different pages of results for this time frame. 
        /// For each iteration, memorize the maximum of all the object ids and reuse it in the URL to request objects for the 
        /// next iteration. Also, handle each object retrieved with the ObjectResponseDelegate given in parameter.
        /// </summary>
        /// <param name="url">Url from Twitter Api to query. Can include a since_id parameter</param>
        /// <param name="sinceMaxDelegate">delegate handling the objects retrieved from the Twitter API</param>
        /// <param name="webExceptionHandlerDelegate">Handle exceptions occuring upon request to the Twitter API</param>
        public void ExecuteSinceMaxQuery(
            string url,
            ObjectResponseDelegate sinceMaxDelegate,
            WebExceptionHandlingDelegate webExceptionHandlerDelegate = null)
        {
            string updatedMethodUrl = url;
            long maxId = Int64.MaxValue;
            int startLoop = 1;
            int nbItemsReceived = 0;

            ObjectResponseDelegate objectDelegate = delegate(Dictionary<string, object> o)
            {
                ++nbItemsReceived;
                long itemId = (long)o["id"];

                if (maxId > itemId)
                {
                    maxId = itemId;
                }

                if (sinceMaxDelegate != null)
                {
                    sinceMaxDelegate(o);
                }
            };

            while (startLoop != nbItemsReceived)
            {
                try
                {
                    dynamic responseObject = ExecuteGETQuery(updatedMethodUrl, objectDelegate, webExceptionHandlerDelegate);

                    if (responseObject == null)
                    {
                        break;
                    }

                    if (startLoop == nbItemsReceived)
                    {
                        break;
                    }

                    // Request the following items starting from the oldest item handled
                    updatedMethodUrl = String.Format(url + "&max_id={0}", maxId);
                    // Avoid to start from the last tweet already handled when we will go over the next loop
                    startLoop = 1;
                }
                catch (WebException wex)
                {
                    if (ExceptionHandler != null)
                    {
                        ExceptionHandler(wex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        #endregion

        #region Twitter Information

        public int GetRateLimit()
        {
            Dictionary<String, object> rateInfos = ExecuteGETQuery(Resources.QueryRateLimit);

            XRateLimit = Int32.Parse(rateInfos["hourly_limit"].ToString());
            XRateLimitResetTimeInSeconds = Int32.Parse(rateInfos["reset_time_in_seconds"].ToString());

            return XRateLimitRemaining;
        }

        #endregion

        #endregion

        #region Exception Handler

        public void ResetExceptionHandler()
        {
            IntegratedExceptionHandler = false;
        }

        #endregion
    }
}
