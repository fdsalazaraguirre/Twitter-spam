using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TweetinCore.Enum;
using TweetinCore.Events;
using TweetinCore.Interfaces;
using System.Net;
using Tweetinvi.Utils;
using Tweetinvi.Properties;

namespace Tweetinvi
{
    // TODO Implement ICloneable
    /// <summary>
    /// Provide information and functions that a user can do
    /// </summary>
    public class User : TwitterObject, IUser
    {
        #region Private Attributes

        #region Twitter API Attributes
        protected bool? _is_translator;
        // Implement notifications
        protected object _notifications;
        protected bool? _profile_use_background_image;
        protected string _profile_background_image_url_https;
        protected string _time_zone;
        protected string _profile_text_color;
        protected string _profile_image_url_https;
        // Implement following
        protected object[] _following;
        protected bool? _verified;
        protected string _profile_background_image_url;
        protected bool? _default_profile_image;
        protected string _profile_link_color;
        protected string _description;
        protected string _id_str;
        protected bool? _contributors_enabled;
        protected bool? _geo_enabled;
        protected int? _favourites_count;
        protected int? _followers_count;
        protected string _profile_image_url;
        // Implement private object _follow_request_sent;
        protected DateTime _created_at;
        protected string _profile_background_color;
        protected bool? _profile_background_tile;
        protected int? _friends_count;
        protected string _url;
        protected bool? _show_all_inline_media;
        protected int? _statuses_count;
        protected string _profile_sidebar_fill_color;
        protected bool? _protected;
        protected string _screen_name;
        protected int? _listed_count;
        protected string _name;
        protected string _profile_sidebar_border_color;
        protected string _location;
        protected long? _id;
        protected bool? _default_profile;
        protected string _lang;
        protected int? _utc_offset;
        #endregion

        #region User API Attributes
        protected List<long> _friend_ids = new List<long>();
        protected List<IUser> _friends;
        protected List<long> _follower_ids;
        protected List<IUser> _followers;
        protected List<IUser> _contributors;
        protected List<IUser> _contributees;

        protected List<ITweet> _timeline;

        // Retweets are a subset of the timeline
        // Tweets retweeted by the current user
        protected List<ITweet> _retweets;
        // Tweets retweeted by the current user's friends
        protected List<ITweet> _friends_retweets;
        // Tweets of the current user that were retweeted by their followers
        protected List<ITweet> _tweets_retweeted_by_followers;


        #endregion

        #endregion

        #region Public Attributes

        #region Twitter API Attributes
        // This region represents the information accessible from a Twitter API
        // when querying for a User

        public bool? IsTranslator
        {
            get { return _is_translator; }
            set { _is_translator = value; }
        }

        // Implement notifications
        public object Notifications
        {
            get { return _notifications; }
            set { _notifications = value; }
        }
        public bool? ProfileUseBackgroundImage
        {
            get { return _profile_use_background_image; }
            set { _profile_use_background_image = value; }
        }
        public string ProfileBackgroundImageURLHttps
        {
            get { return _profile_background_image_url_https; }
            set { _profile_background_image_url_https = value; }
        }
        public string TimeZone
        {
            get { return _time_zone; }
            set { _time_zone = value; }
        }

        public string ProfileTextColor
        {
            get { return _profile_text_color; }
            set { _profile_text_color = value; }
        }

        public string ProfileImageURLHttps
        {
            get { return _profile_image_url_https; }
            set { _profile_image_url_https = value; }
        }

        public object[] Following
        {
            get { return _following; }
            set { _following = value; }
        }

        public bool? Verified
        {
            get { return _verified; }
            set { _verified = value; }
        }

        public string ProfileBackgroundImageURL
        {
            get { return _profile_background_image_url; }
            set { _profile_background_image_url = value; }
        }

        public bool? DefaultProfileImage
        {
            get { return _default_profile_image; }
            set { _default_profile_image = value; }
        }

        public string ProfileLinkColor
        {
            get { return _profile_link_color; }
            set { _profile_link_color = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string IdStr
        {
            get { return _id_str; }
            set { _id_str = value; }
        }
        public bool? ContributorsEnabled
        {
            get { return _contributors_enabled; }
            set { _contributors_enabled = value; }
        }
        public bool? GeoEnabled
        {
            get { return _geo_enabled; }
            set { _geo_enabled = value; }
        }
        public int? FavouritesCount
        {
            get { return _favourites_count; }
            set { _favourites_count = value; }
        }
        public int? FollowersCount
        {
            get { return _followers_count; }
            set { _followers_count = value; }
        }
        public string ProfileImageURL
        {
            get { return _profile_image_url; }
            set { _profile_image_url = value; }
        }
        //public object follow_request_sent
        //{
        //    get { return _follow_request_sent; }
        //    set { _follow_request_sent = value; }
        //}
        public DateTime CreatedAt
        {
            get { return _created_at; }
            set { _created_at = value; }
        }
        public string ProfileBackgroundColor
        {
            get { return _profile_background_color; }
            set { _profile_background_color = value; }
        }
        public bool? ProfileBackgroundTile
        {
            get { return _profile_background_tile; }
            set { _profile_background_tile = value; }
        }
        public int? FriendsCount
        {
            get { return _friends_count; }
            set { _friends_count = value; }
        }
        public string URL
        {
            get { return _url; }
            set { _url = value; }
        }
        public bool? ShowAllInlineMedia
        {
            get { return _show_all_inline_media; }
            set { _show_all_inline_media = value; }
        }
        public int? StatusesCount
        {
            get { return _statuses_count; }
            set { _statuses_count = value; }
        }
        public string ProfileSidebarFillColor
        {
            get { return _profile_sidebar_fill_color; }
            set { _profile_sidebar_fill_color = value; }
        }
        public bool? UserProtected
        {
            get { return _protected; }
            set { _protected = value; }
        }
        public string ScreenName
        {
            get { return _screen_name; }
            set { _screen_name = value; }
        }
        public int? ListedCount
        {
            get { return _listed_count; }
            set { _listed_count = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string ProfileSidebarBorderColor
        {
            get { return _profile_sidebar_border_color; }
            set { _profile_sidebar_border_color = value; }
        }
        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }
        public long? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public bool? DefaultProfile
        {
            get { return _default_profile; }
            set { _default_profile = value; }
        }

        public string Lang
        {
            get { return _lang; }
            set { _lang = value; }
        }

        public int? UTCOffset
        {
            get { return _utc_offset; }
            set { _utc_offset = value; }
        }
        #endregion

        #region Tweetinvi API Attributes

        public IToken UserToken
        {
            get { return _token; }
            set { _token = value; }
        }

        // Friends
        public List<long> FriendIds
        {
            get { return _friend_ids; }
            set { _friend_ids = value; }
        }

        public List<IUser> Friends
        {
            get { return _friends; }
            set { _friends = value; }
        }

        // Followers
        public List<long> FollowerIds
        {
            get { return _follower_ids; }
            set { _follower_ids = value; }
        }

        public List<IUser> Followers
        {
            get { return _followers; }
            set { _followers = value; }
        }

        public List<IUser> Contributors
        {
            get { return _contributors; }
            set { _contributors = value; }
        }

        public List<IUser> Contributees
        {
            get { return _contributees; }
            set { _contributees = value; }
        }

        public List<ITweet> Timeline
        {
            get { return _timeline; }
            set { _timeline = value; }
        }

        public List<ITweet> Retweets
        {
            get { return _retweets; }
            set { _retweets = value; }
        }

        public List<ITweet> FriendsRetweets
        {
            get { return _friends_retweets; }
            set { _friends_retweets = value; }
        }

        public List<ITweet> TweetsRetweetedByFollowers
        {
            get { return _tweets_retweeted_by_followers; }
            set { _tweets_retweeted_by_followers = value; }
        }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public User(bool shareTokenWithChild = true)
        {
            _shareTokenWithChild = shareTokenWithChild;
        }

        #region Create User from Id

        /// <summary>
        /// Create a User and retrieve the propreties through given token
        /// </summary>
        /// <param name="id">UserId</param>
        /// <param name="token">Token saved in class propreties</param>
        /// <param name="shareTokenWithChild">Token shared accross the hosted TwitterObjects</param>
        public User(
            long? id, 
            IToken token = null, 
            bool shareTokenWithChild = true)
            : this(shareTokenWithChild)
        {
            if (id == null)
                throw new Exception("id cannot be null!");
            _id = id;
            _id_str = id.ToString();

            // Register the token for future usage
            _token = token;

            if (GetQueryTokenForConstructor(token) != null)
            {
                PopulateUser(_token);
            }
        }

        /// <summary>
        /// Create a User and retrieve the propreties through given token
        /// </summary>
        /// <param name="id">UserId</param>
        /// <param name="token">Token saved in class propreties</param>
        /// <param name="shareTokenWithChild">Token shared accross the hosted TwitterObjects</param>
        public User(long id, IToken token = null, bool shareTokenWithChild = true)
            : this((long?)id, token, shareTokenWithChild) { }

        #endregion

        #region Create User from username

        /// <summary>
        /// Create a User and retrieve the propreties through given token
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="token">Token saved in class propreties</param>
        /// <param name="shareTokenWithChild"></param>
        public User(string username, IToken token = null, bool shareTokenWithChild = true)
            : this(shareTokenWithChild)
        {
            if (username == null)
            {
                throw new Exception("username cannot be null!");
            }

            _screen_name = username;

            _token = token;

            if (GetQueryTokenForConstructor(token) != null)
            {
                this.PopulateUser(_token);
            }
        }

        #endregion

        #region Create User from dynamic response

        /// <summary>
        /// Create a user from information retrieved from Twitter
        /// </summary>
        /// <param name="userObject">Information retrieved from Twitter</param>
        /// <param name="shareTokenWithChild">Shall the token be shared to objects created from the user</param>
        public User(Dictionary<string, object> userObject, bool shareTokenWithChild = true)
            : this(shareTokenWithChild)
        {
            Populate(userObject);
        }

        /// <summary>
        /// Create a user from information retrieved from Twitter
        /// </summary>
        /// <param name="userObject">Information retrieved from Twitter</param>
        /// <param name="shareTokenWithChild">Shall the token be shared to objects created from the user</param>
        public static User Create(Dictionary<string, object> userObject, bool shareTokenWithChild = true)
        {
            return new User(userObject, shareTokenWithChild);
        }

        /// <summary>
        /// Create a user from information retrieved from Twitter
        /// </summary>
        /// <param name="dynamicUser">Information retrieved from Twitter</param>
        /// <param name="shareTokenWithChild">Shall the token be shared to objects created from the user</param>
        public static User Create(object dynamicUser, bool shareTokenWithChild = true)
        {
            return User.Create(dynamicUser as Dictionary<string, object>, shareTokenWithChild);
        }

        #endregion

        #endregion

        #region Private Methods

        #region Get Contributors

        /// <summary>
        /// Update the exception handler attribute with the 3rd parameter
        /// Get the list of users matching the Twitter request url (contributors or contributees)
        /// </summary>
        /// <param name="token"> Current user token to access the Twitter API</param>
        /// <param name="url">Twitter requets URL</param>
        /// <param name="exceptionHandlerDelegate">Delegate method to handle Twitter request exceptions</param>
        /// <returns>Null if the token parameter is null or if the Twitter request fails. A list of users otherwise (contributors or contributees).</returns>
        private List<IUser> GetContributionObjects(IToken token, String url,
            WebExceptionHandlingDelegate exceptionHandlerDelegate = null)
        {
            token = GetQueryToken(token);

            if (token == null)
            {
                Console.WriteLine("User's token is required");
                return null;
            }

            return ResultGenerator.GetUsers(token, url, null, exceptionHandlerDelegate);
        }

        #endregion

        /// <summary>
        /// Add UserId or ScreenName based on the information currently
        /// stored in the user object
        /// </summary>
        /// <param name="query">Query with the user specified</param>
        private bool AddUserInformationInQuery(ref string query)
        {
            if (_id == null && _screen_name == null)
            {
                return false;
            }

            if (_id != null)
            {
                query += String.Format("&user_id={0}", _id);
            }
            else
            {
                query += String.Format("&screen_name={0}", _screen_name);
            }

            return true;
        }

        #endregion

        #region Public Methods

        #region Populate User

        /// <summary>
        /// Pupulating all the information related with a user
        /// </summary>
        /// <param name="dUser">Dictionary containing all the information coming from Twitter</param>
        public override void Populate(Dictionary<String, object> dUser)
        {
            if (dUser == null)
            {
                throw new ArgumentException("dynamicUser cannot be null");
            }

            if (dUser.GetProp("id") != null || dUser.GetProp("screen_name") != null)
            {
                IsTranslator = dUser.GetProp("is_translator") as bool?;
                Notifications = dUser.GetProp("notifications");
                ProfileUseBackgroundImage = dUser.GetProp("profile_use_background_image") as bool?;
                ProfileBackgroundImageURLHttps = dUser.GetProp("profile_background_image_url_https") as string;
                TimeZone = dUser.GetProp("time_zone") as string;
                ProfileTextColor = dUser.GetProp("profile_text_color") as string;
                ProfileImageURLHttps = dUser.GetProp("profile_image_url_https") as string;
                Following = dUser.GetProp("following") as object[];
                Verified = dUser.GetProp("verified") as bool?;
                ProfileBackgroundImageURL = dUser.GetProp("profile_background_image_url") as string;
                DefaultProfileImage = dUser.GetProp("default_profile_image") as bool?;
                ProfileLinkColor = dUser.GetProp("profile_link_color") as string;
                Description = dUser.GetProp("description") as string;
                IdStr = dUser.GetProp("id_str") as string;
                ContributorsEnabled = dUser.GetProp("contributors_enabled") as bool?;
                GeoEnabled = dUser.GetProp("geo_enabled") as bool?;
                FavouritesCount = dUser.GetProp("favourites_count") as int?;
                FollowersCount = dUser.GetProp("followers_count") as int?;
                ProfileImageURL = dUser.GetProp("profile_image_url") as string;
                //follow_request_sent = dUser.GetProp("follow_request_sent") as ;

                string createdAt = dUser.GetProp("created_at") as string;

                if (createdAt != null)
                {
                    CreatedAt = DateTime.ParseExact(dUser.GetProp("created_at") as string,
                        "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture);
                }

                ProfileBackgroundColor = dUser.GetProp("profile_background_color") as string;
                ProfileBackgroundTile = dUser.GetProp("profile_background_tile") as bool?;
                FriendsCount = dUser.GetProp("friends_count") as int?;
                URL = dUser.GetProp("url") as string;
                ShowAllInlineMedia = dUser.GetProp("show_all_inline_media") as bool?;
                StatusesCount = dUser.GetProp("statuses_count") as int?;
                ProfileSidebarFillColor = dUser.GetProp("profile_sidebar_fill_color") as string;
                UserProtected = dUser.GetProp("protected") as bool?;
                ScreenName = dUser.GetProp("screen_name") as string;
                ListedCount = dUser.GetProp("listed_count") as int?;
                Name = dUser.GetProp("name") as string;
                ProfileSidebarBorderColor = dUser.GetProp("profile_sidebar_border_color") as string;
                Location = dUser.GetProp("location") as string;
                Id = Convert.ToInt64(dUser.GetProp("id"));
                DefaultProfile = dUser.GetProp("default_profile") as bool?;
                Lang = dUser.GetProp("lang") as string;
                UTCOffset = dUser.GetProp("utc_offset") as int?;
            }
            else
                throw new InvalidOperationException("Cannot create 'User' if id does not exist");
        }

        /// <summary>
        /// Populate User basic information retrieving the information thanks to the
        /// default Token
        /// </summary>
        public void PopulateUser()
        {
            PopulateUser(_token);
        }

        /// <summary>
        /// Populate User basic information retrieving the information thanks to a Token
        /// </summary>
        /// <param name="token">Token to use to get infos</param>
        public void PopulateUser(IToken token)
        {
            token = GetQueryToken(token);

            if (token != null)
            {
                string query = Resources.User_GetUser;

                if (AddUserInformationInQuery(ref query))
                {
                    Dictionary<string, object> dynamicUser = token.ExecuteGETQuery(query);
                    Populate(dynamicUser);
                }
            }
        }

        #endregion

        #region Get Friends

        /// <summary>
        /// Get a List of Friends Ids by using the Current Token
        /// </summary>
        /// <param name="createUserIdsList">Whether this method will fill the Friends list</param>
        /// <param name="cursor">Current Page of the query</param>
        /// <returns>List of Friends Id</returns>
        public List<long> GetFriendIds(bool createUserIdsList = false, long cursor = 0)
        {
            return GetFriendIds(_token, createUserIdsList, cursor);
        }

        /// <summary>
        /// Get a List of Friends Ids by using the Current Token
        /// </summary>
        /// <param name="token">Token to operate the query</param>
        /// <param name="createUserIdsList">Whether this method will fill the Friends list</param>
        /// <param name="cursor">Current Page of the query</param>
        /// <param name="maxFriends">Max number of users</param>
        /// <returns>List of Friends Id</returns>
        public List<long> GetFriendIds(IToken token, bool createUserIdsList = false, long cursor = 0, int maxFriends = Int32.MaxValue)
        {
            token = GetQueryToken(token);

            if (token == null)
            {
                return null;
            }

            if (cursor == 0)
            {
                FriendIds = new List<long>();
                Friends = new List<IUser>();
            }

            DynamicResponseDelegate del = delegate(Dictionary<string, object> responseObject, long previousCursor, long nextCursor)
            {
                var userFriendIds = (responseObject["ids"] as IEnumerable<object>) != null ? (responseObject["ids"] as IEnumerable<object>).ToList() : null;

                if (userFriendIds != null)
                {
                    foreach (var friendId in userFriendIds)
                    {
                        FriendIds.Add(Int64.Parse(friendId.ToString()));

                        if (createUserIdsList)
                        {
                            Friends.Add(new User(Int64.Parse(friendId.ToString()))
                            {
                                ObjectToken = _shareTokenWithChild ? this._token : null,
                            });
                        }
                    }

                    return userFriendIds.Count();
                }

                return 0;
            };

            string query = Resources.User_GetFriends;
            if (!AddUserInformationInQuery(ref query))
            {
                return null;
            }

            token.ExecuteCursorQuery(query, 0, maxFriends, del);
            return FriendIds;
        }

        /// <summary>
        /// Creat a list of Friends without requiring 2 queries
        /// </summary>
        public List<IUser> GetFriends(IToken token = null, int maxFriends = 300, long cursor = 0)
        {
            token = GetQueryToken(token);

            List<long> friendIds = GetFriendIds(token, false, cursor, maxFriends);
            List<IUser> friends = UserUtils.Lookup(friendIds, null, token);

            return friends;
        }

        #endregion

        #region Get Followers

        /// <summary>
        /// Get the followers of a User by using the Current token
        /// </summary>
        /// <param name="createFollowerList">Whether this method will fill the Follower list</param>
        /// <param name="cursor">Current Page of the query</param>
        /// <returns>List of Followers Id</returns>
        public List<long> GetFollowerIds(bool createFollowerList = false, long cursor = 0)
        {
            return GetFollowerIds(_token, createFollowerList, cursor);
        }

        /// <summary>
        /// Get the followers of a User by using the specified Token
        /// </summary>
        /// <param name="token">Token to operate a query</param>
        /// <param name="createFollowerList">Whether this method will fill the Follower list</param>
        /// <param name="cursor">Current Page of the query</param>
        /// <param name="maxFollowers">Max number of users</param>
        /// <returns>List of Followers Id</returns>
        public List<long> GetFollowerIds(IToken token, bool createFollowerList = false, long cursor = 0, int maxFollowers = Int32.MaxValue)
        {
            token = GetQueryToken(token);

            if (token == null)
            {
                return null;
            }

            if (cursor == 0)
            {
                FollowerIds = new List<long>();
                Followers = new List<IUser>();
            }

            string query = Resources.User_GetFollowers;

            if (!AddUserInformationInQuery(ref query))
            {
                return null;
            }

            DynamicResponseDelegate del = delegate(Dictionary<string, object> responseObject, long previousCursor, long nextCursor)
            {
                var userIds = (responseObject["ids"] as IEnumerable<object>) != null ? (responseObject["ids"] as IEnumerable<object>).ToList() : null;

                if (userIds != null)
                {
                    foreach (var followerId in userIds)
                    {
                        FollowerIds.Add(Int64.Parse(followerId.ToString()));

                        if (createFollowerList)
                        {
                            Followers.Add(new User(Int64.Parse(followerId.ToString()))
                            {
                                ObjectToken = _shareTokenWithChild ? _token : null,
                            });
                        }
                    }

                    return userIds.Count();
                }

                return 0;
            };

            token.ExecuteCursorQuery(query, del);
            return FollowerIds;
        }

        public List<IUser> GetFollowers(IToken token = null, int maxFollowers = 300, long cursor = 0)
        {
            token = GetQueryToken(token);

            List<long> followerIds = GetFollowerIds(token, false, cursor, maxFollowers);
            List<IUser> followers = UserUtils.Lookup(followerIds, null, token);

            return followers;
        }

        #endregion

        #region Download Profile Image

        /// <summary>
        /// Get the Profile Image for a user / Possibility to download it
        /// </summary>
        /// <param name="size">Size of the image</param>
        /// <param name="https">Use encryted communication</param>
        /// <param name="folderPath">Define location to store it</param>
        /// <returns>File path</returns>
        public string DownloadProfileImage(ImageSize size = ImageSize.normal, string folderPath = "", bool https = false)
        {
            return DownloadProfileImage(_token, size, folderPath, https);
        }

        /// <summary>
        /// Get the Profile Image for a user / Possibility to download it
        /// </summary>
        /// <param name="token">Token used to perform the query</param>
        /// <param name="size">Size of the image</param>
        /// <param name="https">Use encryted communication</param>
        /// <param name="folderPath">Define location to store it</param>
        /// <returns>File path</returns>
        public string DownloadProfileImage(IToken token, ImageSize size = ImageSize.normal, string folderPath = "", bool https = false)
        {
            token = GetQueryToken(token);

            if (token == null)
            {
                return null;
            }

            if (https && String.IsNullOrEmpty(ProfileImageURLHttps) || String.IsNullOrEmpty(ProfileImageURL))
            {
                return null;
            }

            string url = https ? ProfileImageURLHttps : ProfileImageURL;
            string imgName = ScreenName ?? IdStr;
            string sizeFormat = size == ImageSize.original ? "" : String.Format("_{0}", size);
            string filePath = String.Format("{0}{1}{2}.jpg", folderPath, imgName, sizeFormat);

            url = url.Replace("_normal", sizeFormat);

            // Using WebClient to download the image
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, filePath);
            }

            #region Note
            // Using WebRequest
            // if you want to change from WebClient to WebRequest you can simply use the code behind by extending the class
            // WebRequest request = WebRequest.Create(String.Format(query_user_profile_image, screen_name, size));
            // WebResponse response = request.GetResponse(); 
            #endregion

            return filePath;
        }

        #endregion

        #region Get Contributors
        /// <summary>
        /// Get the list of contributors to the account of the current user
        /// Update the matching attribute of the current user if the parameter is true
        /// Return the list of contributors
        /// </summary>
        /// <param name="createContributorList">False by default. Indicates if the _contributors attribute needs to be updated with the result</param>
        /// <returns>The list of contributors to the account of the current user</returns>
        public List<IUser> GetContributors(bool createContributorList = false)
        {
            // Specific error handler
            // Manage the error 400 thrown when contributors are not enabled by the current user
            WebExceptionHandlingDelegate del = delegate(WebException wex)
            {
                int? wexStatusNumber = ExceptionUtils.GetWebExceptionStatusNumber(wex);
                if (wexStatusNumber != null)
                {
                    switch (wexStatusNumber)
                    {
                        case 400:
                            // Don't need to do anything, the method will return null
                            Console.WriteLine("Contributors are not enabled for this user");
                            break;
                        default:
                            // Other errors are not managed
                            throw wex;
                    }
                }
                else
                {
                    throw wex;
                }
            };

            string query = Resources.User_GetContributors;
            if (!AddUserInformationInQuery(ref query))
            {
                return null;
            }

            List<IUser> result = GetContributionObjects(_token, query, del);
            if (createContributorList)
            {
                _contributors = result;
            }

            return result;
        }
        #endregion

        #region Get Contributees
        /// <summary>
        /// Get the list of accounts the current user is allowed to update
        /// Update the matching attribute of the current user if the parameter is true
        /// Return the list of contributees
        /// </summary>
        /// <param name="createContributeeList">False by default. Indicates if the _contributees attribute needs to be updated with the result</param>
        /// <returns>The list of accounts the current user is allowed to update</returns>
        public List<IUser> GetContributees(bool createContributeeList = false)
        {
            string query = Resources.User_GetContributees;
            if (!AddUserInformationInQuery(ref query))
            {
                return null;
            }

            List<IUser> result = GetContributionObjects(_token, query);

            // Update the _contributees attribute if needed
            if (createContributeeList)
            {
                _contributees = result;
            }
            return result;
        }

        #endregion

        #region Get Timeline

        /// <summary>
        /// Retrieve the timeline of the current user from the Twitter API.
        /// Update the corresponding attribute if required by the parameter createTimeline.
        /// Return the timeline of the current user
        /// </summary>
        /// <returns>Null if there is no user token, the timeline of the current user otherwise</returns>
        public List<ITweet> GetUserTimeline(bool createUserTimeline = false, IToken token = null)
        {
            // Handle the possible exceptions thrown by the Twitter API
            WebExceptionHandlingDelegate wexDel = delegate(WebException wex)
            {
                // Get the exception status number
                int? wexStatusNumber = ExceptionUtils.GetWebExceptionStatusNumber(wex);
                if (wexStatusNumber == null)
                {
                    throw wex;
                }

                switch (wexStatusNumber)
                {
                    case 400:
                        //Rate limit reached. Throw a new Exception with a specific message
                        throw new WebException("Rate limit is reached", wex);
                    default:
                        //Throw the exception "as-is"
                        throw wex;
                }
            };

            return GetUserTimeline(token, wexDel, createUserTimeline);
        }

        /// <summary>
        /// Retrieve the timeline of the current user from the Twitter API.
        /// Update the corresponding attribute if required by the parameter createTimeline.
        /// Return the timeline of the current user
        /// </summary>
        /// <param name="token">Token of the current user</param>
        /// <param name="wexDelegate">Handler of WebException thrown by the Twitter API</param>
        /// <param name="createTimeline">False by default. If true, the attribute _timeline is updated with the result</param>
        /// <returns>Null if the user token is null, the timeline of the user represented by the token otherwise</returns>
        private List<ITweet> GetUserTimeline(IToken token,
            WebExceptionHandlingDelegate wexDelegate = null,
            bool createTimeline = false)
        {
            token = GetQueryToken(token);

            if (token == null)
            {
                Console.WriteLine("Token must be specified");
                return null;
            }

            List<ITweet> timeline = new List<ITweet>();

            ObjectResponseDelegate tweetDelegate = delegate(Dictionary<string, object> tweetContent)
                {
                    Tweet t = new Tweet(tweetContent, _shareTokenWithChild ? _token : null);
                    timeline.Add(t);
                };

            token.ExecuteSinceMaxQuery(String.Format(Resources.User_GetUserTimeline, Id), tweetDelegate, wexDelegate);

            if (createTimeline)
            {
                _timeline = timeline;
            }

            return timeline;
        }

        #endregion

        #region Get Favourites

        public List<ITweet> GetFavourites(int count = 20, IToken token = null, bool includeEntities = false)
        {
            count = Math.Min(count, 200);

            string query = String.Format(Resources.User_GetLastFavourites, count, includeEntities);

            return GetFavourites(query, count, token);
        }

        public List<ITweet> GetFavouritesSinceId(long? sinceId, int count = 20,
            bool includeFirstTweet = false, IToken token = null, bool includeEntities = false)
        {
            if (sinceId == null)
            {
                return null;
            }

            if (includeFirstTweet)
            {
                --sinceId;
            }

            count = Math.Min(count, 200);
            string query = String.Format(Resources.User_GetFavouritesSinceId, count, includeEntities, sinceId);

            return GetFavourites(query, count, token);
        }

        public List<ITweet> GetFavouritesSinceId(ITweet sinceTweet, int count = 20,
            bool includeFirstTweet = false, IToken token = null, bool includeEntities = false)
        {
            if (sinceTweet == null)
            {
                return null;
            }

            return GetFavouritesSinceId(sinceTweet.Id, count, includeFirstTweet, token, includeEntities);
        }

        public List<ITweet> GetFavouritesUntilId(long? maxId, int count = 20,
            bool includeLastTweet = false, IToken token = null, bool includeEntities = false)
        {
            if (maxId == null)
            {
                return null;
            }

            if (!includeLastTweet)
            {
                --maxId;
            }

            count = Math.Min(count, 200);
            string query = String.Format(Resources.User_GetFavouritesUntilId, count, includeEntities, maxId);

            return GetFavourites(query, count, token);
        }

        public List<ITweet> GetFavouritesUntilId(ITweet untilTweet, int count = 20,
            bool includeLastTweet = false, IToken token = null, bool includeEntities = false)
        {
            if (untilTweet == null)
            {
                return null;
            }

            return GetFavouritesUntilId(untilTweet.Id, count, includeLastTweet, token, includeEntities);
        }

        public List<ITweet> GetFavouritesBetweenIds(long? sinceId, long? maxId, int count = 20,
            bool includeFirstTweet = false, bool includeLastTweet = false,
            IToken token = null, bool includeEntities = false)
        {
            if (sinceId == null || maxId == null)
            {
                return null;
            }

            if (includeFirstTweet)
            {
                --sinceId;
            }

            if (!includeLastTweet)
            {
                --maxId;
            }

            string query = String.Format(Resources.User_GetFavouritesBetweenIds, count, includeEntities, sinceId, maxId);

            return GetFavourites(query, count, token);
        }

        public List<ITweet> GetFavouritesBetweenIds(ITweet sinceTweet,
            ITweet untilTweet,
            int count = 20,
            bool includeFirstTweet = false,
            bool includeLastTweet = false,
            IToken token = null,
            bool includeEntities = false)
        {
            if (sinceTweet == null || untilTweet == null)
            {
                return null;
            }

            return GetFavouritesBetweenIds(sinceTweet.Id, untilTweet.Id, count, includeFirstTweet, includeLastTweet, token, includeEntities);
        }

        private List<ITweet> GetFavourites(string query, int count, IToken token)
        {
            token = GetQueryToken(token);

            if (token == null || count <= 0 || (_id == null && _screen_name == null))
            {
                return null;
            }

            // Updating the query to have the appropriate user name/id
            if (!AddUserInformationInQuery(ref query))
            {
                return null;
            }

            ObjectCreatedDelegate<ITweet> tweetCreated = delegate(ITweet tweet)
                {
                    // Set the value of the objectToken depending of the context
                    tweet.ObjectToken = _shareTokenWithChild ? this._token : null;
                };

            return ResultGenerator.GetTweets(token, query, tweetCreated, null);
        }

        #endregion

        #endregion

        #region IEquatable<IUser> Members

        /// <summary>
        /// Compare 2 different members and verify if they are the same
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IUser other)
        {
            bool result = 
                _id == other.Id && 
                ScreenName == other.ScreenName;

            return result;
        }

        #endregion
    }
}