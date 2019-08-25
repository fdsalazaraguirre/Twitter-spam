using System;
using System.Linq;
using System.Text;

namespace oAuthConnection.Utils
{
    /// <summary>
    /// Class providing methods to format a string
    /// </summary>
    internal class StringFormater
    {
        private const string UnreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        /// <summary>
        /// Clean a string so that it can be used in an URL
        /// </summary>
        /// <param name="str">string to clean</param>
        /// <returns>Cleaned string that can be added into an URL</returns>
        public static string UrlEncode(string str)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in str)
            {
                if (UnreservedChars.Contains(c))
                {
                    result.Append(c);
                }
                else
                {
                    result.Append('%' + String.Format("{0:X2}", (int)c));
                }
            }

            return result.ToString();
        }
    }
}
