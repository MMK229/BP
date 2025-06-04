#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;
public class WindowsZoomVideoSDKVideoRawData
{
    private IntPtr _rawData;

    public WindowsZoomVideoSDKVideoRawData(IntPtr rawData)
    {
        _rawData = rawData;
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool canAddRef_c(IntPtr pRawData);

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool addRef_c(IntPtr pRawData);

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern int release_c(IntPtr pRawData);
    public bool CanAddRef()
    {
        return canAddRef_c(_rawData);
    }

    public bool AddRef()
    {
        return addRef_c(_rawData);
    }

    public int ReleaseRef()
    {
        return release_c(_rawData);
    }
}
#endif