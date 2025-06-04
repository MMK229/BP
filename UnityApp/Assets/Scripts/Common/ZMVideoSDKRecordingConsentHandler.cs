using System;
public class ZMVideoSDKRecordingConsentHandler
{

#if UNITY_STANDALONE_OSX
    private MacZMVideoSDKRecordingConsentHandler _recordingConsentHelper = null;
    public ZMVideoSDKRecordingConsentHandler(MacZMVideoSDKRecordingConsentHandler recordingConsentHandler)
    {
        _recordingConsentHelper = recordingConsentHandler;
    }
#elif UNITY_ANDROID
    private AndroidZoomVideoSDKRecordingConsentHandler _recordingConsentHelper = null;
    public ZMVideoSDKRecordingConsentHandler(AndroidZoomVideoSDKRecordingConsentHandler recordingConsentHelper)
    {
        _recordingConsentHelper = recordingConsentHelper;
    }
#elif UNITY_STANDALONE_WIN
    private WindowsZoomVideoSDKRecordingConsentHandler _recordingConsentHelper = null;
    public ZMVideoSDKRecordingConsentHandler(WindowsZoomVideoSDKRecordingConsentHandler recordingConsentHandler)
    {
        _recordingConsentHelper = recordingConsentHandler;
    }
#elif UNITY_IOS
    private IOSZMVideoSDKRecordingConsentHandler _recordingConsentHelper = null;
    public ZMVideoSDKRecordingConsentHandler(IOSZMVideoSDKRecordingConsentHandler recordingConsentHandler)
    {
        _recordingConsentHelper = recordingConsentHandler;
    }
#endif

    public bool Accept()
    {
        return _recordingConsentHelper.Accept();
    }

    public bool Decline()
    {
        return _recordingConsentHelper.Decline();
    }

    public ZMVideoSDKRecordingConsentType GetConsentType()
    {
        return _recordingConsentHelper.GetConsentType();
    }
}

