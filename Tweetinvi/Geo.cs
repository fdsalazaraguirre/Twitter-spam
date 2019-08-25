using System.Collections.Generic;
using TweetinCore.Interfaces;
using Tweetinvi.Utils;

namespace Tweetinvi
{
    /// <summary>
    /// Geographic information of a location
    /// </summary>
    public class Geo : IGeo
    {
        #region Private Attributes

        private string _type;
        private ICoordinates _coordinates;

        #endregion

        #region Public Attributes

        public string GeoType
        {
            get { return _type; }
            set { _type = value; }
        }

        public ICoordinates GeoCoordinates
        {
            get { return _coordinates; }
            set { _coordinates = value; }
        }

        #endregion

        /// <summary>
        /// Create a Geo object
        /// </summary>
        /// <param name="type">Geo Type</param>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        private Geo(string type, double latitude, double longitude)
        {
            _type = type;
            _coordinates = new Coordinates(latitude, longitude);
        }

        /// <summary>
        /// Create a Geo object from geo information
        /// </summary>
        /// <param name="geoObject">Geo information</param>
        /// <returns>Geo object</returns>
        public static Geo Create(dynamic geoObject)
        {
            if (!(geoObject is Dictionary<string, object>))
            {
                return null;
            }

            string type = geoObject["type"];

            double latitude = (double)geoObject["coordinates"][0];
            double longitude = (double)geoObject["coordinates"][1];

            return new Geo(type, latitude, longitude);
        }
    }
}
