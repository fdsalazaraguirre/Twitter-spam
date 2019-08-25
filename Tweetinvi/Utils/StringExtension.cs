using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Tweetinvi.Utils
{
    /// <summary>
    /// Extension methods on string classes
    /// </summary>
    public static class StringExtension
    {
        private static Regex _linkParser;
        // Create on demand
        private static Regex LinkParser
        {
            get
            {
                
                if (_linkParser == null)
                {
                    _linkParser = new Regex(@"\b(?:http(?<isSecured>s?)://|www\.)\S+\b", RegexOptions.IgnoreCase);
                }

                return _linkParser;
            }
        }

        #region Extensions

        /// <summary>
        /// Calculate the number of characters remaining to post a Tweet
        /// </summary>
        /// <param name="tweet">Current Text</param>
        /// <returns>Remaining characters</returns>
        public static int TweetRemainingCharacters(string tweet)
        {
            return Tweet.MaxTweetSize - TweetLenght(tweet);
        }

        /// <summary>
        /// Calculate the length of a string using Twitter algorithm
        /// </summary>
        /// <param name="tweet">Text in the tweet</param>
        /// <returns>Size of the current Tweet</returns>
        public static int TweetLenght(string tweet)
        {
            if (tweet == null)
            {
                return 0;
            }

            int size = tweet.Length;

            foreach (Match link in LinkParser.Matches(tweet))
            {
                size = size - link.Value.Length + 22;
                size += link.Groups["isSecured"].Value == "s" ? 1 : 0;
            }

            return size;
        }

        /// <summary>
        /// Encode the input string so that it can be sent to Twitter
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TwitterEncode(this string s)
        {
            string encodedString = Uri.EscapeDataString(s);

            return encodedString;
        }

        /// <summary>
        /// Clean a string so that it can be used in a URL and
        /// sent to Twitter
        /// </summary>
        /// <param name="s">String to clean</param>
        /// <returns>Cleaned string</returns>
        public static string CleanString(this string s)
        {
            return s != null ? (s.HTMLDecode().MySQLClean().ReplaceNonPrintableCharacters('\\')) : null;
        }

        /// <summary>
        /// Decode a string formatted to be used within a url
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string HTMLDecode(this string s)
        {
            return WebUtility.HtmlDecode(s);
        }

        /// <summary>
        /// Give the ability to replace NonPrintableCharacters to another character
        /// This is very useful for streaming as we receive Tweets from around the world
        /// </summary>
        /// <param name="s">String to be updated</param>
        /// <param name="replaceWith">Character to replace by</param>
        /// <returns>String without any of the special characters</returns>
        public static string ReplaceNonPrintableCharacters(this string s, char replaceWith)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] > 51135)
                {
                    result.Append(replaceWith);
                }

                result.Append(s[i]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Clean the string so that it can be used as 
        /// a parameter of a MySql query
        /// </summary>
        /// <param name="s">String to be updated</param>
        /// <returns>String that can be used within MySql</returns>
        public static string MySQLClean(this string s)
        {
            if (s == null)
            {
                return null;
            }

            StringBuilder result = new StringBuilder(s);

            return Regex.Replace(result.Replace("\\", "\\\\").ToString(), "['′ˈ]", "\\'");
        }

        #endregion
    }
}
