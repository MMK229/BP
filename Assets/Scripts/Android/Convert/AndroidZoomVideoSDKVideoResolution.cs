using System.Collections.Generic;
using UnityEngine;

public class AndroidZoomVideoSDKVideoResolution
{

    public AndroidZoomVideoSDKVideoResolution()
    {
    }

    private static AndroidJavaClass videoResolutionClass = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_VIDEO_RESOLUTION_PATH);

    private static Dictionary<ZMVideoSDKResolution, AndroidJavaObject> resolutionDic
        = new Dictionary<ZMVideoSDKResolution, AndroidJavaObject>()
        {
            {ZMVideoSDKResolution.ZMVideoSDKResolution_90P, videoResolutionClass.GetStatic<AndroidJavaObject>("VideoResolution_90P")},
            {ZMVideoSDKResolution.ZMVideoSDKResolution_180P, videoResolutionClass.GetStatic<AndroidJavaObject>("VideoResolution_180P")},
            {ZMVideoSDKResolution.ZMVideoSDKResolution_360P, videoResolutionClass.GetStatic<AndroidJavaObject>("VideoResolution_360P")},
            {ZMVideoSDKResolution.ZMVideoSDKResolution_720P, videoResolutionClass.GetStatic<AndroidJavaObject>("VideoResolution_720P")},
            {ZMVideoSDKResolution.ZMVideoSDKResolution_1080P, videoResolutionClass.GetStatic<AndroidJavaObject>("VideoResolution_1080P")},
        };

    public static AndroidJavaObject GetJavaObject(ZMVideoSDKResolution videoResolutionEnum)
    {
        AndroidJavaObject videoResolution;
        try
        {
            videoResolution = resolutionDic[videoResolutionEnum];
        }
        catch (KeyNotFoundException)
        {
            videoResolution = null;
        }
        return videoResolution;
    }
}

