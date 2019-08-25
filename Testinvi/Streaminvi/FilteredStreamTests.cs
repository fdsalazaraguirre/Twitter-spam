using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Streaminvi;
using TweetinCore.Enum;
using TweetinCore.Interfaces;
using TweetinCore.Interfaces.StreamInvi;
using System.Threading;
using Tweetinvi;
using Timer = System.Timers.Timer;

namespace Testinvi.Streaminvi
{
    [TestClass]
    public class FilteredStreamTests
    {
        [TestInitialize]
        public void Initialize()
        {
            TokenTestSingleton.Initialize(true);
        }

        [TestMethod]
        public void StartStreamTrackRandomUniqueWord()
        {
            // Arrange
            var randomWord = String.Format("Tweetinvi{0}", new Random().Next());
            var expectedMessage = String.Format("Hello {0}", randomWord);
            bool result = false;

            Thread t = new Thread(() =>
            {
                IFilteredStream stream = new FilteredStream();

                stream.AddTrack(randomWord);

                Func<ITweet, bool> listen = delegate(ITweet tweet)
                {
                    if (tweet != null)
                    {
                        result = tweet.Text == expectedMessage;
                    }

                    // End the stream
                    return false;
                };

                stream.StartStream(TokenSingleton.Token, listen);
            });

            t.Start();

            // Act
            ITweet newTweet = new Tweet(expectedMessage, TokenTestSingleton.Instance);
            newTweet.Publish();

            Thread.Sleep(500);
            t.Join();

            // Assert
            Assert.AreEqual(result, true);

            // Cleanup
            newTweet.Destroy();
        }

        [TestMethod]
        public void StartStreamTrackWord1ORWord2InTheSameTweet()
        {
            // Arrange
            var randomWord1 = String.Format("Tweetinvi{0}", new Random().Next());
            var randomWord2 = String.Format("Tweetinvi2{0}", new Random().Next());

            var expectedMessage1 = String.Format("Hello {0}", randomWord1);
            var expectedMessage2 = String.Format("Hello {0}", randomWord2);
            var expectedMessage3 = String.Format("Hello {0} {1}", randomWord1, randomWord2);

            int i = 0;

            Thread t = new Thread(() =>
            {
                IFilteredStream stream = new FilteredStream();

                stream.AddTrack(randomWord1);
                stream.AddTrack(randomWord2);

                Func<ITweet, bool> listen = delegate(ITweet tweet)
                {

                    if (tweet != null)
                    {
                        bool result = tweet.Text == expectedMessage1 ||
                                      tweet.Text == expectedMessage2 ||
                                      tweet.Text == expectedMessage3;

                        if (result)
                        {
                            Debug.WriteLine(tweet.Text);
                            ++i;
                        }
                    }

                    // End the stream
                    return true;
                };

                Timer timer = new Timer(5000);
                timer.Elapsed += (sender, args) =>
                {
                    timer.Stop();
                    stream.StopStream();
                };

                timer.Start();

                stream.StartStream(TokenSingleton.Token, listen);
            });

            t.Start();

            // Act
            ITweet newTweet1 = new Tweet(expectedMessage1, TokenTestSingleton.Instance);
            newTweet1.Publish();

            ITweet newTweet2 = new Tweet(expectedMessage2, TokenTestSingleton.Instance);
            newTweet2.Publish();

            ITweet newTweet3 = new Tweet(expectedMessage3, TokenTestSingleton.Instance);
            newTweet3.Publish();

            t.Join();

            // Cleanup
            newTweet1.Destroy();
            newTweet2.Destroy();
            newTweet3.Destroy();

            // Assert
            Assert.AreEqual(i, 3);
        }

        [TestMethod]
        public void StartStreamTrackWord1ANDWord2InTheSameTweet()
        {
            // Arrange
            var randomWord1 = String.Format("Tweetinvi{0}", new Random().Next());
            var randomWord2 = String.Format("Tweetinvi2{0}", new Random().Next());

            var expectedMessage1 = String.Format("Hello {0}", randomWord1);
            var expectedMessage2 = String.Format("Hello {0}", randomWord2);
            var expectedMessage3 = String.Format("Hello {0} and {1}", randomWord1, randomWord2);

            int i = 0;

            Thread t = new Thread(() =>
            {
                IFilteredStream stream = new FilteredStream();

                stream.AddTrack(String.Format("{0} {1}", randomWord1, randomWord2));

                Func<ITweet, bool> listen = delegate(ITweet tweet)
                {

                    if (tweet != null)
                    {
                        bool result = tweet.Text == expectedMessage1 ||
                                      tweet.Text == expectedMessage2 ||
                                      tweet.Text == expectedMessage3;

                        if (result)
                        {
                            Debug.WriteLine(tweet.Text);
                            ++i;
                        }
                    }

                    // End the stream
                    return true;
                };

                Timer timer = new Timer(5000);
                timer.Elapsed += (sender, args) =>
                {
                    timer.Stop();
                    stream.StopStream();
                };

                timer.Start();

                stream.StartStream(TokenSingleton.Token, listen);
            });

            t.Start();

            // Act
            ITweet newTweet1 = new Tweet(expectedMessage1, TokenTestSingleton.Instance);
            newTweet1.Publish();

            ITweet newTweet2 = new Tweet(expectedMessage2, TokenTestSingleton.Instance);
            newTweet2.Publish();

            ITweet newTweet3 = new Tweet(expectedMessage3, TokenTestSingleton.Instance);
            newTweet3.Publish();

            t.Join();

            // Cleanup
            newTweet1.Destroy();
            newTweet2.Destroy();
            newTweet3.Destroy();

            // Assert
            Assert.AreEqual(i, 1);
        }
    }

    [TestClass]
    public class FilteredStreamMoqTests
    {
        private MockRepository _mockery;
        private Mock<IToken> _mockToken;
        private const string BASE_URL = "https://stream.twitter.com/1.1/statuses/filter.json?";

        [TestInitialize]
        public void TestInitialize()
        {
            _mockery = new MockRepository(MockBehavior.Default)
            {
                DefaultValue = DefaultValue.Mock
            };

            _mockToken = _mockery.Create<IToken>();
        }

        #region QueryConstructor

        [TestMethod]
        public void StartStream_NoTrack_CreateProperWebRequest()
        {
            // Arrange
            var streamFilter = CreateStreamFilter();

            // Act
            try
            {
                streamFilter.StartStream(_mockToken.Object, (Action<ITweet>)null);
            }
            catch (InvalidCastException) { }

            // Assert
            _mockToken.Verify(x => x.GetQueryWebRequest(BASE_URL, HttpMethod.POST, null));
        }

        [TestMethod]
        public void StartStream_UniqueTrack_CreateProperWebRequest()
        {
            // Arrange
            var streamFilter = CreateStreamFilter();
            string track1 = "Track1 is good";
            streamFilter.AddTrack(track1);

            // Act
            try
            {
                streamFilter.StartStream(_mockToken.Object, (Action<ITweet>)null);
            }
            catch (InvalidCastException) { }

            // Assert
            string expectedURL = String.Format("{0}track={1}", BASE_URL, Uri.EscapeDataString(track1));
            _mockToken.Verify(x => x.GetQueryWebRequest(expectedURL, HttpMethod.POST, null));
        }

        [TestMethod]
        public void StartStream_MultipleTracks_CreateProperWebRequest()
        {
            // Arrange
            var streamFilter = CreateStreamFilter();

            const string track1 = "Track1 is good";
            const string track2 = "Track2 is too";
            streamFilter.AddTrack(track1);
            streamFilter.AddTrack(track2);

            // Act
            try
            {
                streamFilter.StartStream(_mockToken.Object, (Action<ITweet>)null);
            }
            catch (InvalidCastException) { }

            // Assert
            string expectedURL = String.Format("{0}track={1}%2C{2}", BASE_URL,
                                               Uri.EscapeDataString(track1),
                                               Uri.EscapeDataString(track2));
            _mockToken.Verify(x => x.GetQueryWebRequest(expectedURL, HttpMethod.POST, null));
        }

        #endregion

        public IFilteredStream CreateStreamFilter()
        {
            return new FilteredStream();
        }
    }
}
