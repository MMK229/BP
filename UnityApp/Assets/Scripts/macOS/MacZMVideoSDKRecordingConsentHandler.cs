#if UNITY_STANDALONE_OSX
using System;
using System.Runtime.InteropServices;

public class MacZMVideoSDKRecordingConsentHandler
{
    public MacZMVideoSDKRecordingConsentHandler(IntPtr _objC_ZMVideoSDKRecordingConsentHandler)
    {
        objC_ZMVideoSDKRecordingConsentHandler = _objC_ZMVideoSDKRecordingConsentHandler;
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern void deallocate_ObjCWrapper(IntPtr zmVideoSDKRecordingConsentHandler);
    ~MacZMVideoSDKRecordingConsentHandler()
    {
        deallocate_ObjCWrapper(objC_ZMVideoSDKRecordingConsentHandler);
    }

    private IntPtr objC_ZMVideoSDKRecordingConsentHandler;
    public IntPtr ObjC_ZMVideoSDKRecordingConsentHandler
    {
        get { return objC_ZMVideoSDKRecordingConsentHandler; }
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern bool acceptRecordingConsent(IntPtr zmVideoSDKRecordingConsentHandler);
    public bool Accept()
    {
        return acceptRecordingConsent(objC_ZMVideoSDKRecordingConsentHandler);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern bool declineRecordingConsent(IntPtr zmVideoSDKRecordingConsentHandler);
    public bool Decline()
    {
        return declineRecordingConsent(objC_ZMVideoSDKRecordingConsentHandler);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern ZMVideoSDKRecordingConsentType getConsentType(IntPtr zmVideoSDKRecordingConsentHandler);
    public ZMVideoSDKRecordingConsentType GetConsentType()
    {
        return getConsentType(objC_ZMVideoSDKRecordingConsentHandler);
    }
}
#endif
