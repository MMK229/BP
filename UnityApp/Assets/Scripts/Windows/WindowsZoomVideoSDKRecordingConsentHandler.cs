#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;

public class WindowsZoomVideoSDKRecordingConsentHandler
{
    private IntPtr consentHandler;
    public WindowsZoomVideoSDKRecordingConsentHandler(IntPtr consentHandler)
    {
        this.consentHandler = consentHandler;
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool acceptRecordingConsent_c(IntPtr handler);
    public bool Accept()
    {
        return acceptRecordingConsent_c(consentHandler);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool declineRecordingConsent_c(IntPtr handler);
    public bool Decline()
    {
        return declineRecordingConsent_c(consentHandler);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKRecordingConsentType getRecordingConsentType_c(IntPtr handler);
    public ZMVideoSDKRecordingConsentType GetConsentType()
    {
        return getRecordingConsentType_c(consentHandler);
    }
}
#endif