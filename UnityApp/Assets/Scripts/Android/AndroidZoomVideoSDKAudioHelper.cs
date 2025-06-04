#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;

public class AndroidZoomVideoSDKAudioHelper
{
    private AndroidJavaObject audioHelper;

    public AndroidZoomVideoSDKAudioHelper(AndroidJavaObject audioHelper)
    {
        this.audioHelper = audioHelper;
    }

    private AndroidJavaObject GetAudioHelper()
    {
        if (audioHelper != null)
        {
            return audioHelper;
        }

        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        {
            audioHelper = zoomSDK.Call<AndroidJavaObject>("getAudioHelper");
        }
        if (audioHelper == null)
        {
            Debug.LogError("ZoomVideoSdkVideoHelper:: No audio helper found.");
        }

        return audioHelper;
    }

    public void MuteAudio(ZMVideoSDKUser user, Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            AndroidJavaObject userObject = user.VideoSDKUser.GetUser();
            if (userObject != null)
            {
                activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    callback(AndroidZoomVideoSDKErrors.GetEnum(GetAudioHelper().Call<int>("muteAudio", userObject)));
                }));
            }

        }
    }

    public void UnMuteAudio(ZMVideoSDKUser user, Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject userObject = user.VideoSDKUser.GetUser();
                callback(AndroidZoomVideoSDKErrors.GetEnum(GetAudioHelper().Call<int>("unMuteAudio", userObject)));
            }));
        }
    }

    public void StartAudio(Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                callback(AndroidZoomVideoSDKErrors.GetEnum(GetAudioHelper().Call<int>("startAudio")));
            }));
        }
    }

    public void StopAudio(Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                callback(AndroidZoomVideoSDKErrors.GetEnum(GetAudioHelper().Call<int>("stopAudio")));
            }));
        }
    }

    public void Subscribe(Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                callback(AndroidZoomVideoSDKErrors.GetEnum(GetAudioHelper().Call<int>("subscribe")));
            }));
        }
    }

    public void UnSubscribe(Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                callback(AndroidZoomVideoSDKErrors.GetEnum(GetAudioHelper().Call<int>("unSubscribe")));
            }));
        }
    }

    public List<ZMVideoSDKSpeakerDevice> GetSpeakerList()
    {
        throw new NotImplementedException();
    }

    public List<ZMVideoSDKMicDevice> GetMicList()
    {
        throw new NotImplementedException();
    }

    public ZMVideoSDKErrors SelectSpeaker(string deviceID, string deviceName)
    {
        throw new NotImplementedException();
    }

    public ZMVideoSDKErrors SelectMic(string deviceID, string deviceName)
    {
        throw new NotImplementedException();
    }
}

#endif
