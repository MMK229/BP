#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;

public class WindowsZoomVideoSDK
{
    private static readonly WindowsZoomVideoSDK instance = new WindowsZoomVideoSDK();
    private static WindowsZoomVideoSDKDelegate delegateObj;
    static WindowsZoomVideoSDK()
    {
    }

    private WindowsZoomVideoSDK()
    {
    }

    public static WindowsZoomVideoSDK Instance
    {
        get
        {
            return instance;
        }
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern ZMVideoSDKErrors initialize_c(ref ZoomVideoSDKInitParams_w param);

    public void Initialize(ZMVideoSDKInitParams config, Action<ZMVideoSDKErrors> callback)
    {
        ZoomVideoSDKInitParams_w paramObj = new ZoomVideoSDKInitParams_w();
        paramObj.domain = config.domain;
        paramObj.logFilePrefix = config.logFilePrefix;
        paramObj.enableLog = config.enableLog;
        paramObj.enableIndirectRawdata = config.enableIndirectRawdata;
        paramObj.audioRawDataMemoryMode = config.audioRawDataMemoryMode;
        paramObj.videoRawDataMemoryMode = config.videoRawDataMemoryMode;
        paramObj.shareRawDataMemoryMode = config.shareRawDataMemoryMode;
        paramObj.extendParam.speakerTestFilePath = config.extendParams.speakerTestFilePath;
        callback(initialize_c(ref paramObj));
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern IntPtr joinSession_c(ref ZMVideoSDKSessionContext param);
    public void JoinSession(ZMVideoSDKSessionContext config, Action<ZMVideoSDKErrors> callback)
    {
        IntPtr session = joinSession_c(ref config);
        if (session != IntPtr.Zero)
        {
            callback(ZMVideoSDKErrors.ZMVideoSDKErrors_Success);
        }
        else
        {
            callback(ZMVideoSDKErrors.ZMVideoSDKErrors_Unknown);
        }
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors leaveSession_c(bool end);
    public void LeaveSession(bool shouldEndSession, Action<ZMVideoSDKErrors> callback)
    {
        callback(leaveSession_c(shouldEndSession));
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern string getSDKVersion_c();
    public string GetSdkVersion()
    {
        return getSDKVersion_c();
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool isInSession_c();
    public bool IsInSession()
    {
        return isInSession_c();
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors cleanup_c();
    public void Cleanup(Action<ZMVideoSDKErrors> callback)
    {
        callback(cleanup_c());
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern IntPtr getAudioHelper_c();
    public ZMVideoSDKAudioHelper GetAudioHelper()
    {
        IntPtr pAudioHelper = getAudioHelper_c();
        if (pAudioHelper == IntPtr.Zero)
        {
            return null;
        }
        WindowsZoomVideoSDKAudioHelper winAudioHelper = new WindowsZoomVideoSDKAudioHelper(pAudioHelper);
        return new ZMVideoSDKAudioHelper(winAudioHelper);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern IntPtr getVideoHelper_c();
    public ZMVideoSDKVideoHelper GetVideoHelper()
    {
        IntPtr pVideoHelper = getVideoHelper_c();
        if (pVideoHelper == IntPtr.Zero)
        {
            return null;
        }
        WindowsZoomVideoSDKVideoHelper winVideoHelper = new WindowsZoomVideoSDKVideoHelper(pVideoHelper);
        return new ZMVideoSDKVideoHelper(winVideoHelper);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern IntPtr getUserHelper_c();
    public ZMVideoSDKUserHelper GetUserHelper()
    {
        IntPtr pUserHelper = getUserHelper_c();
        if (pUserHelper == IntPtr.Zero)
        {
            return null;
        }
        WindowsZoomVideoSDKUserHelper winUserHelper = new WindowsZoomVideoSDKUserHelper(pUserHelper);
        return new ZMVideoSDKUserHelper (winUserHelper);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern IntPtr getChatHelper_c();
    public ZMVideoSDKChatHelper GetChatHelper()
    {
        IntPtr pChatHelper = getChatHelper_c();
        if (pChatHelper == IntPtr.Zero)
        {
            return null;
        }
        WindowsZoomVideoSDKChatHelper winChatHelper = new WindowsZoomVideoSDKChatHelper(pChatHelper);
        return new ZMVideoSDKChatHelper (winChatHelper);
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern IntPtr getSessionInfo_c();
    public ZMVideoSDKSession GetSessionInfo()
    {
        IntPtr cSessionObj = getSessionInfo_c();
        if (cSessionObj == IntPtr.Zero)
        {
            return null;
        }
        WindowsZoomVideoSDKSession winSessionObj = new WindowsZoomVideoSDKSession(cSessionObj);
        ZMVideoSDKSession sessionObj = new ZMVideoSDKSession(winSessionObj);
        return sessionObj;
    }


    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern void addListener_c();
    public void AddListener(IZMVideoSDKDelegate videoSDKDelegate)
    {
        delegateObj = new WindowsZoomVideoSDKDelegate(videoSDKDelegate);
        addListener_c();
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern void removeListener_c();
    public void RemoveListener(IZMVideoSDKDelegate videoSDKDelegate)
    {
        removeListener_c();
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern IntPtr getRecordingHelper_c();
    public ZMVideoSDKRecordingHelper GetRecordingHelper()
    {
        IntPtr pRecordingHelper = getRecordingHelper_c();
        if (pRecordingHelper == IntPtr.Zero)
        {
            return null;
        }
        WindowsZoomVideoSDKRecordingHelper winRecordingHelper = new WindowsZoomVideoSDKRecordingHelper(pRecordingHelper);
        return new ZMVideoSDKRecordingHelper(winRecordingHelper);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern IntPtr getShareHelper_c();
    public ZMVideoSDKShareHelper GetShareHelper()
    {
        IntPtr pShareHelper = getShareHelper_c();
        if (pShareHelper == IntPtr.Zero)
        {
            return null;
        }
        WindowsZoomVideoSDKShareHelper winShareHelper = new WindowsZoomVideoSDKShareHelper(pShareHelper);
        return new ZMVideoSDKShareHelper(winShareHelper);
    }
}

#endif