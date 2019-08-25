using System;
using System.Collections.Generic;
using System.Linq;
using TweetinCore.Events;
using TweetinCore.Interfaces;
using Tweetinvi.Properties;

namespace Tweetinvi
{
    /// <summary>
    /// Access to methods related with user that cannot find
    /// their place in User or TokenUser
    /// </summary>
    public static class UserUtils
    {
        #region Private methods
        
        /// <summary>
        /// Update the current query to create the expected query
        /// </summary>
        private static string EnrichLookupQuery(string query, string extension)
        {
            if (extension.EndsWith("%2C"))
            {
                return query + "&" + extension.Remove(extension.Length - 3);
            }
            return query;
        }
        
        #endregion

        #region Public methods

        /// <summary>
        /// Return a list of users corresponding to the list of user ids and screen names given in parameter.
        /// Throw an exception if the token given in parameter is null or if both lists given in parameters are null.
        /// </summary>
        /// <param name="userIds">
        ///     List of user screen names. This parameter can be null
        ///     List of user ids. This parameter can be null
        /// </param>
        /// <param name="screenNames"></param>
        /// <param name="token">Token used to request the users' information from the Twitter API</param>
        /// <returns>The list of users retrieved from the Twitter API</returns>
        public static List<IUser> Lookup(List<long> userIds, List<string> screenNames, IToken token)
        {
            if (token == null)
            {
                throw new ArgumentException("Token must not be null");
            }

            if (userIds == null && screenNames == null)
            {
                throw new ArgumentException("User ids or screen names must be specified");
            }

            // Maximum number of users that can be requested from the Twitter API (in 1 single request)
            const int listMaxSize = 100;

            List<IUser> users = new List<IUser>();

            if (userIds == null)
            {
                userIds = new List<long>();
            }
            if (screenNames == null)
            {
                screenNames = new List<string>();
            }

            int userIndex = 0;
            int screenNameIndex = 0;
            while ((userIndex < userIds.Count) || (screenNameIndex < screenNames.Count))
            {
                // Keep track of the number of users that we are going to request from the Twitter API
                int indicesSumBeforeLoop = userIndex + screenNameIndex;
                string userIdsStrList = "user_id=";
                string screenNamesStrList = "screen_name=";
                
                // Take request parameters from both names list and id list
                // userIndex + screenNameIndex - indicesSumBeforeLoop) < listMaxSize ==> Check that the number of parameters given to the Twitter API request does not exceed the limit
                while (((userIndex + screenNameIndex - indicesSumBeforeLoop) < listMaxSize)
                    && (userIndex < userIds.Count)
                    && (screenNameIndex < screenNames.Count))
                {
                    screenNamesStrList += screenNames.ElementAt(screenNameIndex++) + "%2C";
                    userIdsStrList += userIds.ElementAt(userIndex++) + "%2C";
                }
                // Take request from id list
                while (((userIndex + screenNameIndex - indicesSumBeforeLoop) < listMaxSize)
                    && (userIndex < userIds.Count))
                {
                    userIdsStrList += userIds.ElementAt(userIndex++) + "%2C";
                }

                // Take name from name list
                while (((userIndex + screenNameIndex - indicesSumBeforeLoop) < listMaxSize)
                    && (screenNameIndex < screenNames.Count))
                {
                    screenNamesStrList += screenNames.ElementAt(screenNameIndex++) + "%2C";
                }

                String query = Resources.UserUtils_Lookup;
                // Add new parameters to the query and format it
                query = EnrichLookupQuery(query, screenNamesStrList);
                query = EnrichLookupQuery(query, userIdsStrList);

                ObjectResponseDelegate objectDelegate = delegate(Dictionary<string, object> responseObject)
                    {
                        User u = User.Create(responseObject);
                        users.Add(u);
                    };

                token.ExecuteGETQuery(query, objectDelegate);
            }

            return users;
        }
        #endregion
    }
}
