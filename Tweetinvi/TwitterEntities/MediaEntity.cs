using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TweetinCore.Interfaces;
using Tweetinvi.Utils;

namespace Tweetinvi.TwitterEntities
{
    /// <summary>
    /// Object storing information related with a Media on Twitter
    /// </summary>
    public class MediaEntity : IMediaEntity
    {
        #region Private Attributes

        /// <summary>
        /// Media Id
        /// </summary>
        protected long? _id;

        /// <summary>
        /// Media Id as a string
        /// </summary>
        protected string _idStr;

        /// <summary>
        /// Url of the media
        /// </summary>
        protected string _mediaURL;

        /// <summary>
        /// Secured Url of the media
        /// </summary>
        protected string _mediaURLHttps;

        /// <summary>
        /// URL properties provide information related with a URL.
        /// For example it stores both a shorten URL and the real
        /// location of a Media
        /// </summary>
        protected IUrlEntity _url;

        /// <summary>
        /// Type of Media
        /// </summary>
        private string _type;

        /// <summary>
        /// Dimensions related with the different possible views of 
        /// a same Media element
        /// </summary>
        private IDictionary<String, IMediaEntitySize> _sizes;

        #endregion

        #region Public Attributes


        public long? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string IdStr
        {
            get { return _idStr; }
            set { _idStr = value; }
        }

        public string MediaURL
        {
            get { return _mediaURL; }
            set { _mediaURL = value; }
        }

        public string MediaURLHttps
        {
            get { return _mediaURLHttps; }
            set { _mediaURLHttps = value; }
        }

        public IUrlEntity URLProperties
        {
            get { return _url; }
            set { _url = value; }
        }

        public string MediaType
        {
            get { return _type; }
            set { _type = value; }
        }

        public IDictionary<String, IMediaEntitySize> Sizes
        {
            get { return _sizes; }
            set { _sizes = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Empty Media Object
        /// </summary>
        public MediaEntity() { }

        /// <summary>
        /// Creates a media object based on information retrieved from Twitter
        /// </summary>
        /// <param name="mediaEntity">Information retrieved from Twitter</param>
        public MediaEntity(Dictionary<String, object> mediaEntity)
        {
            Id = mediaEntity.GetProp("id") as long?;
            IdStr = mediaEntity.GetProp("id_str") as string;
            MediaURL = mediaEntity.GetProp("media_url") as string;
            MediaURLHttps = mediaEntity.GetProp("media_url_https") as string;

            ObservableCollection<int> indices = new ObservableCollection<int>();

            var indicesInfo = mediaEntity.GetProp("indices") as object[];
            if (indicesInfo != null)
            {
                foreach (int indice in indicesInfo)
                {
                    indices.Add(indice);
                }
            }

            URLProperties = new UrlEntity()
            {
                Url = mediaEntity.GetProp("url") as string,
                DisplayedUrl = mediaEntity.GetProp("display_url") as string,
                ExpandedUrl = mediaEntity.GetProp("expanded_url") as string,
                Indices = indices
            };

            MediaType = mediaEntity.GetProp("type") as string;

            Sizes = new Dictionary<string, IMediaEntitySize>();
            Dictionary<String, object> tmpSizes = mediaEntity.GetProp("sizes") as Dictionary<String, object>;

            if (tmpSizes != null)
            {
                foreach (object size in tmpSizes)
                {
                    KeyValuePair<String, object> pair = (KeyValuePair<String, object>)size;
                    Sizes.Add(pair.Key, new MediaEntitySize(pair.Value as Dictionary<String, object>));
                }
            }
        }
        
        #endregion
    }
}
