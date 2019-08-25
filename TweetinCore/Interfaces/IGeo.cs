namespace TweetinCore.Interfaces
{
    /// <summary>
    /// Geographic information of a location
    /// </summary>
    public interface IGeo
    {
        #region IGeo Properties

        // TODO : Complete description
        string GeoType { get; set; }

        /// <summary>
        /// Coordinates of the geographic location
        /// </summary>
        ICoordinates GeoCoordinates { get; set; }
 
        #endregion
    }
}