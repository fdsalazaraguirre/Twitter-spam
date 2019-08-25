using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TweetinCore.Interfaces;
using Tweetinvi;
using System.Threading;

namespace Testinvi
{
    [TestClass]
    public class TokenUserTests
    {
        #region Constructor

        [TestMethod]
        [TestCategory("Constructor"), TestCategory("TokenUser")]
        public void TokenUserConstructor()
        {
            TokenUser me = new TokenUser(TokenTestSingleton.Instance);
            IUser userMe = new User(TokenTestSingleton.ScreenName, TokenTestSingleton.Instance);

            Assert.AreEqual(TokenTestSingleton.ScreenName, me.ScreenName);
            Assert.AreEqual(userMe.Equals(me), true);
        }

        #endregion

        #region GetDirectMessagesReceived

        [TestMethod]
        [TestCategory("GetDirectMessagesReceived"), TestCategory("User")]
        public void UserGetDirectMessagesReceived()
        {
            // This method is not right as the GetDirectMessage retrieve information for the TokenUser
            ITokenUser u = new TokenUser(TokenTestSingleton.Instance);
            List<IMessage> messages = u.GetLatestDirectMessagesReceived();

            Assert.AreNotEqual(messages, null);
            Assert.AreNotEqual(messages.Count, 0);
        }

        #endregion

        #region GetDirectMessagesSent

        [TestMethod]
        [TestCategory("GetDirectMessagesSent"), TestCategory("User")]
        public void UserGetDirectMessagesSent()
        {
            // This method is not right as the GetDirectMessage retrieve information for the TokenUser
            ITokenUser u = new TokenUser(TokenTestSingleton.Instance);
            string testValue = string.Format("Hello Test ({0})!", DateTime.Now);
            IMessage msg = new Message(testValue, u);
            msg.Send(TokenTestSingleton.Instance);

            Thread.Sleep(2000);
            List<IMessage> messages = u.GetLatestDirectMessagesSent();

            Assert.AreNotEqual(messages, null);
            Assert.AreEqual(messages[0].Text == testValue, true);
            Assert.AreEqual(messages[0].Sender.Equals(u), true);
        }

        #endregion

        #region HomeTimeline

        [TestMethod]
        [TestCategory("GetHomeTimeline"), TestCategory("User")]
        public void UserGetLatestHomeTimeline()
        {
            ITokenUser u = new TokenUser(TokenTestSingleton.Instance);
            List<ITweet> tweets = u.GetLatestHomeTimeline();

            Assert.AreNotEqual(tweets, null);
            Assert.AreNotEqual(tweets.Count, 0);
        }

        [TestMethod]
        [TestCategory("GetHomeTimeline"), TestCategory("User")]
        public void UserGetHomeTimeline()
        {
            ITokenUser u = new TokenUser(TokenTestSingleton.Instance);
            List<ITweet> tweets = u.GetHomeTimeline(20, true, true);

            Assert.AreNotEqual(tweets, null);
            Assert.AreNotEqual(tweets.Count, 0);
        }

        #endregion

        #region MentionsTimeline

        [TestMethod]
        [TestCategory("GetMentions"), TestCategory("User")]
        public void UserGetMentions()
        {
            ITokenUser u = new TokenUser(TokenTestSingleton.Instance);
            List<IMention> mentions = u.GetLatestMentionsTimeline();

            Assert.AreNotEqual(mentions, null);
            Assert.AreNotEqual(mentions.Count, 0);
        }

        #endregion

        #region GetBlockedUsers

        [TestMethod]
        [TestCategory("GetBlockedUsers"), TestCategory("User")]
        public void UserGetBlockedUsers()
        {
            ITokenUser u = new TokenUser(TokenTestSingleton.Instance);
            List<IUser> users = u.GetBlockedUsers();

            Assert.AreNotEqual(users, null);
            Assert.AreNotEqual(users.Count, 0);
        }

        #endregion

        #region Suggested Users

        [TestMethod]
        [TestCategory("Suggested Users"), TestCategory("TokenUser")]
        public void UserSuggestedUsers()
        {
            ITokenUser u = new TokenUser(TokenTestSingleton.Instance);
            List<ISuggestedUserList> userList = u.GetSuggestedUserList();

            Assert.AreNotEqual(userList, null);
            Assert.AreNotEqual(userList.Count, 0);
        }

        #endregion
    }
}
