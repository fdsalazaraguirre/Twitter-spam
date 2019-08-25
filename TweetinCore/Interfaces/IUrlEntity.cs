using System.Collections.Generic;

namespace TweetinCore.Interfaces
{
    /// <summary>
    /// Information related with an URL in twitter
    /// </summary>
    public interface IUrlEntity
    {
        /// <summary>
        /// Real url
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Message displayed instead of the url
        /// </summary>
        string DisplayedUrl { get; set; }

        /// <summary>
        /// The fully resolved URL
        /// </summary>
        string ExpandedUrl { get; set; }

        /// <summary>
        /// The character positions the url was extracted from
        /// </summary>
        IList<int> Indices { get; set; }
    }
}
