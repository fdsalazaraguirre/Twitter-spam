using System;
using System.Collections.Generic;
using System.Globalization;
using TweetinCore.Interfaces;
using Tweetinvi.Properties;
using Tweetinvi.Utils;

namespace Tweetinvi
{
    /// <summary>
    /// Message that can be sent privately between Twitter users
    /// </summary>
    public class Message : TwitterObject, IMessage
    {
        #region Private Attributes

        private long _id;
        private DateTime _createdAt;
        private IUser _sender;
        private IUser _receiver;
        private string _text;

        private bool _messageCreatedFromApi;

        #endregion

        #region Twitter API Fields

        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { _createdAt = value; }
        }

        public IUser Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        public IUser Receiver
        {
            get { return _receiver; }
            set { _receiver = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        #endregion

        #region Tweetinvi API Fields

        /// <summary>
        /// Whether the message is coming from Twitter or from the API
        /// </summary>
        public bool MessageCreatedFromAPI
        {
            get { return _messageCreatedFromApi; }
        }

        #endregion

        #region Constructors
        private Message()
        {
            // default constructor not available from outside the class
        }

        /// <summary>
        /// Create a Message from its id. Its content is retrieved from the Twitter API using the valid token given in parameter.
        /// Throw an argument exception if one of the parameters is null.
        /// </summary>
        /// <param name="id">id of the message</param>
        /// <param name="token">token used to request the message's data to the Twitter API</param>
        /// <exception cref="ArgumentException">One of the argument is null</exception>
        public Message(long id, IToken token)
            : this()
        {
            // id and token must be specified to be able to retrieve the message data from the Twitter API
            if (token == null)
            {
                throw new ArgumentException("Token must be defined to retrieve content.");
            }

            _token = token;
            // Retrieve the message data from the Twitter API
            object messageContent = token.ExecuteGETQuery(String.Format(Resources.Messages_GetDirectMessage, id));

            // Populate the message with the data retrieved from the Twitter API
            Populate((Dictionary<String, object>)messageContent);
        }

        /// <summary>
        /// Create a Message from the object given in parameter
        /// Throw an argument exception if the message content is null or if its type is wrong.
        /// </summary>
        /// <param name="messageContent">Values of the message's attributes (type Dictionary[String, object])</param>
        /// <param name="token">Token for doing operation</param>
        /// <exception cref="ArgumentException">Argument is null or its type is wrong</exception>
        public Message(Dictionary<String, object> messageContent, IToken token = null)
            : this()
        {
            if (messageContent != null)
            {
                _token = token;
                Populate(messageContent);
            }
            else
            {
                throw new ArgumentException("Cannot create a message from a null object");
            }
        }

        /// <summary>
        /// Create a new Message that does not exist in Twitter
        /// </summary>
        /// <param name="text">Text to be sent</param>
        /// <param name="receiver">Receiver of the message</param>
        /// <param name="token">Token to perform operation</param>
        public Message(string text, IUser receiver, IToken token = null)
            : this()
        {
            _text = text;
            _receiver = receiver;
            _messageCreatedFromApi = true;
            _token = token;
        }

        #endregion

        #region Public methods

        #region Show
        #endregion

        #region Get Basic Message Info
        #endregion

        #region Send Message

        /// <summary>
        /// Send a message created from the application
        /// </summary>
        /// <param name="token">Token used to do the operation</param>
        /// <param name="receiver">User receiving the message</param>
        public bool Send(IToken token = null, IUser receiver = null)
        {
            token = GetQueryToken(token);

            if (token == null)
            {
                return false;
            }

            IUser messageReceiver = receiver ?? _receiver;

            if (messageReceiver == null)
            {
                throw new ArgumentException("Receiver cannot be null");
            }

            if (messageReceiver.Id == null && messageReceiver.ScreenName == null)
            {
                throw new ArgumentException("Receiver is invalid");
            }

            // Sending the message
            if (messageReceiver.Id != null)
            {
                token.ExecutePOSTQuery(String.Format(Resources.Messages_SendToUserId, _text, messageReceiver.Id));
            }
            else
            {
                token.ExecutePOSTQuery(String.Format(Resources.Messages_SendToUserScreenName,
                                                          _text, messageReceiver.ScreenName));
            }

            return true;
        }

        #endregion

        #endregion

        #region Private Methods
        /// <summary>
        /// Create a Message and fill it with some information given in parameter (creation date, id, text)
        /// This information can be retrieved directly from the content without creating any new object
        /// </summary>
        /// <param name="messageContent">Values of the message's attributes</param>
        private void FillBasicMessageInfoFromDictionary(Dictionary<String, object> messageContent)
        {
            this.CreatedAt = DateTime.ParseExact(messageContent.GetProp("created_at") as string,
            "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture);
            this.Id = Convert.ToInt64(messageContent.GetProp("id") as long?);
            this.Text = messageContent.GetProp("text") as string;
        }

        /// <summary>
        /// Create a Message and fill all its fields.
        /// Do nothing if the parameter is null.
        /// </summary>
        /// <param name="messageContent">Values of the message's attributes</param>
        public override void Populate(Dictionary<String, object> messageContent)
        {
            // cannot set the message fields if the content is not available
            if (messageContent == null)
            {
                return;
            }

            // set the basic fields of the message
            FillBasicMessageInfoFromDictionary(messageContent);

            if (messageContent.GetProp("sender") is Dictionary<String, object>)
            {
                this.Sender = User.Create(messageContent.GetProp("sender"));
            }

            if (messageContent.GetProp("recipient") is Dictionary<String, object>)
            {
                this.Receiver = User.Create(messageContent.GetProp("recipient"));
            }

            _messageCreatedFromApi = false;
        }
        #endregion

        #region IEquatable Members
        
        public bool Equals(IMessage other)
        {
            bool result = 
                Id == other.Id && 
                Text == other.Text &&
                Sender.Equals(other.Sender) &&
                Receiver.Equals(other.Receiver);

            return result;
        } 

        #endregion
    }
}