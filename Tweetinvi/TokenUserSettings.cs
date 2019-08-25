using System.Collections.Generic;
using Tweetinvi.Utils;

namespace Tweetinvi
{
    /// <summary>
    /// Settings of a TokenUser
    /// </summary>
    public class TokenUserSettings
    {
        #region Public Properties
        // TODO : Implement time_zone
        public bool? Protected { get; private set; }
        public string ScreenName { get; private set; }
        public bool? AlwaysUseHttps { get; private set; }
        public bool? UseCookiePersonalization { get; private set; }
        // TODO : Implement sleep_time
        public bool? GeoEnabled { get; private set; }
        public string Language { get; private set; }
        public bool? DiscoverableByEmail { get; private set; }
        public bool? DiscoverableByMobilePhone { get; set; }
        public bool? DisplaySensitiveMedia { get; private set; }
        // TODO : Implement trend_location 
        #endregion

        /// <summary>
        /// Create settings based on information retrieved from twitter
        /// </summary>
        /// <param name="settingsValues"></param>
        public TokenUserSettings(Dictionary<string, object> settingsValues)
        {
            // TODO : Implement time_zone here
            Protected = settingsValues.GetProp("protected") as bool?;
            ScreenName = settingsValues.GetProp("screen_name") as string;
            AlwaysUseHttps = settingsValues.GetProp("always_use_https") as bool?;
            UseCookiePersonalization = settingsValues.GetProp("use_cookie_personalization") as bool?;
            // TODO : Implement sleep_time here
            GeoEnabled = settingsValues.GetProp("geo_enabled") as bool?;
            Language = settingsValues.GetProp("language") as string;
            DiscoverableByEmail = settingsValues.GetProp("discoverable_by_email") as bool?;
            DiscoverableByMobilePhone = settingsValues.GetProp("discoverable_by_mobile_phone") as bool?;
            DisplaySensitiveMedia = settingsValues.GetProp("display_sensitive_media") as bool?;
            // TODO : Implement trend_location here
        }
    }
}
