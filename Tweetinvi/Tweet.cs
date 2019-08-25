using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using TweetinCore.Events;
using TweetinCore.Interfaces;
using Tweetinvi.Properties;
using Tweetinvi.TwitterEntities;
using Tweetinvi.Utils;

namespace Tweetinvi
{
    /// <summary>
    /// Class representing a Tweet
    /// https://dev.twitter.com/docs/api/1/get/statuses/show/%3Aid
    /// </summary>
    [Serializable]
    public class Tweet : TwitterObject, ICloneable, ITweet
    {
        #region Private Attributes

        public const Int16 MaxTweetSize = 140;

        #region Twitter API Attributes

        private DateTime _created_at;
        private long? _id;
        private string _id_str;
        private string _text;
        private string _source;
        private bool? _truncated;
        private long? _in_reply_to_status_id;
        private string _in_reply_to_status_id_str;
        private long? _in_reply_to_user_id;
        private string _in_reply_to_user_id_str;
        private string _in_reply_to_screen_name;
        private IUser _user;
        private IGeo _geo;

        // Implement Coordinates
        private ICoordinates _coordinates;

        // Implement Place
        private string _place;

        // Implement Contributors
        private int[] _contributors;

        private int? _retweet_count;
        private ITweetEntities _entities;
        private bool? _favourited;
        private bool? _retweeted;
        private bool? _possibly_sensitive;

        #endregion

        #region Tweetinvi API Attributes

        private List<ITweet> _retweets;

        private bool _isTweetPublished;
        private bool _isTweetDestroyed;

        #endregion

        #endregion

        #region Public Attributes

        #region Twitter API Attributes

        public DateTime CreatedAt
        {
            get { return _created_at; }
            set
            {
                if (_created_at != value)
                {
                    _created_at = value;
                    OnPropertyChanged("created_at");
                }
            }
        }

        public long? Id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }

        public long? IdValue { get; private set; }

        public string IdStr
        {
            get { return _id_str; }
            set { _id_str = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        public bool? Truncated
        {
            get { return _truncated; }
            set { _truncated = value; }
        }

        public long? InReplyToStatusId
        {
            get { return _in_reply_to_status_id; }
            set { _in_reply_to_status_id = value; }
        }

        public string InReplyToStatusIdStr
        {
            get { return _in_reply_to_status_id_str; }
            set { _in_reply_to_status_id_str = value; }
        }

        public long? InReplyToUserId
        {
            get { return _in_reply_to_user_id; }
            set { _in_reply_to_user_id = value; }
        }

        public string InReplyToUserIdStr
        {
            get { return _in_reply_to_user_id_str; }
            set { _in_reply_to_user_id_str = value; }
        }

        public string InReplyToScreenName
        {
            get { return _in_reply_to_screen_name; }
            set { _in_reply_to_screen_name = value; }
        }

        public IUser Creator
        {
            get { return _user; }
            set { _user = value; }
        }

        public IGeo Location
        {
            get { return _geo; }
            set { _geo = value; }
        }

        public ICoordinates LocationCoordinates
        {
            get { return _coordinates; }
            set { _coordinates = value; }
        }

        public int[] ContributorsIds
        {
            get { return _contributors; }
            set { _contributors = value; }
        }

        public int? RetweetCount
        {
            get { return _retweet_count; }
            set { _retweet_count = value; }
        }

        public ITweetEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public bool? Favourited
        {
            get { return _favourited; }
            set
            {
                SetFavourite(value);
            }
        }

        public bool? Retweeted
        {
            get { return _retweeted; }
            set { _retweeted = value; }
        }

        public bool? PossiblySensitive
        {
            get { return _possibly_sensitive; }
            set { _possibly_sensitive = value; }
        }

        //public DateTime tweet_creation_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        //public long tweet_id { get; set; }
        //public string tweet_date { get; set; }
        //public string tweet_user { get; set; }
        //public string tweet_user_login { get; set; }
        //public string tweet_text { get; set; }
        //public string tweet_place { get; set; }
        //public string tweet_language { get; set; }
        #endregion

        #region Tweetinvi API Accessors

        public List<IHashTagEntity> Hashtags
        {
            get
            {
                if (Entities != null)
                {
                    return Entities.Hashtags;
                }

                return null;
            }

            set
            {
                if (Entities != null)
                {
                    Entities.Hashtags = value;
                }
            }
        }

        public List<IUrlEntity> Urls
        {
            get
            {
                if (Entities != null)
                {
                    return Entities.Urls;
                }

                return null;
            }

            set
            {
                if (Entities != null)
                {
                    Entities.Urls = value;
                }
            }
        }

        public List<IMediaEntity> Media
        {
            get
            {
                if (Entities != null)
                {
                    return Entities.Medias;
                }

                return null;
            }

            set
            {
                if (Entities != null)
                {
                    Entities.Medias = value;
                }
            }
        }

        public List<IUserMentionEntity> UserMentions
        {
            get
            {
                if (Entities != null)
                {
                    return Entities.UserMentions;
                }

                return null;
            }

            set
            {
                if (Entities != null)
                {
                    Entities.UserMentions = value;
                }
            }
        }

        #endregion

        #region Tweetinvi API Attributes

        public int Length
        {
            get { return _text == null ? 0 : StringExtension.TweetLenght(_text); }
        }

        public bool IsTweetPublished
        {
            get { return _isTweetPublished; }
            set { _isTweetPublished = value; }
        }

        public bool IsTweetDestroyed
        {
            get { return _isTweetDestroyed; }
        }

        public DateTime TweetCreationDate = DateTime.Now;

        public List<ITweet> Retweets
        {
            get { return _retweets; }
            set { _retweets = value; }
        }

        public ITweet Retweeting { get; private set; }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Tweet(bool shareTokenWithChild = true)
        {
            // Default constructor that should be called by all constructors
            // Basic initilization

            _isTweetDestroyed = false;
            _shareTokenWithChild = shareTokenWithChild;
        }

        #region Create Tweet from Id

        /// <summary>
        /// Create a Tweet and retrieve the propreties through given token
        /// </summary>
        /// <param name="id">TweetId</param>
        /// <param name="token">Token saved in class propreties</param>
        /// <param name="cleanString">Should the text be cleanup when retrieved</param>
        /// <param name="shareTokenWithChild">Token shared accross the hosted TwitterObjects</param>
        public Tweet(long? id, IToken token = null, bool cleanString = true, bool shareTokenWithChild = true)
            : this(shareTokenWithChild)
        {
            if (id == null)
            {
                throw new Exception("id cannot be null!");
            }

            _id = id;
            _id_str = id.ToString();

            // Every tweet created from an id is considered being
            // a tweet that will be retrieved from Twitter
            _isTweetPublished = true;

            _token = token;

            if (GetQueryTokenForConstructor(token) != null)
            {
                PopulateTweet(_token, cleanString);
            }
        }

        /// <summary>
        /// Create a Tweet and retrieve the propreties through given token
        /// </summary>
        /// <param name="id">TweetId</param>
        /// <param name="token">Token saved in class propreties</param>
        /// <param name="cleanString">Should the text be cleanup when retrieved</param>
        /// <param name="shareTokenWithChild">Token shared accross the hosted TwitterObjects</param>
        public Tweet(long id, IToken token = null, bool cleanString = true, bool shareTokenWithChild = true)
            : this((long?)id, token, cleanString, shareTokenWithChild) { }

        #endregion

        #region Create Tweet from dynamic response

        /// <summary>
        /// Create a user from information retrieved from Twitter
        /// </summary>
        /// <param name="tweetObject">Information retrieved from Twitter</param>
        /// <param name="token">Token saved in class propreties</param>
        /// <param name="shareTokenWithChild">Shall the token be shared to objects created from the user</param>
        public Tweet(Dictionary<string, object> tweetObject, 
            IToken token = null, 
            bool shareTokenWithChild = true)
            : this(shareTokenWithChild)
        {
            if (tweetObject.GetProp("id") != null)
            {
                Populate(tweetObject);
                _token = token;
            }
            else
            {
                throw new InvalidOperationException("Cannot create 'Tweet' if id does not exist");
            }
        }

        #endregion

        #region Create a Tweet to be send

        /// <summary>
        /// Create a basic Tweet
        /// </summary>
        /// <param name="text">Text of the Tweet</param>
        /// <param name="token">Token saved to send the Tweet</param>
        /// <param name="shareTokenWithChild">Token shared accross the hosted TwitterObjects</param>
        public Tweet(string text, IToken token = null, bool shareTokenWithChild = true)
            : this(shareTokenWithChild)
        {
            int tweetLength = StringExtension.TweetLenght(text);

            // Reducing the size of the Tweet until it can be posted in Twitter
            while (tweetLength > MaxTweetSize)
            {
                text = text.Substring(0, MaxTweetSize);
                tweetLength = StringExtension.TweetLenght(text);
            }

            _text = text;
            _isTweetPublished = false;
            _favourited = false;
            _token = token;
        }

        /// <summary>
        /// Create a Tweet in reply to another Tweet
        /// </summary>
        /// <param name="text">Text of the Tweet</param>
        /// <param name="replyToTweet">Tweet object to be replied to</param>
        /// <param name="token">Token saved to send the Tweet</param>
        /// <param name="shareTokenWithChild">Token shared accross the hosted TwitterObjects</param>
        public Tweet(string text, Tweet replyToTweet, IToken token = null, bool shareTokenWithChild = true)
            : this(text, token, shareTokenWithChild)
        {
            if (replyToTweet == null || replyToTweet.Id == null)
            {
                throw new Exception("Cannot reply to an email not published!");
            }

            _in_reply_to_status_id = replyToTweet.Id;
        }

        /// <summary>
        /// Create a Tweet in reply to another Tweet
        /// </summary>
        /// <param name="text">Text of the Tweet</param>
        /// <param name="replyToTweetId">Tweet object to be replied to</param>
        /// <param name="token">Token saved to send the Tweet</param>
        /// <param name="shareTokenWithChild">Token shared accross the hosted TwitterObjects</param>
        public Tweet(string text, long replyToTweetId, IToken token = null, bool shareTokenWithChild = true)
            : this(text, token, shareTokenWithChild)
        {
            _in_reply_to_status_id = replyToTweetId;
        }

        #endregion

        #endregion

        #region Public Methods

        #region Populate Tweet

        /// <summary>
        /// Populate the information of Tweet for which we set the ID
        /// </summary>
        public void PopulateTweet()
        {
            PopulateTweet(_token);
        }

        /// <summary>
        /// Populate the information of Tweet for which we set the ID
        /// </summary>
        /// <param name="token">Token to perform the query</param>
        /// <param name="cleanString">Clean the string so that it is readalble</param>
        public void PopulateTweet(IToken token, bool cleanString = true)
        {
            token = GetQueryToken(token);

            if (token != null)
            {
                string query;

                if (_id != null)
                {
                    query = String.Format(Resources.Tweet_GetFromIdWithEntities, Id);
                }
                else
                {
                    throw new Exception("Id needs to be set to retrieve it from Twitter.");
                }

                // If 404 error throw Exception that Tweet has not been created

                WebExceptionHandlingDelegate wex = delegate(WebException exception)
                    {
                        int indexOfStatus = exception.Response.Headers.AllKeys.ToList().IndexOf("status");

                        if (indexOfStatus != -1)
                        {
                            string statusValue = exception.Response.Headers.Get(indexOfStatus);

                            // The tweet with the specific Id does not exist
                            if (statusValue == "404 Not Found")
                            {
                                // Throwing an exception will stop the creation of the Tweet
                                throw new Exception(String.Format("Tweet[{0}] does not exist!", Id));
                            }
                        }
                    };

                Dictionary<String, object> dynamicTweet = token.ExecuteGETQuery(query, null, wex);

                Populate(dynamicTweet, cleanString);
                _token = token;
            }
        }

        /// <summary>
        /// Populate a Tweet from information retrieved from Twitter
        /// </summary>
        /// <param name="dTweet">Information retrieved from Twitter</param>
        public override void Populate(Dictionary<String, object> dTweet)
        {
            Populate(dTweet, true);
        }

        #endregion

        #region Publish Tweet

        public bool Publish(IToken token = null, bool getUserInfo = false)
        {
            string query = String.Format(Resources.Tweet_Publish, _text.TwitterEncode(), getUserInfo);

            return Publish(token, query);
        }

        /// <summary>
        /// Publish a Tweet created from the API
        /// </summary>
        /// <param name="replyToTweetId">Tweet id of the tweet we reply to</param>
        /// <param name="token">Token to be used to send the Tweet</param>
        /// <param name="getUserInfo">
        /// Whether we want to have all information related with the user in the response
        /// </param>
        /// <returns>Whether the Tweet has successfully been sent</returns>
        public bool PublishInReplyTo(long? replyToTweetId, IToken token = null, bool getUserInfo = false)
        {
            if (replyToTweetId == null)
            {
                return false;
            }

            string query = String.Format(Resources.Tweet_PublishInReplyTo, _text.TwitterEncode(), getUserInfo, replyToTweetId);

            return Publish(token, query);
        }

        /// <summary>
        /// Publish a Tweet created from the API
        /// </summary>
        /// <param name="replyToTweet">Tweet we reply to</param>
        /// <param name="token">Token to be used to send the Tweet</param>
        /// <param name="getUserInfo">
        /// Whether we want to have all information related with the user in the response
        /// </param>
        /// <returns>Whether the Tweet has successfully been sent</returns>
        public bool PublishInReplyTo(ITweet replyToTweet, IToken token = null, bool getUserInfo = false)
        {
            if (replyToTweet == null || replyToTweet.Id == null)
            {
                return false;
            }

            return PublishInReplyTo(replyToTweet.Id, token, getUserInfo);
        }

        /// <summary>
        /// Publish a Tweet created from the API
        /// </summary>
        /// <param name="latitude">Lattitude</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="displayCoordinates">Display the coordinates on Twitter</param>
        /// <param name="token">Token to be used to send the Tweet</param>
        /// <param name="getUserInfo">
        /// Whether we want to have all information related with the user in the response
        /// </param>
        /// <returns>Whether the Tweet has successfully been sent</returns>
        public bool PublishWithGeo(double latitude,
            double longitude,
            bool displayCoordinates = true,
            IToken token = null,
            bool getUserInfo = false)
        {
            string queryWithParameters = String.Format(Resources.Tweet_PublishWithGeo,
                    _text.TwitterEncode(), getUserInfo, latitude, longitude, displayCoordinates);

            return Publish(token, queryWithParameters);
        }

        /// <summary>
        /// Publish a Tweet created from the API
        /// </summary>
        /// <param name="latitude">Lattitude</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="displayCoordinates">Display the coordinates on Twitter</param>
        /// <param name="token">Token to be used to send the Tweet</param>
        /// <param name="replyToTweetId">Tweet id of the tweet we reply to</param>
        /// <param name="getUserInfo">
        /// Whether we want to have all information related with the user in the response
        /// </param>
        /// <returns>Whether the Tweet has successfully been sent</returns>
        public bool PublishWithGeoInReplyTo(double latitude,
            double longitude,
            long? replyToTweetId,
            bool displayCoordinates = true,
            IToken token = null,
            bool getUserInfo = false)
        {
            if (replyToTweetId == null)
            {
                return false;
            }

            string queryWithParameters = String.Format(Resources.Tweet_PublishWithGeoInReplyTo,
                    _text.TwitterEncode(), getUserInfo, latitude, longitude, displayCoordinates, replyToTweetId);

            return Publish(token, queryWithParameters);
        }

        /// <summary>
        /// Publish a Tweet created from the API
        /// </summary>
        /// <param name="latitude">Lattitude</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="displayCoordinates">Display the coordinates on Twitter</param>
        /// <param name="token">Token to be used to send the Tweet</param>
        /// <param name="replyToTweet">Tweet we want reply to</param>
        /// <param name="getUserInfo">
        /// Whether we want to have all information related with the user in the response
        /// </param>
        /// <returns>Whether the Tweet has successfully been sent</returns>
        public bool PublishWithGeoInReplyTo(double latitude,
            double longitude,
            ITweet replyToTweet,
            bool displayCoordinates = true,
            IToken token = null,
            bool getUserInfo = false)
        {
            if (replyToTweet == null || replyToTweet.Id == null)
            {
                return false;
            }

            string queryWithParameters = String.Format(Resources.Tweet_PublishWithGeoInReplyTo,
                    _text.TwitterEncode(), getUserInfo, latitude, longitude, displayCoordinates, replyToTweet.Id);

            return Publish(token, queryWithParameters);
        }

        protected bool Publish(IToken token, string query)
        {
            token = GetQueryToken(token);

            // If id exists it means that the tweet already exist on twitter
            // We cannot republish this tweet
            if (_id != null || _isTweetPublished || _isTweetDestroyed ||
                String.IsNullOrEmpty(_text) || token == null)
            {
                return false;
            }

            bool result = true;

            WebExceptionHandlingDelegate wexHandler = delegate(WebException wex)
            {
                int indexOfStatus = wex.Response.Headers.AllKeys.ToList().IndexOf("status");

                if (indexOfStatus >= 0)
                {
                    string statusValue = wex.Response.Headers.Get(indexOfStatus);

                    switch (statusValue)
                    {
                        case "403 Forbidden":
                            // You are trying to post the same Tweet another time!
                            result = false;
                            break;
                        default:
                            throw wex;
                    }
                }
                else
                {
                    throw wex;
                }
            };

            ObjectResponseDelegate objectDelegate = Populate;

            token.ExecutePOSTQuery(query, objectDelegate, wexHandler);

            return result;
        }

        #endregion

        #region Retweets

        public ITweet PublishRetweet()
        {
            if (_id == null || !IsTweetPublished || IsTweetDestroyed)
            {
                return null;
            }

            Tweet result = null;

            string query = String.Format(Resources.Tweet_PublishRetweet, _id);

            WebExceptionHandlingDelegate wex = delegate(WebException exception)
                {
                    if (exception.GetWebExceptionStatusNumber() == 403)
                    {
                        result = null;
                    }
                };

            ObjectResponseDelegate objectDelegate = delegate(Dictionary<string, object> responseObject)
                {
                    result = new Tweet(responseObject, _shareTokenWithChild ? _token : null);
                };

            _token.ExecutePOSTQuery(query, objectDelegate, wex);

            return result;
        }

        /// <summary>
        /// Get all the retweets of the current tweet
        /// </summary>
        /// <returns>
        ///     Null if the current tweet has a null if or a null token. 
        ///     The list of tweets representing the retweets of the current tweet otherwise.
        /// </returns>
        public List<ITweet> GetRetweets(
            bool getUserInfo = false,
            bool refreshRetweetList = false,
            IToken token = null)
        {
            token = GetQueryToken(token);

            if (Id == null || token == null)
            {
                return null;
            }

            List<ITweet> retweets = new List<ITweet>();

            ObjectResponseDelegate retweetDelegate = delegate(Dictionary<string, object> responseObject)
                {
                    Tweet t = new Tweet(responseObject, _shareTokenWithChild ? this._token : null);
                    retweets.Add(t);
                };

            try
            {
                token.ExecuteGETQuery(String.Format(Resources.Tweet_GetRetweetOfTweet, Id), retweetDelegate, null);

                if (refreshRetweetList)
                {
                    this._retweets = retweets;
                }
            }
            catch (WebException)
            {
                throw;
            }

            return retweets;
        }
        #endregion

        #region Destroy

        public bool Destroy(IToken token = null, bool getUserInfo = false)
        {
            token = GetQueryToken(token);

            // We cannot destroy a tweet that has not been published
            // We cannot destroy a tweet that has already been destroyed
            if (!_isTweetPublished || _isTweetDestroyed || _id == null || token == null)
            {
                return false;
            }

            bool result = true;

            // If a WebException occurs, the deletion has not been performed
            WebExceptionHandlingDelegate wex = delegate
            {
                result = false;
            };

            string destroyQuery = String.Format(Resources.Tweet_Destroy, _id);
            token.ExecutePOSTQuery(destroyQuery, null, wex);

            _isTweetDestroyed = result;
            return result;
        }

        #endregion

        #region Favourite

        public void SetFavourite(bool? isFavourite, IToken token = null)
        {
            token = GetQueryToken(token);

            if (token == null || isFavourite == _favourited || isFavourite == null ||
                !_isTweetPublished || _isTweetDestroyed || _id == null)
            {
                return;
            }

            string query = (bool)isFavourite ?
                  String.Format(Resources.Tweet_CreateFavourite, _id)
                : String.Format(Resources.Tweet_DestroyFavourite, _id);

            ObjectResponseDelegate responseDelegate = delegate(Dictionary<string, object> responseObject)
                {
                    _favourited = responseObject.GetProp("favorited") as bool?;
                };

            token.ExecutePOSTQuery(query, responseDelegate, null);
        }

        #endregion

        #region Override

        public override string ToString()
        {
            return String.Format("'{0}', {1}, '{2}', '{3}', '{4}', '{5}'",
                Id, CreatedAt, Creator.Name, Text, Creator.Lang, TweetCreationDate);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate a tweet from information retrieved from twitter
        /// </summary>
        /// <param name="dTweet">Object containing Tweet information</param>
        /// <param name="cleanString">Does the string needs to be cleanup when retrieved</param>
        protected virtual void Populate(Dictionary<String, object> dTweet, bool cleanString)
        {
            if (dTweet.GetProp("id") != null)
            {
                CreatedAt = DateTime.ParseExact(dTweet.GetProp("created_at") as string,
                    "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture);
                Id = Convert.ToInt64(dTweet.GetProp("id_str"));
                IdValue = Convert.ToInt64(dTweet.GetProp("id"));
                IdStr = dTweet.GetProp("id_str") as string;
                Text = dTweet.GetProp("text") as string;
                Source = dTweet.GetProp("source ") as string;
                Truncated = dTweet.GetProp("truncated") as bool?;
                InReplyToStatusId = dTweet.GetProp("in_reply_to_status_id") as int?;
                InReplyToStatusIdStr = dTweet.GetProp("in_reply_to_status_id_str") as string;
                InReplyToUserId = dTweet.GetProp("in_reply_to_user_id") as int?;
                InReplyToUserIdStr = dTweet.GetProp("in_reply_to_user_id_str") as string;
                InReplyToScreenName = dTweet.GetProp("in_reply_to_screen_name") as string;
                Creator = User.Create(dTweet.GetProp("user"));

                if (_shareTokenWithChild)
                {
                    Creator.ObjectToken = _token;
                }

                Location = Geo.Create(dTweet.GetProp("geo"));

                if (Location != null)
                {
                    LocationCoordinates = Location.GeoCoordinates;
                }

                // Create Place
                _place = dTweet.GetProp("place") as string;

                // Create Contributors
                var contributors = dTweet.GetProp("contributors");

                RetweetCount = dTweet.GetProp("retweet_count") as int?;

                if (dTweet.ContainsKey("entities"))
                {
                    Entities = new TweetEntities(dTweet["entities"] as Dictionary<String, object>);
                }

                Favourited = dTweet.GetProp("favorited") as bool?;
                Retweeted = dTweet.GetProp("retweeted") as bool?;
                PossiblySensitive = dTweet.GetProp("possibly_sensitive") as bool?;

                if (dTweet.ContainsKey("retweeted_status"))
                {
                    var retweet = dTweet.GetProp("retweeted_status") as Dictionary<string, object>;

                    if (retweet != null)
                    {
                        Retweeting = new Tweet(retweet);
                    }
                }

                if (cleanString)
                {
                    Text = Text.CleanString();
                }

                _isTweetPublished = true;
            }
        }

        #endregion

        #region IEquatable<ITweet> Members

        /// <summary>
        /// Compare 2 ITweets and check if they are the same
        /// </summary>
        /// <param name="other">Object to compare to</param>
        /// <returns>Boolean indicating if they are the same</returns>
        public bool Equals(ITweet other)
        {
            // Equals is currently used to compare only if 2 tweets are the same
            // We do not look for the tweet version (DateTime)

            bool result = 
                _id == other.Id &&
                _text == other.Text &&
                _isTweetPublished == other.IsTweetPublished &&
                _isTweetDestroyed == other.IsTweetDestroyed;

            return result;
        }

        #endregion

        #region ICloneable
        
        /// <summary>
        /// Copy a Tweet into a new one
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Tweet clone = new Tweet();

            clone._in_reply_to_user_id_str = _in_reply_to_user_id_str;
            clone._text = _text;
            clone._id_str = _id_str;
            clone._favourited = Favourited;
            clone._source = _source;
            clone._created_at = _created_at;
            clone._user = _user;
            clone._retweet_count = _retweet_count;
            clone._retweeted = _retweeted;

            return clone;
        }
        
        #endregion
    }
}
