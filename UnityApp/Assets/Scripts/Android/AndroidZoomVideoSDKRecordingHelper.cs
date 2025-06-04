using System;
using UnityEngine;

public class AndroidZoomVideoSDKRecordingHelper
{

    private AndroidJavaObject _recordingHelper;

    public AndroidZoomVideoSDKRecordingHelper(AndroidJavaObject recordingHelper)
	{
        _recordingHelper = recordingHelper;
	}

    private AndroidJavaObject GetRecordingHelper()
    {
        if (_recordingHelper != null)
        {
            return _recordingHelper;
        }

        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        {
            _recordingHelper = zoomSDK.Call<AndroidJavaObject>("getRecordingHelper");
        }
        if (_recordingHelper == null)
        {
            throw new MissingMethodException("ZoomVideoSDKRecordingHelper:: No recording helper found.");
        }

        return _recordingHelper;
    }

    public ZMVideoSDKErrors CanStartRecording()
    {
        return AndroidZoomVideoSDKErrors.GetEnum(GetRecordingHelper().Call<int>("canStartRecording"));
    }

    public void StartCloudRecording(Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            AndroidJavaObject recordingHelper = GetRecordingHelper();
            if (recordingHelper != null)
            {
                activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    callback(AndroidZoomVideoSDKErrors.GetEnum(GetRecordingHelper().Call<int>("startCloudRecording")));
                }));
            }
        }
    }

    public void StopCloudRecording(Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            AndroidJavaObject recordingHelper = GetRecordingHelper();
            if (recordingHelper != null)
            {
                activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    callback(AndroidZoomVideoSDKErrors.GetEnum(GetRecordingHelper().Call<int>("stopCloudRecording")));
                }));
            }
        }
    }

    public void PauseCloudRecording(Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            AndroidJavaObject recordingHelper = GetRecordingHelper();
            if (recordingHelper != null)
            {
                activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    callback(AndroidZoomVideoSDKErrors.GetEnum(GetRecordingHelper().Call<int>("pauseCloudRecording")));
                }));
            }
        }
    }

    public void ResumeCloudRecording(Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            AndroidJavaObject recordingHelper = GetRecordingHelper();
            if (recordingHelper != null)
            {
                activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    callback(AndroidZoomVideoSDKErrors.GetEnum(GetRecordingHelper().Call<int>("resumeCloudRecording")));
                }));
            }
        }
    }

    public ZMVideoSDKRecordingStatus GetCloudRecordingStatus()
    {
        return AndroidZoomVideoSDKRecordingStatus.GetEnum(GetRecordingHelper().Call<AndroidJavaObject>("getCloudRecordingStatus").Call<string>("name"));
    }
}

