using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TweetinCore.Interfaces;
using Tweetinvi;
using System;

namespace Testinvi
{
    [TestClass]
    public class FavouriteTests
    {
        #region Default Value

        [TestMethod]
        [TestCategory("Default"), TestCategory("Favourites")]
        public void FavouritesDefault()
        {
            IToken token = TokenTestSingleton.Instance;

            string text = string.Format("Favouriting tweet {0}", DateTime.Now);
            ITweet newTweet = new Tweet(text, token);
            // When a Tweet has not been published it is not favourited
            Assert.AreEqual(newTweet.Favourited, false);
            newTweet.Publish();
            // This tweet is a published tweet
            Assert.AreEqual(newTweet.Favourited, false);

            // This tweet won't be able to get published
            ITweet newTweetCopy = new Tweet(text, token);
            newTweetCopy.Publish();
            Assert.AreEqual(newTweet.Favourited, false);
            
            ITweet existingTweet = new Tweet(newTweet.Id);
            // We currently don't know the value of favourite of this tweet
            // because no query has been performed
            Assert.AreEqual(existingTweet.Favourited, null);

            newTweet.Destroy();
        }

        #endregion

        #region Utils

        // [TestMethod]
        public void ClearFavourites()
        {
            Tweet t = new Tweet("hello", TokenTestSingleton.Instance);
            t.Publish();
            IUser u = t.Creator;
            t.Destroy();

            List<ITweet> favourites = u.GetFavourites(200);

            do
            {
                foreach (var favourite in favourites)
                {
                    favourite.Destroy();
                }

                favourites = u.GetFavourites(200);
            } while (favourites.Count != 0);
        }

        #endregion

        #region Create Favourite

        [TestMethod]
        [TestCategory("Create"), TestCategory("Favourites")]
        public void FavouritesCreate1()
        {
            IToken token = TokenTestSingleton.Instance;

            ITweet newTweet = new Tweet(String.Format("Favouriting tweet {0}", DateTime.Now), token);
            Assert.AreEqual(newTweet.Favourited, false);
            newTweet.Publish();
            Assert.AreEqual(newTweet.Favourited, false);
            newTweet.Favourited = true;
            Assert.AreEqual(newTweet.Favourited, true);

            newTweet.Destroy();
        }

        [TestMethod]
        [TestCategory("Create"), TestCategory("Favourites")]
        public void FavouritesCreate1_1()
        {
            IToken token = TokenTestSingleton.Instance;

            ITweet newTweet = new Tweet(String.Format("Favouriting tweet {0}", DateTime.Now), token);
            Assert.AreEqual(newTweet.Favourited, false);
            newTweet.Publish();
            Assert.AreEqual(newTweet.Favourited, false);
            newTweet.SetFavourite(true);
            Assert.AreEqual(newTweet.Favourited, true);

            newTweet.Destroy();
        }

        [TestMethod]
        [TestCategory("Create"), TestCategory("Favourites")]
        public void FavouritesCreate2()
        {
            IToken token = TokenTestSingleton.Instance;

            ITweet newTweet = new Tweet(String.Format("Favouriting tweet {0}", DateTime.Now), token);
            Assert.AreEqual(newTweet.Favourited, false);
            newTweet.Publish();
            Assert.AreEqual(newTweet.Favourited, false);
            newTweet.Destroy();
            Assert.AreEqual(newTweet.Favourited, false);
            newTweet.Favourited = true;
            Assert.AreEqual(newTweet.Favourited, false);

            newTweet.Destroy();
        }

        [TestMethod]
        [TestCategory("Create"), TestCategory("Favourites")]
        public void FavouritesCreate2_1()
        {
            IToken token = TokenTestSingleton.Instance;

            ITweet newTweet = new Tweet(String.Format("Favouriting tweet {0}", DateTime.Now), token);
            Assert.AreEqual(newTweet.Favourited, false);
            newTweet.Publish();
            Assert.AreEqual(newTweet.Favourited, false);
            newTweet.Destroy();
            Assert.AreEqual(newTweet.Favourited, false);
            newTweet.SetFavourite(true);
            Assert.AreEqual(newTweet.Favourited, false);

            newTweet.Destroy();
        }

        [TestMethod]
        [TestCategory("Create"), TestCategory("Favourites")]
        public void FavouritesCreate3()
        {
            IToken token = TokenTestSingleton.Instance;

            ITweet newTweet = new Tweet(String.Format("Favouriting tweet {0}", DateTime.Now), token);
            Assert.AreEqual(newTweet.Favourited, false);
            newTweet.Publish();
            Assert.AreEqual(newTweet.Favourited, false);
            newTweet.Favourited = true;
            Assert.AreEqual(newTweet.Favourited, true);
            newTweet.Favourited = false;
            Assert.AreEqual(newTweet.Favourited, false);

            // Getting the item we have just created
            ITweet existingTweet = new Tweet(newTweet.Id, token);
            existingTweet.Favourited = true;
            Assert.AreEqual(existingTweet.Favourited, true);

            newTweet.Destroy();
        }

        [TestMethod]
        [TestCategory("Create"), TestCategory("Favourites")]
        public void FavouritesCreate4()
        {
            IToken token = TokenTestSingleton.Instance;

            ITweet newTweet = new Tweet(String.Format("Favouriting tweet {0}", DateTime.Now), token);
            Assert.AreEqual(newTweet.Favourited, false);
            newTweet.Publish();
            Assert.AreEqual(newTweet.Favourited, false);
            newTweet.Favourited = true;
            Assert.AreEqual(newTweet.Favourited, true);
            newTweet.Favourited = false;
            Assert.AreEqual(newTweet.Favourited, false);

            // Setting the value even though we don't know what is the current value
            ITweet existingTweet = new Tweet(newTweet.Id);
            existingTweet.ObjectToken = token;
            existingTweet.Favourited = true;
            Assert.AreEqual(existingTweet.Favourited, true);

            newTweet.Destroy();
        }

        [TestMethod]
        [TestCategory("Create"), TestCategory("Favourites")]
        public void FavouritesCreate5()
        {
            IToken token = TokenTestSingleton.Instance;

            ITweet newTweet = new Tweet(String.Format("Favouriting tweet {0}", DateTime.Now), token);
            Assert.AreEqual(newTweet.Favourited, false);
            newTweet.Publish();

            // Setting the value without token specified
            ITweet existingTweet = new Tweet(newTweet.Id);
            existingTweet.Favourited = true;
            Assert.AreEqual(existingTweet.Favourited, null);

            newTweet.Destroy();
        }

        #endregion

        #region Get Favourites

        // The getTest must be done on an account without any existing favourite
        [TestMethod]
        [TestCategory("Get"), TestCategory("Favourites")]
        public void GetFavourites1()
        {
            IToken token = TokenTestSingleton.Instance;

            string text = String.Format("Favouriting tweet {0}", DateTime.Now);

            // Create and favourite a first tweet
            ITweet newTweet = new Tweet(text, token);
            newTweet.Publish();
            newTweet.Favourited = true;

            IUser creator = newTweet.Creator;
            
            List<ITweet> favourites = creator.GetFavourites();
            
            Assert.AreEqual(favourites.Count, 1);
            Assert.AreEqual(favourites[0].Equals(newTweet), true);

            // Create and favourite a second tweet
            ITweet newTweet2 = new Tweet(text + " - bis", token);
            newTweet2.Publish();
            newTweet2.Favourited = true;

            favourites = creator.GetFavourites();

            Assert.AreEqual(favourites.Count, 2);
            Assert.AreEqual(favourites[0].Equals(newTweet2), true);
            Assert.AreEqual(favourites[1].Equals(newTweet), true);

            // Remove Favourite from first tweet
            newTweet.Favourited = false;

            favourites = creator.GetFavourites();
            Assert.AreEqual(favourites.Count, 1);
            Assert.AreEqual(favourites[0].Equals(newTweet2), true);

            // Destroy Second Tweet
            newTweet2.Destroy();

            favourites = creator.GetFavourites();
            Assert.AreEqual(favourites.Count, 0);

            // Cleanup test
            newTweet.Destroy();
        }


        // The getTest must be done on an account without any existing favourite
        [TestMethod]
        [TestCategory("Get"), TestCategory("Favourites")]
        public void FavouritesSinceId1()
        {
            IToken token = TokenTestSingleton.Instance;

            string text = String.Format("Favouriting tweet {0}", DateTime.Now);

            // Create and favourite a first tweet
            ITweet tweet1 = new Tweet(text, token);
            tweet1.Publish();
            tweet1.Favourited = true;

            ITweet tweet2 = new Tweet(text + " - 2", token);
            tweet2.Publish();
            tweet2.Favourited = true;

            ITweet tweet3 = new Tweet(text + " - 3", token);
            tweet3.Publish();
            tweet3.Favourited = true;

            IUser creator = tweet1.Creator;

            // Checking that the 2 functions return the same thing
            List<ITweet> favouritesIncludingSourceTweet = creator.GetFavouritesSinceId(tweet1, 20, true);
            List<ITweet> favouritesIncludingSourceTweetId = creator.GetFavouritesSinceId(tweet1.Id, 20, true);
            Assert.AreEqual(favouritesIncludingSourceTweet.Count, 3);
            Assert.AreEqual(favouritesIncludingSourceTweetId.Count, 3);

            for (int i = 0; i < favouritesIncludingSourceTweet.Count; ++i)
            {
                Assert.AreEqual(favouritesIncludingSourceTweet[i].Equals(favouritesIncludingSourceTweetId[i]), true);
            }

            // Checking that the 2 functions return the same thing
            List<ITweet> favouritesSinceTweet = creator.GetFavouritesSinceId(tweet1);
            List<ITweet> favouritesSinceId = creator.GetFavouritesSinceId(tweet1.Id);

            Assert.AreEqual(favouritesSinceTweet.Count, favouritesSinceId.Count);

            for (int i = 0; i < favouritesSinceTweet.Count; ++i)
            {
                Assert.AreEqual(favouritesSinceTweet[i].Equals(favouritesSinceId[i]), true);
            }

            // Analyzing the result
            Assert.AreEqual(favouritesSinceTweet.Count, 2);
            Assert.AreEqual(favouritesSinceTweet[0].Equals(tweet3), true);
            Assert.AreEqual(favouritesSinceTweet[1].Equals(tweet2), true);

            // Checking after Tweet1 deletion
            tweet1.Destroy();

            favouritesSinceTweet = creator.GetFavouritesSinceId(tweet1);

            Assert.AreEqual(favouritesSinceTweet.Count, 2);
            Assert.AreEqual(favouritesSinceTweet[0].Equals(tweet3), true);
            Assert.AreEqual(favouritesSinceTweet[1].Equals(tweet2), true);

            tweet3.Destroy();

            favouritesSinceTweet = creator.GetFavouritesSinceId(tweet1);

            Assert.AreEqual(favouritesSinceTweet.Count, 1);
            Assert.AreEqual(favouritesSinceTweet[0].Equals(tweet2), true);

            tweet2.Destroy();

            favouritesSinceTweet = creator.GetFavouritesSinceId(tweet1);

            Assert.AreEqual(favouritesSinceTweet.Count, 0);
        }

        // The getTest must be done on an account without any existing favourite
        [TestMethod]
        [TestCategory("Get"), TestCategory("Favourites")]
        public void FavouritesUntilId1()
        {
            IToken token = TokenTestSingleton.Instance;

            string text = String.Format("Favouriting tweet {0}", DateTime.Now);

            // Create and favourite a first tweet
            ITweet tweet1 = new Tweet(text, token);
            tweet1.Publish();
            tweet1.Favourited = true;

            ITweet tweet2 = new Tweet(text + " - 2", token);
            tweet2.Publish();
            tweet2.Favourited = true;

            ITweet tweet3 = new Tweet(text + " - 3", token);
            tweet3.Publish();
            tweet3.Favourited = true;

            IUser creator = tweet1.Creator;

            // Checking that the 2 functions return the same thing
            List<ITweet> favouritesIncludingSourceTweet = creator.GetFavouritesUntilId(tweet3, 20, true);
            List<ITweet> favouritesIncludingSourceTweetId = creator.GetFavouritesUntilId(tweet3.Id, 20, true);
            Assert.AreEqual(favouritesIncludingSourceTweet.Count, 3);
            Assert.AreEqual(favouritesIncludingSourceTweetId.Count, 3);

            for (int i = 0; i < favouritesIncludingSourceTweet.Count; ++i)
            {
                Assert.AreEqual(favouritesIncludingSourceTweet[i].Equals(favouritesIncludingSourceTweetId[i]), true);
            }

            // Checking that the 2 functions return the same thing
            List<ITweet> favouritesSinceTweet = creator.GetFavouritesUntilId(tweet3);
            List<ITweet> favouritesSinceId = creator.GetFavouritesUntilId(tweet3.Id);

            Assert.AreEqual(favouritesSinceTweet.Count, favouritesSinceId.Count);

            for (int i = 0; i < favouritesSinceTweet.Count; ++i)
            {
                Assert.AreEqual(favouritesSinceTweet[i].Equals(favouritesSinceId[i]), true);
            }

            // Analyzing the result
            Assert.AreEqual(favouritesSinceTweet.Count, 2);
            Assert.AreEqual(favouritesSinceTweet[0].Equals(tweet2), true);
            Assert.AreEqual(favouritesSinceTweet[1].Equals(tweet1), true);

            // Checking after Tweet1 deletion
            tweet3.Destroy();

            favouritesSinceTweet = creator.GetFavouritesUntilId(tweet3);

            Assert.AreEqual(favouritesSinceTweet.Count, 2);
            Assert.AreEqual(favouritesSinceTweet[0].Equals(tweet2), true);
            Assert.AreEqual(favouritesSinceTweet[1].Equals(tweet1), true);

            tweet1.Destroy();

            favouritesSinceTweet = creator.GetFavouritesUntilId(tweet3);

            Assert.AreEqual(favouritesSinceTweet.Count, 1);
            Assert.AreEqual(favouritesSinceTweet[0].Equals(tweet2), true);

            tweet2.Destroy();

            favouritesSinceTweet = creator.GetFavouritesUntilId(tweet3);

            Assert.AreEqual(favouritesSinceTweet.Count, 0);
        }

        // The getTest must be done on an account without any existing favourite
        [TestMethod]
        [TestCategory("Get"), TestCategory("Favourites")]
        public void FavouritesBetweenIds1()
        {
            IToken token = TokenTestSingleton.Instance;

            string text = String.Format("Favouriting tweet {0}", DateTime.Now);

            // Create and favourite a first tweet
            ITweet tweet1 = new Tweet(text, token);
            tweet1.Publish();
            tweet1.Favourited = true;

            ITweet tweet2 = new Tweet(text + " - 2", token);
            tweet2.Publish();
            tweet2.Favourited = true;

            ITweet tweet3 = new Tweet(text + " - 3", token);
            tweet3.Publish();
            tweet3.Favourited = true;

            IUser creator = tweet1.Creator;

            // Checking that the 2 functions return the same thing
            List<ITweet> favouritesIncludingBothTweet = creator.GetFavouritesBetweenIds(tweet1, tweet3, 20, true, true);
            List<ITweet> favouritesIncludingBothTweetId = creator.GetFavouritesBetweenIds(tweet1.Id, tweet3.Id, 20, true, true);
            Assert.AreEqual(favouritesIncludingBothTweet.Count, 3);
            Assert.AreEqual(favouritesIncludingBothTweetId.Count, 3);

            for (int i = 0; i < favouritesIncludingBothTweet.Count; ++i)
            {
                Assert.AreEqual(favouritesIncludingBothTweet[i].Equals(favouritesIncludingBothTweetId[i]), true);
            }

            // Checking the result of until and since
            List<ITweet> favouritesIncludingSourceTweet = creator.GetFavouritesBetweenIds(tweet1, tweet3, 20, true, false);
            List<ITweet> favouritesIncludingUntilTweet = creator.GetFavouritesBetweenIds(tweet1, tweet3, 20, false, true);

            Assert.AreEqual(favouritesIncludingSourceTweet.Count, 2);
            Assert.AreEqual(favouritesIncludingUntilTweet.Count, 2);

            Assert.AreEqual(favouritesIncludingSourceTweet[0].Equals(tweet2), true);
            Assert.AreEqual(favouritesIncludingSourceTweet[1].Equals(tweet1), true);

            Assert.AreEqual(favouritesIncludingUntilTweet[0].Equals(tweet3), true);
            Assert.AreEqual(favouritesIncludingUntilTweet[1].Equals(tweet2), true);

            // Analyzing the result
            List<ITweet> favouritesSinceTweet = creator.GetFavouritesBetweenIds(tweet1, tweet3);
            List<ITweet> favouritesSinceId = creator.GetFavouritesBetweenIds(tweet1.Id, tweet3.Id);

            Assert.AreEqual(favouritesSinceTweet.Count, favouritesSinceId.Count);

            for (int i = 0; i < favouritesSinceTweet.Count; ++i)
            {
                Assert.AreEqual(favouritesSinceTweet[i].Equals(favouritesSinceId[i]), true);
            }

            Assert.AreEqual(favouritesSinceTweet.Count, 1);
            Assert.AreEqual(favouritesSinceTweet[0].Equals(tweet2), true);

            // Checking after Tweet1 deletion
            tweet3.Destroy();

            favouritesSinceTweet = creator.GetFavouritesBetweenIds(tweet1, tweet3);

            Assert.AreEqual(favouritesSinceTweet.Count, 1);
            Assert.AreEqual(favouritesSinceTweet[0].Equals(tweet2), true);

            tweet1.Destroy();

            favouritesSinceTweet = creator.GetFavouritesBetweenIds(tweet1, tweet3);

            Assert.AreEqual(favouritesSinceTweet.Count, 1);
            Assert.AreEqual(favouritesSinceTweet[0].Equals(tweet2), true);

            tweet2.Destroy();

            favouritesSinceTweet = creator.GetFavouritesBetweenIds(tweet1, tweet3);

            Assert.AreEqual(favouritesSinceTweet.Count, 0);
        } 
        #endregion
    }
}
