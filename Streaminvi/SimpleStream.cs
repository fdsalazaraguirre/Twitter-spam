using System;
using System.Collections.Generic;
using System.Net;
using Streaminvi;
using TweetinCore.Enum;
using TweetinCore.Interfaces;

namespace Tweetinvi
{
    /// <summary>
    /// Mehtods providing access to the Twitter stream API
    /// </summary>
    public class SimpleStream : BaseStream
    {
        private readonly string _streamURL;

        /// <summary>
        /// Constructor defining the delegate used each time a Tweet is retrieved from the Streaming API
        /// </summary>
        /// <param name="url">Url of the expected stream</param>
        public SimpleStream(string url)
        {
            _streamURL = url;
        }

        public override void StartStream(IToken token, Func<ITweet, bool> processTweetDelegate)
        {
            Func<HttpWebRequest> generateWebRequest = delegate
            {
                return token.GetQueryWebRequest(_streamURL, HttpMethod.GET);
            };

            Func<string, bool> generateTweetDelegate = x =>
            {
                var jsonTweet = _jsSerializer.Deserialize<dynamic>(x) as Dictionary<string, object>;

                if (jsonTweet != null)
                {
                    if (jsonTweet.ContainsKey("delete"))
                    {
                        return true;
                    }

                    ITweet t = new Tweet(jsonTweet);
                    return processTweetDelegate(t);
                }

                // The information sent from Twitter was not the expected object
                return true;
            };

            _streamResultGenerator.StartStream(generateTweetDelegate, generateWebRequest);
        }
    }
}
