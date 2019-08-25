using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi.Utils;

namespace Testinvi
{
    [TestClass]
    public class StringExtensionTests
    {
        [TestMethod]
        public void TestLengthWith2Urls()
        {
            string test = "Hello http://tweetinvi.codeplex.com/salutLescopains 3615 Gerard www.linviIsMe.com piloupe";

            int twitterLength = StringExtension.TweetLenght(test);
            
            Assert.AreEqual(twitterLength, 71);
        }

        [TestMethod]
        public void TestLengthWith2UrlsAndHttps()
        {
            string test = "Hello https://tweetinvi.codeplex.com/salutLescopains 3615 Gerard www.linviIsMe.com piloupe";

            int twitterLength = StringExtension.TweetLenght(test);

            Assert.AreEqual(twitterLength, 72);
        }

        [TestMethod]
        public void TestLengthWithSmallUrl()
        {
            string test = "www.co.co";

            int twitterLength = StringExtension.TweetLenght(test);

            Assert.AreEqual(twitterLength, 22);
        }
    }
}
