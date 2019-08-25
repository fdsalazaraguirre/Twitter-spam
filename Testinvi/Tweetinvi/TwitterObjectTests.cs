using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi;
using TweetinCore.Interfaces;
using TwitterToken;

namespace Testinvi
{
    [TestClass]
    public class TwitterObjectTests
    {
        private class MyTwitterObject : TwitterObject
        {
            public override void Populate(Dictionary<string, object> data)
            {
                throw new NotImplementedException();
            }

            public new IToken GetQueryToken(IToken token)
            {
                return base.GetQueryToken(token);
            }
        }

        #region GetQueryToken
        // TokenP = Token from Parameter
        // TokenS = Token Store in the TwitterObject
        // TokenI = Token Instance of the TokenSingleton

        [TestMethod]
        public void GetQueryToken_TokenPNotNull_ReturnTokenP()
        {
            // Arrange
            IToken tokenP = new Token("p", "p", "p", "p");
            MyTwitterObject o = new MyTwitterObject();

            // Act
            var result = o.GetQueryToken(tokenP);

            // Assert
            Assert.AreEqual(result, tokenP);
        }

        [TestMethod]
        public void GetQueryToken_TokenPNullTokenSNotNull_ReturnTokenS()
        {
            // Arrange
            IToken tokenS = new Token("s", "s", "s", "s");
            MyTwitterObject o = new MyTwitterObject();
            o.ObjectToken = tokenS;

            // Act
            var result = o.GetQueryToken(null);

            // Assert
            Assert.AreEqual(result, tokenS);
        }

        [TestMethod]
        public void GetQueryToken_TokenPNullTokenSNullTokenINotNull_ReturnTokenI()
        {
            // Arrange
            IToken tokenI = new Token("i", "i", "i", "i");
            MyTwitterObject o = new MyTwitterObject();
            TokenSingleton.Token = tokenI;

            // Act
            var result = o.GetQueryToken(null);

            // Assert
            Assert.AreEqual(result, tokenI);
        }

        [TestMethod]
        public void GetQueryToken_TokenPNullTokenSNotNullTokenINotNull_ReturnTokenS()
        {
            // Arrange
            IToken tokenS = new Token("s", "s", "s", "s");
            IToken tokenI = new Token("i", "i", "i", "i");

            MyTwitterObject o = new MyTwitterObject();

            o.ObjectToken = tokenS;
            TokenSingleton.Token = tokenI;

            // Act
            var result = o.GetQueryToken(null);

            // Assert
            Assert.AreEqual(result, tokenS);
        }

        [TestMethod]
        public void GetQueryToken_TokenPNotNullTokenSNotNullTokenINotNull_ReturnTokenS()
        {
            // Arrange
            IToken tokenP = new Token("p", "p", "p", "p");
            IToken tokenS = new Token("s", "s", "s", "s");
            IToken tokenI = new Token("i", "i", "i", "i");

            MyTwitterObject o = new MyTwitterObject();

            o.ObjectToken = tokenS;
            TokenSingleton.Token = tokenI;

            // Act
            var result = o.GetQueryToken(tokenP);

            // Assert
            Assert.AreEqual(result, tokenP);
        } 

        #endregion
    }
}
