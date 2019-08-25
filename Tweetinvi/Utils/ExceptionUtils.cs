using System;
using System.Linq;
using System.Net;

namespace Tweetinvi.Utils
{
    /// <summary>
    /// Extension methods for Exception
    /// </summary>
    public static class ExceptionUtils
    {
        /// <summary>
        /// Provide the exception status number of a WebException
        /// </summary>
        /// <param name="wex">WebException</param>
        /// <returns>Status Number</returns>
        public static int? GetWebExceptionStatusNumber(this WebException wex)
        {
            if (wex == null)
            {
                return null;
            }

            int indexOfStatus = wex.Response.Headers.AllKeys.ToList().IndexOf("status");

            if (indexOfStatus == -1)
            {
                indexOfStatus = wex.Response.Headers.AllKeys.ToList().IndexOf("Status");
            }

            if (indexOfStatus == -1)
            {
                return null;
            }

            string statusValue = wex.Response.Headers.Get(indexOfStatus);
            char[] t = new[] { ' ' };
            string[] statusContent = statusValue.Split(t);

            if (statusContent.Length > 0)
            {
                return Convert.ToInt32(statusContent[0]);
            }

            return null;
        }
    }
}
