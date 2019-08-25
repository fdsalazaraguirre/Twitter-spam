using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TweetinCore.Interfaces;
using TweetinCore.Interfaces.StreamInvi;
using Tweetinvi;
using Timer = System.Timers.Timer;

namespace Testinvi.Streaminvi
{
    [TestClass]
    public class SimpleStreamTests
    {
        [TestInitialize]
        public void Initialize()
        {
            TokenTestSingleton.Initialize(true);
        }

        [TestMethod]
        public void StartSampleStream()
        {
            // Arrange
            int i = 0;

            Thread t = new Thread(() =>
            {
                IStream stream = new SimpleStream("https://stream.twitter.com/1.1/statuses/sample.json");

                Action<ITweet> listen = delegate(ITweet tweet)
                {
                    if (tweet != null)
                    {
                        ++i;
                    }
                };

                Timer timer = new Timer(1000);
                timer.Elapsed += (sender, args) =>
                {
                    timer.Stop();
                    stream.StopStream();
                };

                timer.Start();

                stream.StartStream(TokenSingleton.Token, listen);
            });

            // Act
            t.Start();
            t.Join();

            // Assert
            Assert.AreNotEqual(i, 0);
        }

        // This should be a test for firehose stream but cannot be Tested
        // Without an account with privileged access
        public void StartFirehoseStream()
        {
            // Arrange
            int i = 0;

            Thread t = new Thread(() =>
            {
                IStream stream = new SimpleStream("https://stream.twitter.com/1.1/statuses/firehose.json");

                Action<ITweet> listen = delegate(ITweet tweet)
                {
                    if (tweet != null)
                    {
                        ++i;
                    }
                };

                Timer timer = new Timer(1000);
                timer.Elapsed += (sender, args) =>
                {
                    timer.Stop();
                    stream.StopStream();
                };

                timer.Start();

                stream.StartStream(TokenSingleton.Token, listen);
            });

            // Act
            t.Start();
            t.Join();

            // Assert
            Assert.AreNotEqual(i, 0);
        }
    }
}
