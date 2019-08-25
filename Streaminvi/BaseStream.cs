using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using TweetinCore.Interfaces;
using TweetinCore.Interfaces.StreamInvi;
using Tweetinvi;

namespace Streaminvi
{
    /// <summary>
    /// Base behavior of a Stream
    /// </summary>
    public abstract class BaseStream : IStream
    {
        protected readonly JavaScriptSerializer _jsSerializer;
        protected readonly StreamResultGenerator _streamResultGenerator;

        protected BaseStream()
        {
            _jsSerializer = new JavaScriptSerializer();
            _streamResultGenerator = new StreamResultGenerator();
        }

        /// <summary>
        /// Create a simple method to process objects created by the StreamResultGenerator
        /// </summary>
        /// <param name="processTweetDelegate">Delegate to be used for each new Tweet</param>
        protected Func<string, bool> DefaultObjectProcessor(Func<ITweet, bool> processTweetDelegate)
        {
            return delegate(string obj)
            {
                var jsonTweet = _jsSerializer.Deserialize<dynamic>(obj) as Dictionary<string, object>;

                if (jsonTweet != null)
                {
                    ITweet t = new Tweet(jsonTweet);
                    return processTweetDelegate(t);
                }

                // The information sent from Twitter was not the expected object
                return true;
            };
        }

        #region IStream Members
        
        public virtual void StartStream(IToken token, Action<ITweet> processTweetDelegate)
        {
            StartStream(token, x =>
            {
                processTweetDelegate(x);
                return true;
            });
        }

        public abstract void StartStream(IToken token, Func<ITweet, bool> processTweetDelegate);

        public void ResumeStream()
        {
            _streamResultGenerator.ResumeStream();
        }

        public void PauseStream()
        {
            _streamResultGenerator.PauseStream();
        }

        public void StopStream()
        {
            _streamResultGenerator.StopStream();
        } 

        #endregion
    }
}
