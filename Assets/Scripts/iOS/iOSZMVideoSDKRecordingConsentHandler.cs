#if UNITY_IOS
using System;
using System.Runtime.InteropServices;

public class IOSZMVideoSDKRecordingConsentHandler
{
    public IOSZMVideoSDKRecordingConsentHandler(IntPtr _objC_ZMVideoSDKRecordingConsentHandler)
    {
        objC_ZMVideoSDKRecordingConsentHandler = _objC_ZMVideoSDKRecordingConsentHandler;
    }

    [DllImport ("__Internal")]
	private static extern void deallocate_ObjCWrapper(IntPtr zmVideoSDKRecordingConsentHandler);
    ~IOSZMVideoSDKRecordingConsentHandler()
    {
        deallocate_ObjCWrapper(objC_ZMVideoSDKRecordingConsentHandler);
    }

    private IntPtr objC_ZMVideoSDKRecordingConsentHandler;
    public IntPtr ObjC_ZMVideoSDKRecordingConsentHandler
    {
        get { return objC_ZMVideoSDKRecordingConsentHandler; }
    }

    [DllImport ("__Internal")]
	private static extern bool accept(IntPtr zmVideoSDKRecordingConsentHandler);
    public bool Accept()
    {
        return accept(objC_ZMVideoSDKRecordingConsentHandler);
    }

    [DllImport ("__Internal")]
	private static extern bool decline(IntPtr zmVideoSDKRecordingConsentHandler);
    public bool Decline()
    {
        return decline(objC_ZMVideoSDKRecordingConsentHandler);
    }

    [DllImport ("__Internal")]
	private static extern ZMVideoSDKRecordingConsentType getConsentType(IntPtr zmVideoSDKRecordingConsentHandler);
    public ZMVideoSDKRecordingConsentType GetConsentType()
    {
        return getConsentType(objC_ZMVideoSDKRecordingConsentHandler);
    }
}
#endif
