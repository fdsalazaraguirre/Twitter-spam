using System.Collections.Generic;
using TweetinCore.Events;
using TweetinCore.Interfaces;

namespace Tweetinvi.Utils
{
    /// <summary>
    /// Delegate to happen after a object T has been created
    /// </summary>
    /// <typeparam name="T">Object type to be created</typeparam>
    /// <param name="obj">Object that has been created</param>
    public delegate void ObjectCreatedDelegate<in T>(T obj);

    /// <summary>
    /// Delegate to happen to create an object of type T
    /// </summary>
    /// <typeparam name="T">Object type to be created</typeparam>
    /// <param name="data">Data to be used to create the object</param>
    /// <returns>New object of the expected type</returns>
    public delegate T ObjectCreatorDelegate<T>(Dictionary<string, object> data);

    /// <summary>
    /// Provide simplified ways to query a list of TwitterObjects
    /// </summary>
    public static class ResultGenerator
    {
        /// <summary>
        /// Provide simplified ways to query a list of Users
        /// </summary>
        /// <param name="token">Token to be used</param>
        /// <param name="queryUrl">URL from which we expect results</param>
        /// <param name="objectCreatedDelegate">Delegate to happen after a User being created</param>
        /// <param name="wex">WebException to manage errors</param>
        /// <returns>Collection of users retrieved from the query</returns>
        public static List<IUser> GetUsers(IToken token,
            string queryUrl,
            ObjectCreatedDelegate<IUser> objectCreatedDelegate = null,
            WebExceptionHandlingDelegate wex = null)
        {
            ObjectCreatorDelegate<IUser> userCreator = delegate(Dictionary<string, object> data)
            {
                return new User(data);
            };

            return GetListOfTwitterObject(token, queryUrl, userCreator, objectCreatedDelegate, wex);
        }

        /// <summary>
        /// Provide simplified ways to query a list of Tweets
        /// </summary>
        /// <param name="token">Token to be used</param>
        /// <param name="queryUrl">URL from which we expect results</param>
        /// <param name="objectCreatedDelegate">Delegate to happen after a Tweet being created</param>
        /// <param name="wex">WebException to manage errors</param>
        /// <returns>Collection of tweets retrieved from the query</returns>
        public static List<ITweet> GetTweets(IToken token,
            string queryUrl,
            ObjectCreatedDelegate<ITweet> objectCreatedDelegate = null,
            WebExceptionHandlingDelegate wex = null)
        {
            ObjectCreatorDelegate<ITweet> tweetCreator = delegate(Dictionary<string, object> data)
            {
                return new Tweet(data);
            };

            return GetListOfTwitterObject(token, queryUrl, tweetCreator, objectCreatedDelegate, wex);
        }

        /// <summary>
        /// Provide simplified ways to query a list of Mentions
        /// </summary>
        /// <param name="token">Token to be used</param>
        /// <param name="queryUrl">URL from which we expect results</param>
        /// <param name="objectCreatedDelegate">Delegate to happen after a Mention being created</param>
        /// <param name="wex">WebException to manage errors</param>
        /// <returns>Collection of mention retrieved from the query</returns>
        public static List<IMention> GetMentions(IToken token,
           string queryUrl,
           ObjectCreatedDelegate<IMention> objectCreatedDelegate = null,
           WebExceptionHandlingDelegate wex = null)
        {
            ObjectCreatorDelegate<IMention> userCreator = delegate(Dictionary<string, object> data)
            {
                return new Mention(data);
            };

            return GetListOfTwitterObject(token, queryUrl, userCreator, objectCreatedDelegate, wex);
        }

        /// <summary>
        /// Provide simplified ways to query a list of Messages
        /// </summary>
        /// <param name="token">Token to be used</param>
        /// <param name="queryUrl">URL from which we expect results</param>
        /// <param name="objectCreatedDelegate">Delegate to happen after a Message being created</param>
        /// <param name="wex">WebException to manage errors</param>
        /// <returns>Collection of messages retrieved from the query</returns>
        public static List<IMessage> GetMessages(IToken token,
          string queryUrl,
          ObjectCreatedDelegate<IMessage> objectCreatedDelegate = null,
          WebExceptionHandlingDelegate wex = null)
        {
            ObjectCreatorDelegate<IMessage> userCreator = delegate(Dictionary<string, object> data)
            {
                return new Message(data);
            };

            return GetListOfTwitterObject(token, queryUrl, userCreator, objectCreatedDelegate, wex);
        }

        /// <summary>
        /// Provide simplified ways to query a list of TwitterObject
        /// </summary>
        /// <param name="token">Token to be used</param>
        /// <param name="queryUrl">URL from which we expect results</param>
        /// <param name="objectCreatorDelegate">Method to be used to create the expected object</param>
        /// <param name="objectCreatedDelegate">Delegate to happen after a TwitterObject being created</param>
        /// <param name="wex">WebException to manage errors</param>
        /// <returns>Collection of TwitterObject retrieved from the query</returns>
        public static List<T> GetListOfTwitterObject<T>(IToken token,
            string queryUrl,
            ObjectCreatorDelegate<T> objectCreatorDelegate,
            ObjectCreatedDelegate<T> objectCreatedDelegate = null,
            WebExceptionHandlingDelegate wex = null)
        {
            if (token == null || objectCreatorDelegate == null)
            {
                return null;
            }

            List<T> result = new List<T>();

            ObjectResponseDelegate objectDelegate = delegate(Dictionary<string, object> objectContent)
            {
                T newObject = objectCreatorDelegate(objectContent);

                if (objectCreatedDelegate != null)
                {
                    objectCreatedDelegate(newObject);
                }

                result.Add(newObject);
            };

            token.ExecuteGETQuery(queryUrl, objectDelegate, wex);

            return result;
        }
    }
}
