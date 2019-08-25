namespace TweetinCore.Interfaces.StreamInvi
{
    /// <summary>
    /// Set of methods to add filters to a Stream
    /// </summary>
    public interface IFilteredStream : IStream
    {
        /// <summary>
        /// Add a keyword/sentence to Track
        /// </summary>
        void AddTrack(string track);

        /// <summary>
        /// Remove a keyword/sentence that was tracked
        /// </summary>
        void RemoveTrack(string track);

        /// <summary>
        /// Remove all tracked keywords
        /// </summary>
        void ClearTrack();
    }
}
