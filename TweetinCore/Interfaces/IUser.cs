using System;
using System.Collections.Generic;
using TweetinCore.Enum;

namespace TweetinCore.Interfaces
{
    /// <summary>
    /// Contract defining what a user on twitter can do
    /// </summary>
    public interface IUser : ITwitterObject, IEquatable<IUser>
    {
        #region Twitter API Fields
        /// <summary>
        /// User id
        /// </summary>
        long? Id { get; set; }

        /// <summary>
        /// User id as a string
        /// </summary>
        string IdStr { get; set; }

        /// <summary>
        /// Name of the user account
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Name displayed on twitter
        /// </summary>
        string ScreenName { get; set; }

        /// <summary>
        /// Location specified in the profile
        /// </summary>
        string Location { get; set; }

        /// <summary>
        /// User is a member of the Twitter's translator community
        /// </summary>
        bool? IsTranslator { get; set; }

        /// <summary>
        /// Give information related with the use of a background image in twitter
        /// </summary>
        bool? ProfileUseBackgroundImage { get; set; }

        /// <summary>
        /// Secured URL of the background image
        /// </summary>
        string ProfileBackgroundImageURLHttps { get; set; }

        /// <summary>
        /// User time zone
        /// </summary>
        string TimeZone { get; set; }

        /// <summary>
        /// Text color for the profile
        /// </summary>
        string ProfileTextColor { get; set; }

        /// <summary>
        /// Secured Profile image location
        /// </summary>
        string ProfileImageURLHttps { get; set; }

        /// <summary>
        /// Following users
        /// </summary>
        object[] Following { get; set; }

        /// <summary>
        /// Verified user
        /// </summary>
        bool? Verified { get; set; }

        /// <summary>
        /// URL of the background image
        /// </summary>
        string ProfileBackgroundImageURL { get; set; }

        /// <summary>
        /// Default profile Image
        /// </summary>
        bool? DefaultProfileImage { get; set; }

        /// <summary>
        /// Color of a link to the profile
        /// </summary>
        string ProfileLinkColor { get; set; }

        /// <summary>
        /// Description of the user
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Does the user authorized contributors
        /// </summary>
        bool? ContributorsEnabled { get; set; }

        /// <summary>
        /// Geographic information authorized for the user
        /// </summary>
        bool? GeoEnabled { get; set; }

        /// <summary>
        /// Number of favourites
        /// </summary>
        int? FavouritesCount { get; set; }

        /// <summary>
        /// Number of followers
        /// </summary>
        int? FollowersCount { get; set; }

        /// <summary>
        /// Profile image location
        /// </summary>
        string ProfileImageURL { get; set; }

        /// <summary>
        /// User account creation date
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// Background color of the profile page
        /// </summary>
        string ProfileBackgroundColor { get; set; }

        /// <summary>
        /// Indicates that the user's ProfileBackgroundImageUrl should be tiled
        /// </summary>
        bool? ProfileBackgroundTile { get; set; }

        /// <summary>
        /// Number of friends
        /// </summary>
        int? FriendsCount { get; set; }

        /// <summary>
        /// URL related with the user
        /// </summary>
        string URL { get; set; }

        /// <summary>
        /// Indicates that the user would like to see media inline
        /// </summary>
        bool? ShowAllInlineMedia { get; set; }

        /// <summary>
        /// The number of tweets (including retweets) issued by the user. 
        /// </summary>
        int? StatusesCount { get; set; }

        /// <summary>
        /// The hexadecimal color the user has chosen to display 
        /// sidebar backgrounds with in their Twitter UI.
        /// </summary>
        string ProfileSidebarFillColor { get; set; }

        /// <summary>
        /// When true, indicates that this user has chosen to protect their Tweets.
        /// It means that only specific followers can view the tweets
        /// </summary>
        bool? UserProtected { get; set; }

        /// <summary>
        /// The number of public lists that this user is a member of
        /// </summary>
        int? ListedCount { get; set; }

        /// <summary>
        /// The hexadecimal color the user has chosen to display 
        /// sidebar borders with in their Twitter UI. 
        /// </summary>
        string ProfileSidebarBorderColor { get; set; }

        /// <summary>
        /// Specify if you have a default profile
        /// </summary>
        bool? DefaultProfile { get; set; }

        /// <summary>
        /// Account language used
        /// </summary>
        string Lang { get; set; }

        /// <summary>
        /// The offset from GMT/UTC in seconds. 
        /// </summary>
        int? UTCOffset { get; set; }

        #endregion

        #region Tweetinvi API Fields

        /// <summary>
        /// List of friend Ids
        /// </summary>
        List<long> FriendIds { get; set; }

        /// <summary>
        /// List of friends with their profile information
        /// Requires a query per friend user
        /// </summary>
        List<IUser> Friends { get; set; }

        /// <summary>
        /// List of follower ids
        /// </summary>
        List<long> FollowerIds { get; set; }

        /// <summary>
        /// List of followers with their profile information
        /// Requires a query per friend user
        /// </summary>
        List<IUser> Followers { get; set; }

        /// <summary>
        /// List of contributors of the account
        /// </summary>
        List<IUser> Contributors { get; set; }

        /// <summary>
        /// List of the account the user is contributing to
        /// </summary>
        List<IUser> Contributees { get; set; }

        /// <summary>
        /// List of tweets as displayed on the timeline
        /// </summary>
        List<ITweet> Timeline { get; set; }

        /// <summary>
        /// List of tweets that actually are retweets
        /// </summary>
        List<ITweet> Retweets { get; set; }

        /// <summary>
        /// List of retweets from friends
        /// </summary>
        List<ITweet> FriendsRetweets { get; set; }

        /// <summary>
        /// Tweets the user created that have been retweeted by followers
        /// </summary>
        List<ITweet> TweetsRetweetedByFollowers { get; set; }

        #endregion

        #region Create User

        /// <summary>
        /// Populate User basic information retrieving the information thanks to the
        /// default Token
        /// </summary>
        void PopulateUser();

        /// <summary>
        /// Populate User basic information retrieving the information thanks to a Token
        /// <param name="token">Token to use to get infos</param>
        /// </summary>
        void PopulateUser(IToken token);

        #endregion

        #region Friends

        /// <summary>
        /// Get a List of friends of the user
        /// </summary>
        /// <param name="createUserList">Whether this should populate the Friends property</param>
        /// <param name="cursor">Cursor for iteration</param>
        /// <returns>List of Friends Ids</returns>
        List<long> GetFriendIds(bool createUserList = false, long cursor = 0);

        /// <summary>
        /// Get a List of friends of the user
        /// </summary>
        /// <param name="token">Token to execute the query</param>
        /// <param name="createUserList">Whether this should populate the Friends property</param>
        /// <param name="cursor">Cursor for iteration</param>
        /// <param name="maxFriends">Max number of users</param>
        /// <returns>List of Friends Ids</returns>
        List<long> GetFriendIds(IToken token, bool createUserList = false, long cursor = 0, int maxFriends = Int32.MaxValue);

        /// <summary>
        /// Creat a list of Friends
        /// </summary>
        /// <param name="token">Token to execute the query</param>
        /// <param name="maxFriends">Maximum number of friends the method can return</param>
        /// <param name="cursor">Cursor for iteration</param>
        /// <returns></returns>
        List<IUser> GetFriends(IToken token = null, int maxFriends = 300, long cursor = 0);

        #endregion

        #region Followers

        /// <summary>
        /// Get a list of followers of the user
        /// </summary>
        /// <param name="createFollowerList">Whether this should populate the Friends property</param>
        /// <param name="cursor">Cursor for iteration</param>
        /// <returns>List of Followers Ids</returns>
        List<long> GetFollowerIds(bool createFollowerList = false, long cursor = 0);

        /// <summary>
        /// Get a list of followers of the user
        /// </summary>
        /// <param name="token">Token to do the query</param>
        /// <param name="createFollowerList">Whether this should populate the Friends property</param>
        /// <param name="cursor">Cursor for iteration</param>
        /// <param name="maxFollowers">Maximum number of followers to retrieve</param>
        /// <returns>List of Followers Ids</returns>
        List<long> GetFollowerIds(IToken token, bool createFollowerList = false, long cursor = 0, int maxFollowers = Int32.MaxValue);

        /// <summary>
        /// Get a list of followers of the user
        /// </summary>
        /// <param name="token">Token to do the query</param>
        /// <param name="cursor">Cursor for iteration</param>
        /// <param name="maxFollowers">Maximum number of followers to retrieve</param>
        /// <returns>List of Followers Ids</returns>
        List<IUser> GetFollowers(IToken token = null, int maxFollowers = 300, long cursor = 0);

        #endregion

        #region Images

        /// <summary>
        /// Get the Profile Image for a user without specifying a Token / Possibility to download it
        /// </summary>
        /// <param name="size">Size of the image</param>
        /// <param name="https">Calling Https Url</param>
        /// <param name="folderPath">Define location to store it</param>
        /// <returns>Url to access the image from a browser</returns>
        string DownloadProfileImage(ImageSize size = ImageSize.normal, string folderPath = "", bool https = false);

        /// <summary>
        /// Get the Profile Image for a user / Possibility to download it
        /// </summary>
        /// <param name="token"></param>
        /// <param name="size">Size of the image</param>
        /// <param name="https">Calling https Url</param>
        /// <param name="folderPath">Define location to store it</param>
        /// <returns>Filepath of the image</returns>
        string DownloadProfileImage(IToken token, ImageSize size = ImageSize.normal, string folderPath = "", bool https = false);
        
        #endregion

        #region Contributors

        /// <summary>
        /// Get the list of contributors to the account of the current user
        /// Update the matching attribute of the current user if the parameter is true
        /// Return the list of contributors
        /// </summary>
        /// <param name="createContributorList">False by default. Indicates if the _contributors attribute needs to be updated with the result</param>
        /// <returns>The list of contributors to the account of the current user</returns>
        List<IUser> GetContributors(bool createContributorList = false);

        /// <summary>
        /// Get the list of accounts the current user is allowed to update
        /// Update the matching attribute of the current user if the parameter is true
        /// Return the list of contributees
        /// </summary>
        /// <param name="createContributeeList">False by default. Indicates if the _contributees attribute needs to be updated with the result</param>
        /// <returns>The list of accounts the current user is allowed to update</returns>
        List<IUser> GetContributees(bool createContributeeList = false);

        #endregion

        #region Timeline
        
        #region Get User Timeline

        /// <summary>
        /// Retrieve the timeline of the current user from the Twitter API.
        /// Update the corresponding attribute if required by the parameter createTimeline.
        /// Return the timeline of the current user
        /// </summary>
        /// <returns>Null if there is no user token, the timeline of the current user otherwise</returns>
        List<ITweet> GetUserTimeline(bool createUserTimeline = false, IToken token = null);

        #endregion

        #endregion

        #region Favourites

        /// <summary>
        /// Get the latest favourited tweets of the User
        /// </summary>
        /// <param name="count">Number of tweets requested</param>
        /// <param name="token">Token used to perform the request</param>
        /// <param name="includeEntities">Include entities to the result elements</param>
        /// <returns>Collection of favourited tweets</returns>
        List<ITweet> GetFavourites(int count = 20, IToken token = null, bool includeEntities = false);

        /// <summary>
        /// Get the latest favourited tweets of the User
        /// </summary>
        /// <param name="sinceId">Tweet cannot be returned TweetId smaller than sinceId </param>
        /// <param name="count">Number of tweets requested</param>
        /// <param name="includeFirstTweet">Include the tweet from which the research starts</param>
        /// <param name="token">Token used to perform the request</param>
        /// <param name="includeEntities">Include entities to the result elements</param>
        /// <returns>Collection of favourited tweets</returns>
        List<ITweet> GetFavouritesSinceId(long? sinceId, int count = 20, bool includeFirstTweet = false,
            IToken token = null, bool includeEntities = false);

        /// <summary>
        /// Get the latest favourited tweets of the User
        /// </summary>
        /// <param name="sinceTweet">Tweet cannot be returned if it has been sent before sinceTweet</param>
        /// <param name="count">Number of tweets requested</param>
        /// <param name="includeFirstTweet">Include the tweet from which the research starts</param>
        /// <param name="token">Token used to perform the request</param>
        /// <param name="includeEntities">Include entities to the result elements</param>
        /// <returns>Collection of favourited tweets</returns>
        List<ITweet> GetFavouritesSinceId(ITweet sinceTweet, int count = 20, bool includeFirstTweet = false,
            IToken token = null, bool includeEntities = false);

        /// <summary>
        /// Get the latest favourited tweets of the User
        /// </summary>
        /// <param name="maxId">Tweet cannot be returned TweetId bigger than maxId </param>
        /// <param name="count">Number of tweets requested</param>
        /// <param name="includeLastTweet">Include the tweet the search stops at</param>
        /// <param name="token">Token used to perform the request</param>
        /// <param name="includeEntities">Include entities to the result elements</param>
        /// <returns>Collection of favourited tweets</returns>
        List<ITweet> GetFavouritesUntilId(long? maxId, int count = 20, bool includeLastTweet = false,
            IToken token = null, bool includeEntities = false);

        /// <summary>
        /// Get the latest favourited tweets of the User
        /// </summary>
        /// <param name="untilTweet">Tweet cannot be returned if it has been sent after untilTweet</param>
        /// <param name="count">Number of tweets requested</param>
        /// <param name="includeLastTweet">Include the tweet the search stops at</param>
        /// <param name="token">Token used to perform the request</param>
        /// <param name="includeEntities">Include entities to the result elements</param>
        /// <returns>Collection of favourited tweets</returns>
        List<ITweet> GetFavouritesUntilId(ITweet untilTweet, int count = 20, bool includeLastTweet = false,
                                          IToken token = null, bool includeEntities = false);

        /// <summary>
        /// Get the latest favourited tweets of the User
        /// </summary>
        /// <param name="sinceId">Tweet cannot be returned TweetId smaller than sinceId </param>
        /// <param name="maxId">Tweet cannot be returned TweetId bigger than maxId </param>
        /// <param name="count">Number of tweets requested</param>
        /// <param name="includeFirstTweet">Include the tweet from which the research starts</param>
        /// <param name="includeLastTweet">Include the tweet the search stops at</param>
        /// <param name="token">Token used to perform the request</param>
        /// <param name="includeEntities">Include entities to the result elements</param>
        /// <returns>Collection of favourited tweets</returns>
        List<ITweet> GetFavouritesBetweenIds(long? sinceId, long? maxId, int count = 20,
            bool includeFirstTweet = false, bool includeLastTweet = false,
            IToken token = null, bool includeEntities = false);

        /// <summary>
        /// Get the latest favourited tweets of the User
        /// </summary>
        /// <param name="sinceTweet">Tweet cannot be returned if it has been sent before sinceTweet</param>
        /// <param name="untilTweet">Tweet cannot be returned if it has been sent after untilTweet</param>
        /// <param name="count">Number of tweets requested</param>
        /// <param name="includeFirstTweet">Include the tweet from which the research starts</param>
        /// <param name="includeLastTweet">Include the tweet the search stops at</param>
        /// <param name="token">Token used to perform the request</param>
        /// <param name="includeEntities">Include entities to the result elements</param>
        /// <returns>Collection of favourited tweets</returns>
        List<ITweet> GetFavouritesBetweenIds(ITweet sinceTweet, ITweet untilTweet, int count = 20,
            bool includeFirstTweet = false, bool includeLastTweet = false,
            IToken token = null, bool includeEntities = false);

        #endregion
    }
}
