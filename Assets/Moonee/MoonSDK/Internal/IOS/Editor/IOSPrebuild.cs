using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Globalization;
using Google;

namespace Moonee.MoonSDK.Internal.Editor
{
    public class IOSPrebuild : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        private const float MinIosVersion = 13.0f;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.iOS) 
            {
                return;
            }
            SetResolverSettings();
            SetPlayerSettings();
        }

        private static void SetResolverSettings()
        {
            IOSResolver.PodfileGenerationEnabled = true;
            IOSResolver.PodToolExecutionViaShellEnabled = true;
            IOSResolver.UseProjectSettings = true;
            IOSResolver.CocoapodsIntegrationMethodPref = IOSResolver.CocoapodsIntegrationMethod.Project;
        }

        private static void SetPlayerSettings()
        {
            PlayerSettings.iOS.allowHTTPDownload = true;

            bool isToChangeVErsion = true;

            if (float.TryParse(PlayerSettings.iOS.targetOSVersionString, out float iosMinVersion))
            {
                if (iosMinVersion >= MinIosVersion) 
                {
                    isToChangeVErsion = false;
                }
            }
            if (isToChangeVErsion) 
            {
                PlayerSettings.iOS.targetOSVersionString = MinIosVersion.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}