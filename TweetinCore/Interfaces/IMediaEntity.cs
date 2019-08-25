using System;
using System.Collections.Generic;

namespace TweetinCore.Interfaces
{
    /// <summary>
    /// Media element posted in Twitter
    /// </summary>
    public interface IMediaEntity
    {
        #region IMediaEntity Properties

        /// <summary>
        /// Media Id
        /// </summary>
        long? Id { get; set; }

        /// <summary>
        /// Media Id as a string
        /// </summary>
        string IdStr { get; set; }

        /// <summary>
        /// Url of the media
        /// </summary>
        string MediaURL { get; set; }

        /// <summary>
        /// Secured Url of the media
        /// </summary>
        string MediaURLHttps { get; set; }

        /// <summary>
        /// URL properties provide information related with a URL.
        /// For example it stores both a shorten URL and the real
        /// location of a Media
        /// </summary>
        IUrlEntity URLProperties { get; set; }

        /// <summary>
        /// Type of Media
        /// </summary>
        string MediaType { get; set; }

        /// <summary>
        /// Dimensions related with the different possible views of 
        /// a same Media element
        /// </summary>
        IDictionary<String, IMediaEntitySize> Sizes { get; set; } 
        #endregion
    }
}
