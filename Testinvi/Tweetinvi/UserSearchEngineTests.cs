using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TweetinCore.Interfaces;
using Tweetinvi;

namespace Testinvi
{
    [TestClass]
    public class UserSearchEngineTests
    {
        [TestMethod]
        public void SimpleSearch()
        {
            string searchQuery = "TweetinviApi";

            UserSearchEngine searchEngine = new UserSearchEngine(TokenTestSingleton.Instance);
            List<IUser> searchResult = searchEngine.Search(searchQuery);

            Assert.AreNotEqual(searchResult, null);
            Assert.AreEqual(searchResult.Count, 1);
            Assert.AreEqual(searchResult[0].Id, 1577389800);
            Assert.AreEqual(searchResult[0].ScreenName, "TweetinviApi");
        }

        [TestMethod]
        public void SimpleSearch2()
        {
            string searchQuery = "invi";

            IUserSearchEngine searchEngine = new UserSearchEngine(TokenTestSingleton.Instance);
            List<IUser> searchResult = searchEngine.Search(searchQuery, 2, 2);

            Assert.AreNotEqual(searchResult, null);
            Assert.AreEqual(searchResult.Count, 2);
        }

        [TestMethod]
        public void SimpleSearch3()
        {
            string searchQuery = "TweetinviApi";

            UserSearchEngine searchEngine = new UserSearchEngine(TokenTestSingleton.Instance);
            List<IUser> searchResult = searchEngine.Search(searchQuery, 2, 0);

            Assert.AreNotEqual(searchResult, null);
            Assert.AreEqual(searchResult.Count, 1);
            Assert.AreEqual(searchResult[0].Id, 1577389800);
            Assert.AreEqual(searchResult[0].ScreenName, "TweetinviApi");
        }
    }
}
