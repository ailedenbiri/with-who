using System.IO;
using UnityEditor.Android;

namespace Moonee.MoonSDK.Internal.Editor
{
    public class AndroidPostBuild : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder => 0;

        public void OnPostGenerateGradleAndroidProject(string projectPath)
        {
            projectPath += "/../";
            var fileInfo = new FileInfo(Path.Combine(projectPath, "gradle.properties"));
            string[] content = { "android.useAndroidX=true", "android.enableJetifier = true" };
            string[] contentNew = {"android.useAndroidX=true", "android.enableJetifier = true", "unityStreamingAssets=.unity3d**STREAMING_ASSETS**" };
#if UNITY_2020_1_OR_NEWER
            File.WriteAllLines(fileInfo.FullName, contentNew);
#else
            File.WriteAllLines(fileInfo.FullName, content);
#endif
        }
    }
}