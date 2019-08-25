using System;
using System.Collections.Generic;
using TweetinCore.Events;
using TweetinCore.Interfaces;
using Tweetinvi.Properties;

namespace Tweetinvi
{
    /// <summary>
    /// Class providing tools to perform search of user on twitter
    /// </summary>
    public class UserSearchEngine : IUserSearchEngine
    {
        #region Fields

        /// <summary>
        /// Token to be used by default to perform operations
        /// </summary>
        private readonly IToken _token;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        protected UserSearchEngine()
        {
            // Basic constructor
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">
        /// Token to be used to perform operation if not specified otherwise
        /// </param>
        public UserSearchEngine(IToken token)
            : this()
        {
            _token = token;
        }

        #endregion

        #region IUserSearchEngine Members
        
        /// <summary>
        /// Search users in twitter
        /// </summary>
        /// <param name="searchQuery">Search to be sent</param>
        /// <param name="token">Token used to perform the search</param>
        /// <returns>Collection of results</returns>
        public List<IUser> Search(string searchQuery, IToken token = null)
        {
            return Search(searchQuery, false, token);
        }

        /// <summary>
        /// Search users in twitter
        /// </summary>
        /// <param name="searchQuery">Search to be sent</param>
        /// <param name="includeEntities">Include entities to the results</param>
        /// <param name="token">Token used to perform the search</param>
        /// <returns>Collection of results</returns>
        public List<IUser> Search(string searchQuery, bool includeEntities, IToken token = null)
        {
            return Search(searchQuery, 20, 0, token, includeEntities);
        }

        /// <summary>
        /// Perform a basic search on Twitter API and returns the T type specified
        /// </summary>
        /// <param name="searchQuery">Search to be sent</param>
        /// <param name="resultPerPage">Number of result per page</param>
        /// <param name="pageNumber">Page expected</param>
        /// <param name="token">Token used to perform the search</param>
        /// <param name="includeEntities">Include entities to the results</param>
        /// <returns>Collection of results</returns>
        public List<IUser> Search(string searchQuery, int resultPerPage, int pageNumber,
            IToken token = null, bool includeEntities = false)
        {
            token = GetQueryToken(token);

            if (token == null)
            {
                throw new Exception("A token is required to perform an request on twitter API");
            }

            List<IUser> result = new List<IUser>();

            // Delegate that will create users from each of the objects retrieved from the search
            ObjectResponseDelegate objectResponseDelegate = delegate(Dictionary<string, object> responseObject)
            {
                IUser user = User.Create(responseObject);

                if (user != null)
                {
                    result.Add(user);
                }
            };

            string httpQuery = string.Format(Resources.UserSearchEngine_SimpleSearch, searchQuery, resultPerPage, pageNumber);
            token.ExecuteGETQuery(httpQuery, objectResponseDelegate, null);

            return result;
        } 

        #endregion

        #region Utils

        /// <summary>
        /// Get a Token for a query
        /// </summary>
        /// <param name="token">Token passed to a function requiring a token</param>
        /// <returns>Appropriate token to perform the query</returns>
        private IToken GetQueryToken(IToken token)
        {
            return token ?? _token;
        }

        #endregion
    }
}
