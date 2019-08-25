using System.Collections.Generic;
using System.Net;
using TweetinCore.Enum;

namespace TweetinCore.Interfaces.oAuth
{
    /// <summary>
    /// Generator of HttpWebRequest using OAuth specification
    /// </summary>
    public interface IOAuthWebRequestGenerator
    {
        /// <summary>
        /// Generates an HttpWebRequest by giving minimal information
        /// </summary>
        /// <param name="url">URL we expect to send/retrieve information to/from</param>
        /// <param name="httpMethod">HTTP Method for the request</param>
        /// <param name="parameters">Parameters used to generate the query</param>
        /// <returns>The appropriate WebRequest</returns>
        HttpWebRequest GenerateWebRequest(
            string url,
            HttpMethod httpMethod,
            IEnumerable<IOAuthQueryParameter> parameters);
    }
}
