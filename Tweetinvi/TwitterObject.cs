using System.Collections.Generic;
using System.ComponentModel;
using TweetinCore.Events;
using TweetinCore.Interfaces;

namespace Tweetinvi
{
    /// <summary>
    /// Base class of all objects in Twitter
    /// </summary>
    public abstract class TwitterObject : ITwitterObject, INotifyPropertyChanged
    {
        #region Properties

        protected IToken _token;

        /// <summary>
        /// Token stored in the object to access information
        /// </summary>
        public IToken ObjectToken
        {
            get { return _token; }
            set { _token = value; }
        }

        protected bool _shareTokenWithChild;
        /// <summary>
        /// This parameter defines whether we want to share the Token
        /// With the TwitterObject hosted by this
        /// </summary>
        public bool ShareTokenWithChild
        {
            get { return _shareTokenWithChild; }
            set { _shareTokenWithChild = value; }
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// PropertyChanged event handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify that a property with a specific name has been changed
        /// </summary>
        /// <param name="name">Name of the property</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Populate a constructor automatically
        /// </summary>
        /// <param name="token"></param>
        /// <param name="populate"></param>
        /// <returns></returns>
        protected virtual IToken GetQueryTokenForConstructor(IToken token, bool populate = true)
        {
            if (TokenSingleton.AutomaticallyPopulateNewObjects && populate)
            {
                return GetQueryToken(token);
            }

            return token ?? _token;
        }

        /// <summary>
        /// Use a current Token or the token of current object
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected virtual IToken GetQueryToken(IToken token)
        {
            return token ?? _token ?? TokenSingleton.Token;
        }

        /// <summary>
        /// Populate the content of a TwitterObject
        /// </summary>
        /// <param name="data">Data retrieved from Twitter</param>
        public abstract void Populate(Dictionary<string, object> data);

        #endregion
    }
}
