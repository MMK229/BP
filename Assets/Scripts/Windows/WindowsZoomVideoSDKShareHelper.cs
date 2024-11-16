#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;

public class WindowsZoomVideoSDKShareHelper
{
    private IntPtr shareHelper;
    public WindowsZoomVideoSDKShareHelper(IntPtr shareHelper)
    {
        this.shareHelper = shareHelper;
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern ZMVideoSDKErrors startShareScreen_c(IntPtr shareHelper, string monitorId, ref ZMVideoSDKShareOption option);
    public ZMVideoSDKErrors StartShareScreen(string monitorId, ZMVideoSDKShareOption option)
    {
        return startShareScreen_c(shareHelper, monitorId, ref option);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors stopShare_c(IntPtr shareHelper);
    public void StopShare(Action<ZMVideoSDKErrors> callback)
    {
        callback(stopShare_c(shareHelper));
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool isSharingOut_c(IntPtr shareHelper);
    public bool IsSharingOut()
    {
        return isSharingOut_c(shareHelper);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool isScreenSharingOut_c(IntPtr shareHelper);
    public bool IsScreenSharingOut()
    {
        return isScreenSharingOut_c(shareHelper);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool isOtherSharing_c(IntPtr shareHelper);
    public bool IsOtherUserSharingScreen()
    {
        return isOtherSharing_c(shareHelper);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors lockShare_c(IntPtr shareHelper, bool enable);
    public void LockScreenShare(bool enable, Action<ZMVideoSDKErrors> callback)
    {
        callback(lockShare_c(shareHelper, enable));
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool isShareLocked_c(IntPtr shareHelper);
    public bool IsScreenSharingLocked()
    {
        return isShareLocked_c(shareHelper);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors enableShareDeviceAudio_c(IntPtr shareHelper, bool enable);
    public void EnableShareDeviceAudio(bool enable, Action<ZMVideoSDKErrors> callback)
    {
        callback(enableShareDeviceAudio_c(shareHelper, enable));
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool isShareDeviceAudioEnabled_c(IntPtr shareHelper);
    public bool IsShareDeviceAudioEnabled()
    {
        return isShareDeviceAudioEnabled_c(shareHelper);
    }
}
#endif