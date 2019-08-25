using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TweetinCore.Interfaces;
using Tweetinvi.Utils;

namespace Tweetinvi.TwitterEntities
{
    /// <summary>
    /// Object storing information related with an URL on twitter
    /// </summary>
    public class UrlEntity : IUrlEntity
    {
        #region Private Attributes

        private string _url;
        private string _displayedUrl;
        private string _expanded_url;
        private IList<int> _indices;

        #endregion

        #region Public Attributes

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public string DisplayedUrl
        {
            get { return _displayedUrl; }
            set { _displayedUrl = value; }
        }

        public string ExpandedUrl
        {
            get { return _expanded_url; }
            set { _expanded_url = value; }
        }

        public IList<int> Indices
        {
            get { return _indices; }
            set { _indices = value; }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a URL without any information
        /// </summary>
        public UrlEntity() { }

        /// <summary>
        /// Creates a URL with information retrieved from Twitter
        /// </summary>
        /// <param name="urlEntity">Information retrieved from Twitter</param>
        public UrlEntity(Dictionary<String, object> urlEntity)
        {
            Url = urlEntity.GetProp("url") as string;
            DisplayedUrl = urlEntity.GetProp("display_url") as string;
            ExpandedUrl = urlEntity.GetProp("expanded_url") as string;
            Indices = new ObservableCollection<int>();

            object[] indices = urlEntity.GetProp("indices") as object[];
            if (indices != null)
            {
                foreach (int indice in indices)
                {
                    Indices.Add(indice);
                }
            }
        }
        #endregion
    }
}
