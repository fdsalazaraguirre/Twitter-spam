using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TweetinCore.Interfaces;
using Tweetinvi;
using TwitterToken;
using System.Windows;
using UILibrary;

namespace Testinvi
{
    /// <summary>
    /// Integration test of the TokenCreator
    /// </summary>
    [TestClass]
    public class TokenCreatorTests
    {
        /// <summary>
        /// This method is used as a delegate and shows a WPF interface 
        /// used to validate the application by providing the verifier key
        /// </summary>
        /// <param name="validationUrl">Twitter link for consumer validation</param>
        /// <returns>Verifier Key</returns>
        public int GetCaptcha(string validationUrl)
        {
            // This will be our verifier key
            int result = -1;
            
            Thread enterCaptchaThread = new Thread(() =>
            {
                // We open a WPF application that will ask the 
                // user to get its credentials

                Application app = new Application();
                result = app.Run(new ValidateApplicationCaptchaWindow(validationUrl, true));
            });

            // We start the application and wait for the response
            enterCaptchaThread.SetApartmentState(ApartmentState.STA);
            enterCaptchaThread.Start();
            enterCaptchaThread.Join();

            // We now have the verifier key and return it
            return result;
        }

        /// <summary>
        /// [REQUIREMENTS] You Need to specify a username in your Token class!
        /// Test that we can create a Token for a ConsumerKey and ConsumerSecret
        /// This test requires the tester to specify the verifier code provided on twitter
        /// </summary>
        [TestMethod]
        public void GenerateToken()
        {
            ITokenCreator creator = new TokenCreator(TokenTestSingleton.Instance.ConsumerKey, 
                                                    TokenTestSingleton.Instance.ConsumerSecret);

            IToken token = creator.CreateToken(GetCaptcha);
            ITokenUser loggedUser = new TokenUser(token);

            Assert.AreNotEqual(token, null);
            Assert.AreEqual(loggedUser.ScreenName.ToLower(), TokenTestSingleton.ScreenName.ToLower());
        }

        [TestMethod]
        public void GenerateTokenWithError()
        {
            ITokenCreator creator = new TokenCreator(TokenTestSingleton.Instance.ConsumerKey,
                                                    TokenTestSingleton.Instance.ConsumerSecret);

            IToken token = creator.CreateToken(delegate
                {
                    return 42;
                });
            
            Assert.AreEqual(token, null);
        }
    }
}
