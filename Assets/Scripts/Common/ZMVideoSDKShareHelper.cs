using System;

public class ZMVideoSDKShareHelper
{

#if UNITY_STANDALONE_OSX
    private MacZMVideoSDKShareHelper _videoSDKShareHelper = null;
    public ZMVideoSDKShareHelper(MacZMVideoSDKShareHelper videoSDKShareHelper)
    {
        _videoSDKShareHelper = videoSDKShareHelper;
    }
#elif UNITY_ANDROID
    private AndroidZoomVideoSDKShareHelper _videoSDKShareHelper = null;
    public ZMVideoSDKShareHelper(AndroidZoomVideoSDKShareHelper videoSDKShareHelper)
    {
        _videoSDKShareHelper = videoSDKShareHelper;
    }
#elif UNITY_STANDALONE_WIN
    private WindowsZoomVideoSDKShareHelper _videoSDKShareHelper = null;
    public ZMVideoSDKShareHelper(WindowsZoomVideoSDKShareHelper videoSDKShareHelper)
    {
        _videoSDKShareHelper = videoSDKShareHelper;
    }
#elif UNITY_IOS
    private IOSZMVideoSDKShareHelper _videoSDKShareHelper = null;
    public ZMVideoSDKShareHelper(IOSZMVideoSDKShareHelper videoSDKShareHelper)
    {
        _videoSDKShareHelper = videoSDKShareHelper;
    }
#endif

    /**
     * Share a selected screen through Intent.
     *
     * @param data the intent data for retriving the MediaProjection obtained from a succesful screen capture request.
     *             see:<a href="https://developer.android.com/reference/android/media/projection/MediaProjectionManager.html">
     *             https://developer.android.com/reference/android/media/projection/MediaProjectionManager.html</a>
     * @return If the function succeeds, the return value is Errors_Success.
     * Otherwise failed. To get extended error information, see {@link ZMVideoSDKErrors}.
     */

#if UNITY_ANDROID

    public void StartShareScreen(Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKShareHelper.StartShareScreen(callback);
    }
#elif UNITY_STANDALONE_WIN
    public ZMVideoSDKErrors StartShareScreen(string monitorId, ZMVideoSDKShareOption option)
    {
        return _videoSDKShareHelper.StartShareScreen(monitorId, option);
    }
#elif UNITY_STANDALONE_OSX
    public ZMVideoSDKErrors StartShareScreen(uint monitorId, ZMVideoSDKShareOption option)
    {
        return _videoSDKShareHelper.StartShareScreen(monitorId, option);
    }
#endif

    /**
     * Stop view or screen share.
     *
     * @return If the function succeeds, the return value is Errors_Success.
     * Otherwise failed. To get extended error information, see {@link ZMVideoSDKErrors}.
     */
    public void StopShare(Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKShareHelper.StopShare(callback);
    }

    /**
     * Determine whether the current user is sharing.
     *
     * @return true indicates the current user is sharing, otherwise false.
     */
    public bool IsSharingOut()
    {
        return _videoSDKShareHelper.IsSharingOut();
    }

    /**
     * Determine whether the current user is sharing the screen.
     *
     * @return true indicates the current user is sharing the screen, otherwise false.
     */
    public bool IsScreenSharingOut()
    {
        return _videoSDKShareHelper.IsScreenSharingOut();
    }

    /**
     * Determine whether other user is sharing.
     *
     * @return true indicates another user is sharing, otherwise false.
     */
    public bool IsOtherUserSharingScreen()
    {
        return _videoSDKShareHelper.IsOtherUserSharingScreen();
    }

    /**
     * Lock sharing the view or screen. Only the host can call this method.
     *
     * @param lock true to lock sharing.
     * @return If the function succeeds, the return value is Errors_Success.
     * Otherwise failed. To get extended error information, see {@link ZMVideoSDKErrors}.
     */
    public void LockScreenShare(bool enable, Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKShareHelper.LockScreenShare(enable, callback);
    }

    /**
     * Determine whether sharing the view or screen is locked.
     *
     * @return true indicates that sharing is locked, otherwise false.
     */
    public bool IsScreenSharingLocked()
    {
        return _videoSDKShareHelper.IsScreenSharingLocked();
    }

    /**
     * Enable or disable the device sound when sharing. The SDK does not support sharing device audio, for example, when you've enabled virtual speaker.
     * This feature need Build.VERSION.SDK_INT >=29  and has RECORD_AUDIO Permission.
     *
     * @param enable true to enable. false to disable.
     * @return If the function succeeds, the return value is {@link ZMVideoSDKErrors#Errors_Success}. Otherwise failed. To get extended error information, see {@link ZoomVideoSDKErrors}
     */
    public void EnableShareDeviceAudio(bool enable, Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKShareHelper.EnableShareDeviceAudio(enable, callback);
    }

    /**
     * Determine if the SDK has enabled share device sound.
     *
     * @return true if enabled, otherwise false.
     */
    public bool IsShareDeviceAudioEnabled()
    {
        return _videoSDKShareHelper.IsShareDeviceAudioEnabled();
    }
}