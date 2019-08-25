using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TweetinCore.Interfaces;
using Tweetinvi;

namespace Testinvi
{
    [TestClass]
    public class TweetTests
    {
        [TestMethod]
        [TestCategory("Constructor"), TestCategory("Tweet")]
        public void Tweet_ConstructorAndPopulate1()
        {
            bool errorIs404 = false;

            try
            {
                ITweet tweet = new Tweet(-1);
                tweet.PopulateTweet(TokenTestSingleton.Instance);
            }
            catch (Exception wex)
            {
                errorIs404 = wex.Message == "Tweet[-1] does not exist!";
            }

            Assert.AreEqual(errorIs404, true);
        }

        [TestMethod]
        [TestCategory("Constructor"), TestCategory("Tweet")]
        public void Tweet_ConstructorAndPopulate2()
        {
            bool errorIs404 = false;

            try
            {
                // ReSharper disable UnusedVariable
                var noTweet = new Tweet(-1, TokenTestSingleton.Instance);
                // ReSharper restore UnusedVariable
                Assert.Fail();
            }
            catch (Exception wex)
            {
                errorIs404 = wex.Message == "Tweet[-1] does not exist!";
            }

            Assert.AreEqual(errorIs404, true);
        }

        #region Publish
        [TestMethod]
        [TestCategory("Publish"), TestCategory("Tweet")]
        public void Tweet_PublishTweet1()
        {
            IToken token = TokenTestSingleton.Instance;
            TokenUser currentUser = new TokenUser(token);

            // Create Tweet locally
            ITweet tweet = new Tweet(String.Format("Hello Tweetinvi {0}", DateTime.Now), token);

            // Send the Tweet
            tweet.Publish();

            Assert.AreNotEqual(tweet.Id, null);
            Assert.AreEqual(currentUser.Equals(tweet.Creator), true);
            Assert.AreEqual(tweet.IsTweetPublished, true);

            tweet.Destroy();
        }

        [TestMethod]
        [TestCategory("Publish"), TestCategory("Tweet")]
        public void Tweet_PublishTweet2()
        {
            // Create Tweet locally
            ITweet tweet = new Tweet(String.Format("Hello Tweetinvi {0}", DateTime.Now));

            // Send the Tweet
            tweet.Publish(TokenTestSingleton.Instance);

            Assert.AreNotEqual(tweet.Id, null);
            Assert.AreEqual(tweet.IsTweetPublished, true);

            tweet.Destroy();
        }

        [TestMethod]
        [TestCategory("Publish"), TestCategory("Tweet")]
        public void Tweet_PublishTweet3()
        {
            // Create Tweet locally
            ITweet tweet = new Tweet(String.Format("Hello Tweetinvi {0}", DateTime.Now));

            // Send the Tweet
            tweet.Publish();

            Assert.AreEqual(tweet.Id, null);
            Assert.AreEqual(tweet.IsTweetPublished, true);

            tweet.Destroy();
        }

        [TestMethod]
        [TestCategory("Publish"), TestCategory("Tweet")]
        public void Tweet_PublishTweetWithGeo1()
        {
            // Create Tweet locally
            ITweet tweet = new Tweet(String.Format("Hello Tweetinvi With Geo {0}", DateTime.Now));

            double latitude = 37.7821120598956;
            double longitude = -122.400612831116;

            // Send the Tweet
            tweet.PublishWithGeo(latitude, longitude, true, TokenTestSingleton.Instance);

            // Twitter stores a location to a precision of 8
            double latitude_result = Math.Round(latitude, 8);
            double longitude_result = Math.Round(longitude, 8);

            Assert.AreNotEqual(tweet.Id, null);
            Assert.AreEqual(tweet.IsTweetPublished, true);
            Assert.AreEqual(tweet.Location.GeoCoordinates.Lattitude, latitude_result);
            Assert.AreEqual(tweet.Location.GeoCoordinates.Longitude, longitude_result);
            Assert.AreEqual(tweet.LocationCoordinates.Lattitude, latitude_result);
            Assert.AreEqual(tweet.LocationCoordinates.Longitude, longitude_result);

            tweet.Destroy();
        }

        [TestMethod]
        [TestCategory("Publish"), TestCategory("Tweet")]
        public void Tweet_PublishTweetWithGeo2()
        {
            // Create Tweet locally
            ITweet tweet = new Tweet(String.Format("Hello Tweetinvi With Geo {0}", DateTime.Now), TokenTestSingleton.Instance);

            double latitude = 37.7821120598956;
            double longitude = -122.400612831116;

            // Send the Tweet
            tweet.PublishWithGeo(latitude, longitude, true);

            // Twitter stores a location to a precision of 8
            double latitude_result = Math.Round(latitude, 8);
            double longitude_result = Math.Round(longitude, 8);

            Assert.AreNotEqual(tweet.Id, null);
            Assert.AreEqual(tweet.IsTweetPublished, true);
            Assert.AreEqual(tweet.Location.GeoCoordinates.Lattitude, latitude_result);
            Assert.AreEqual(tweet.Location.GeoCoordinates.Longitude, longitude_result);
            Assert.AreEqual(tweet.LocationCoordinates.Lattitude, latitude_result);
            Assert.AreEqual(tweet.LocationCoordinates.Longitude, longitude_result);

            tweet.Destroy();
        }

        [TestMethod]
        [TestCategory("Publish"), TestCategory("Tweet")]
        public void Tweet_PublishReply()
        {
            IToken token = TokenTestSingleton.Instance;

            // Create Tweet locally
            ITweet tweet = new Tweet(String.Format("Hello Tweetinvi {0}", DateTime.Now), token);

            // Send the Tweet
            bool result = tweet.Publish();
            Assert.AreEqual(result, true);

            ITweet reply = null;
            if (result)
            {
                reply = new Tweet(String.Format("Nice speech Tweetinvi {0}", DateTime.Now), token);

                result &= reply.PublishInReplyTo(tweet);
            }

            Assert.AreEqual(result, true);

            if (reply == null)
            {
                Assert.Fail();
            }

            Assert.AreEqual(reply.IsTweetPublished, true);

            tweet.Destroy();
            reply.Destroy();
        }

        [TestMethod]
        [TestCategory("Publish"), TestCategory("Tweet")]
        public void Tweet_PublishReplyWithGeo()
        {
            IToken token = TokenTestSingleton.Instance;

            // Create Tweet locally
            ITweet tweet = new Tweet(String.Format("Hello Tweetinvi {0}", DateTime.Now), token);

            // Send the Tweet
            bool result = tweet.Publish();
            Assert.AreEqual(result, true);

            ITweet reply = null;
            if (result)
            {
                reply = new Tweet(String.Format("Nice speech Tweetinvi {0}", DateTime.Now), token);
                const double latitude = 37.7821120598956;
                const double longitude = -122.400612831116;

                result &= reply.PublishWithGeoInReplyTo(latitude, longitude, tweet);
            }

            Assert.AreEqual(result, true);

            if (reply == null)
            {
                Assert.Fail();
            }

            Assert.AreEqual(reply.IsTweetPublished, true);

            tweet.Destroy();
            reply.Destroy();
        }

        [TestMethod]
        [TestCategory("Publish"), TestCategory("Tweet")]
        public void Tweet_PublishReplyWithGeo2()
        {
            IToken token = TokenTestSingleton.Instance;

            // Create Tweet locally
            ITweet tweet = new Tweet(String.Format("Hello Tweetinvi {0}", DateTime.Now), token);

            // Send the Tweet
            bool result = tweet.Publish();
            Assert.AreEqual(result, true);

            ITweet reply = null;
            if (result)
            {
                reply = new Tweet(String.Format("Nice speech Tweetinvi {0}", DateTime.Now), token);
                const double latitude = 37.7821120598956;
                const double longitude = -122.400612831116;

                result &= reply.PublishWithGeoInReplyTo(latitude, longitude, tweet.Id);
            }

            Assert.AreEqual(result, true);

            if (reply == null)
            {
                Assert.Fail();
            }

            Assert.AreEqual(reply.IsTweetPublished, true);

            tweet.Destroy();
            reply.Destroy();
        }

        [TestMethod]
        [TestCategory("Publish"), TestCategory("Tweet")]
        public void Tweet_RePublish()
        {
            IToken token = TokenTestSingleton.Instance;

            // Create Tweet locally
            ITweet tweet = new Tweet(String.Format("Hello Tweetinvi {0}", DateTime.Now), token);
            bool result = tweet.Publish();

            Assert.AreEqual(result, true);

            // Republishing a tweet is not allowed
            result &= tweet.Publish();
            Assert.AreEqual(result, false);
            Assert.AreEqual(tweet.IsTweetPublished, true);

            tweet.Destroy();
        }

        [TestMethod]
        [TestCategory("Publish"), TestCategory("Tweet")]
        public void Tweet_PublishWithHashTag()
        {
            IToken token = TokenTestSingleton.Instance;
            TokenUser currentUser = new TokenUser(token);

            // Create Tweet locally
            ITweet tweet = new Tweet(String.Format("#API Hello Tweetinvi {0}", DateTime.Now), token);

            // Send the Tweet
            tweet.Publish();

            Assert.AreNotEqual(tweet.Id, null);
            Assert.AreEqual(currentUser.Equals(tweet.Creator), true);
            Assert.AreEqual(tweet.IsTweetPublished, true);

            // tweet.Destroy();
        }

        [TestMethod]
        [TestCategory("Publish"), TestCategory("Tweet")]
        public void Tweet_PublishWithBigURL()
        {
            IToken token = TokenTestSingleton.Instance;
            TokenUser currentUser = new TokenUser(token);

            string messageWithoutUrl = "Hello Tweetinvi! I believe that your API is great at this date of ("
                                       + DateTime.Now + ") - ";

            string messageUrl = "https://maps.google.com/?ie=UTF8&ll=51.502759,-0.13278&spn=" +
                                "0.03211,0.084543&t=h&z=14&vpsrc=6&iwloc=A" +
                                "&q=Saint+James+Park&cid=18398735506960162143";

            string bigString = messageWithoutUrl + messageUrl;
            
            Assert.AreEqual(messageWithoutUrl.Length < 140 - 22, true);
            Assert.AreEqual(messageUrl.Length > 22, true);
            Assert.AreEqual(bigString.Length > 140, true);

            // Create Tweet locally
            ITweet tweet = new Tweet(bigString, token);

            // Send the Tweet
            tweet.Publish();

            Assert.AreNotEqual(tweet.Id, null);
            Assert.AreEqual(currentUser.Equals(tweet.Creator), true);
            Assert.AreEqual(tweet.IsTweetPublished, true);

            tweet.Destroy();
        }

        #endregion

        #region Destroy

        [TestMethod]
        [TestCategory("Destroy"), TestCategory("Tweet")]
        public void Tweet_Destroy()
        {
            IToken token = TokenTestSingleton.Instance;
            ITweet tweet = new Tweet(String.Format("Hello Tweetinvi {0}", DateTime.Now), token);

            Assert.AreEqual(tweet.IsTweetPublished, false);
            Assert.AreEqual(tweet.IsTweetDestroyed, false);

            tweet.Publish();

            Assert.AreEqual(tweet.IsTweetPublished, true);
            Assert.AreEqual(tweet.IsTweetDestroyed, false);
            
            tweet.Destroy();

            Assert.AreNotEqual(tweet.Id, null);
            Assert.AreEqual(tweet.IsTweetPublished, true);
            // Even if the Tweet has been destroyed the Tweet is still being published
            Assert.AreEqual(tweet.IsTweetDestroyed, true);
        }

        [TestMethod]
        [TestCategory("Destroy"), TestCategory("Tweet")]
        public void Tweet_Destroy2()
        {
            IToken token = TokenTestSingleton.Instance;
            ITweet tweet = new Tweet(String.Format("Hello Tweetinvi {0}", DateTime.Now), token);

            Assert.AreEqual(tweet.IsTweetPublished, false);
            Assert.AreEqual(tweet.IsTweetDestroyed, false);

            tweet.Publish();

            Assert.AreEqual(tweet.IsTweetPublished, true);
            Assert.AreEqual(tweet.IsTweetDestroyed, false);

            bool result = tweet.Destroy();

            Assert.AreNotEqual(tweet.Id, null);
            Assert.AreEqual(result, true);
            Assert.AreEqual(tweet.IsTweetPublished, true);
            // Even if the Tweet has been destroyed the Tweet is still being published
            Assert.AreEqual(tweet.IsTweetDestroyed, true);

            result = tweet.Destroy();
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        [TestCategory("Destroy"), TestCategory("Tweet")]
        public void Tweet_Destroy3()
        {
            IToken token = TokenTestSingleton.Instance;
            ITweet tweet = new Tweet(String.Format("Hello Tweetinvi {0}", DateTime.Now), token);

            Assert.AreEqual(tweet.IsTweetPublished, false);
            Assert.AreEqual(tweet.IsTweetDestroyed, false);

            tweet.Publish();
            long? currentTweetId = tweet.Id;

            Assert.AreEqual(tweet.IsTweetPublished, true);
            Assert.AreEqual(tweet.IsTweetDestroyed, false);

            bool result = tweet.Destroy();

            Assert.AreNotEqual(tweet.Id, null);
            Assert.AreEqual(result, true);
            Assert.AreEqual(tweet.IsTweetPublished, true);
            // Even if the Tweet has been destroyed the Tweet is still being published
            Assert.AreEqual(tweet.IsTweetDestroyed, true);

            bool objectExist = true;
            try
            {
                // ReSharper disable UnusedVariable
                ITweet destroyedTweet = new Tweet(currentTweetId, token);
                // ReSharper restore UnusedVariable

                Assert.Fail();
            }
            catch (Exception ex)
            {
                if (ex.Message == String.Format("Tweet[{0}] does not exist!", currentTweetId))
                {
                    objectExist = false;
                }
            }

            Assert.AreEqual(objectExist, false);
        }

        #endregion

        #region GetRetweets

        [TestMethod]
        [TestCategory("Retweet"), TestCategory("Tweet")]
        public void GetRetweets()
        {
            IToken token = TokenTestSingleton.Instance;
            IUser tweetinviApi = new User("tweetinviApi", token);
            List<ITweet> tweets = tweetinviApi.GetUserTimeline();

            // Publishing retweet
            if (tweets.Count != 0)
            {
                ITweet t = tweets[0];
                ITweet retweet = t.PublishRetweet();
                
                // If null, means it has not been created
                Assert.AreNotEqual(retweet, null);
                
                List<ITweet> retweets = t.GetRetweets();
                Assert.AreNotEqual(retweets, null);
                Assert.AreNotEqual(retweets.Count, 0);
                // The last retweet created should be the one created
                ITweet lastRetweet = retweets[0];
                Assert.AreNotEqual(lastRetweet.Retweeting, null);
                Assert.AreEqual(retweet.Equals(lastRetweet), true);
                Assert.AreEqual(retweet.Retweeting.Equals(t), true);

                retweet.Destroy();
            }
        }

        [TestMethod]
        [TestCategory("Retweet"), TestCategory("Tweet")]
        public void GetRetweets_403_AfterReRetweet()
        {
            IToken token = TokenTestSingleton.Instance;
            IUser tweetinviApi = new User("tweetinviApi", token);
            List<ITweet> tweets = tweetinviApi.GetUserTimeline();

            // Publishing retweet
            if (tweets.Count != 0)
            {
                ITweet t = tweets[0];
                ITweet retweet = t.PublishRetweet();

                // If null, means it has not been created
                Assert.AreNotEqual(retweet, null);

                List<ITweet> retweets = t.GetRetweets();
                Assert.AreNotEqual(retweets, null);
                Assert.AreNotEqual(retweets.Count, 0);
                // The last retweet created should be the one created
                ITweet lastRetweet = retweets[0];
                Assert.AreNotEqual(lastRetweet.Retweeting, null);
                Assert.AreEqual(retweet.Equals(lastRetweet), true);
                Assert.AreEqual(retweet.Retweeting.Equals(t), true);

                // We try to retweet a tweet that we already retweeted
                ITweet retweet2 = t.PublishRetweet();
                Assert.AreEqual(retweet2, null);

                bool destroyResult = retweet.Destroy();
                Assert.AreEqual(destroyResult, true);

                bool doesNotExist = false;
                try
                {
                    // ReSharper disable UnusedVariable
                    ITweet notExisting = new Tweet(retweet.Id, token);
                    // ReSharper restore UnusedVariable
                }
                catch (Exception)
                {
                    doesNotExist = true;
                }
                
                Assert.AreEqual(doesNotExist, true);

                // This is required as Twitter blocks the creation of retweet of the
                // same component quickly after the destruction
                Thread.Sleep(10000);
                ITweet t2 = new Tweet(t.Id, TokenTestSingleton.Instance);
                ITweet retweet3 = t2.PublishRetweet();

                Assert.AreNotEqual(retweet3, null);
                retweet3.Destroy();
            }
        }

        #endregion
    }
}
