using System.Collections.Generic;
using TweetinCore.Interfaces;
using Tweetinvi.Utils;

namespace Tweetinvi.TwitterEntities
{
    /// <summary>
    /// Object storing information related with media size on Twitter
    /// </summary>
    public class MediaEntitySize : IMediaEntitySize
    {
        #region Private Attributes

        private int? _w;
        private string _resize;
        private int? _h;

        #endregion

        #region Public Attributes
        public int? Width
        {
            get { return _w; }
            set { _w = value; }
        }

        public string Resize
        {
            get { return _resize; }
            set { _resize = value; }
        }

        public int? Height
        {
            get { return _h; }
            set { _h = value; }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates an Empty MediaSize
        /// </summary>
        public MediaEntitySize() { }

        /// <summary>
        /// Creates a media size from information retrived from Twitter
        /// </summary>
        /// <param name="mediaEntitySize">Information retrieved from Twitter</param>
        public MediaEntitySize(Dictionary<string, object> mediaEntitySize)
        {
            Width = mediaEntitySize.GetProp("w") as int?;
            Resize = mediaEntitySize.GetProp("resize") as string;
            Height = mediaEntitySize.GetProp("h") as int?;
        } 

        #endregion
    }
}