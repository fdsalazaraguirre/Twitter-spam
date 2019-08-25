using System.Collections.Generic;
using System.Net;
using TweetinCore.Enum;
using TweetinCore.Events;

namespace TweetinCore.Interfaces.oAuth
{
    /// <summary>
    /// Generate a Token that can be used to perform OAuth queries
    /// </summary>
    public interface IOAuthToken
    {
        /// <summary>
        /// Generate a collection of parameters based on the credentials it hosts
        /// </summary>
        /// <returns></returns>
        IEnumerable<IOAuthQueryParameter> GenerateParameters();

        /// <summary>
        /// Get the HttpWebRequest expected from the given parameters
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
        /// Get and send the result of a WebRequest
        /// </summary>
        /// <param name="url">URL we expect to send/retrieve information to/from</param>
        /// <param name="httpMethod">HTTP Method for the request</param>
        /// <param name="exceptionHandler">Exception Handler called when an excpetion occurs</param>
        /// <returns>The appropriate WebRequest</returns>
        string ExecuteQuery(
            string url, 
            HttpMethod httpMethod, 
            WebExceptionHandlingDelegate exceptionHandler);

        /// <summary>
        /// Get the HttpWebRequest expected from the given parameters
        /// </summary>
        /// <param name="url">URL we expect to send/retrieve information to/from</param>
        /// <param name="httpMethod">HTTP Method for the request</param>
        /// <param name="exceptionHandler">Exception Handler called when an excpetion occurs</param>
        /// <param name="parameters">Parameters used to generate the query</param>
        /// <returns>The appropriate WebRequest</returns>
        string ExecuteQueryWithSpecificParameters(
            string url,
            HttpMethod httpMethod,
            WebExceptionHandlingDelegate exceptionHandler,
            IEnumerable<IOAuthQueryParameter> parameters);
    }
}
