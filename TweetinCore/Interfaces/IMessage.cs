using System;

namespace TweetinCore.Interfaces
{
    /// <summary>
    /// Message that can be sent privately between Twitter users
    /// </summary>
    public interface IMessage : ITwitterObject, IEquatable<IMessage>
    {
        #region IMessage Properties

        /// <summary>
        /// Id of the Message
        /// </summary>
        long Id { get; set; }

        /// <summary>
        /// Text contained in the message
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Creation date of the message
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// User who sent the message
        /// </summary>
        IUser Sender { get; set; }

        /// <summary>
        /// Receiver of the message
        /// </summary>
        IUser Receiver { get; set; }

        #endregion

        #region IMessage Methods

        /// <summary>
        /// Send a message to a user
        /// </summary>
        /// <param name="token">Token used to send the message</param>
        /// <param name="receiver">The receiver must be a Friend</param>
        /// <returns>If the message has successfully be sent</returns>
        bool Send(IToken token = null, IUser receiver = null);

        #endregion
    }
}
