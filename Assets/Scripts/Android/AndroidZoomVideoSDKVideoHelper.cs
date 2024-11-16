#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;

public class AndroidZoomVideoSDKVideoHelper
{
    private AndroidJavaObject videoHelper;

    public AndroidZoomVideoSDKVideoHelper(AndroidJavaObject videoHelper)
    {
        this.videoHelper = videoHelper;
    }

    private AndroidJavaObject GetVideoHelper()
    {
        if (videoHelper != null)
        {
            return videoHelper;
        }

        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        {
            videoHelper = zoomSDK.Call<AndroidJavaObject>("getVideoHelper");
            if (videoHelper == null)
            {
                throw new Exception("No Video Helper Found");
            }
        }
        return videoHelper;
    }

    public void SetVideoQualityPreference(ZMVideoSDKPreferenceSetting config, Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                using (AndroidJavaObject videoPreference = new AndroidJavaObject(Config.ZOOM_VIDEO_SDK_VIDEO_PREFERENCE_SETTING_PATH))
                using (AndroidJavaObject videoPreferenceMode = AndroidZoomVideoSDKVideoPreferenceMode.GetJavaObject(config.mode))
                {
                    videoPreference.Set<AndroidJavaObject>("mode", videoPreferenceMode);

                    if (config.mode == ZMVideoSDKVideoPreferenceMode.ZMVideoSDKVideoPreferenceMode_Custom)
                    {
                        videoPreference.Set<int>("maximumFrameRate", (int)config.maximumFrameRate);
                        videoPreference.Set<int>("minimumFrameRate", (int)config.minimumFrameRate);
                    }
                    int result = GetVideoHelper().Call<int>("setVideoQualityPreference", videoPreference);
                    callback(AndroidZoomVideoSDKErrors.GetEnum(result));
                }
            }));
        }
    }

    public List<ZMVideoSDKCameraDevice> GetCameraList()
    {
        List<ZMVideoSDKCameraDevice> devices = new List<ZMVideoSDKCameraDevice>();
        using (AndroidJavaObject deviceList = GetVideoHelper().Call<AndroidJavaObject>("getCameraList"))
        {
            int deviceListSize = deviceList.Call<int>("size");
            for (int i = 0; i < deviceListSize; i++)
            {
                ZMVideoSDKCameraDevice deviceItem = new ZMVideoSDKCameraDevice();
                using (AndroidJavaObject device = deviceList.Call<AndroidJavaObject>("get", i))
                {
                    deviceItem.deviceID = device.Call<string>("getDeviceId");
                    deviceItem.deviceName = device.Call<string>("getDeviceName");
                    deviceItem.isSelectedDevice = device.Call<bool>("isSelectedDevice");
                }
                devices.Add(deviceItem);
            }
        }
        return devices;
    }

    public uint GetNumberOfCameras()
    {
        return (uint) GetVideoHelper().Call<int>("getNumberOfCameras");
    }

    public void RotateMyVideo(ZMVideoRotation rotation, Action<bool> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                callback(GetVideoHelper().Call<bool>("RotateMyVideo", AndroidZoomVideoRotation.GetValue(rotation)));
            }));
        }

    }

    public void StartVideo(Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                callback(AndroidZoomVideoSDKErrors.GetEnum(GetVideoHelper().Call<int>("startVideo")));
            }));
        }
    }

    public void StopVideo(Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                callback(AndroidZoomVideoSDKErrors.GetEnum(GetVideoHelper().Call<int>("stopVideo")));
            }));
        }
    }

    public void SwitchCamera(string deviceId)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                if (deviceId != null)
                {
                    using (AndroidJavaObject deviceList = GetVideoHelper().Call<AndroidJavaObject>("getCameraList"))
                    {
                        int deviceListSize = deviceList.Call<int>("size");
                        for (int i = 0; i < deviceListSize; i++)
                        {
                            using (AndroidJavaObject device = deviceList.Call<AndroidJavaObject>("get", i))
                            {
                                if (String.Equals(device.Call<string>("getDeviceId"), deviceId))
                                {
                                    GetVideoHelper().Call<bool>("switchCamera", device);
                                }
                            }
                        }
                    }
                }
                GetVideoHelper().Call<bool>("switchCamera");
            }));
        }
    }

    public void SwitchCamera(Action<bool> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                callback(videoHelper.Call<bool>("switchCamera"));
            }));
        }
    }

    public void MirrorMyVideo(bool enable)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                GetVideoHelper().Call<int>("mirrorMyVideo", enable);
            }));
        }
    }

    public bool IsMyVideoMirrored()
    {
        return GetVideoHelper().Call<bool>("isMyVideoMirrored");
    }

    public bool IsOriginalAspectRatioEnabled()
    {
        return GetVideoHelper().Call<bool>("isOriginalAspectRatioEnabled");
    }

    public void EnableOriginalAspectRatio(bool enable, Action<bool> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                callback(GetVideoHelper().Call<bool>("enableOriginalAspectRatio", enable));

            }));
        }
    }

    public (ZMVideoSDKErrors, bool) CanControlCamera(string deviceID)
    {
        throw new NotImplementedException();
    }

    public ZMVideoSDKErrors TurnCameraLeft(uint range, string deviceID)
    {
        throw new NotImplementedException();
    }

    public ZMVideoSDKErrors TurnCameraRight(uint range, string deviceID)
    {
        throw new NotImplementedException();
    }

    public ZMVideoSDKErrors TurnCameraUp(uint range, string deviceID)
    {
        throw new NotImplementedException();
    }

    public ZMVideoSDKErrors TurnCameraDown(uint range, string deviceID)
    {
        throw new NotImplementedException();
    }

    public ZMVideoSDKErrors ZoomCameraIn(uint range, string deviceID)
    {
        throw new NotImplementedException();
    }

    public ZMVideoSDKErrors ZoomCameraOut(uint range, string deviceID)
    {
        throw new NotImplementedException();
    }

    public bool EnableMultiStreamVideo(string cameraDeviceID, string customDeviceName)
    {
        throw new NotImplementedException();
    }

    public bool DisableMultiStreamVideo(string cameraDeviceID)
    {
        throw new NotImplementedException();
    }

    public string GetDeviceIDByMyPipe(ZMVideoSDKRawDataPipe zmVideoSDKRawDataPipe)
    {
        throw new NotImplementedException();
    }

    public bool SelectCamera(string cameraDeviceID)
    {
        throw new NotImplementedException();
    }
}
#endif
