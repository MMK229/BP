#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;

public class WindowsZoomVideoSDKRawDataPipe
{
    private IntPtr _rawDataPipe;
    private IntPtr _sdkRawDataDelegate;
    private string userId;
    private WindowsZoomVideoSDKRawDataPipeDelegate rawDataDelegate;

    public WindowsZoomVideoSDKRawDataPipe(IntPtr rawDataPipe, string userId)
    {
        _rawDataPipe = rawDataPipe;
        this.userId = userId;
    }
    public IntPtr Instance()
    {
        return _rawDataPipe;
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern IntPtr new_raw_data_delegate_instance_c(string userId);

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors subscribe_c(ZMVideoSDKResolution resolution, IntPtr pipe, IntPtr delegateObj);
    public ZMVideoSDKErrors Subscribe(ZMVideoSDKResolution resolution, IZMVideoSDKRawDataPipeDelegate dataDelegate)
    {
        _sdkRawDataDelegate = new_raw_data_delegate_instance_c(userId);
        rawDataDelegate = new WindowsZoomVideoSDKRawDataPipeDelegate();
        rawDataDelegate.AddDelegate(userId, dataDelegate);
        return subscribe_c(resolution, _rawDataPipe, _sdkRawDataDelegate);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors unSubscribe_c(IntPtr pipe, IntPtr delegateObj);
    public ZMVideoSDKErrors UnSubscribe()
    {
        rawDataDelegate.RemoveDelegate(userId);
        return unSubscribe_c(_rawDataPipe, _sdkRawDataDelegate);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKRawDataType getRawdataType_c(IntPtr pipe);
    public ZMVideoSDKRawDataType GetRawdataType()
    {
        return getRawdataType_c(_rawDataPipe);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKVideoStatus getVideoStatus_c(IntPtr pipe); 
    public ZMVideoSDKVideoStatus GetVideoStatus()
    {
        return getVideoStatus_c(_rawDataPipe);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKShareStatus getShareStatus_c(IntPtr pipe);
    public ZMVideoSDKShareStatus GetShareStatus()
    {
        return getShareStatus_c(_rawDataPipe);
    }

}
#endif