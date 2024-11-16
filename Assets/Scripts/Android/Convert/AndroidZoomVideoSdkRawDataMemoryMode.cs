using System.Collections.Generic;
using UnityEngine;

public class AndroidZoomVideoSdkRawDataMemoryMode
{

    public AndroidZoomVideoSdkRawDataMemoryMode()
    {
    }

    private static AndroidJavaClass memoryMode = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_RAW_DATA_MEMORY_MODE_PATH);

    private static Dictionary<ZMVideoSDKRawDataMemoryMode, AndroidJavaObject> rawDataMemoryMode
        = new Dictionary<ZMVideoSDKRawDataMemoryMode, AndroidJavaObject>()
        {
            {ZMVideoSDKRawDataMemoryMode.ZMVideoSDKRawDataMemoryMode_Heap, memoryMode.GetStatic<AndroidJavaObject>("ZoomVideoSDKRawDataMemoryModeHeap")},
            {ZMVideoSDKRawDataMemoryMode.ZMVideoSDKRawDataMemoryMode_Stack, memoryMode.GetStatic<AndroidJavaObject>("ZoomVideoSDKRawDataMemoryModeStack")},
        };

    public static AndroidJavaObject GetJavaObject(ZMVideoSDKRawDataMemoryMode name)
    {
        AndroidJavaObject mode;
        try
        {
            mode = rawDataMemoryMode[name];
        }
        catch (KeyNotFoundException)
        {
            mode = null;
        }
        return mode;
    }
}

