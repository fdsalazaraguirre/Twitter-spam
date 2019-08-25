namespace TweetinCore.Interfaces
{
    /// <summary>
    /// Coordinates of a geographical location
    /// </summary>
    public interface ICoordinates
    {
        #region ICoordinates Properties
        
        /// <summary>
        /// Lattitude of the coordinate
        /// </summary>
        double Lattitude { get; set; }

        /// <summary>
        /// Longitude of the coordinate
        /// </summary>
        double Longitude { get; set; } 
        
        #endregion
    }
}
