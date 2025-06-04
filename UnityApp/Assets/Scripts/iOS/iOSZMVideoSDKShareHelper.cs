#if UNITY_IOS
using System;
using System.Runtime.InteropServices;

public class IOSZMVideoSDKShareHelper
{
    public IOSZMVideoSDKShareHelper(IntPtr _objC_ZMVideoSDKShareHelper)
    {
        objC_ZMVideoSDKShareHelper = _objC_ZMVideoSDKShareHelper;
    }

    [DllImport ("__Internal")]
	private static extern void deallocate_ObjCWrapper(IntPtr zmVideoSDKShareHelper);
    ~IOSZMVideoSDKShareHelper()
    {
        deallocate_ObjCWrapper(objC_ZMVideoSDKShareHelper);
    }

    private IntPtr objC_ZMVideoSDKShareHelper;
    public IntPtr ObjC_ZMVideoSDKShareHelper
    {
        get { return objC_ZMVideoSDKShareHelper; }
    }

    public void StartShareScreen(Action<ZMVideoSDKErrors> callback)
    {
        throw new NotImplementedException();
    }
    public void StopShare(Action<ZMVideoSDKErrors> callback)
    {
        throw new NotImplementedException();
    }

    [DllImport ("__Internal")]
	private static extern bool isSharingOut(IntPtr zmVideoSDKShareHelper);
    public bool IsSharingOut()
    {
        return isSharingOut(objC_ZMVideoSDKShareHelper);
    }

    [DllImport ("__Internal")]
	private static extern bool isScreenSharingOut(IntPtr zmVideoSDKShareHelper);
    public bool IsScreenSharingOut()
    {
        return isScreenSharingOut(objC_ZMVideoSDKShareHelper);
    }

    [DllImport ("__Internal")]
	private static extern bool isOtherUserSharingScreen(IntPtr zmVideoSDKShareHelper);
    public bool IsOtherUserSharingScreen()
    {
        return isOtherUserSharingScreen(objC_ZMVideoSDKShareHelper);
    }

    [DllImport ("__Internal")]
	private static extern ZMVideoSDKErrors lockScreenShare(IntPtr zmVideoSDKShareHelper, bool enable);
    public void LockScreenShare(bool enable, Action<ZMVideoSDKErrors> callback)
    {
        callback(lockScreenShare(objC_ZMVideoSDKShareHelper, enable));
    }

    [DllImport ("__Internal")]
	private static extern bool isScreenSharingLocked(IntPtr zmVideoSDKShareHelper);
    public bool IsScreenSharingLocked()
    {
        return isScreenSharingLocked(objC_ZMVideoSDKShareHelper);
    }

    [DllImport ("__Internal")]
	private static extern ZMVideoSDKErrors enableShareDeviceAudio(IntPtr zmVideoSDKShareHelper, bool enable);
    public void EnableShareDeviceAudio(bool enable, Action<ZMVideoSDKErrors> callback)
    {
        callback(enableShareDeviceAudio(objC_ZMVideoSDKShareHelper, enable));
    }

    [DllImport ("__Internal")]
	private static extern bool isShareDeviceAudioEnabled(IntPtr zmVideoSDKShareHelper);
    public bool IsShareDeviceAudioEnabled()
    {
        return isShareDeviceAudioEnabled(objC_ZMVideoSDKShareHelper);
    }
}
#endif
