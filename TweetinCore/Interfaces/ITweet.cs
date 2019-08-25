using System;
using System.Collections.Generic;

namespace TweetinCore.Interfaces
{
    /// <summary>
    /// ... Well a Tweet :)
    /// </summary>
    public interface ITweet : ITwitterObject, IEquatable<ITweet>
    {
        #region Twitter API Properties

        /// <summary>
        /// id of the Tweet
        /// </summary>
        long? Id { get; set; }

        /// <summary>
        /// Id of tweet as a string
        /// </summary>
        string IdStr { get; set; }

        /// <summary>
        /// Formatted text of the tweet
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// source field
        /// </summary>
        string Source { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool? Truncated { get; set; }

        /// <summary>
        /// In_reply_to_status_id
        /// </summary>
        long? InReplyToStatusId { get; set; }

        /// <summary>
        /// In_reply_to_status_id_str
        /// </summary>
        string InReplyToStatusIdStr { get; set; }

        /// <summary>
        /// In_reply_to_user_id
        /// </summary>
        long? InReplyToUserId { get; set; }

        /// <summary>
        /// In_reply_to_user_id_str
        /// </summary>
        string InReplyToUserIdStr { get; set; }

        /// <summary>
        /// In_reply_to_screen_name
        /// </summary>
        string InReplyToScreenName { get; set; }

        /// <summary>
        /// User who created the Tweet
        /// </summary>
        IUser Creator { get; set; }

        /// <summary>
        /// Location from where the Tweet has been sent
        /// </summary>
        IGeo Location { get; set; }

        /// <summary>
        /// Coordinates of the location from where the tweet has been sent
        /// </summary>
        ICoordinates LocationCoordinates { get; set; }

        /// <summary>
        /// Ids of the users who contributed in the Tweet
        /// </summary>
        int[] ContributorsIds { get; set; }

        /// <summary>
        /// Number of retweets related with this tweet
        /// </summary>
        int? RetweetCount { get; set; }

        /// <summary>
        /// Entities contained in the tweet
        /// </summary>
        ITweetEntities Entities { get; set; }

        /// <summary>
        /// Is the tweet favourited
        /// </summary>
        bool? Favourited { get; set; }

        /// <summary>
        /// Has the tweet been retweeted
        /// </summary>
        bool? Retweeted { get; set; }

        /// <summary>
        /// Is the tweet potentialy sensitive
        /// </summary>
        bool? PossiblySensitive { get; set; }

        #endregion

        #region Tweetinvi API Properties

        /// <summary>
        /// Determine the length of the Text using Twitter algorithm
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Date at which the Tweet has been created in the application
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// Collection of hashtags associated with a Tweet
        /// </summary>
        List<IHashTagEntity> Hashtags { get; set; }

        /// <summary>
        /// Collection of urls associated with a tweet
        /// </summary>
        List<IUrlEntity> Urls { get; set; }

        /// <summary>
        /// Collection of medias associated with a tweet
        /// </summary>
        List<IMediaEntity> Media { get; set; }

        /// <summary>
        /// Collection of tweets mentioning this tweet
        /// </summary>
        List<IUserMentionEntity> UserMentions { get; set; }

        /// <summary>
        /// Collection of tweets retweeting this tweet
        /// </summary>
        List<ITweet> Retweets { get; set; }

        /// <summary>
        /// If the tweet is a retweet this field provides
        /// the tweet that it retweeted
        /// </summary>
        ITweet Retweeting { get; }

        /// <summary>
        /// Inform us if this tweet has been published on Twitter
        /// </summary>
        bool IsTweetPublished { get; }
        
        /// <summary>
        /// Inform us if this tweet was destroyed
        /// </summary>
        bool IsTweetDestroyed { get; }

        #endregion

        #region Constructors
        
        /// <summary>
        /// Populate the information of Tweet for which we set the ID
        /// </summary>
        void PopulateTweet();

        /// <summary>
        /// Populate the information of Tweet for which we set the ID
        /// </summary>
        /// <param name="token">Token to perform the query</param>
        /// <param name="cleanString">Clean the string so that it is readalble</param>
        void PopulateTweet(IToken token, bool cleanString = true); 

        #endregion

        #region Publish

        /// <summary>
        /// Publish a Tweet created from the API
        /// </summary>
        /// <param name="token">Token to be used to send the Tweet</param>
        /// <param name="getUserInfo">
        /// Whether we want to have all information related with the user in the response
        /// </param>
        /// <returns>Whether the Tweet has successfully been sent</returns>
        bool Publish(IToken token = null, bool getUserInfo = false);

        /// <summary>
        /// Publish a Tweet created from the API
        /// </summary>
        /// <param name="replyToTweetId">Tweet id of the tweet we reply to</param>
        /// <param name="token">Token to be used to send the Tweet</param>
        /// <param name="getUserInfo">
        /// Whether we want to have all information related with the user in the response
        /// </param>
        /// <returns>Whether the Tweet has successfully been sent</returns>
        bool PublishInReplyTo(long? replyToTweetId, IToken token = null, bool getUserInfo = false);

        /// <summary>
        /// Publish a Tweet created from the API
        /// </summary>
        /// <param name="replyToTweet">Tweet we reply to</param>
        /// <param name="token">Token to be used to send the Tweet</param>
        /// <param name="getUserInfo">
        /// Whether we want to have all information related with the user in the response
        /// </param>
        /// <returns>Whether the Tweet has successfully been sent</returns>
        bool PublishInReplyTo(ITweet replyToTweet, IToken token = null, bool getUserInfo = false);

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
        bool PublishWithGeo(double latitude, double longitude,
            bool displayCoordinates = true, IToken token = null, bool getUserInfo = false);

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
        bool PublishWithGeoInReplyTo(double latitude, double longitude, long? replyToTweetId,
            bool displayCoordinates = true, IToken token = null, bool getUserInfo = false);

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
        bool PublishWithGeoInReplyTo(double latitude, double longitude, ITweet replyToTweet,
            bool displayCoordinates = true, IToken token = null, bool getUserInfo = false); 

        #endregion

        #region Favourites

        /// <summary>
        /// Create or Remove a Tweet from favourite
        /// </summary>
        /// <param name="isFavourite">Does the tweet needs to be added as a Favourite</param>
        /// <param name="token">Token used to perform the query</param>
        void SetFavourite(bool? isFavourite, IToken token = null);

        #endregion

        #region Destroy

        /// <summary>
        /// Destroy a Tweet
        /// </summary>
        /// <param name="token">Token to perform the operation</param>
        /// <param name="getUserInfo">Whether we expect to have user information detailed</param>
        /// <returns>Whether the Tweet has been successfully destroyed</returns>
        bool Destroy(IToken token = null, bool getUserInfo = false);

        #endregion

        #region Retweets

        /// <summary>
        /// Retweet from the current Token User
        /// </summary>
        /// <returns>If the retweet has been successfully publish</returns>
        ITweet PublishRetweet();

        /// <summary>
        /// Get all the retweets of the current tweet
        /// </summary>
        /// <returns>Null if the current tweet has a null if or a null token. The list of tweets representing the retweets of the current tweet otherwise.</returns>
        List<ITweet> GetRetweets(bool getUserInfo = false, bool refreshRetweetList = false, IToken token = null); 
        #endregion
    }
}
