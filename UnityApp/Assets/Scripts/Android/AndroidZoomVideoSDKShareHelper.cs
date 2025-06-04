using System;
using UnityEngine;

public class AndroidZoomVideoSDKShareHelper
{
    private AndroidJavaObject _shareHelper;

    public AndroidZoomVideoSDKShareHelper(AndroidJavaObject shareHelper)
	{
        _shareHelper = shareHelper;
    }

    private AndroidJavaObject GetShareHelper()
    {
        if (_shareHelper != null)
        {
            return _shareHelper;
        }

        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_SHARE_HELPER_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        {
            _shareHelper = zoomSDK.Call<AndroidJavaObject>("getShareHelper");
        }
        if (_shareHelper == null)
        {
            Debug.LogError("ZoomVideoSdkShareHelper:: No share helper found.");
        }

        return _shareHelper;
    }

    public void StartShareScreen(Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        {
            AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject shareHelper = GetShareHelper();
            if (shareHelper != null)
            {
                activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    string mpsStr = activity.GetStatic<string>("MEDIA_PROJECTION_SERVICE");
                    AndroidJavaObject manager = activity.Call<AndroidJavaObject>("getSystemService", mpsStr);
                    if (manager != null)
                    {
                        AndroidJavaObject captureIntent = manager.Call<AndroidJavaObject>("createScreenCaptureIntent");
                        activity.Call("startActivityForResult", captureIntent, 1001);
                    }
                }));
            }
        }
    }

    public void StopShare(Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            AndroidJavaObject shareHelper = GetShareHelper();
            if (shareHelper != null)
            {
                activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    callback(AndroidZoomVideoSDKErrors.GetEnum(GetShareHelper().Call<int>("stopShare")));
                }));
            }
        }
    }

    public bool IsSharingOut()
    {
        return (bool)(GetShareHelper()?.Call<bool>("isSharingOut"));
    }

    public bool IsScreenSharingOut()
    {
        return (bool)(GetShareHelper()?.Call<bool>("isScreenSharingOut"));
    }

    public bool IsOtherUserSharingScreen()
    {
        return (bool)(GetShareHelper()?.Call<bool>("isOtherSharing"));
    }

    public void LockScreenShare(bool enable, Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            AndroidJavaObject shareHelper = GetShareHelper();
            if (shareHelper != null)
            {
                activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    callback(AndroidZoomVideoSDKErrors.GetEnum(GetShareHelper().Call<int>("lockShare", enable)));
                }));
            }
        }
    }

    public bool IsScreenSharingLocked()
    {
        return (bool)(GetShareHelper()?.Call<bool>("isShareLocked"));
    }

    public void EnableShareDeviceAudio(bool enable, Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            AndroidJavaObject shareHelper = GetShareHelper();
            if (shareHelper != null)
            {
                activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    callback(AndroidZoomVideoSDKErrors.GetEnum(GetShareHelper().Call<int>("enableShareDeviceAudio", enable)));
                }));
            }
        }
    }

    public bool IsShareDeviceAudioEnabled()
    {
        return (bool)(GetShareHelper()?.Call<bool>("isShareDeviceAudioEnabled"));
    }
}

