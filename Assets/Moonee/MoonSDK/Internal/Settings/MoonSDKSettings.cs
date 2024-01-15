using UnityEngine;

namespace Moonee.MoonSDK.Internal
{
    [CreateAssetMenu(fileName = "Assets/Resources/Moonee/MoonlightSDKSettings", menuName = "MoonlightSDK/Settings file")]
    public class MoonSDKSettings : ScriptableObject
    {
        private const string SETTING_RESOURCES_PATH = "MoonSDK/Settings";

        public static MoonSDKSettings Load() => Resources.Load<MoonSDKSettings>(SETTING_RESOURCES_PATH);

        [Header("Moonlight SDK version 1.0.2", order = 0)]

        [Space(10)]
        public bool GameAnalytics;

        [ConditionalHide("GameAnalytics")]
        [Tooltip("Your GameAnalytics Ios Game Key")]
        public string gameAnalyticsIosGameKey;

        [ConditionalHide("GameAnalytics")]
        [Tooltip("Your GameAnalytics Ios Secret Key")]
        public string gameAnalyticsIosSecretKey;

        [ConditionalHide("GameAnalytics")]
        [Tooltip("Your GameAnalytics Android Game Key")]
        public string gameAnalyticsAndroidGameKey;

        [ConditionalHide("GameAnalytics")]
        [Tooltip("Your GameAnalytics Android Secret Key")]
        public string gameAnalyticsAndroidSecretKey;

        [Space(10)]
        public bool Facebook;

        [ConditionalHide("Facebook")]
        [Tooltip("The Facebook App Id of your game")]
        public string facebookAppId;
        [ConditionalHide("Facebook")]
        [Tooltip("The Facebook Client Id of your game")]
        public string facebookClientId;

        [Space(10)]
        [Tooltip("Youк studio logo for splash screen")]
        public bool StudioLogo;
        [ConditionalHide("StudioLogo")]
        public Sprite studioLogoSprite;
    }
}

