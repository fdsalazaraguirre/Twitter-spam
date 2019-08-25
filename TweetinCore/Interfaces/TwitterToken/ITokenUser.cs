using System.Collections.Generic;
using TweetinCore.Events;

namespace TweetinCore.Interfaces
{
    /// <summary>
    /// User associated with a Token, this "privileged" user
    /// has access private information like messages, timeline...
    /// </summary>
    public interface ITokenUser : IUser
    {
        #region Tweets

        /// <summary>
        /// Send a very simple Tweet with a simple message
        /// </summary>
        /// <param name="text">Text of the tweet</param>
        /// <returns>Tweet has been successfully sent</returns>
        bool SendTweet(string text);

        /// <summary>
        /// Send a very simple Tweet with a simple message
        /// </summary>
        /// <param name="tweet">Tweet to be sent</param>
        /// <returns>Tweet has been successfully sent</returns>
        bool SendTweet(ITweet tweet);

        #endregion

        #region Direct Messages

        /// <summary>
        /// List of Messages received
        /// </summary>
        List<IMessage> LatestDirectMessagesReceived { get; }

        /// <summary>
        /// List of messages sent
        /// </summary>
        List<IMessage> LatestDirectMessagesSent { get; }

        /// <summary>
        /// Get the list of direct messages received by the user
        /// Update the matching attribute of the current user if the parameter is true
        /// Return the list of direct messages received by the user
        /// </summary>
        /// <param name="count">Maximum number of messages retrieved</param>
        /// <returns>Collection of direct messages received by the user</returns>
        List<IMessage> GetLatestDirectMessagesReceived(int count = 20);

        /// <summary>
        /// Get the list of direct messages received by the user
        /// Update the matching attribute of the current user if the parameter is true
        /// Return the list of direct messages received by the user
        /// </summary>
        /// <param name="token">Token to be used to perform the operation</param>
        /// <param name="count">Maximum number of messages retrieved</param>
        /// <returns>Collection of direct messages received by the user</returns>
        List<IMessage> GetLatestDirectMessagesReceived(IToken token, int count = 20);

        /// <summary>
        /// Get the list of direct messages sent by the user
        /// Update the matching attribute of the current user if the parameter is true
        /// Return the list of direct messages sent by the user
        /// </summary>
        /// <param name="count">Maximum number of messages retrieved</param>
        /// <returns>Collection of direct messages sent by the user</returns>
        List<IMessage> GetLatestDirectMessagesSent(int count = 20);

        /// <summary>
        /// Get the list of direct messages sent by the user
        /// Update the matching attribute of the current user if the parameter is true
        /// Return the list of direct messages sent by the user
        /// </summary>
        /// <param name="token">Token to be used to perform the operation</param>
        /// <param name="count">Maximum number of messages retrieved</param>
        /// <returns>Collection of direct messages sent by the user</returns>
        List<IMessage> GetLatestDirectMessagesSent(IToken token, int count = 20);

        #endregion

        #region Timeline

        #region Home Timeline

        /// <summary>
        /// List of tweets as displayed on the Home timeline
        /// Storing the information so that it is not required 
        /// to fetch the data again
        /// </summary>
        List<ITweet> LatestHomeTimeline { get; }

        /// <summary>
        /// Get the latest tweets of the TokenUser Home timeline
        /// </summary>
        /// <param name="count">Number of tweets expected</param>
        /// <returns>Tweets of the Home timeline of the connected user</returns>
        List<ITweet> GetLatestHomeTimeline(int count = 40);

        /// <summary>
        /// Get the Home timeline of the current user
        /// </summary>
        /// <param name="count">Maximum number of Tweet received</param>
        /// <param name="get_user_info">User information sent with the Tweet</param>
        /// <param name="exclude_replies">When true the replies to tweet are not retrieved</param>
        /// <param name="token">Token to be used to perform the query</param>
        /// <param name="since_id">Minimum Id of tweet to receive</param>
        /// <param name="max_id">Maximum Id of twee to receive</param>
        /// <param name="wex">Exception handler</param>
        /// <returns>Tweets from the Home timeline</returns>
        List<ITweet> GetHomeTimeline(
            int count = 20,
            bool get_user_info = true,
            bool exclude_replies = true,
            IToken token = null,
            int? since_id = null,
            int? max_id = null,
            WebExceptionHandlingDelegate wex = null);

        #endregion

        #region Mentions Timeline
        
        /// <summary>
        /// List of tweets as displayed on the Mentions timeline
        /// Storing the information so that it is not required 
        /// to fetch the data again
        /// </summary>
        List<IMention> LatestMentionsTimeline { get; }

        /// <summary>
        /// Get the latest tweets of the TokenUser Mentions timeline
        /// </summary>
        /// <param name="count">Number of tweets expected</param>
        /// <returns>Tweets of the Mentions timeline of the connected user</returns>
        List<IMention> GetLatestMentionsTimeline(int count = 20);

        #endregion

        #endregion

        #region Blocked Users

        /// <summary>
        /// User blocked list with their profile information
        /// </summary>
        List<IUser> BlockedUsers { get; set; }

        /// <summary>
        /// User of user ids blocked
        /// </summary>
        List<long> BlockedUsersIds { get; set; }

        /// <summary>
        /// Retrieve the users blocked by the current user.
        /// Populate the corresponding attributes according to the value of the boolean parameters.
        /// Return the list of users blocked by the current user.
        /// </summary>
        /// <param name="createBlockUsers">True by default. Update the attribute _blocked_users if this parameter is true</param>
        /// <param name="createBlockedUsersIds">True by default. Update the attribute _blocked_users_ids if this parameter is true</param>
        /// <returns>Null if there is no valid token for the current user. Otherwise, The list of users blocked by the current user.</returns>
        List<IUser> GetBlockedUsers(bool createBlockUsers = true, bool createBlockedUsersIds = true);

        /// <summary>
        /// Retrieve the ids of the users blocked by the current user.
        /// Populate the corresponding attribute according to the value of the boolean parameter.
        /// Return the list of ids of the users blocked by the current user.
        /// </summary>
        /// <param name="createBlockedUsersIds">True by default. Update the attribute _blocked_users_ids if this parameter is true</param>
        /// <returns>Null if there is no valid token for the current user. Otherwise, The list of ids of the users blocked by the current user.</returns>
        List<long> GetBlockedUsersIds(bool createBlockedUsersIds = true);

        #endregion

        #region List

        /// <summary>
        /// Provide a List users that are likely to be of interest
        /// </summary>
        List<ISuggestedUserList> SuggestedUserList { get; set; }

        /// <summary>
        /// Retrieve the lists of suggested users associated to the current user from the Twitter API.
        /// Update the corresponding attribute according to the value of the parameter. 
        /// Return the lists of suggested users.
        /// </summary>
        /// <param name="createSuggestedUserList">update the _suggestedUserList if true</param>
        /// <returns>null if the token parameter is null, the lists of suggested users otherwise</returns>
        List<ISuggestedUserList> GetSuggestedUserList(bool createSuggestedUserList = true);

        #endregion
    }
}
