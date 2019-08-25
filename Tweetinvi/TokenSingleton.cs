using TweetinCore.Interfaces;

namespace Tweetinvi
{
    /// <summary>
    /// Store credential information across the application
    /// </summary>
    public static class TokenSingleton
    {
        /// <summary>
        /// This property specify whether the Token of the TokenSingleton should 
        /// be used to populate a new object which does not specify a Token
        /// </summary>
        public static bool AutomaticallyPopulateNewObjects { get; set; }

        private static ITokenUser _tokenUser;
        /// <summary>
        /// TokenUser associated with the current Token
        /// </summary>
        public static ITokenUser TokenUser
        {
            get
            {
                if (_tokenUser == null && Token != null)
                {
                    _tokenUser = new TokenUser(_token);
                }

                return _tokenUser;
            }
        }

        private static IToken _token;
        /// <summary>
        /// Token to share across your applicatio
        /// </summary>
        public static IToken Token
        {
            get { return _token; }
            set
            {
                if (_token != value)
                {
                    _token = value;
                    _tokenUser = null;
                }
            }
        }
    }
}
