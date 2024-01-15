using System.Collections.Generic;

namespace Moonee.MoonSDK.Internal.Editor
{
    public static class BuildErrorConfig
    {
        public enum ErrorID
        {
            SettingsNoFacebookAppID,
            SettingsNoFacebookClientID,
            GANoIOSKey,
            GANoAndroidAndKey,
        }

        public static readonly Dictionary<ErrorID, string> ErrorMessageDict = new Dictionary<ErrorID, string>
         {
             {ErrorID.SettingsNoFacebookAppID, "Moon SDK Settings is missing Facebook App ID"},
             {ErrorID.SettingsNoFacebookClientID, "Moon SDK Settings is missing Facebook Client ID"},
             {ErrorID.GANoIOSKey, "Moon SDK Settings is missing iOS GameAnalytics keys"},
             {ErrorID.GANoAndroidAndKey, "Moon SDK Settings is missing Android GameAnalytics keys! add 'ignore' in both fields to disable Android analytics"},
         };
    }
}
