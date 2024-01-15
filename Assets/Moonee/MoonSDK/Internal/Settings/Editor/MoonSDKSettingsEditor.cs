using System;
using UnityEditor;
using UnityEngine;
using Moonee.MoonSDK.Internal.Analytics.Editor;

namespace Moonee.MoonSDK.Internal.Editor
{
    [CustomEditor(typeof(MoonSDKSettings))]
    public class MoonSDKSettingsEditor : UnityEditor.Editor
    {
        private MoonSDKSettings SDKSettings => target as MoonSDKSettings;

        [MenuItem("Moonee/Moon SDK/Edit Settings", false, 100)]
        private static void EditSettings()
        {
            Selection.activeObject = CreateMooneeSDKSettings();
        }
        private static MoonSDKSettings CreateMooneeSDKSettings()
        {
            MoonSDKSettings settings = MoonSDKSettings.Load();
            if (settings == null) {
                settings = CreateInstance<MoonSDKSettings>();

                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                    AssetDatabase.CreateFolder("Assets", "Resources");

                if (!AssetDatabase.IsValidFolder("Assets/Resources/MoonSDK"))
                    AssetDatabase.CreateFolder("Assets/Resources", "MoonSDK");

                AssetDatabase.CreateAsset(settings, "Assets/Resources/MoonSDK/Settings.asset");
                settings = MoonSDKSettings.Load();
            }
            return settings;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(15);

#if UNITY_IOS || UNITY_ANDROID      
            if (GUILayout.Button(Environment.NewLine + "Check and Sync Settings" + Environment.NewLine)) {
                CheckAndUpdateSdkSettings(SDKSettings);
            }
#else
            EditorGUILayout.HelpBox(BuildErrorConfig.ErrorMessageDict[BuildErrorConfig.ErrorID.INVALID_PLATFORM], MessageType.Error);   
#endif
        }
        private static void CheckAndUpdateSdkSettings(MoonSDKSettings settings)
        {
            GameAnalyticsPreBuild.CheckAndUpdateGameAnalyticsSettings(settings);
            FacebookPreBuild.CheckAndUpdateFacebookSettings(settings);
        }
    }
}