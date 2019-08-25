using System;
using System.Collections.Generic;
using TweetinCore.Interfaces;
using Tweetinvi.Utils;

namespace Tweetinvi
{
    /// <summary>
    /// Twitter mention
    /// </summary>
    public class Mention : Tweet, IMention
    {
        #region Private Attributes

        private string _annotations;
        
        #endregion

        #region Public Attributes
        
        public string Annotations
        {
            get { return _annotations; }
            set { _annotations = value; }
        }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Mention()
        {
            // Default constructor inheriting from the default Tweet constructor
        }

        #region Create Mentions from Tweet Constructors
        
        /// <summary>
        /// Create a Mention, if the Token is specified the mention is populated
        /// </summary>
        /// <param name="id">Mention id</param>
        /// <param name="token">Token to be used to perform query</param>
        /// <param name="cleanString">Does the string needs to be cleaned up</param>
        public Mention(long? id, IToken token = null, bool cleanString = true) 
            : base(id, token, cleanString)
        { }

        /// <summary>
        /// Create a Mention, if the Token is specified the mention is populated
        /// </summary>
        /// <param name="id">Mention id</param>
        /// <param name="token">Token to be used to perform query</param>
        /// <param name="cleanString">Does the string needs to be cleaned up</param>
        public Mention(long id, IToken token = null, bool cleanString = true)
            : this((long?)id, token, cleanString) { }

        #endregion

        #region Create Mention from object (inheriting from similar Tweet constructor)
        
        /// <summary>
        /// Create a mention from information retrieved from Twitter
        /// </summary>
        /// <param name="mentionObject">Information retrieved from Twitter</param>
        public Mention(Dictionary<string, object> mentionObject)
            : base(mentionObject)
        {
        }

        #endregion

        #region Private and Protected Methods

        /// <summary>
        /// Populate a mention from information retrieved 
        /// </summary>
        /// <param name="dTweet">Object containing Mention information</param>
        /// <param name="cleanString">Does the string needs to be cleanup when retrieved</param>
        protected override void Populate(Dictionary<String, object> dTweet, bool cleanString = true)
        {
            // initialize all the fields associated to the Tweet class
            base.Populate(dTweet, cleanString);

            if (Id != null)
            {
                // If Tweet initialization worked, initialize the fields associated to the Mention class
                _annotations = dTweet.GetProp("annotations") as string;
            }
        }

        #endregion

        #endregion
    }
}