using System;
using System.Collections.Generic;
using System.Linq;
using TweetinCore.Interfaces;
using Tweetinvi.Utils;

namespace Tweetinvi.TwitterEntities
{
    /// <summary>
    /// Class storing multiple types of TweetEntities
    /// https://dev.twitter.com/docs/tweet-entities
    /// </summary>
    public class TweetEntities : ITweetEntities
    {
        #region Private Attributes

        /// <summary>
        /// Collection of urls associated with a tweet
        /// </summary>
        private List<IUrlEntity> _urls;

        /// <summary>
        /// Collection of medias associated with a tweet
        /// </summary>
        private List<IMediaEntity> _medias;

        /// <summary>
        /// Collection of tweets mentioning this tweet
        /// </summary>
        private List<IUserMentionEntity> _userMentions;

        /// <summary>
        /// Collection of hashtags associated with a Tweet
        /// </summary>
        private List<IHashTagEntity> _hashtags;
        
        #endregion

        #region Public Attributes
        public List<IUrlEntity> Urls
        {
            get { return _urls; }
            set
            {
                _urls = value;
            }
        }

        public List<IMediaEntity> Medias
        {
            get { return _medias; }
            set
            {
                _medias = value;
            }
        }

        public List<IUserMentionEntity> UserMentions
        {
            get { return _userMentions; }
            set { _userMentions = value; }
        }

        public List<IHashTagEntity> Hashtags
        {
            get { return _hashtags; }
            set { _hashtags = value; }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates an empty store of Entities
        /// </summary>
        public TweetEntities() { }

        /// <summary>
        /// Creates a store of entities based on information retrieved from Twitter
        /// </summary>
        /// <param name="entities">Twitter information</param>
        public TweetEntities(Dictionary<String, object> entities)
        {
            Urls = entities.GetProp("urls") != null ?
                (from x in (entities.GetProp("urls") as object[])
                 select new UrlEntity(x as Dictionary<String, object>) as IUrlEntity)
                 .ToList() : null;

            Medias = entities.GetProp("media") != null ?
                (from x in (entities.GetProp("media") as object[])
                 select new MediaEntity(x as Dictionary<String, object>) as IMediaEntity)
                 .ToList() : null;

            UserMentions = entities.GetProp("user_mentions") != null ?
                (from x in (entities.GetProp("user_mentions") as object[])
                 select new UserMentionEntity(x as Dictionary<String, object>) as IUserMentionEntity)
                 .ToList() : null;

            Hashtags = entities.GetProp("hashtags") != null ?
                (from x in (entities.GetProp("hashtags") as object[])
                 select new HashTagEntity(x as Dictionary<String, object>) as IHashTagEntity)
                 .ToList() : null;
        } 
        #endregion
    }
}
