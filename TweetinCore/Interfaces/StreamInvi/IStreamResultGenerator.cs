using System;
using System.Net;
using Tweetinvi.Utils;

namespace TweetinCore.Interfaces.StreamInvi
{
    /// <summary>
    /// Set of methods to extract objects from any kind of stream
    /// </summary>
    public interface IStreamResultGenerator
    {
        /// <summary>
        /// Current state of the stream analysis
        /// </summary>
        StreamState StreamState { get; }

        /// <summary>
        /// Start extracting objects from the stream
        /// </summary>
        /// <param name="processTweet">Method to call foreach object</param>
        /// <param name="generateWebRequest">How to generate the WebRequest to access the stream</param>
        void StartStream(Func<string, bool> processTweet, Func<HttpWebRequest> generateWebRequest);

        /// <summary>
        /// Run the stream
        /// </summary>
        void ResumeStream();

        /// <summary>
        /// Pause the stream
        /// </summary>
        void PauseStream();

        /// <summary>
        /// Stop the stream
        /// </summary>
        void StopStream();
    }
}
