using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TweetinCore.Interfaces;
using Tweetinvi.Utils;

namespace Tweetinvi.TwitterEntities
{
    /// <summary>
    /// A hashtag is a keyword prefixed by # and representing a category of tweet
    /// This class stores information related with an user mention
    /// </summary>
    public class HashTagEntity : IHashTagEntity
    {
        #region Private Attributes

        /// <summary>
        /// HashTag name
        /// </summary>
        private string _text;

        /// <summary>
        /// The character positions the Hashtag was extracted from
        /// </summary>
        private IList<int> _indices;

        #endregion

        #region Public Attributes

        /// <summary>
        /// See Interface
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// See Interface
        /// </summary>
        public IList<int> Indices
        {
            get { return _indices; }
            set { _indices = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an empty Hashtag entity
        /// </summary>
        public HashTagEntity() { }

        /// <summary>
        /// Creates an Hashtag entity based on information retrieved from twitter
        /// </summary>
        /// <param name="hashTagEntity">Information retrieved from Twitter</param>
        public HashTagEntity(Dictionary<String, object> hashTagEntity)
        {
            Text = hashTagEntity.GetProp("text") as string;
            Indices = new ObservableCollection<int>();

            var hashTagIndices = hashTagEntity.GetProp("indices") as object[];

            if (hashTagIndices != null)
            {
                foreach (int indice in hashTagIndices)
                {
                    Indices.Add(indice);
                }
            }
        }

        #endregion
    }
}
