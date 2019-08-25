using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TweetinCore.Events;
using TweetinCore.Interfaces;

namespace Testinvi
{
    /// <summary>
    /// Token test goal is to test Token object by himself
    /// It does not test the TokenBuilder.cs functions
    /// </summary>
    [TestClass]
    public class TokenTests
    {
        #region Private Attributes
        private TestContext testContextInstance; 
        #endregion

        #region Public Attributes
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        #endregion

        #region Constructor
        public TokenTests()
        {
        } 
        #endregion

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        #region QueryWithMultipleResponses

        [TestMethod]
        public void ExecuteQuery()
        {
            IToken token = TokenTestSingleton.Instance;

            // Retrieving information from Twitter API through Token method ExecuteRequest
            Dictionary<string, object>[] timeline = token.ExecuteGETQueryReturningCollectionOfObjects("https://api.twitter.com/1.1/statuses/home_timeline.json");

            // Working on each different object sent as a response to the Twitter API query
            for (int i = 0; i < timeline.Length; ++i)
            {
                Dictionary<String, object> post = timeline[i];
                Debug.WriteLine("{0} : {1}\n", i, post["text"]);
            }
        }

        [TestMethod]
        public void ExecuteQueryWithDelegate()
        {
            IToken token = TokenTestSingleton.Instance;

            // Retrieving information from Twitter API through Token method ExecuteRequest
            ObjectResponseDelegate objectResponseDelegate = delegate(Dictionary<string, object> responseObject)
            {
                Debug.WriteLine("{0}\n", responseObject["text"]);
            };

            token.ExecuteGETQuery("https://api.twitter.com/1.1/statuses/home_timeline.json", objectResponseDelegate);
        }

        #endregion
    }
}
