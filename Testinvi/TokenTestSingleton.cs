using System.Configuration;
using Tweetinvi;
using TwitterToken;
using TweetinCore.Interfaces;

namespace Testinvi
{
    public class TokenTestSingleton
    {
        private static IToken _token;
        private static string _screeName;

        public static void Reset()
        {
            _token = null;
            _screeName = null;
        }

        public static string ScreenName
        {
            get { return _screeName; }
        }

        public static IToken Instance
        {
            get { return _token; }
        }

        public static void Initialize(bool reset)
        {
            if (reset)
            {
                Reset();
            }
        }

        static TokenTestSingleton()
        {
            // Start with classic Token
            _token = new Token(
                        ConfigurationManager.AppSettings["token_AccessToken"],
                        ConfigurationManager.AppSettings["token_AccessTokenSecret"],
                        ConfigurationManager.AppSettings["token_ConsumerKey"],
                        ConfigurationManager.AppSettings["token_ConsumerSecret"]);

            _screeName = ConfigurationManager.AppSettings["token_UserScreenName"];

            // After Reset should use TokenSingleton
            TokenSingleton.Token = new Token(
                        ConfigurationManager.AppSettings["token_AccessToken"],
                        ConfigurationManager.AppSettings["token_AccessTokenSecret"],
                        ConfigurationManager.AppSettings["token_ConsumerKey"],
                        ConfigurationManager.AppSettings["token_ConsumerSecret"]);

            _screeName = TokenSingleton.TokenUser.ScreenName;
        }
    }
}
