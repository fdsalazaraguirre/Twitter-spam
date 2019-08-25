using System;
using System.Collections.Generic;
using System.Net;
using TweetinCore.Enum;
using TweetinCore.Events;
using TweetinCore.Interfaces.oAuth;

namespace TweetinCore.Interfaces
{
    /// <summary>
    /// Set of methods allowing a User to execute queries with specific credentials
    /// </summary>
    public interface IToken : ITokenCredentials
    {
        #region Properties

        /// <summary>
        /// Credentials that can be required to perform queries on Twitter
        /// </summary>
        ITokenCredentials TwitterCredentials { get; set; }

        /// <summary>
        /// Query limit of the Token
        /// </summary>
        int XRateLimit { get; set; }

        /// <summary>
        /// Remaining queries the Token can perform before
        /// Twitter blocks it
        /// </summary>
        int XRateLimitRemaining { get; set; }

        /// <summary>
        /// DateTime when the IToken limit will be reset
        /// </summary>
        DateTime XRateLimitResetTime { get; set; }

        /// <summary>
        /// Duration in seconds before the IToken limit is reset
        /// </summary>
        int XRateLimitResetTimeInSeconds { get; set; }

        /// <summary>
        /// Are the exceptions handled by the integrated ExceptionHandler
        /// </summary>
        bool IntegratedExceptionHandler { get; set; }

        /// <summary>
        /// Exception Handler "overriding" any Exception Handler
        /// It is called if the IntegratedExceptionHandler is activated only
        /// </summary>
        WebExceptionHandlingDelegate ExceptionHandler { get; set; } 

        #endregion

        #region RateLimit
        
        /// <summary>
        /// Get Information concerning the Token querying limitations
        /// </summary>
        /// <returns>Number of queries available</returns>
        int GetRateLimit(); 
        
        #endregion

        #region Classic Query
        
        /// <summary>
        /// Generate a HttpWebResponse to enable Twitter connection
        /// </summary>
        /// <param name="url">URL we expect to send/retrieve information to/from</param>
        /// <param name="httpMethod">HTTP Method for the request</param>
        /// <param name="parameters">Parameters used to generate the query</param>
        /// <returns>The appropriate WebRequest</returns>
        HttpWebRequest GetQueryWebRequest(
            string url,
            HttpMethod httpMethod,
            IEnumerable<IOAuthQueryParameter> parameters = null);

        /// <summary>
        /// Method allowing to query an url
        /// </summary>
        /// <param name="url">URL we expect to send/retrieve information to/from</param>
        /// <param name="objectDeletage">Method used foreach of the result element</param>
        /// <param name="exceptionHandler">Method used to manage an Exception</param>
        /// <returns>
        /// Return a dynamic object containing the json information 
        /// representing the value from a json response
        /// </returns>
        Dictionary<string, object> ExecuteGETQuery(
            string url,
            ObjectResponseDelegate objectDeletage = null,
            WebExceptionHandlingDelegate exceptionHandler = null);

        /// <summary>
        /// Method allowing to query and post information to an url
        /// </summary>
        /// <param name="url">Url from Twitter Api to query</param>
        /// <param name="exceptionHandler">Method used to manage an Exception</param>
        /// <param name="objectDeletage">Method used foreach of the result element</param>
        /// <returns>
        /// Return a dynamic object containing the json information 
        /// representing the value from a json response
        /// </returns>
        Dictionary<string, object> ExecutePOSTQuery(
            string url,
            ObjectResponseDelegate objectDeletage = null,
            WebExceptionHandlingDelegate exceptionHandler = null);

        /// <summary>
        /// Method allowing to execute a GET query an url and retrieve multiple objects
        /// </summary>
        /// <param name="url">URL we expect to send/retrieve information to/from</param>
        /// <param name="objectDeletage">Method used foreach of the result element</param>
        /// /// <param name="exceptionHandler">Method used to manage an Exception</param>
        /// <returns>
        /// Return a dynamic object containing the json information 
        /// representing the value from a json response
        /// </returns>
        Dictionary<string, object>[] ExecuteGETQueryReturningCollectionOfObjects(
            string url,
            ObjectResponseDelegate objectDeletage = null,
            WebExceptionHandlingDelegate exceptionHandler = null);

        /// <summary>
        /// Method allowing to execute a POST query an url and retrieve multiple objects
        /// </summary>
        /// <param name="url">URL we expect to send/retrieve information to/from</param>
        /// <param name="objectDeletage">Method used foreach of the result element</param>
        /// /// <param name="exceptionHandler">Method used to manage an Exception</param>
        /// <returns>
        /// Return a dynamic object containing the json information 
        /// representing the value from a json response
        /// </returns>
        Dictionary<string, object>[] ExecutePOSTQueryReturningCollectionOfObjects(
            string url,
            ObjectResponseDelegate objectDeletage = null,
            WebExceptionHandlingDelegate exceptionHandler = null); 
        
        #endregion

        #region Cursor Query
        
        /// <summary>
        /// Execute a query that requires a cursor to iterate on the results
        /// </summary>
        /// <param name="url">Url from Twitter Api to query</param>
        /// <param name="cursorDelegate">
        /// Method to call each time we are getting a new cursor
        /// (each time a query is called)
        /// </param>
        /// <returns>Resulting objects</returns>
        Dictionary<string, object> ExecuteCursorQuery(
            string url,
            DynamicResponseDelegate cursorDelegate);

        /// <summary>
        /// Execute a query that requires a cursor to iterate on the results
        /// </summary>
        /// <param name="url">Url from Twitter Api to query</param>
        /// <param name="cursor">Cursor where we want to start the query</param>
        /// <param name="max">Maximum number of objects to retrieve</param>
        /// <param name="cursorDelegate">
        /// Method to call each time we are getting a new cursor
        /// (each time a query is called)
        /// </param>
        /// <param name="exceptionHandler">Method used to manage an Exception</param>
        /// <returns>Resulting objects</returns>
        Dictionary<string, object> ExecuteCursorQuery(
            string url,
            long cursor = 0,
            int max = Int32.MaxValue,
            DynamicResponseDelegate cursorDelegate = null,
            WebExceptionHandlingDelegate exceptionHandler = null); 

        #endregion

        #region SinceMax Query
        
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
        void ExecuteSinceMaxQuery(
            string url,
            ObjectResponseDelegate sinceMaxDelegate,
            WebExceptionHandlingDelegate webExceptionHandlerDelegate); 
        
        #endregion

        #region Exception Handler
        
        /// <summary>
        /// Reset the Exception Management of the Token
        /// </summary>
        void ResetExceptionHandler(); 
        
        #endregion
    }
}
