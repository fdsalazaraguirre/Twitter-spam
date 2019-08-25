using System.Collections.Generic;

namespace TweetinCore.Interfaces
{
    /// <summary>
    /// A hashtag is a keyword prefixed by # and representing a category of tweet
    /// </summary>
    public interface IHashTagEntity
    {
        #region IHashTagEntity Properties

        /// <summary>
        /// HashTag name
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// The character positions the Hashtag was extracted from
        /// </summary>
        IList<int> Indices { get; set; } 

        #endregion
    }
}
