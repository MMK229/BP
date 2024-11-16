using UnityEditor;
using UnityEditor.Build.Reporting;
using System;

public class BuildAutomation
{
    static string outputPath = "../output/ZoomVideoSDK/";
    static string buildPath = "../build/";
    private static bool BuildApp(BuildTarget target, string outputfilename)
    {
        string[] levels = new string[] { "Assets/Scenes/unity-zoom-video-sdk.unity"};
        BuildReport report = BuildPipeline.BuildPlayer(levels, buildPath + outputfilename, target, BuildOptions.None);

        if (report.summary.result == BuildResult.Failed)
        {
            Console.Write("Build failed for target " + target);
        }

        return report.summary.result == BuildResult.Succeeded ? true : false;
    }

    [MenuItem("MyTools/MacOS Postprocess Build")]
    public static bool BuildMacOSApp()
    {
        return BuildApp(BuildTarget.StandaloneOSX, "ZoomMacOSUnityVideoSDK");
    }

    [MenuItem("MyTools/iOS Postprocess Build")]
    public static bool BuildiOSApp()
    {
        return BuildApp(BuildTarget.iOS, "ZoomIOSUnityVideoSDK");
    }

    [MenuItem("MyTools/Android Postprocess Build")]
    public static bool BuildAndroidApp()
    {
        return BuildApp(BuildTarget.Android, "ZoomAndroidUnityVideoSDK.apk");
    }

    [MenuItem("MyTools/Windows Postprocess Build")]
    public static bool BuildWindowsApp()
    {
        return BuildApp(BuildTarget.StandaloneWindows64, "ZoomWindowsUnityVideoSDK");
    }

    [MenuItem("MyTools/Build Assets")]
    public static void ExportAssets()
    {
        AssetDatabase.ExportPackage("Assets", outputPath + "ZoomVideoSDK.unitypackage", ExportPackageOptions.Recurse|ExportPackageOptions.IncludeDependencies);
    }

    public static void BuildForCI()
    {
        BuildMacOSApp();
        BuildAndroidApp();
        BuildWindowsApp();
        BuildiOSApp();
        ExportAssets();
    }
}
