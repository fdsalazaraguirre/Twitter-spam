using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TweetinCore.Interfaces;
using Tweetinvi.Properties;
using TweetinCore.Events;
using Tweetinvi.Utils;

namespace Tweetinvi
{
    /// <summary>
    /// A token user is unique to a Token and provides action that will
    /// be executed from the connected user and that are not available
    /// from another user like (read my messages)
    /// </summary>
    public class TokenUser : User, ITokenUser
    {
        #region Attributes

        protected List<IMessage> _directMessagesReceived;
        protected List<IMessage> _directMessagesSent;
        protected List<IMention> _mentionsTimeline;
        protected List<ITweet> _homeTimeline;
        protected List<ISuggestedUserList> _suggested_user_list;

        // List of users blocked by the current user
        protected List<IUser> _blocked_users;
        // List of ids of the users blocked by the current user
        protected List<long> _blocked_users_ids;

        #endregion

        #region Properties

        public TokenUserSettings Settings { get; set; }

        public List<IMessage> LatestDirectMessagesReceived
        {
            get { return _directMessagesReceived; }
            protected set { _directMessagesReceived = value; }
        }

        public List<IMessage> LatestDirectMessagesSent
        {
            get { return _directMessagesSent; }
            protected set { _directMessagesSent = value; }
        }

        public List<IMention> LatestMentionsTimeline
        {
            get { return _mentionsTimeline; }
            protected set { _mentionsTimeline = value; }
        }

        public List<ITweet> LatestHomeTimeline
        {
            get { return _homeTimeline; }
            protected set { _homeTimeline = value; }
        }

        public List<ISuggestedUserList> SuggestedUserList
        {
            get { return _suggested_user_list; }
            set { _suggested_user_list = value; }
        }

        public List<IUser> BlockedUsers
        {
            get { return _blocked_users; }
            set { _blocked_users = value; }
        }

        public List<long> BlockedUsersIds
        {
            get { return _blocked_users_ids; }
            set { _blocked_users_ids = value; }
        }

        #endregion

        #region Constructors

        public TokenUser(IToken token)
            : base("", null)
        {
            if (token == null)
            {
                throw new Exception("A token user cannot be created without associating it to a Token");
            }

            _token = token;

            GetSettings();

            if (Settings != null)
            {
                _screen_name = Settings.ScreenName;
                this.PopulateUser(_token);
            }
        }

        #endregion

        #region ITokenUser Members

        #region SendTweet

        public bool SendTweet(string text)
        {
            Tweet t = new Tweet(text, _token);
            return t.Publish();
        }

        public bool SendTweet(ITweet tweet)
        {
            return tweet.Publish(_token);
        }

        #endregion

        #region Direct Messages

        #region Get Direct Messages

        /// <summary>
        /// Request the list of direct message from Twitter using the token and the URL given in parameter
        /// Handle the exception when the token represents a user who did not authorize the application to get the direct messages
        /// </summary>
        /// <param name="token"></param>
        /// <param name="url"></param>
        /// <param name="directMessageDelegate"></param>
        /// <returns>The list of messages retrieved from the Twitter API</returns>
        private List<IMessage> GetDirectMessages(IToken token,
            string url,
            ObjectCreatedDelegate<IMessage> directMessageDelegate = null)
        {
            token = GetQueryToken(token);

            if (token == null || url == null)
            {
                throw new ArgumentException("token or request url is null");
            }

            WebExceptionHandlingDelegate webExceptionDelegate = delegate(WebException wex)
            {
                int? wexStatusNumber = ExceptionUtils.GetWebExceptionStatusNumber(wex);
                if (wexStatusNumber == null)
                {
                    throw wex;
                }

                switch (wexStatusNumber)
                {
                    case 403:
                        Exception e = new Exception("Not enough permission to access direct messages. " +
                                                    "Update the application access level and ask the user to reauthorize it.");
                        throw e;
                    default:
                        throw wex;
                }
            };

            return ResultGenerator.GetMessages(token, url, directMessageDelegate, webExceptionDelegate);
        }

        #endregion

        #region Get Direct Messages Sent

        public List<IMessage> GetLatestDirectMessagesSent(int count = 20)
        {
            return GetLatestDirectMessagesSent(_token, count);
        }

        public List<IMessage> GetLatestDirectMessagesSent(IToken token, int count = 20)
        {
            string query = String.Format(Resources.TokenUser_GetLatestDirectMessagesSent, count);
            ObjectCreatedDelegate<IMessage> messageSenderDelegate = delegate(IMessage message)
                {
                    message.Sender = this;
                };

            List<IMessage> result = GetDirectMessages(token, query, messageSenderDelegate);
            LatestDirectMessagesSent = result;

            return result;
        }

        #endregion

        #region Get Direct Messages Received

        public List<IMessage> GetLatestDirectMessagesReceived(int count = 20)
        {
            return GetLatestDirectMessagesReceived(_token, count);
        }

        public List<IMessage> GetLatestDirectMessagesReceived(IToken token, int count = 20)
        {
            string query = String.Format(Resources.TokenUser_GetLatestDirectMessagesReceived, count);
            ObjectCreatedDelegate<IMessage> messageSenderDelegate = delegate(IMessage message)
            {
                message.Receiver = this;
            };

            List<IMessage> result = GetDirectMessages(token, query, messageSenderDelegate);
            LatestDirectMessagesReceived = result;

            return result;
        }

        #endregion

        #endregion

        #region Timeline

        #region Home Timeline

        public List<ITweet> GetLatestHomeTimeline(int count = 40)
        {
            string query = String.Format(Resources.TokenUser_GetLatestHomeTimeline, count, true, true);
            return ResultGenerator.GetTweets(_token, query);
        }

        /// <summary>
        /// Get the Home timeline of the current user
        /// </summary>
        /// <param name="token">Token to be used to perform the query</param>
        /// <returns>Tweets from the Home timeline</returns>
        public List<ITweet> GetHomeTimeline(IToken token)
        {
            return GetHomeTimeline(20, true, true, token);
        }

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
        public List<ITweet> GetHomeTimeline(
            int count = 20,
            bool get_user_info = true,
            bool exclude_replies = true,
            IToken token = null,
            int? since_id = null,
            int? max_id = null,
            WebExceptionHandlingDelegate wex = null)
        {
            IToken queryToken = token ?? _token;

            return GetHomeTimeLine(queryToken, count, get_user_info, exclude_replies,
                                   since_id, max_id, wex);
        }

        /// <summary>
        /// Get the Home timeline of the current user
        /// </summary>
        /// <param name="queryToken">Token to be used to perform the query</param>
        /// <param name="count">Maximum number of Tweet received</param>
        /// <param name="trim_user">User information sent with the Tweet</param>
        /// <param name="exclude_replies">When true the replies to tweet are not retrieved</param>
        /// <param name="since_id">Minimum Id of tweet to receive</param>
        /// <param name="max_id">Maximum Id of twee to receive</param>
        /// <param name="wex">Exception handler</param>
        /// <returns>Tweets from the Home timeline</returns>
        private List<ITweet> GetHomeTimeLine(
            IToken queryToken,
            int count,
            bool trim_user,
            bool exclude_replies,
            int? since_id,
            int? max_id,
            WebExceptionHandlingDelegate wex)
        {
            if (queryToken == null)
            {
                Console.WriteLine("Token must be specified");
                return null;
            }

            string query;

            if (since_id == null && max_id == null)
            {
                query = String.Format(Resources.TokenUser_GetLatestHomeTimeline, count, trim_user, exclude_replies);
            }
            else
            {
                query = String.Format(Resources.TokenUser_GetHomeTimeline,
                    count, trim_user, exclude_replies, since_id, max_id);
            }

            return ResultGenerator.GetTweets(queryToken, query, null, wex);
        }

        #endregion

        #region Mentions Timeline

        public List<IMention> GetLatestMentionsTimeline(int count = 20)
        {
            string query = String.Format(Resources.TokenUser_GetLatestMentionTimeline, count);

            WebExceptionHandlingDelegate wexDelegate = delegate(WebException wex)
            {
                int? wexStatusNumber = ExceptionUtils.GetWebExceptionStatusNumber(wex);
                switch (wexStatusNumber)
                {
                    case 400:
                        throw new WebException("Wrong token for user id = " + this.Id, wex);
                    default:
                        throw wex;
                }
            };

            LatestMentionsTimeline = ResultGenerator.GetMentions(_token, query, null, wexDelegate);
            return LatestMentionsTimeline;
        }

        #endregion

        #endregion

        #region Get Blocked Users

        /// <summary>
        /// Reinitialize the list of ids of the users blocked by the current user if required by the boolean parameter.
        /// Retrieve information about the users blocked by the current user from the Twitter API.
        /// </summary>
        /// <param name="blockedUsersQuery">Request to send to the Twitter API</param>
        /// <param name="del">delegate that will handle the data sent in the Twitter API's response</param>
        /// <param name="createBlockedUsersIds">True by default. Update the attribute _blocked_users_ids if this parameter is true</param>
        /// <returns>Null if there is no valid token for the current user. Otherwise, The list of ids of the users blocked by the current user.</returns>
        private void executeBlockedUsersQuery(string blockedUsersQuery, 
            DynamicResponseDelegate del, 
            bool createBlockedUsersIds = true)
        {
            // Reset the list of blocked users' ids if required
            if (createBlockedUsersIds)
            {
                _blocked_users_ids = null;
                _blocked_users_ids = new List<long>();
            }

            WebExceptionHandlingDelegate wexDel = delegate(WebException wex)
            {
                int? wexStatusNumber = ExceptionUtils.GetWebExceptionStatusNumber(wex);
                switch (wexStatusNumber)
                {
                    // Handle the Exception when the user's token is not valid
                    case 401:
                        throw new WebException("Blocked users can only be accessed using the user's token. Any other token is rejected.", wex);
                    default:
                        throw wex;
                }
            };

            _token.ExecuteCursorQuery(blockedUsersQuery, 0, Int32.MaxValue, del, wexDel);
        }

        /// <summary>
        /// Retrieve the users blocked by the current user.
        /// Populate the corresponding attributes according to the value of the boolean parameters.
        /// Return the list of users blocked by the current user.
        /// </summary>
        /// <param name="createBlockUsers">True by default. Update the attribute _blocked_users if this parameter is true</param>
        /// <param name="createBlockedUsersIds">True by default. Update the attribute _blocked_users_ids if this parameter is true</param>
        /// <returns>Null if there is no valid token for the current user. Otherwise, The list of users blocked by the current user.</returns>
        public List<IUser> GetBlockedUsers(bool createBlockUsers = true, bool createBlockedUsersIds = true)
        {
            if (_token == null)
            {
                return null;
            }

            List<IUser> blockedUsers = new List<IUser>();
            DynamicResponseDelegate blockedUsersDel = delegate(Dictionary<string, object> twitterBlockedUsers, long previousCursor, long nextCursor)
            {
                // Get the users from the Twitter API's response and store them in the users list
                foreach (var tbu in (IEnumerable<object>)twitterBlockedUsers["users"])
                {
                    IUser blockedUser = User.Create(tbu);
                    if (blockedUser != null)
                    {
                        blockedUser.ObjectToken = _shareTokenWithChild ? this._token : null;
                        blockedUsers.Add(blockedUser);
                        if (createBlockedUsersIds)
                        {
                            // update the list of ids of the blocked users if required
                            if (blockedUser.Id != null)
                            {
                                _blocked_users_ids.Add(Int64.Parse(blockedUser.Id.ToString()));
                            }
                        }
                    }
                }

                return ((IEnumerable<object>) twitterBlockedUsers["users"]).Count();
            };

            executeBlockedUsersQuery(Resources.TokenUser_GetBlockedUsers, blockedUsersDel);

            // Update the list of blocked users if required
            if (createBlockUsers)
            {
                _blocked_users = blockedUsers;
            }

            return blockedUsers;
        }

        /// <summary>
        /// Retrieve the ids of the users blocked by the current user.
        /// Populate the corresponding attribute according to the value of the boolean parameter.
        /// Return the list of ids of the users blocked by the current user.
        /// </summary>
        /// <param name="createBlockedUsersIds">True by default. Update the attribute _blocked_users_ids if this parameter is true</param>
        /// <returns>Null if there is no valid token for the current user. Otherwise, The list of ids of the users blocked by the current user.</returns>
        public List<long> GetBlockedUsersIds(bool createBlockedUsersIds = true)
        {
            if (_token == null)
            {
                return null;
            }

            List<long> blockedUsersIds = new List<long>();
            DynamicResponseDelegate blockedUsersDel = delegate(Dictionary<string, object> twitterBlockedUsersIds, long previousCursor, long nextCursor)
            {
                // Get the ids of the blocked users from the Twitter API's response and store them in the result list
                foreach (var id in (IEnumerable<object>)twitterBlockedUsersIds["ids"])
                {
                    blockedUsersIds.Add(Int64.Parse(id.ToString()));
                }

                return ((IEnumerable<object>)twitterBlockedUsersIds["ids"]).Count();
            };

            executeBlockedUsersQuery(Resources.TokenUser_GetBlockedUsersIds, blockedUsersDel, createBlockedUsersIds);

            // Update the attribute if required
            if (createBlockedUsersIds)
            {
                _blocked_users_ids = blockedUsersIds;
            }

            return blockedUsersIds;
        }

        #endregion

        #region Get Suggested List
        /// <summary>
        /// Retrieve the lists of suggested users associated to the current user from the Twitter API.
        /// Update the corresponding attribute according to the value of the parameter. 
        /// Return the lists of suggested users.
        /// </summary>
        /// <param name="createSuggestedUserList">update the _suggestedUserList if true</param>
        /// <returns>null if the token parameter is null, the lists of suggested users otherwise</returns>
        public List<ISuggestedUserList> GetSuggestedUserList(bool createSuggestedUserList = true)
        {
            if (_token == null)
            {
                return null;
            }

            List<ISuggestedUserList> suggUserList = new List<ISuggestedUserList>();

            ObjectResponseDelegate objectDelegate = delegate(Dictionary<string, object> responseObject)
            {
                suggUserList.Add(new SuggestedUserList(
                    (string)responseObject["name"],
                    (string)responseObject["slug"],
                    (int)responseObject["size"]));
            };

            // Retrieve the lists of suggested users from the Twitter API
            _token.ExecuteGETQuery(Resources.TokenUser_GetUserSuggestions, objectDelegate);

            // Update the attribute if requested
            if (createSuggestedUserList)
            {
                _suggested_user_list = suggUserList;
            }

            return suggUserList;
        }
        #endregion

        #endregion

        #region Settings

        /// <summary>
        /// Retrieve the settings of the Token's owner
        /// </summary>
        private void GetSettings()
        {
            ObjectResponseDelegate objectDelegate = delegate(Dictionary<string, object> responseObject)
                {
                    Settings = new TokenUserSettings(responseObject);
                };

            _token.ExecuteGETQuery(Resources.TokenUser_GetAccountSettings, objectDelegate);
        }

        #endregion
    }
}
