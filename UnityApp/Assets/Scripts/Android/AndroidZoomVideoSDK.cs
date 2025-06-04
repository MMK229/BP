#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;

public class AndroidZoomVideoSDK
{
    private static readonly AndroidZoomVideoSDK instance = new AndroidZoomVideoSDK();
    private Dictionary<IZMVideoSDKDelegate, AndroidZoomVideoSDKDelegate> sdkDelegateDic
        = new Dictionary<IZMVideoSDKDelegate, AndroidZoomVideoSDKDelegate>();

    static AndroidZoomVideoSDK()
    {
    }

    private AndroidZoomVideoSDK()
    {
    }

    public static AndroidZoomVideoSDK Instance
    {
        get
        {
            return instance;
        }
    }

    public void Initialize(ZMVideoSDKInitParams config, Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        {
            AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject videoRawDataMemoryMode = null;
                AndroidJavaObject audioRawDataMemoryMode = null;
                AndroidJavaObject shareRawDataMemoryMode = null;
                AndroidJavaObject extendParams = null;
                using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
                using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
                using (AndroidJavaObject param = new AndroidJavaObject(Config.ZOOM_VIDEO_SDK_INIT_PARAMS_PATH))
                {
                    if (!String.IsNullOrEmpty(config.domain))
                    {
                        string d = config.domain;
                        param.Set<string>("domain", d);
                    }
                    if (!String.IsNullOrEmpty(config.logFilePrefix))
                    {
                        param.Set<string>("logFilePrefix", config.logFilePrefix);
                    }
                    param.Set<bool>("enableLog", config.enableLog);
                    param.Set<bool>("enableFullHD", config.enableFullHD);
                    videoRawDataMemoryMode = AndroidZoomVideoSdkRawDataMemoryMode.GetJavaObject(config.videoRawDataMemoryMode);
                    param.Set<AndroidJavaObject>("videoRawDataMemoryMode", videoRawDataMemoryMode);
                    audioRawDataMemoryMode = AndroidZoomVideoSdkRawDataMemoryMode.GetJavaObject(config.audioRawDataMemoryMode);
                    param.Set<AndroidJavaObject>("audioRawDataMemoryMode", audioRawDataMemoryMode);
                    shareRawDataMemoryMode = AndroidZoomVideoSdkRawDataMemoryMode.GetJavaObject(config.shareRawDataMemoryMode);
                    param.Set<AndroidJavaObject>("shareRawDataMemoryMode", shareRawDataMemoryMode);
                    extendParams = new AndroidJavaObject(Config.ZOOM_VIDEO_SDK_EXTEND_PARAMS_PATH);
                    extendParams.Set<int>("wrapperType", 4);
                    if (!String.IsNullOrEmpty(config.extendParams.speakerTestFilePath))
                    {
                        extendParams.Set<string>("speakerTestFilePath", config.extendParams.speakerTestFilePath);
                    }
                    param.Set<AndroidJavaObject>("extendParam", extendParams);

                    try
                    {
                        callback(AndroidZoomVideoSDKErrors.GetEnum(zoomSDK.Call<int>("initialize", activity, param)));
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("initialize error " + e.ToString());
                    }
                    JavaObjectHelper.Dispose(extendParams);
                    JavaObjectHelper.Dispose(videoRawDataMemoryMode);
                    JavaObjectHelper.Dispose(audioRawDataMemoryMode);
                    JavaObjectHelper.Dispose(shareRawDataMemoryMode);
                }
            }));
        }
    }

    public void JoinSession(ZMVideoSDKSessionContext config, Action<ZMVideoSDKErrors> callback)
    {
        AndroidJavaObject audioOption = new AndroidJavaObject(Config.ZOOM_VIDEO_SDK_AUDIO_OPTION_PATH);
        audioOption.Set<bool>("connect", config.audioOption.connect);
        audioOption.Set<bool>("mute", config.audioOption.mute);
        AndroidJavaObject videoOption = new AndroidJavaObject(Config.ZOOM_VIDEO_SDK_VIDEO_OPTION_PATH);
        videoOption.Set<bool>("localVideoOn", config.videoOption.localVideoOn);
        AndroidJavaObject sessionContext = new AndroidJavaObject(Config.ZOOM_VIDEO_SDK_SESSION_CONTEXT_PATH);
        sessionContext.Set<string>("sessionName", config.sessionName);
        sessionContext.Set<string>("userName", config.userName);
        sessionContext.Set<string>("token", config.token);
        if (!String.IsNullOrEmpty(config.sessionPassword))
        {
            sessionContext.Set<string>("sessionPassword", config.sessionPassword);
        }
        if (config.sessionIdleTimeoutMins == 0)
        {
            config.sessionIdleTimeoutMins = 40;
        }
        sessionContext.Set<int>("sessionIdleTimeoutMins", (int)config.sessionIdleTimeoutMins);
        sessionContext.Set<AndroidJavaObject>("audioOption", audioOption);
        sessionContext.Set<AndroidJavaObject>("videoOption", videoOption);
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                try
                {
                    using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
                    using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
                    using (AndroidJavaObject session = zoomSDK.Call<AndroidJavaObject>("joinSession", sessionContext))
                    {
                        callback(ZMVideoSDKErrors.ZMVideoSDKErrors_Success);
                    }
                }
                catch (Exception)
                {
                    callback(ZMVideoSDKErrors.ZMVideoSDKErrors_Internal_Error);
                    Debug.Log("Joined session failed");
                }
                JavaObjectHelper.Dispose(videoOption);
                JavaObjectHelper.Dispose(audioOption);
                JavaObjectHelper.Dispose(sessionContext);
            }
            ));
        }
    }

    public void LeaveSession(bool shouldEndSession, Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
                using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
                {
                    int result = zoomSDK.Call<int>("leaveSession", shouldEndSession);
                    callback(AndroidZoomVideoSDKErrors.GetEnum(result));
                }
            }));
        }
    }

    public string GetSdkVersion()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        {
            return zoomSDK.Call<string>("getSDKVersion");
        }
    }

    public bool IsInSession()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        {
            return zoomSDK.Call<bool>("isInSession");
        }
    }

    public void Cleanup(Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
                using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
                {
                    int result = zoomSDK.Call<int>("cleanup");
                    if (AndroidZoomVideoSDKErrors.GetEnum(result) == ZMVideoSDKErrors.ZMVideoSDKErrors_Success)
                    {
                        Debug.Log("SDK cleanup successfully");
                    }
                    else
                    {
                        Debug.Log("SDK failed to cleanup with error code: " + result);
                    }
                    callback(AndroidZoomVideoSDKErrors.GetEnum(result));
                }
            }
            ));
        }
    }

    public ZMVideoSDKSession GetSessionInfo()
    {
        AndroidZoomVideoSDKSession androidSession = new AndroidZoomVideoSDKSession();
        ZMVideoSDKSession zmSession = new ZMVideoSDKSession(androidSession);
        return zmSession;
    }

    public void AddListener(IZMVideoSDKDelegate videoSDKDelegate)
    {
        AndroidZoomVideoSDKDelegate androidSDKDelegate = new AndroidZoomVideoSDKDelegate(videoSDKDelegate);
        sdkDelegateDic.Add(videoSDKDelegate, androidSDKDelegate);
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        {
            zoomSDK.Call("addListener", androidSDKDelegate);
        }
    }

    public void RemoveListener(IZMVideoSDKDelegate videoSDKDelegate)
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        {
            zoomSDK.Call("removeListener", sdkDelegateDic[videoSDKDelegate]);
        }
    }

    public ZMVideoSDKAudioHelper GetAudioHelper()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        {
            AndroidJavaObject audioHelperObj = zoomSDK.Call<AndroidJavaObject>("getAudioHelper");
            AndroidZoomVideoSDKAudioHelper androidAudioHelper = new AndroidZoomVideoSDKAudioHelper(audioHelperObj);
            ZMVideoSDKAudioHelper audioHelper = new ZMVideoSDKAudioHelper(androidAudioHelper);
            return audioHelper;
        }
    }

    public ZMVideoSDKVideoHelper GetVideoHelper()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        {
            AndroidJavaObject androidVideoHelper = zoomSDK.Call<AndroidJavaObject>("getVideoHelper");
            ZMVideoSDKVideoHelper videoHelper = new ZMVideoSDKVideoHelper(androidVideoHelper);
            return videoHelper;
        }
    }

    public ZMVideoSDKUserHelper GetUserHelper()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        {
            AndroidJavaObject userHelperObject = zoomSDK.Call<AndroidJavaObject>("getUserHelper");
            AndroidZoomVideoSDKUserHelper androidUserHelper = new AndroidZoomVideoSDKUserHelper(userHelperObject);
            ZMVideoSDKUserHelper userHelper = new ZMVideoSDKUserHelper(androidUserHelper);
            return userHelper;
        }
    }

    public ZMVideoSDKChatHelper GetChatHelper()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        {
            AndroidJavaObject javaChatHelper = zoomSDK.Call<AndroidJavaObject>("getChatHelper");
            AndroidZoomVideoSDKChatHelper androidChatHelper = new AndroidZoomVideoSDKChatHelper(javaChatHelper);
            ZMVideoSDKChatHelper chatHelper = new ZMVideoSDKChatHelper(androidChatHelper);
            return chatHelper;
        }
    }

    public ZMVideoSDKShareHelper GetShareHelper()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        {
            AndroidJavaObject javaShareHelper = zoomSDK.Call<AndroidJavaObject>("getShareHelper");
            AndroidZoomVideoSDKShareHelper androidShareHelper = new AndroidZoomVideoSDKShareHelper(javaShareHelper);
            ZMVideoSDKShareHelper shareHelper = new ZMVideoSDKShareHelper(androidShareHelper);
            return shareHelper;
        }
    }

    public ZMVideoSDKRecordingHelper GetRecordingHelper()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        {
            AndroidJavaObject javaRecordingHelper = zoomSDK.Call<AndroidJavaObject>("getRecordingHelper");
            AndroidZoomVideoSDKRecordingHelper androidRecordingHelper = new AndroidZoomVideoSDKRecordingHelper(javaRecordingHelper);
            ZMVideoSDKRecordingHelper recordingHelper = new ZMVideoSDKRecordingHelper(androidRecordingHelper);
            return recordingHelper;
        }
    }
}

#endif
