using System.Collections.Generic;
using UnityEngine;

public class AndroidZoomVideoSDKVideoPreferenceMode
{

    public AndroidZoomVideoSDKVideoPreferenceMode()
    {
    }

    private static AndroidJavaClass preferenceModeClass = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_VIDEO_PREFERENCE_MODE_PATH);

    private static Dictionary<ZMVideoSDKVideoPreferenceMode, AndroidJavaObject> preferenceModeDic
        = new Dictionary<ZMVideoSDKVideoPreferenceMode, AndroidJavaObject>()
        {
            {ZMVideoSDKVideoPreferenceMode.ZMVideoSDKVideoPreferenceMode_Balance, preferenceModeClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKVideoPreferenceMode_Balance")},
            {ZMVideoSDKVideoPreferenceMode.ZMVideoSDKVideoPreferenceMode_Sharpness, preferenceModeClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKVideoPreferenceMode_Sharpness")},
            {ZMVideoSDKVideoPreferenceMode.ZMVideoSDKVideoPreferenceMode_Smoothness, preferenceModeClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKVideoPreferenceMode_Smoothness")},
            {ZMVideoSDKVideoPreferenceMode.ZMVideoSDKVideoPreferenceMode_Custom, preferenceModeClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKVideoPreferenceMode_Custom")},
        };

    public static AndroidJavaObject GetJavaObject(ZMVideoSDKVideoPreferenceMode mode)
    {
        AndroidJavaObject preferenceMode;
        try
        {
            preferenceMode = preferenceModeDic[mode];
        }
        catch (KeyNotFoundException)
        {
            preferenceMode = null;
        }
        return preferenceMode;
    }
}

