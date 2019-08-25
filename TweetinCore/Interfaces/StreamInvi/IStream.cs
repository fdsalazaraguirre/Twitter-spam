using System;

namespace TweetinCore.Interfaces.StreamInvi
{
    /// <summary>
    /// Set of methods to control and manage a stream
    /// </summary>
    public interface IStream
    {
        /// <summary>
        /// Start an infinite stream (can be stopped from PauseStream/StopStream)
        /// </summary>
        /// <param name="token">Token against which the stream is using the API</param>
        /// <param name="processTweetDelegate">Method that will be called foreach object</param>
        void StartStream(IToken token, Action<ITweet> processTweetDelegate);

        /// <summary>
        /// Start a stream that you can stop from the processTweetDelegate
        /// </summary>
        /// <param name="token">Token against which the stream is using the API</param>
        /// <param name="processTweetDelegate">
        /// Method that will be called foreach object 
        /// returns whether the stream should continue
        /// </param>
        void StartStream(IToken token, Func<ITweet, bool> processTweetDelegate);
     
        /// <summary>
        /// Resume a stopped Stream
        /// </summary>
        void ResumeStream();

        /// <summary>
        /// Pause a running Stream
        /// </summary>
        void PauseStream();

        /// <summary>
        /// Stop a running or paused stream
        /// </summary>
        void StopStream();
    }
}
