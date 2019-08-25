namespace TweetinCore.Interfaces
{
    /// <summary>
    /// Object created from Twitter
    /// </summary>
    public interface ITwitterObject
    {
        /// <summary>
        /// Token that the object host to execute query
        /// </summary>
        IToken ObjectToken { get; set; }
    }
}
